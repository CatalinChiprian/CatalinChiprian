
from pydub import AudioSegment
from moviepy.editor import VideoFileClip, TextClip, CompositeVideoClip, AudioFileClip, ImageClip
from moviepy.video.fx.all import crop
import random
from scripts.media_obtainer import search_images, search_videos, download_video, download_image
from config.settings import AUDIO_OUTPUT_PATH, CLOCK_SOUND_PATH, IMAGEMAGICK_PATH, VIDEO_OUTPUT_PATH


from moviepy.config import change_settings
change_settings({"IMAGEMAGICK_BINARY": IMAGEMAGICK_PATH})


def combine_audio_with_clock(word_timings):
    combined = AudioSegment.from_file(AUDIO_OUTPUT_PATH)
    clock_sound = AudioSegment.from_file(CLOCK_SOUND_PATH)
    
    # Insert clock sound at appropriate intervals
    for segment_timings in word_timings:
        characters = segment_timings['characters']
        start_times = segment_timings['character_start_times_seconds']
        end_times = segment_timings['character_end_times_seconds']
        
        i = 0
        while i < len(characters):
            if characters[i] == '<' and ''.join(characters[i:i+6]) == '<break':
                clock_start_time = start_times[i-1] * 1000  # Convert to milliseconds
                clock_end_time = end_times[i] * 1000  # Convert to milliseconds
                clock_duration = clock_end_time - clock_start_time
                trimmed_clock_sound = clock_sound[:clock_duration]
                combined = combined.overlay(trimmed_clock_sound, position=clock_start_time)
            i += 1

    combined.export(AUDIO_OUTPUT_PATH, format="mp3")
    print("Combined audio with clock sound!")

def create_video(word_timings, image_timings, topic):
    # Load the combined audio to get its duration
    audio_clip = AudioFileClip(AUDIO_OUTPUT_PATH)
    audio_duration = audio_clip.duration
    
    # Search for a background video related to the topic
    background_video_query = f"{topic} 4k"
    video_urls = search_videos(background_video_query)

# Try to download and load the first available background video
    video = None
    for url in video_urls:
        downloaded_video_path = download_video(url)
        if downloaded_video_path:
            try:
                video = VideoFileClip(downloaded_video_path)
                break
            except Exception as e:
                print(f"Error loading video from {downloaded_video_path}: {e}")
    
    if video is None:
        raise Exception("No valid background video found.")
    
    # Calculate the start time for the random cut
    max_start_time = video.duration - audio_duration
    start_time = random.uniform(0, max_start_time)
    
    # Cut the video to the specified length from the random start time
    cut_video = video.subclip(start_time, start_time + audio_duration)
    
    # Remove the original audio from the background video
    cut_video = cut_video.without_audio()
    
    # Crop the video to TikTok size (9:16 aspect ratio)
    width, height = cut_video.size
    new_width = height * 9 / 16
    crop_x = (width - new_width) / 2
    cropped_video = crop(cut_video, x1=crop_x, x2=crop_x + new_width)

    # Create a generator for TextClip
    generator = lambda txt: TextClip(
        txt,
        font="Oswald",
        fontsize=60,
        color='white',
        stroke_color="black",
        stroke_width=3,
    )
    
    # Add subtitles to the video using word timings
    subtitle_clips = []
    for segment_timings in word_timings:
        characters = segment_timings['characters']
        start_times = segment_timings['character_start_times_seconds']
        end_times = segment_timings['character_end_times_seconds']
        
        word = ""
        word_start_time = start_times[0]
        for i, char in enumerate(characters):
            if char == '<' and ''.join(characters[i:i+6]) == '<break':
                while i < len(characters) and characters[i] != '>':
                    i += 1
                i += 1  # Skip the '>'
                word_start_time = start_times[i] if i < len(start_times) else word_start_time
                continue
            if char.isspace() or i == len(characters) - 1:
                if word:
                    word_end_time = end_times[i - 1]
                    subtitle = generator(word)
                    subtitle = subtitle.set_position(('center', height * 0.8)).set_duration(word_end_time - word_start_time).set_start(word_start_time)  # Center position
                    subtitle_clips.append(subtitle)
                    word = ""
                word_start_time = start_times[i]
            else:
                word += char

    # Add images to the video using image timings
    image_clips = []
    image_width = new_width * 0.8  # 80% of the new width
    image_height = height * 0.4  # 40% of the height
    for i, search_term in enumerate(image_timings['images']):
        image_urls = search_images(search_term)
        image = None
        for url in image_urls:
            image = download_image(url)
            if image is not None:
                break
        if image is not None:
            image_clip = ImageClip(image).resize(width=image_width, height=image_height).set_position('center').set_duration(image_timings['image_end_times_seconds'][i] - image_timings['image_start_times_seconds'][i]).set_start(image_timings['image_start_times_seconds'][i])
            image_clips.append(image_clip)

    final_video = CompositeVideoClip([cropped_video] + image_clips+ subtitle_clips)
    final_video = final_video.set_audio(audio_clip)

    # Trim the final video to cut the weird ending
    final_video = final_video.subclip(0, audio_duration - 0.2)
    
    final_video.write_videofile(VIDEO_OUTPUT_PATH, codec='libx264', audio_codec='aac')
    print(f"Video saved to {VIDEO_OUTPUT_PATH}")