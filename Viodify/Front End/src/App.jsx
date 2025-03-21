import React, { useState } from "react";
import { Video, Image, Mic, Brain, Play, Loader2 } from "lucide-react";
import axios from "axios";

function App() {
  const [contentType, setContentType] = useState("QUIZ");
  const [prompt, setPrompt] = useState("");
  const [isGenerating, setIsGenerating] = useState(false);

  const handleGenerate = async () => {
    setIsGenerating(true);
    try {
      const response = await axios.post("http://localhost:5000/generate", {
        prompt,
        contentType,
      });
      console.log(response.data.message);
    } catch (error) {
      console.error("Error generating video:", error);
    } finally {
      setIsGenerating(false);
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-900 via-purple-900 to-violet-900 text-white">
      <div className="container mx-auto px-4 sm:px-6 lg:px-8 py-8 sm:py-12">
        {/* Hero Section */}
        <div className="text-center mb-8 sm:mb-12">
          <div className="flex justify-center mb-4 sm:mb-6">
            <Video className="w-12 h-12 sm:w-16 sm:h-16 text-purple-400" />
          </div>
          <h1 className="text-3xl sm:text-4xl lg:text-5xl font-bold mb-3 sm:mb-4">
            AI Video Generator
          </h1>
          <p className="text-lg sm:text-xl text-purple-200 max-w-xl sm:max-w-2xl mx-auto px-4">
            Create engaging videos automatically with AI. Perfect for social
            media content, educational material, and more.
          </p>
        </div>

        {/* Main Interface */}
        <div className="max-w-3xl mx-auto bg-white/10 backdrop-blur-lg rounded-xl sm:rounded-2xl p-4 sm:p-6 lg:p-8 mb-8 sm:mb-12">
          {/* Content Type Selection */}
          <div className="mb-6 sm:mb-8">
            <label className="block text-purple-200 mb-2 sm:mb-3 text-sm">
              Select Content Type
            </label>
            <div className="grid grid-cols-1 sm:grid-cols-3 gap-3 sm:gap-4">
              {["QUIZ", "FACTS", "GENERAL_INFORMATION"].map((type) => (
                <button
                  key={type}
                  onClick={() => setContentType(type)}
                  className={`p-3 sm:p-4 rounded-lg border-2 transition-all text-sm sm:text-base ${
                    contentType === type
                      ? "border-purple-400 bg-purple-400/20"
                      : "border-white/20 hover:border-purple-400/50"
                  }`}
                >
                  {type.replace("_", " ")}
                </button>
              ))}
            </div>
          </div>

          {/* Prompt Input */}
          <div className="mb-6 sm:mb-8">
            <label className="block text-purple-200 mb-2 sm:mb-3 text-sm">
              Enter Your Prompt
            </label>
            <textarea
              value={prompt}
              onChange={(e) => setPrompt(e.target.value)}
              placeholder="e.g., Create a quiz about world capitals..."
              className="w-full h-24 sm:h-32 bg-white/5 border-2 border-white/20 rounded-lg p-3 sm:p-4 text-white placeholder:text-white/50 focus:border-purple-400 focus:outline-none transition-colors text-sm sm:text-base"
              style={{
                scrollbarWidth: "thin",
                scrollbarColor: "rgba(255, 255, 255, 0.3) transparent",
              }}
            />
          </div>

          {/* Generate Button */}
          <button
            onClick={handleGenerate}
            disabled={isGenerating || !prompt}
            className="w-full bg-purple-600 hover:bg-purple-700 disabled:bg-purple-900 disabled:cursor-not-allowed py-3 sm:py-4 rounded-lg font-semibold transition-colors flex items-center justify-center gap-2 text-sm sm:text-base"
          >
            {isGenerating ? (
              <>
                <Loader2 className="w-4 h-4 sm:w-5 sm:h-5 animate-spin" />
                Generating Video...
              </>
            ) : (
              <>
                <Play className="w-4 h-4 sm:w-5 sm:h-5" />
                Generate Video
              </>
            )}
          </button>
        </div>

        {/* Features */}
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4 sm:gap-6 lg:gap-8">
          <div className="bg-white/5 backdrop-blur-sm rounded-lg sm:rounded-xl p-4 sm:p-6">
            <Brain className="w-6 h-6 sm:w-8 sm:h-8 text-purple-400 mb-3 sm:mb-4" />
            <h3 className="text-lg sm:text-xl font-semibold mb-2">
              AI Text Generation
            </h3>
            <p className="text-sm sm:text-base text-purple-200">
              Powered by Gemini AI to create engaging and informative content
              automatically.
            </p>
          </div>
          <div className="bg-white/5 backdrop-blur-sm rounded-lg sm:rounded-xl p-4 sm:p-6">
            <Mic className="w-6 h-6 sm:w-8 sm:h-8 text-purple-400 mb-3 sm:mb-4" />
            <h3 className="text-lg sm:text-xl font-semibold mb-2">
              Text to Speech
            </h3>
            <p className="text-sm sm:text-base text-purple-200">
              Natural-sounding voice synthesis for professional narration.
            </p>
          </div>
          <div className="bg-white/5 backdrop-blur-sm rounded-lg sm:rounded-xl p-4 sm:p-6 sm:col-span-2 lg:col-span-1">
            <Image className="w-6 h-6 sm:w-8 sm:h-8 text-purple-400 mb-3 sm:mb-4" />
            <h3 className="text-lg sm:text-xl font-semibold mb-2">
              Smart Image Selection
            </h3>
            <p className="text-sm sm:text-base text-purple-200">
              Automatic selection and integration of relevant images for your
              content.
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}

export default App;
