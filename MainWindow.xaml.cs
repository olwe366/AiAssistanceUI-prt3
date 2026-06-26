using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading.Tasks;

namespace AiAssistanceUI
{
    public partial class MainWindow : Window
    {
        private ChatbotEngine chatbot;
        private SpeechSynthesizer speechSynthesizer;
        private SpeechRecognitionEngine speechRecognizer;
        private bool isListening = false;
        private bool isSpeaking = false;
        private TaskManager taskManager;
        private QuizManager quizManager;
        private bool showFullLog = false;
        private List<string> currentLogEntries = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            InitializeChatbot();
            InitializeSpeech();
            InitializeTaskManager();
            InitializeQuiz();
            ShowWelcomeMessage();
        }

        private void InitializeChatbot()
        {
            chatbot = new ChatbotEngine();
            chatbot.OnMemoryUpdate += UpdateMemoryStatus;
        }

        private void InitializeSpeech()
        {
            // Initialize text-to-speech
            try
            {
                speechSynthesizer = new SpeechSynthesizer();
                speechSynthesizer.SetOutputToDefaultAudioDevice();
                speechSynthesizer.Rate = 0;
                speechSynthesizer.Volume = 100;
                speechSynthesizer.SpeakStarted += (s, e) => isSpeaking = true;
                speechSynthesizer.SpeakCompleted += (s, e) => isSpeaking = false;
            }
            catch (Exception ex)
            {
                AddBotMessage("⚠️ Text-to-speech is not available on this system.", false);
            }

            // Initialize speech recognition
            try
            {
                speechRecognizer = new SpeechRecognitionEngine();
                speechRecognizer.LoadGrammar(new DictationGrammar());
                speechRecognizer.SetInputToDefaultAudioDevice();
                speechRecognizer.SpeechRecognized += SpeechRecognizer_SpeechRecognized;
            }
            catch (Exception ex)
            {
                AddBotMessage("⚠️ Voice recognition is not available on this system.", false);
            }
        }

        private void InitializeTaskManager()
        {
            taskManager = new TaskManager();
            RefreshTaskList();
        }

        private void InitializeQuiz()
        {
            quizManager = new QuizManager();
            QuizStartPanel.Visibility = Visibility.Visible;
            QuizQuestionPanel.Visibility = Visibility.Collapsed;
            QuizResultsPanel.Visibility = Visibility.Collapsed;
        }

        private void RefreshTaskList()
        {
            try
            {
                var tasks = taskManager.GetAllTasks();
                TaskListBox.ItemsSource = tasks;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tasks: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowWelcomeMessage()
        {
            string welcomeMessage = @"🛡️ Welcome to Cyber Security Awareness Bot!

I'm your personal cybersecurity guardian, here to help you stay safe online.

💡 I can help you with:
• 🔐 Password Safety
• ⚠️ Scam Detection  
• 🔒 Privacy Protection
• 🎣 Phishing Prevention
• 🦠 Malware Protection

📝 Try asking:
• 'Tell me about password safety'
• 'How to avoid scams?'
• 'What are privacy tips?'
• 'My name is [your name]' (I'll remember it!)
• 'Add task - [description]' (Add a task)
• 'Remind me to [description]' (Set a reminder)
• 'Start quiz' (Test your knowledge)
• 'Show activity log' (See recent actions)

🤖 How can I help you stay safe today?";

            AddBotMessage(welcomeMessage, true);

            // Voice greeting with delay
            Task.Delay(500).ContinueWith(_ =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (speechSynthesizer != null)
                    {
                        speechSynthesizer.SpeakAsync("Welcome to Cyber Security Awareness Bot. Your personal cybersecurity guardian. How can I help you stay safe today?");
                    }
                });
            });
        }

        private void ProcessUserInput()
        {
            if (UserInputTextBox is TextBox userInputTextBox)
            {
                string userInput = userInputTextBox.Text.Trim();
                if (string.IsNullOrEmpty(userInput))
                    return;

                // Add user message to chat
                AddUserMessage(userInput);

                // Check for "show more" command for logs
                if (userInput.ToLower().Contains("show more") && showFullLog)
                {
                    ShowFullLog();
                    userInputTextBox.Clear();
                    ScrollToBottom();
                    return;
                }

                // Process with chatbot
                ChatbotResponse response = chatbot.ProcessUserInput(userInput);

                // Update sentiment indicator
                UpdateSentimentIndicator(response.Sentiment);

                // Add bot response
                AddBotMessage(response.Message, true);

                // Speak response if not already speaking and response is appropriate
                if (response.ShouldSpeak && !isSpeaking && speechSynthesizer != null)
                {
                    string cleanMessage = System.Text.RegularExpressions.Regex.Replace(response.Message, @"[^\w\s\.\,\!\?\-]", "");
                    speechSynthesizer.SpeakAsync(cleanMessage);
                }

                // Clear input
                userInputTextBox.Clear();

                // Refresh task list if a task was added
                if (userInput.ToLower().Contains("add task") || userInput.ToLower().Contains("remind me"))
                {
                    RefreshTaskList();
                }

                // Auto-scroll to bottom
                ScrollToBottom();
            }
        }

        private void ShowFullLog()
        {
            var fullLog = new ActivityLogger().GetFullLog();
            if (fullLog.Count > 0)
            {
                string result = "📋 Complete Activity Log:\n\n";
                for (int i = 0; i < fullLog.Count; i++)
                {
                    result += $"  {i + 1}. {fullLog[i]}\n";
                }
                AddBotMessage(result, true);
                showFullLog = false;
            }
        }

        // UI Event Handlers

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessUserInput();
        }

        private void UserInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
            {
                e.Handled = true;
                ProcessUserInput();
            }
        }

        private void VoiceInputButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isListening)
            {
                StartVoiceRecognition();
            }
            else
            {
                StopVoiceRecognition();
            }
        }

        private void StartVoiceRecognition()
        {
            try
            {
                if (speechRecognizer != null)
                {
                    speechRecognizer.RecognizeAsync(RecognizeMode.Multiple);
                    isListening = true;
                    VoiceInputButton.Content = "🔴 Stop";
                    VoiceInputButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D13858"));
                    AddBotMessage("🎤 Listening... Please speak your question clearly.", false);
                }
            }
            catch (Exception ex)
            {
                AddBotMessage("❌ Voice recognition error: " + ex.Message, false);
            }
        }

        private void StopVoiceRecognition()
        {
            if (speechRecognizer != null)
            {
                speechRecognizer.RecognizeAsyncStop();
                isListening = false;
                VoiceInputButton.Content = "🎤 Voice";
                VoiceInputButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E74C3C"));
            }
        }

        private void SpeechRecognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string recognizedText = e.Result.Text;
            Dispatcher.Invoke(() =>
            {
                if (UserInputTextBox is TextBox userInputTextBox)
                {
                    userInputTextBox.Text = recognizedText;
                }
                StopVoiceRecognition();
                ProcessUserInput();
            });
        }

        // Task Management Event Handlers

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string title = TaskTitleTextBox.Text.Trim();
                string description = TaskDescriptionTextBox.Text.Trim();
                string reminder = TaskReminderTextBox.Text.Trim();

                if (string.IsNullOrEmpty(title))
                {
                    MessageBox.Show("Please enter a task title.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                taskManager.AddTask(title, description, reminder);
                RefreshTaskList();

                // Clear input fields
                TaskTitleTextBox.Clear();
                TaskDescriptionTextBox.Clear();
                TaskReminderTextBox.Clear();

                AddBotMessage($"✅ Task '{title}' added successfully!", false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding task: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MarkCompleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TaskListBox.SelectedItem is TaskItem selectedTask)
                {
                    taskManager.MarkAsComplete(selectedTask.Id);
                    RefreshTaskList();
                    AddBotMessage($"✅ Task '{selectedTask.Title}' marked as complete!", false);
                }
                else
                {
                    MessageBox.Show("Please select a task to mark as complete.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error marking task as complete: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TaskListBox.SelectedItem is TaskItem selectedTask)
                {
                    var result = MessageBox.Show($"Are you sure you want to delete task '{selectedTask.Title}'?",
                        "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        taskManager.DeleteTask(selectedTask.Id);
                        RefreshTaskList();
                        AddBotMessage($"🗑️ Task '{selectedTask.Title}' deleted.", false);
                    }
                }
                else
                {
                    MessageBox.Show("Please select a task to delete.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting task: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Quiz Event Handlers

        private void StartQuizButton_Click(object sender, RoutedEventArgs e)
        {
            quizManager.ResetQuiz();
            QuizStartPanel.Visibility = Visibility.Collapsed;
            QuizQuestionPanel.Visibility = Visibility.Visible;
            QuizResultsPanel.Visibility = Visibility.Collapsed;
            DisplayCurrentQuestion();
            new ActivityLogger().Log("Quiz started");
        }

        private void DisplayCurrentQuestion()
        {
            var question = quizManager.GetCurrentQuestion();

            QuizScoreText.Text = $"Score: {quizManager.Score} / {quizManager.TotalQuestions}";
            QuizQuestionText.Text = question.Question;
            QuizFeedbackText.Text = "";
            NextQuestionButton.Visibility = Visibility.Collapsed;

            OptionARadio.IsChecked = false;
            OptionBRadio.IsChecked = false;
            OptionCRadio.IsChecked = false;
            OptionDRadio.IsChecked = false;

            if (question.IsTrueFalse)
            {
                OptionARadio.Content = "True";
                OptionARadio.Tag = "True";
                OptionBRadio.Content = "False";
                OptionBRadio.Tag = "False";
                OptionCRadio.Visibility = Visibility.Collapsed;
                OptionDRadio.Visibility = Visibility.Collapsed;
            }
            else
            {
                OptionARadio.Content = question.Options[0];
                OptionARadio.Tag = "A";
                OptionBRadio.Content = question.Options[1];
                OptionBRadio.Tag = "B";
                OptionCRadio.Content = question.Options[2];
                OptionCRadio.Tag = "C";
                OptionDRadio.Content = question.Options[3];
                OptionDRadio.Tag = "D";
                OptionCRadio.Visibility = Visibility.Visible;
                OptionDRadio.Visibility = Visibility.Visible;
            }

            OptionARadio.IsEnabled = true;
            OptionBRadio.IsEnabled = true;
            OptionCRadio.IsEnabled = true;
            OptionDRadio.IsEnabled = true;
        }

        private string GetSelectedAnswer()
        {
            if (OptionARadio.IsChecked == true) return OptionARadio.Tag.ToString();
            if (OptionBRadio.IsChecked == true) return OptionBRadio.Tag.ToString();
            if (OptionCRadio.IsChecked == true) return OptionCRadio.Tag.ToString();
            if (OptionDRadio.IsChecked == true) return OptionDRadio.Tag.ToString();
            return null;
        }

        private void SubmitAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            string answer = GetSelectedAnswer();
            if (answer == null)
            {
                QuizFeedbackText.Text = "Please select an answer first.";
                return;
            }

            bool correct = quizManager.SubmitAnswer(answer);
            string feedback = quizManager.GetFeedback(correct);
            QuizFeedbackText.Text = (correct ? "✅ " : "❌ ") + feedback;
            QuizScoreText.Text = $"Score: {quizManager.Score} / {quizManager.TotalQuestions}";

            OptionARadio.IsEnabled = false;
            OptionBRadio.IsEnabled = false;
            OptionCRadio.IsEnabled = false;
            OptionDRadio.IsEnabled = false;

            NextQuestionButton.Visibility = Visibility.Visible;
        }

        private void NextQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            if (quizManager.IsFinished())
            {
                QuizQuestionPanel.Visibility = Visibility.Collapsed;
                QuizResultsPanel.Visibility = Visibility.Visible;
                QuizFinalScoreText.Text = quizManager.GetFinalScore();
                QuizFinalMessageText.Text = quizManager.GetFinalMessage();
                new ActivityLogger().Log($"Quiz completed - score: {quizManager.Score} out of {quizManager.TotalQuestions}");
            }
            else
            {
                DisplayCurrentQuestion();
            }
        }

        private void RestartQuizButton_Click(object sender, RoutedEventArgs e)
        {
            quizManager.ResetQuiz();
            QuizStartPanel.Visibility = Visibility.Collapsed;
            QuizQuestionPanel.Visibility = Visibility.Visible;
            QuizResultsPanel.Visibility = Visibility.Collapsed;
            DisplayCurrentQuestion();
        }

        // UI Helper Methods

        private void AddUserMessage(string message)
        {
            Border messageBorder = new Border
            {
                Style = (Style)FindResource("ChatBubbleUser"),
                Margin = new Thickness(10, 5, 50, 5)
            };

            StackPanel contentStack = new StackPanel();

            TextBlock avatar = new TextBlock
            {
                Text = "👤 You:",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CAF0F8")),
                Margin = new Thickness(0, 0, 0, 5),
                FontSize = 11
            };
            contentStack.Children.Add(avatar);

            TextBlock messageText = new TextBlock
            {
                Text = message,
                Style = (Style)FindResource("MessageTextUser"),
                TextWrapping = TextWrapping.Wrap
            };

            contentStack.Children.Add(messageText);
            messageBorder.Child = contentStack;
            if (ChatMessagesPanel is Panel panel)
            {
                panel.Children.Add(messageBorder);
            }
        }

        private void AddBotMessage(string message, bool withAvatar = true)
        {
            Border messageBorder = new Border
            {
                Style = (Style)FindResource("ChatBubbleBot"),
                Margin = new Thickness(50, 5, 10, 5)
            };

            StackPanel contentStack = new StackPanel();

            if (withAvatar)
            {
                TextBlock avatar = new TextBlock
                {
                    Text = "🤖 Guardian:",
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00B4D8")),
                    Margin = new Thickness(0, 0, 0, 5),
                    FontSize = 11
                };
                contentStack.Children.Add(avatar);
            }

            TextBlock messageText = new TextBlock
            {
                Text = message,
                Style = (Style)FindResource("MessageTextBot"),
                TextWrapping = TextWrapping.Wrap
            };

            contentStack.Children.Add(messageText);
            messageBorder.Child = contentStack;
            if (ChatMessagesPanel is Panel panel)
            {
                panel.Children.Add(messageBorder);
            }
        }

        private void UpdateSentimentIndicator(string sentiment)
        {
            if (SentimentIndicator is TextBlock sentimentTextBlock)
            {
                string sentimentEmoji = sentiment switch
                {
                    "worried" => "😟",
                    "frustrated" => "😤",
                    "curious" => "🤔",
                    _ => "😊"
                };

                sentimentTextBlock.Text = $"{sentimentEmoji} {sentiment}";

                switch (sentiment.ToLower())
                {
                    case "worried":
                        sentimentTextBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F4A261"));
                        break;
                    case "frustrated":
                        sentimentTextBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E94560"));
                        break;
                    case "curious":
                        sentimentTextBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00B4D8"));
                        break;
                    default:
                        sentimentTextBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4CAF50"));
                        break;
                }
            }
        }

        private void UpdateMemoryStatus(string info)
        {
            Dispatcher.Invoke(() =>
            {
                AddBotMessage($"🧠 Memory: {info}", false);
            });
        }

        private void ScrollToBottom()
        {
            if (ChatScrollViewer is ScrollViewer scrollViewer)
            {
                scrollViewer.ScrollToBottom();
            }
        }

        private void UserInputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (UserInputTextBox is TextBox userInputTextBox)
            {
                if (userInputTextBox.Text.Length > 100)
                {
                    userInputTextBox.Height = 60;
                }
                else
                {
                    userInputTextBox.Height = 45;
                }
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            if (speechSynthesizer != null)
            {
                speechSynthesizer.Dispose();
            }
            if (speechRecognizer != null)
            {
                speechRecognizer.Dispose();
            }
            base.OnClosed(e);
        }
    }
}