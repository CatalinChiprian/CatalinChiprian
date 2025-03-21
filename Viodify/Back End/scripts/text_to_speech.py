from elevenlabs.client import ElevenLabs
from elevenlabs import save
import base64
from config.settings import ELEVENLABS_API_KEY, ELEVENLABS_VOICE_ID, AUDIO_OUTPUT_PATH

client = ElevenLabs(
  api_key=ELEVENLABS_API_KEY,
)

def text_to_speech(text):
    word_timings = []
    # Generate TTS for the entire text with timing information
    audio = client.text_to_speech.convert_with_timestamps(
        text=text,
        voice_id=ELEVENLABS_VOICE_ID
    )
    
    # Extract the audio data from the response
    audio_data = base64.b64decode(audio.audio_base_64)
    audio_path = AUDIO_OUTPUT_PATH
    save(audio_data, audio_path)
    word_timings.append({
            "characters": audio.alignment.characters,
            "character_start_times_seconds": audio.alignment.character_start_times_seconds,
            "character_end_times_seconds": audio.alignment.character_end_times_seconds
        })

    print("Generated audio with timings!")

    return word_timings

# voice=Voice(voice_id='82hBsVN6GRUwWKT8d1Kz') indian voice
# voice=Voice(voice_id='xctasy8XvGp2cVO9HL9k') Allison, female, english voice
# voice_id="JBFqnCBsd6RMkjVDRZzb" Male, english voice
# voice_id="ZF6FPAbjXT4488VcRRnw" Amelia, female, english voice
