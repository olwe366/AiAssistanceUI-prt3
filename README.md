# AiAssistanceUI-prt3
# Cybersecurity Awareness Chatbot

A comprehensive WPF desktop application designed to educate users about cybersecurity best practices through interactive chat, task management, and knowledge testing.

## 📋 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Technical Stack](#technical-stack)
- [Architecture](#architecture)
- [Installation](#installation)
- [Usage Guide](#usage-guide)
- [Project Structure](#project-structure)
- [Key Components](#key-components)
- [Database Schema](#database-schema)
- [Future Enhancements](#future-enhancements)
- [Contributing](#contributing)
- [License](#license)

## 🎯 Overview

The Cybersecurity Awareness Chatbot is an interactive desktop application that serves as a personal cybersecurity guardian. It combines natural language processing, speech capabilities, task management, and educational quizzes to help users understand and implement security best practices.

### Core Mission
- Educate users about cybersecurity threats and prevention
- Provide practical security tips and advice
- Track security-related tasks and reminders
- Test knowledge through interactive quizzes
- Log user activities for learning tracking

## ✨ Features

### 🤖 Intelligent Chatbot
- **Natural Language Processing**: Understands user queries about cybersecurity topics
- **Contextual Awareness**: Remembers conversation context and user preferences
- **Sentiment Analysis**: Detects user sentiment and responds empathetically
- **Personalized Responses**: Remembers user names and interests
- **Keyword-Based Education**: Provides detailed information on:
  - Password safety and best practices
  - Scam detection and prevention
  - Privacy protection
  - Phishing identification
  - Malware protection
  - Encryption basics
  - Two-factor authentication
  - Data backup strategies

### 🎤 Voice Interaction
- **Text-to-Speech**: Reads responses aloud with natural voice synthesis
- **Speech Recognition**: Accepts voice input for hands-free interaction
- **Voice Commands**: Support for voice-based task creation and queries

### 📋 Task Management
- **Create Tasks**: Add security-related tasks with descriptions
- **Set Reminders**: Assign reminders to tasks
- **Track Progress**: Mark tasks as complete
- **Delete Tasks**: Remove completed or unnecessary tasks
- **Persistent Storage**: Tasks saved in SQLite database

### 🧠 Educational Quizzes
- **Multiple Question Types**: True/False and multiple-choice questions
- **Instant Feedback**: Explanations for correct and incorrect answers
- **Score Tracking**: Real-time scoring and progress monitoring
- **Comprehensive Topics**: Covers all major cybersecurity areas
- **Restart Capability**: Allows repeated practice

### 📊 Activity Logging
- **Audit Trail**: Records all user interactions and actions
- **Recent Activity**: View recent actions and responses
- **Full History**: Access complete activity log
- **Debug Support**: Logs errors and system events

## 🛠️ Technical Stack

### Frameworks & Libraries
- **.NET Framework**: net8.0-windows
- **UI Framework**: Windows Presentation Foundation (WPF)
- **Database**: SQLite with Entity Framework Core
- **Speech**: System.Speech (TTS and STT)
- **Testing**: Microsoft.Extensions.Logging.Console

### Key Dependencies
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.Proxies" Version="8.0.10" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="8.0.10" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.10" />
<PackageReference Include="Microsoft.Extensions.Logging.Console" Version="10.0.9" />
<PackageReference Include="System.Speech" Version="10.0.9" />
```

## 🏗️ Architecture

### Design Patterns
- **MVC Pattern**: Separation of concerns with XAML views, code-behind controllers, and model classes
- **Repository Pattern**: Data access abstraction through storage helpers
- **Observer Pattern**: Event-driven communication between components
- **Singleton Pattern**: Shared resources and services
- **Factory Pattern**: Dynamic object creation for responses

### Component Architecture

```
┌─────────────────────────────────────────────────────────┐
│                    MainWindow                           │
│  ┌──────────────┐  ┌──────────────┐  ┌─────────────┐  │
│  │   Chat Tab   │  │  Tasks Tab   │  │  Quiz Tab   │  │
│  └──────────────┘  └──────────────┘  └─────────────┘  │
│  ┌──────────────┐  ┌──────────────┐  ┌─────────────┐  │
│  │ ChatbotEngine│  │ TaskManager  │  │ QuizManager │  │
│  └──────────────┘  └──────────────┘  └─────────────┘  │
│  ┌──────────────┐  ┌──────────────┐  ┌─────────────┐  │
│  │SpeechSynth   │  │ ActivityLog  │  │StorageHelper│  │
│  └──────────────┘  └──────────────┘  └─────────────┘  │
└─────────────────────────────────────────────────────────┘
                           │
                    ┌──────────────┐
                    │   SQLite DB  │
                    └──────────────┘
```

### Data Flow
1. **User Input** → MainWindow captures input via keyboard or voice
2. **Processing** → ChatbotEngine processes input through NLP pipeline
3. **Response Generation** → Engine generates appropriate response
4. **UI Update** → Response displayed with sentiment analysis
5. **Speech Synthesis** → Optional voice output
6. **Logging** → ActivityLogger records the interaction
7. **Persistence** → Tasks and logs stored in SQLite

## 🚀 Installation

### Prerequisites
- Windows 10/11 operating system
- .NET 8.0 SDK or runtime
- Visual Studio 2022 or later (recommended)
- SQLite support (included)

### Build from Source

1. **Clone the repository**
```bash
git clone [repository-url]
cd Cybersecurity-Chatbot
```

2. **Restore NuGet packages**
```bash
dotnet restore
```

3. **Build the application**
```bash
dotnet build -c Release
```

4. **Run the application**
```bash
dotnet run --project AiAssistanceUI.csproj
```

### Visual Studio Setup
1. Open `AiAssistanceUI.sln` in Visual Studio
2. Restore NuGet packages (right-click solution → Restore NuGet Packages)
3. Set `AiAssistanceUI` as startup project
4. Press F5 to build and run

## 📖 Usage Guide

### Quick Start
1. Open the app and go to the **Chat** tab.
2. Ask a cybersecurity question (example: "How can I improve password safety?").
3. Add one security task in the **Tasks** tab.
4. Run one quiz round in the **Quiz** tab to check your understanding.

### Getting Started
The application opens with the Chat tab active, featuring a welcome message and quick-start tips.

### Chat Tab
- **Text Input**: Type your question in the text box at the bottom
- **Voice Input**: Click the "🎤 Voice" button and speak
- **Send**: Press Enter or click "Send ➤" to submit
- **Sentiment**: Watch the sentiment indicator for emotional context

### Common Chat Commands

#### Learning Topics
```
"Tell me about password safety"
"How to avoid scams?"
"What are privacy tips?"
"Explain phishing"
"About malware protection"
```

#### Personalization
```
"My name is [YourName]"
"I'm interested in [topic]"
"What is my name?"
```

#### Task Management
```
"Add task - [description]"
"Remind me to [description]"
```

#### Quiz & Logs
```
"Start quiz"
"Show activity log"
```

### Tasks Tab
1. **Add Task**: Fill in title, description, and optional reminder
2. **Complete Task**: Select a task and click "Mark Complete"
3. **Delete Task**: Select a task and click "Delete"
4. **View Tasks**: All tasks displayed in list with status indicators

### Quiz Tab
1. **Start Quiz**: Click "Start Quiz" button
2. **Answer Questions**: Select an option and click "Submit Answer"
3. **Get Feedback**: Read explanation for each answer
4. **Progress**: Track score and question progress
5. **Restart**: Click "Play Again" to retake quiz

## 📁 Project Structure

```
AiAssistanceUI/
├── Core/
│   ├── ChatbotEngine.cs          # Main chatbot logic
│   ├── ChatbotEngineBase.cs      # Base engine functionality
│   └── IChatbotEngine.cs         # Engine interface
│
├── Data/
│   ├── ApplicationDbContext.cs   # Entity Framework context
│   ├── TaskStorageHelper.cs      # Task data access
│   └── database.db               # SQLite database file
│
├── Models/
│   ├── TaskItem.cs              # Task entity
│   ├── LogEntry.cs              # Log entity
│   ├── UserInfo.cs              # User memory
│   ├── ConversationContext.cs   # Conversation state
│   ├── QuizManager.cs           # Quiz logic
│   └── QuizQuestions.cs         # Question model
│
├── Services/
│   ├── ActivityLogger.cs        # Logging service
│   ├── TaskManager.cs           # Task operations
│   └── UserInfo.cs              # User management
│
├── UI/
│   ├── MainWindow.xaml          # Main window UI
│   ├── MainWindow.xaml.cs       # Main window logic
│   ├── App.xaml                 # Application resources
│   ├── App.xaml.cs              # Application entry
│   ├── BooleanToBrushConverter.cs # Value converter
│   └── AssemblyInfo.cs          # Assembly metadata
│
└── AiAssistanceUI.csproj        # Project configuration
```

## 🔑 Key Components

### ChatbotEngine
The heart of the application, responsible for:
- **Input Processing**: Parses and understands user messages
- **Intent Recognition**: Identifies user intent (learn, task, quiz, etc.)
- **Response Generation**: Creates appropriate responses using keyword matching and NLP
- **Memory Management**: Stores and recalls user information
- **Sentiment Analysis**: Detects emotional tone of user input
- **Context Tracking**: Maintains conversation flow

### ActivityLogger
Provides comprehensive logging functionality:
- **Log Storage**: Saves activities to SQLite database
- **Retrieval Methods**: Get recent or complete logs
- **Error Handling**: Graceful failure with debug output
- **Performance**: Efficient querying with pagination

### QuizManager
Educational assessment engine:
- **Question Management**: 10+ cybersecurity questions
- **Score Tracking**: Real-time performance metrics
- **Answer Validation**: Checks correctness with explanations
- **Progress Tracking**: Tracks completion state
- **Restart Capability**: Resets quiz for repeated attempts

### TaskManager
Task management system:
- **CRUD Operations**: Create, read, update, delete tasks
- **Status Tracking**: Mark tasks complete/incomplete
- **Reminder Support**: Store reminder information
- **Data Persistence**: SQLite-backed storage

### Speech Integration
- **Text-to-Speech**: Natural voice responses using System.Speech
- **Speech Recognition**: Voice command input with dictation grammar
- **Async Operations**: Non-blocking speech synthesis and recognition
- **Error Resilience**: Graceful fallback when speech services unavailable

## 💾 Database Schema

### TaskItem Table
| Column | Type | Description |
|--------|------|-------------|
| Id | INTEGER | Primary key |
| Title | TEXT | Task title |
| Description | TEXT | Detailed description |
| Reminder | TEXT | Reminder information |
| IsComplete | BOOLEAN | Completion status |
| CreatedAt | TEXT | Creation timestamp |

### LogEntry Table
| Column | Type | Description |
|--------|------|-------------|
| Id | INTEGER | Primary key |
| Description | TEXT | Log message |
| CreatedAt | TEXT | Log timestamp |

## 🚀 Future Enhancements

### Planned Features
- [ ] **Cloud Synchronization**: Sync tasks and data across devices
- [ ] **Advanced NLP**: Integration with OpenAI API for enhanced responses
- [ ] **Multi-language Support**: Localization for international users
- [ ] **Dark Mode**: Theme toggle for better UX
- [ ] **Export Functions**: Export logs and quiz results
- [ ] **Progress Analytics**: Visual charts of learning progress
- [ ] **Custom Quiz Creation**: Allow users to create their own quiz
- [ ] **Mobile Companion**: Cross-platform mobile application
- [ ] **Security News Feed**: Real-time cybersecurity news integration
- [ ] **Gamification**: Achievement badges and learning streaks

### Technical Improvements
- [ ] **Unit Testing**: Comprehensive test coverage
- [ ] **Dependency Injection**: Cleaner architecture
- [ ] **Async/Await**: Improved async operations
- [ ] **Caching**: Performance optimization
- [ ] **API Integration**: RESTful endpoints for extensibility

## 🤝 Contributing

We welcome contributions! Please follow these guidelines:

1. **Fork the repository**
2. **Create a feature branch**
   ```bash
   git checkout -b feature/amazing-feature
   ```
3. **Make your changes**
4. **Test thoroughly**
5. **Commit with descriptive messages**
   ```bash
   git commit -m "Add some amazing feature"
   ```
6. **Push to your branch**
   ```bash
   git push origin feature/amazing-feature
   ```
7. **Create a Pull Request**

### Coding Standards
- Follow C# coding conventions
- Use meaningful variable and method names
- Add XML documentation for public members
- Write clean, maintainable code
- Include error handling and logging

## 📝 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🙏 Acknowledgments

- **System.Speech**: For speech synthesis and recognition capabilities
- **Entity Framework Core**: For database operations
- **WPF Community**: For UI framework excellence
- **Cybersecurity Community**: For educational content and best practices

**Built with ❤️ for cybersecurity awareness**
