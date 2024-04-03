using System;
using System.Windows;
using System.Windows.Controls;

namespace MathQuiz
{
    public partial class MainWindow : Window
    {
        private string playerName;
        private int totalQuestions = 10;
        private int currentQuestion = 0;
        private int correctAnswers = 0;
        private Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            playerName = txtName.Text;
            if (string.IsNullOrWhiteSpace(playerName))
            {
                MessageBox.Show("Voer je naam in.");
                return;
            }

            string difficulty = (cmbDifficulty.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (string.IsNullOrWhiteSpace(difficulty))
            {
                MessageBox.Show("Selecteer een moeilijkheidsgraad.");
                return;
            }

            StartQuiz(difficulty);
        }

        private void StartQuiz(string difficulty)
        {
            QuizWindow quizWindow = new QuizWindow(playerName, difficulty);
            quizWindow.QuestionAnswered += QuizWindow_QuestionAnswered;
            quizWindow.ShowDialog();
        }

        private void QuizWindow_QuestionAnswered(bool isCorrect)
        {
            currentQuestion++;

            if (isCorrect)
                correctAnswers++;

            if (currentQuestion == totalQuestions)
            {
                MessageBox.Show($"Je hebt {correctAnswers} van de {totalQuestions} vragen goed beantwoord.");
                currentQuestion = 0;
                correctAnswers = 0;
            }
        }
    }
}
