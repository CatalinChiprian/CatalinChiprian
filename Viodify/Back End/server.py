from flask import Flask, request, jsonify
from flask_cors import CORS
from scripts.text_to_speech import text_to_speech
from scripts.text_generation import generate_text, generate_images_transcript
from scripts.video_creation import create_video, combine_audio_with_clock
from enum import Enum

app = Flask(__name__)
CORS(app)

class ContentType(Enum):
    QUIZ = "QUIZ"
    FACTS = "FACTS"
    GENERAL_INFORMATION = "GENERAL_INFORMATION"

@app.route('/generate', methods=['POST'])
def generate():
    data = request.json
    prompt = data.get('prompt') 
    content_type = data.get('contentType')
    
    if not prompt or not content_type:
        return jsonify({"status": "error", "message": "Missing 'prompt' or 'contentType'"}), 400

    topic, text = generate_text(prompt)
    word_timings = text_to_speech(text)
    image_timings = generate_images_transcript(word_timings)

    if content_type == ContentType.QUIZ.value:
        combine_audio_with_clock(word_timings)

    create_video(word_timings, image_timings, topic)

    return jsonify({"status": "success", "message": "Video generated successfully!"})

if __name__ == '__main__':
    app.run(debug=True)