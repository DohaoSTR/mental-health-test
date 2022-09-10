using MentalHealthAnalysis.AnalyticsView;
using MentalHealthAnalysis.DatasetView;
using MentalHealthAnalysis.Test;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MentalHealthAnalysis
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<Question> _viewQuestions;
        private readonly List<Answer> _userAnswers;

        private readonly List<Question> _datasetQuestions;

        private int _indexTest;

        private TestUserControl _currentTestControl;
        private DatasetControl _datasetControl;
        private ResultControl _resultControl;
        private AnalyticsControl _analyticsControl;

        private const int ColumnsCount = 36;
        private const int RowsCount = 1433;

        private readonly Dataset _dataset;

        public MainWindow()
        {
            InitializeComponent();

            _dataset = new Dataset("datasetStringValues.xlsx", new DatasetSettings(RowsCount, ColumnsCount));
            _datasetQuestions = _dataset.Questions;

            List<Answer> twoStandardAnswers = new List<Answer>
            {
                new Answer("Да"),
                new Answer("Нет")
            };

            List<Answer> threeStandardAnswers = new List<Answer>
            {
                new Answer("Да"),
                new Answer("Нет"),
                new Answer("Не знаю")
            };

            List<Answer> vacationAnswers = new List<Answer>
            {
                new Answer("Очень легко"),
                new Answer("Легко"),
                new Answer("Немного трудно"),
                new Answer("Трудно"),
                new Answer("Ни легко, ни трудно"),
                new Answer("Не знаю")
            };

            List<Question> questions = new List<Question>()
            {
                new Question("1. Предоставляется ли вам пособие по охране психического здоровья в рамках медицинского страхования?", threeStandardAnswers),
                new Question("2. Знаете ли вы, какие варианты психиатрической помощи доступны в рамках страхового покрытия?", threeStandardAnswers),
                new Question("3. Обсуждали ли вы когда-либо вопросы психического здоровья (с семьей, друзьями и т.д.)?", threeStandardAnswers),
                new Question("4. Предлагали ли вам (коллеги, семья и т.д.) ресурсы для получения дополнительной информации о проблемах психического здоровья и вариантах обращения за помощью?", threeStandardAnswers),
                new Question("5. Защищена ли ваша анонимность, если вы решите воспользоваться ресурсами для лечения психического здоровья или злоупотребления психоактивными веществами?", threeStandardAnswers),
                new Question("6. Если проблемы с психическим здоровьем побудили вас запросить отпуск по болезни с работы (учебы или любой другой деятельности), насколько сложно будет его получить?", vacationAnswers),
                new Question("7. Считаете ли вы, что обсуждение вашего психического здоровья (с специалистом, семьей, друзьями и т.д.) будет иметь негативные последствия?", threeStandardAnswers),
                new Question("8. Считаете ли вы, что обсуждение вашего физического здоровья (с специалистом, семьей, друзьями и т.д.) будет иметь негативные последствия?", threeStandardAnswers),
                new Question("9. Чувствуете ли вы себя комфортно, обсуждая свое психическое здоровье (с специалистом, друзьями, коллегами)?", threeStandardAnswers),
                new Question("10. Чувствуете ли вы себя комфортно, обсуждая свое психическое здоровье с семьей?", threeStandardAnswers),
                new Question("11. Считаете ли вы, что ваше окружение относится к вашему психическому здоровью так же серьезно, как и к физическому?", threeStandardAnswers),
                new Question("12. Наблюдаете ли вы негативные последствия для людей, которые открыто говорят о проблемах психического здоровья?", twoStandardAnswers),

                new Question("13. Предоставляли ли вам когда нибудь (работодатели и т.д.) пособия по охране психического здоровья?", threeStandardAnswers),
                new Question("14. Были ли вы осведомлены ранее, о возможностях психиатрической помощи?", twoStandardAnswers),
                new Question("15. Обсуждали вы свое психическое здоровье с специалистом?", threeStandardAnswers),
                new Question("16. Получали ли вы когда нибудь ресурсы, чтобы узнать больше о проблемах психического здоровья и о том, как обратиться за помощью?", twoStandardAnswers),
                new Question("17. Была бы защищена ваша анонимность, если вы бы раньше решили воспользоваться услугами по охране психического здоровья или лечению наркомании?", threeStandardAnswers),
                new Question("18. Считаете ли вы, что обсуждение вашего психического здоровья могло иметь для вас негативные последствия?", threeStandardAnswers),
                new Question("19. Считаете ли вы, что обсуждение вашего физического здоровья могло иметь для вас негативные последствия?", twoStandardAnswers),
                new Question("20. Хотели бы вы сейчас обсудить свое психическое здоровья (с семьей, друзьями и т.д.)?", twoStandardAnswers),

                new Question("21. Хотели бы вы сейчас обсудить свое психическое здоровье с специалистом?", threeStandardAnswers),
                new Question("22. Чувствовали ли вы, что раньше ваше окружение относилось к вашему психическому здоровью так же серьезно, как и к физическому?", threeStandardAnswers),
                new Question("23. Слышали ли вы ранее или наблюдали негативные последствия для людей с проблемами психического здоровья?", twoStandardAnswers),
                new Question("24. Хотели бы вы в будущем иметь возможность обсуждать проблемы своего физического здоровья?", threeStandardAnswers),
                new Question("25. Считаете ли вы, что идентификация вас как человека с проблемами психического здоровья повредит вашей жизнедеятельности (карьере, учебе, отношениям и т.д.)?", threeStandardAnswers),
                new Question("26. Считаете ли вы, что окружающие вас люди отнеслись бы к вам более негативно, если бы знали, что вы страдаете от проблем с психическим здоровьем?", threeStandardAnswers),
                new Question("27. Вы бы охотно поделились с друзьями и семьей тем, что у вас психическое заболевание?", threeStandardAnswers),
                new Question("28. Наблюдали ли вы или сталкивались с неподдерживающей или плохо обработанной реакцией на проблему психического здоровья ранее или сейчас?", threeStandardAnswers),
                new Question("29. Наблюдали ли вы, как другой человек, обсуждавший психическое расстройство, демотивировал вас на выявление проблемы с вашим психическим здоровьем?", threeStandardAnswers),

                new Question("30. Есть ли у вас в семейной истории психические заболевания?", threeStandardAnswers),
                new Question("31. Были ли у вас в прошлом психические заболевания?", twoStandardAnswers),
                new Question("32. Обращались ли вы когда-нибудь за лечением по поводу проблем с психическим здоровьем к специалисту в области психического здоровья?", twoStandardAnswers),
                new Question("33. Проблемы с психическим здоровьем, могут помешать вашей жизнедеятельности (работе, учебе и т.д.) при эффективном лечении?", twoStandardAnswers),
                new Question("34. Проблемы с психическим здоровьем, могут помешать вашей жизнедеятельности (работе, учебе и т.д.) при не эффективном лечении?", twoStandardAnswers),
                new Question("35. Есть ли у вас в настоящее время психическое расстройство?", twoStandardAnswers)
            };

            _viewQuestions = questions;
            _userAnswers = new List<Answer>();

            SetTestControl();
        }

        private void Control_Confirm(Answer selectedAnswer)
        {
            _indexTest += 1;
            _currentTestControl.Confirm -= Control_Confirm;
            _currentTestControl.Return -= Control_Return;
            _userAnswers[_indexTest - 1] = selectedAnswer;

            if (_indexTest == _viewQuestions.Count - 1)
            {
                _currentTestControl = new TestUserControl(_viewQuestions[_indexTest], StatusQuestion.Result);
                _currentTestControl.Confirm += Control_Confirm;
                _currentTestControl.Return += Control_Return;
                _currentTestControl.GetResult += Control_GetResult;
            }
            else
            {
                _currentTestControl = new TestUserControl(_viewQuestions[_indexTest], StatusQuestion.Standard);
                _currentTestControl.Confirm += Control_Confirm;
                _currentTestControl.Return += Control_Return;
            }

            WorkSpaceGrid.Children.Clear();
            WorkSpaceGrid.Children.Add(_currentTestControl);
        }

        private void Control_GetResult(Answer selectedAnswer)
        {
            _indexTest += 1;
            _userAnswers[_indexTest - 1] = selectedAnswer;

            GetResultControl();
        }

        private void Control_Return(Question currentQuestion)
        {
            int index = _viewQuestions.FindIndex(q => q == currentQuestion);

            if (index == 0)
            {
                return;
            }

            _indexTest -= 1;
            _currentTestControl.Confirm -= Control_Confirm;
            _currentTestControl.Return -= Control_Return;

            _currentTestControl = new TestUserControl(_viewQuestions[_indexTest], StatusQuestion.Standard);
            _currentTestControl.Confirm += Control_Confirm;
            _currentTestControl.Return += Control_Return;

            AddControl(_currentTestControl);
        }

        private void TestControl_Click(object sender, RoutedEventArgs e)
        {
            if (_resultControl != null)
            {
                AddControl(_resultControl);
            }
            else
            {
                if (_indexTest != 0 && _indexTest != _viewQuestions.Count - 1)
                {
                    _currentTestControl = new TestUserControl(_viewQuestions[_indexTest], StatusQuestion.Standard);
                    _currentTestControl.Confirm += Control_Confirm;
                    _currentTestControl.Return += Control_Return;

                    AddControl(_currentTestControl);
                }
                else if(_indexTest != 0 && _indexTest == _viewQuestions.Count - 1)
                {
                    _currentTestControl = new TestUserControl(_viewQuestions[_indexTest], StatusQuestion.Result);
                    _currentTestControl.Confirm += Control_Confirm;
                    _currentTestControl.Return += Control_Return;
                    _currentTestControl.GetResult += Control_GetResult;

                    AddControl(_currentTestControl);
                }
                else
                {
                    SetTestControl();
                }
            }
        }

        private void GetResultControl()
        {
            _resultControl = new ResultControl(_dataset, _userAnswers);
            _resultControl.Complete += ResultControl_Complete;

            AddControl(_resultControl);
        }

        private void ResultControl_Complete()
        {
            _resultControl.Complete -= ResultControl_Complete;
            _resultControl = null;

            _indexTest = 0;
            _userAnswers.Clear();

            for (int index = 0; index < 35; index++)
            {
                _userAnswers.Add(new Answer(""));
            }

            _currentTestControl = new TestUserControl(_viewQuestions[_indexTest], StatusQuestion.Standard);
            _currentTestControl.Confirm += Control_Confirm;
            _currentTestControl.Return += Control_Return;

            AddControl(_currentTestControl);
        }

        private void DatasetContol_Click(object sender, RoutedEventArgs e)
        {
            if (_datasetControl != null)
            {
                AddControl(_datasetControl);
            }
            else
            {
                _datasetControl = new DatasetControl(_dataset);

                AddControl(_datasetControl);
            }
        }

        private void SetTestControl()
        {
            _indexTest = 0;
            _userAnswers.Clear();

            for (int index = 0; index < 35; index++)
            {
                _userAnswers.Add(new Answer(""));
            }

            _currentTestControl = new TestUserControl(_viewQuestions[_indexTest], StatusQuestion.Standard);
            _currentTestControl.Confirm += Control_Confirm;
            _currentTestControl.Return += Control_Return;

            AddControl(_currentTestControl);
        }

        private void AddControl(UserControl control)
        {
            WorkSpaceGrid.Children.Clear();
            WorkSpaceGrid.Children.Add(control);
        }

        private void AnalyticsControl_Click(object sender, RoutedEventArgs e)
        {
            if (_analyticsControl != null)
            {
                AddControl(_analyticsControl);
            }
            else
            {
                _analyticsControl = new AnalyticsControl(_dataset);

                AddControl(_analyticsControl);
            }
        }
    }
}