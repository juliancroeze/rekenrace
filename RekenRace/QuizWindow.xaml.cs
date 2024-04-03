using System;
using System.ComponentModel;
using System.Windows;

namespace MathQuiz
{
    public partial class QuizWindow : Window, INotifyPropertyChanged
    {
        private string playerName;
        private string difficulty;
        private int currentQuestion = 0;
        private int correctAnswers = 0;
        private Random random = new Random();
        private string[] operators = new string[] { "+", "-", "*", "/" };
        private string[] difficultyLevels = new string[] { "Eenvoudig", "Gemiddeld", "Moeilijk" };
        private string[] questions;
        private string[] answers;
        private string[] userAnswers;
        private bool isCorrect;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Question
        {
            get { return questions[currentQuestion]; }
        }

        private string result;
        public string Result
        {
            get { return result; }
            set
            {
                result = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Result"));
            }
        }

        public QuizWindow(string playerName, string difficulty)
        {
            InitializeComponent();
            this.playerName = playerName;
            this.difficulty = difficulty;
            GenerateQuestions();
            DataContext = this;
        }

        private void GenerateQuestions()
        {
            int maxOperand = 10;
            int maxResult = 20;

            switch (difficulty)
            {
                case "Eenvoudig":
                    maxOperand = 10;
                    maxResult = 20;
                    break;
                case "Gemiddeld":
                    maxOperand = 20;
                    maxResult = 50;
                    break;
                case "Moeilijk":
                    maxOperand = 50;
                    maxResult = 100;
                    break;
            }

            questions = new string[10];
            answers = new string[10];
            userAnswers = new string[10];

            for (int i = 0; i < 10; i++)
            {
                double num1 = random.Next(1, maxOperand + 1);
                double num2 = random.Next(1, maxOperand + 1);

                string op = operators[random.Next(0, operators.Length)];
                string question = $"{num1} {op} {num2} = ?";

                double result = 0;

                switch (op)
                {
                    case "+":
                        result = num1 + num2;
                        break;
                    case "-":
                        result = num1 - num2;
                        break;
                    case "*":
                        result = num1 * num2;
                        break;
                    case "/":
                        double num3 = num1 * num2;
                        question = $"{num3} {op} {num2} = ?";
                        result = num3 / num2;
                        break;
                }
                questions[i] = question;

                answers[i] = result.ToString();
            }
        }


        private void Answer_Click(object sender, RoutedEventArgs e)
        {
            if (currentQuestion < 10)
            {
                userAnswers[currentQuestion] = txtAnswer.Text;

                if (userAnswers[currentQuestion] == answers[currentQuestion])
                {
                    isCorrect = true;
                    correctAnswers++;
                    Result = "Goed!";
                }
                else
                {
                    isCorrect = false;
                    Result = "Fout!";
                }
                MessageBox.Show(answers[currentQuestion]);

                currentQuestion++;

                if (currentQuestion < 10)
                {
                    txtAnswer.Text = "";
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Question"));
                }
                else
                {
                    MessageBox.Show($"Je hebt {correctAnswers} van de {currentQuestion} vragen goed beantwoord.");
                    QuestionAnswered?.Invoke(isCorrect);
                    Close();
                }
            }
        }

        public delegate void QuestionAnsweredEventHandler(bool isCorrect);
        public event QuestionAnsweredEventHandler QuestionAnswered;
    }
}
