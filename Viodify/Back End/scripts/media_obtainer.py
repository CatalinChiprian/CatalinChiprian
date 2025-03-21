import requests
from PIL import Image
from io import BytesIO
from googleapiclient.discovery import build
import numpy as np
from pytubefix import YouTube
from config.settings import GOOGLE_API_KEY, GOOGLE_SEARCH_ENGINE_ID

def search_images(query, num_images=5):
    service = build("customsearch", "v1", developerKey=GOOGLE_API_KEY)
    res = service.cse().list(
        q=query,
        cx=GOOGLE_SEARCH_ENGINE_ID,
        searchType="image",
        num=num_images
    ).execute()
    image_urls = [item['link'] for item in res['items']]
    return image_urls

def search_videos(query, max_results=5):
    youtube = build('youtube', 'v3', developerKey=GOOGLE_API_KEY)
    request = youtube.search().list(
        q=query,
        part='snippet',
        type='video',
        maxResults=max_results,
        videoDuration='medium'
    )
    response = request.execute()
    video_urls = [f"https://www.youtube.com/watch?v={item['id']['videoId']}" for item in response['items']]
    return video_urls

def download_video(url):
    path = './assets/background/'
    filename = 'background.mp4'
    try:
        yt = YouTube(url)
        yt = yt.streams.filter(file_extension='mp4').order_by('resolution').desc().first()
        yt.download(path, filename=filename)
        return path + filename
    except Exception as e:
        print(f"Error downloading YouTube video from {url}: {e}")
        return None


def download_image(url):
    try:
        response = requests.get(url)
        response.raise_for_status()
        img = Image.open(BytesIO(response.content)).convert("RGB")  # Convert to RGB
        return np.array(img)
    except (requests.RequestException, Image.UnidentifiedImageError) as e:
        print(f"Error downloading image from {url}: {e}")
        return None