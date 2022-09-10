using System;
using System.Windows;
using System.Windows.Controls;

namespace MentalHealthAnalysis.Test
{
    /// <summary>
    /// Interaction logic for TestUserControl.xaml
    /// </summary>
    public partial class TestUserControl : UserControl
    {
        public Action<Answer> Confirm;
        public Action<Question> Return;
        public Action<Answer> GetResult;

        private readonly Question _currentQuestion;

        public TestUserControl(Question question, StatusQuestion statusQuestion)
        {
            InitializeComponent();

            _currentQuestion = question;

            if (statusQuestion == StatusQuestion.Result)
            {
                ConfirmButton.Visibility = Visibility.Collapsed;
                ResultButton.Visibility = Visibility.Visible;
            }

            DataContext = question;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            Answer selectedAnswer = (Answer)AnswersListView.SelectedItem;

            if (selectedAnswer != null)
            {
                Confirm(selectedAnswer);
            }
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            Return(_currentQuestion);
        }

        private void GetResultButton_Click(object sender, RoutedEventArgs e)
        {
            Answer selectedAnswer = (Answer)AnswersListView.SelectedItem;

            if (selectedAnswer != null)
            {
                GetResult(selectedAnswer);
            }
        }
    }
}