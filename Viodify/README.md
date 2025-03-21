# 🎬 Vidiofy

**Vidiofy** is an AI-powered video generation platform that transforms text prompts into fully composed short videos using language models, text-to-speech synthesis, image search, and video editing—all stitched together in a seamless experience.

---

## 🚀 Features

- 🧠 **Text Generation** with [LLaMA] — creates structured and contextual scripts
- 🗣️ **Voice Synthesis** using [ElevenLabs] — generates natural-sounding speech
- 🖼️ **Smart Image Selection** via [Google Custom Search API]
- 🎞️ **Automatic Video Assembly** using [MoviePy]
- 📺 **TikTok-style output (9:16 aspect ratio)**
- 🌐 **Frontend** built with **React** + TailwindCSS
- 🐍 **Backend** powered by **Flask (Python)** and REST API

---

## 🛠️ Tech Stack

| Frontend  | Backend   | AI / Tools       | Media |
|-----------|-----------|------------------|-------|
| React     | Python    | LLaMA            | MoviePy |
| Tailwind  | Flask     | ElevenLabs API   | pydub |
| Axios     | Flask-CORS| Google Search API| OpenCV |

---

## ⚙️ Setup Instructions

### 🔹 1. Clone the repository
```bash
git clone https://github.com/yourusername/vidiofy.git
cd vidiofy
```

### 🔹 2. Install Backend Requirements
```bash
cd "Back End"
python -m venv .venv
.venv\Scripts\activate   # or source .venv/bin/activate (Linux/Mac)
pip install -r requirements.txt
```

### 🔹 3. Setup settings.py
```bash
cp config/settings.template.py config/settings.py
Fill in your API keys for:
ElevenLabs
Google API
Path to LLaMA or any required local models
```

### 🔹 4. Run Flask Backend
```bash
python server.py
```

### 🔹 5. Setup & Run Frontend
```bash
cd ../"Front End"
npm install
npm run dev
```


## 📂 Project Structure
```
├── Back End/
│   ├── server.py
│   ├── config/
│   │   ├── settings.py             # (ignored)
│   │   └── settings.template.py    # (template)
│   └── scripts/
│       ├── text_generation.py
│       ├── text_to_speech.py
│       ├── video_creation.py
│       └── media_obtainer.py
│
├── Front End/
│   ├── src/
│   │   ├── App.jsx
│   │   ├── main.jsx
│   │   └── index.css
│   ├── public/
│   └── vite.config.js
├── assets/
│   ├──audio
│   └──background
├── results/
│   ├──audio
│   ├──text
│   └──video
```



## 📥 Output
The final generated video is saved to:
results/video/output.mp4