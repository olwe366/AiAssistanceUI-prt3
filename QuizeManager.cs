using AiAssistanceUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAssistanceUI
{
    public class QuizManager
    {
        private List<QuizQuestions> _questions;
        private int _currentIndex = 0;
        private int _score = 0;

        public int Score => _score;
        public int TotalQuestions => _questions.Count;

        public QuizManager()
        {
            _questions = new List<QuizQuestions>
        {
            new QuizQuestions
            {
                Question = "What should you do if you receive an email asking for your password?",
                Options = new List<string> { "Reply with your password", "Delete the email", "Report the email as phishing", "Ignore it" },
                CorrectAnswer = "C",
                Explanation = "Reporting phishing emails helps prevent scams and protects others from the same attack.",
                IsTrueFalse = false
            },
            new QuizQuestions
            {
                Question = "True or False: Using the same strong password across multiple accounts is safe.",
                CorrectAnswer = "False",
                Explanation = "Even a strong password becomes risky if reused — one breached site exposes every account using it.",
                IsTrueFalse = true
            },
            new QuizQuestions
            {
                Question = "Which is the clearest sign a website connection is secure?",
                Options = new List<string> { "A padlock icon and 'https://' in the address bar", "A flashy 'Verified Safe' banner", "A popup saying the site is secure", "The site loads quickly" },
                CorrectAnswer = "A",
                Explanation = "HTTPS encrypts data between your browser and the site; the padlock confirms it's active.",
                IsTrueFalse = false
            },
            new QuizQuestions
            {
                Question = "True or False: A caller claiming to be from your bank's IT department asking you to confirm your PIN is a normal security check.",
                CorrectAnswer = "False",
                Explanation = "Legitimate banks never ask for your PIN or password over the phone — this is social engineering.",
                IsTrueFalse = true
            },
            new QuizQuestions
            {
                Question = "What is the best practice for creating a strong password?",
                Options = new List<string> { "Use your birthdate", "Use a mix of letters, numbers, and symbols", "Use 'password123'", "Use your pet's name" },
                CorrectAnswer = "B",
                Explanation = "A strong password should be long, unique, and include a mix of character types.",
                IsTrueFalse = false
            },
            new QuizQuestions
            {
                Question = "True or False: Two-factor authentication (2FA) adds an extra layer of security to your accounts.",
                CorrectAnswer = "True",
                Explanation = "2FA requires a second form of verification, making it harder for attackers to gain access.",
                IsTrueFalse = true
            },
            new QuizQuestions
            {
                Question = "What is the primary purpose of regular data backups?",
                Options = new List<string> { "To free up space on your device", "To protect against data loss from hardware failure or malware", "To speed up your computer", "To share files with friends" },
                CorrectAnswer = "B",
                Explanation = "Regular backups ensure you can recover your data in case of accidental deletion, hardware failure, or malware attacks.",
                IsTrueFalse = false
            },
            new QuizQuestions
            {
                Question = "True or False: Adjusting your privacy settings on social media can help protect your personal information.",
                CorrectAnswer = "True",
                Explanation = "Reviewing and adjusting privacy settings limits who can see your information and reduces exposure to potential threats.",
                IsTrueFalse = true
            }, 
            new QuizQuestions
            {
                Question = "What is the primary purpose of antivirus software?",
                Options = new List<string> { "To speed up your computer", "To protect against malware and viruses", "To manage your passwords", "To backup your data" },
                CorrectAnswer = "B",
                Explanation = "Antivirus software helps detect, prevent, and remove malicious software, protecting your system from potential threats.",
                IsTrueFalse = false
            },
            new QuizQuestions
            {
                Question = "True or False: Ransomware is a type of malware that encrypts your files and demands payment for their release.",
                CorrectAnswer = "True",
                Explanation = "Ransomware can lock you out of your own data, and paying the ransom does not guarantee file recovery.",
                IsTrueFalse = true
            },
           
        };
        }

        public QuizQuestions GetCurrentQuestion()
        {
            return _questions[_currentIndex];
        }

        public bool IsFinished()
        {
            return _currentIndex >= _questions.Count;
        }

        // Your turn:

        public bool SubmitAnswer(string answer)
        {
            bool isCorrect = string.Equals(answer, _questions[_currentIndex].CorrectAnswer, StringComparison.OrdinalIgnoreCase);
            if (isCorrect) _score++;
            _currentIndex++;
            return isCorrect;
        }

        public string GetFeedback(bool correct)
        {
            
            if (_currentIndex == 0 || _currentIndex > _questions.Count)
            {
                return string.Empty;
            }
            var lastQuestion = _questions[_currentIndex - 1];
            return (correct ? "Correct! " : "Incorrect. ") + lastQuestion.Explanation;
        }

        public string GetFinalScore()
        {
            return $"{_score} / {_questions.Count}";
        }

        public string GetFinalMessage()
        {
            return _score >= _questions.Count * 0.7 ? "Great job!" : "Keep learning — try again!";
        }

        public void ResetQuiz()
        {
            _currentIndex = 0;
            _score = 0;
        }
    }
}

