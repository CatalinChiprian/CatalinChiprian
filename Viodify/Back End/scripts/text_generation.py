from google import genai
from google.genai import types
import json
import re
from config.settings import GEMINI_API_KEY, TEXT_OUTPUT_PATH

client = genai.Client(api_key=GEMINI_API_KEY)

text_generation_instruct = (
    "Global instructions: You are an expert text-to-speech writer for TikTok shorts. "
    "Include only the necessary text (around 60 seconds) with no '\\n'. "
    "Start by stating in ONE WORD the topic of the content. "
    "For example: 'Countries'. or 'Space'. "
    "Be funny, engaging, and motivate viewers to watch the entire video. "

    "Quiz instructions: Introduce the quiz topic. Insert a break after each question using "
    "<break time=\"5s\"/> (or as specified). Start with an easy question and increase "
    "difficulty up to 'impossible' (e.g., 'If you get this one right, you're a genius/expert at [topic]!'). "
    "Introduce answers engagingly (e.g., 'If you said..., then you're right') and add funny comments "
    "like 'If you got it wrong, don't worry, you're not alone!'. End by asking viewers how many they got right "
    "and to comment their score. Optionally ask them to like, share, and follow. "

    "Facts instructions: Introduce the topic (e.g., 'Did you know that...?'). No breaks are needed. "
    "Present facts in an interesting and humorous way, adding comments like 'If you didn't know this, don't worry, you're not alone!'. "
    "End by asking if the viewer knew the fact and to comment, with an optional call to like, share, and follow. "

    "General information instructions: Introduce the topic (e.g., 'Today we're going to talk about...'). "
    "No breaks are needed. Present the information clearly and engagingly, with humorous comments as needed. "
    "End by asking if the viewer knew the information and to comment, and optionally ask them to like, share, and follow."
)

def generate_text(prompt):
    response = client.models.generate_content(
        model = "gemini-2.0-flash",
        config = types.GenerateContentConfig(
            system_instruction=text_generation_instruct),
        contents = prompt
    )
    with open(TEXT_OUTPUT_PATH, "w", encoding="utf-8") as file:
        file.write(response.text)
    print("Generated text!")

    # Extract the first word to determine the type and remove it from the text
    words = response.text.split()
    topic = words[0]
    return topic, response.text


example_json = {
    "images": [
        {
            "image": "search_term_1",
            "image_start_times_seconds": 0.0,
            "image_end_times_seconds": 2.717
        },
        {
            "image": "search_term_2",
            "image_start_times_seconds": 5.717,
            "image_end_times_seconds": 9.653
        },
        {
            "image": "search_term_3",
            "image_start_times_seconds": 15.653,
            "image_end_times_seconds": 20.054
        }
    ]
}

image_transcript_instruct = (
    "You are given a transcript of audio word timings in three lists: 'characters', 'character_start_times_seconds', and 'character_end_times_seconds'. "
    "Your task is to generate image prompts and their associated display timings based on the spoken content. Follow these steps exactly: "
    "Use the example JSON as a guide: " + json.dumps(example_json, indent=4) + " "
    "1. Reconstruct the full spoken text by concatenating all characters. "
    "2. Split the text into words using whitespace. For each word, determine its start time as the start time of its first character and its end time as the end time of its last character. "
    "3. Identify natural segmentation points in the audio, such as sentence boundaries (periods, exclamation points, question marks) or significant pauses. "
    "4. For each identified segment, decide whether an image is necessary. Only generate an image prompt if the segment emphasizes a key point or would benefit from a visual representation. "
    "   It is acceptable to leave some segments without any image if the content is more narrative or doesn’t require visual reinforcement. "
    "   However, if you do think an image is needed, then keep the image at least 3 seconds long. "
    "5. When generating an image prompt, use engaging and relevant keywords that closely describe the audio content of that segment. "
    "6. Create arrays for the image prompts ('images'), the corresponding start times ('image_start_times_seconds'), and the corresponding end times ('image_end_times_seconds'). "
    "You must make sure to have the start and end times match the audio timings accurately."
    "7. Return the output as a JSON object with a single key 'images' containing these arrays. "
    "Include only the necessary text for the JSON output, nothing else. "
    "Remember: images are only needed when they emphasize or enhance the spoken content; do not force images for every segment."
    "Images are not needed for the outro, when users are asked to like, share, and follow."
)



def generate_images_transcript(word_timings):
    # Initialize lists to store the combined results
    all_characters = []
    all_start_times = []
    all_end_times = []

    # Iterate over the list of word timings
    for segment_timings in word_timings:
        characters = segment_timings['characters']
        start_times = segment_timings['character_start_times_seconds']
        end_times = segment_timings['character_end_times_seconds']
        
        all_characters.extend(characters)
        all_start_times.extend(start_times)
        all_end_times.extend(end_times)
    
    # Format the input data for the model
    # characters_str = ' '.join(all_characters)
    # start_times_str = ' '.join(map(str, all_start_times))
    # end_times_str = ' '.join(map(str, all_end_times))
    input_data = f"Characters: {characters}\nStart times: {start_times}\nEnd times: {end_times}"

    print(input_data)
    
    
    response = client.models.generate_content(
        model="gemini-2.0-flash",
        config=types.GenerateContentConfig(
            system_instruction=image_transcript_instruct
        ),
        contents=input_data
    )
    
    clean_text = re.sub(r"^```(?:json)?\s*", "", response.text)
    clean_text = re.sub(r"\s*```$", "", clean_text)
    print(clean_text)
    # Parse the JSON response
    response_data = json.loads(clean_text)
    
    images = [item['image'] for item in response_data['images']]
    image_start_time_seconds = [item['image_start_times_seconds'] for item in response_data['images']]
    image_end_times_seconds = [item['image_end_times_seconds'] for item in response_data['images']]
    
    return {
        'images': images,
        'image_start_times_seconds': image_start_time_seconds,
        'image_end_times_seconds': image_end_times_seconds
    }