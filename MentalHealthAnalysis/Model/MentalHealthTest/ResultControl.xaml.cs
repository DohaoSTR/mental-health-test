using MentalHealthAnalysis.Neural;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MentalHealthAnalysis.Test
{
    /// <summary>
    /// Interaction logic for ResultControl.xaml
    /// </summary>
    public partial class ResultControl : UserControl
    {
        public Action Complete;

        private readonly Dataset _dataset;

        private readonly List<Answer> _userAnswers;

        private NeuralNetwork _neuralNetwork;

        private const int OutputCount = 1;

        private int LayersCount = 1;
        private int NeurounsCount = 10;
        private double LearningRate = 0.001;
        private int Epoch = 100;
        private int[] Layers;

        public ResultControl(Dataset dataset, IEnumerable<Answer> userAnswers)
        {
            InitializeComponent();

            _userAnswers = (List<Answer>)userAnswers;

            _dataset = dataset;

            Layers = new int[LayersCount];
            for (int i = 0; i < LayersCount; i++)
            {
                Layers[i] = NeurounsCount;
            }


            GetResultsNeuron(LearningRate, Layers, Epoch);

            DataContext = GetUserAnswers();
        }

        private Question GetUserAnswers()
        {
            List<Answer> dataContextAnswers = new List<Answer>();
            int index = 0;

            foreach (Answer answer in _userAnswers)
            {
                index++;

                dataContextAnswers.Add(new Answer(index + ". " + answer.Text));
            }

            Question userAnswers = new Question("", dataContextAnswers);

            return userAnswers;
        }

        private double NeuronMethod(int outputCount, double learningRate, int[] layers, int epoch)
        {
            Topology topology = new Topology(_dataset.ColumnsCount, outputCount, learningRate, layers);
            _neuralNetwork = new NeuralNetwork(topology);

            List<Tuple<double, double[]>> tuples = _dataset.Normalize();
            _tuples = tuples;

            _neuralNetwork.Learn(tuples, epoch);

            double[] currentAnswersNormilized = _dataset.GetNormalizedUserAnswers(_userAnswers);

            List<double> results = new List<double>();
            foreach (double data in currentAnswersNormilized)
            {
                double res = _neuralNetwork.FeedForward(data).Output;
                results.Add(res);
            }

            return results[0];
        }

        private List<Tuple<double, double[]>> _tuples;

        private double GetAccuracy()
        {
            List<double> results = GetResults();
            List<double[]> rowsValuesList = GetRowsValues();

            List<double> selectedAnswersResult = new List<double>();

            for (int index = 0; index < results.Count; index++)
            {
                double res = _neuralNetwork.FeedForward(rowsValuesList[index]).Output;
                selectedAnswersResult.Add(res);
            }

            double sum = 0;
            for (int index = 0; index < results.Count; index++)
            {
                double excepted = results[index];
                double actual = selectedAnswersResult[index];

                if (excepted == 1 && excepted - 0.5 < actual)
                {
                    sum += 1;
                }

                if (excepted == 0 && excepted + 0.5 > actual)
                {
                    sum += 1;
                }
            }

            return sum / results.Count;
        }

        private List<double> GetResults()
        {
            List<double> resultsList = new List<double>();

            foreach (Tuple<double, double[]> tuple in _tuples)
            {
                resultsList.Add(tuple.Item1);
            }

            return resultsList;
        }

        private List<double[]> GetRowsValues()
        {
            List<double[]> rowsValuesList = new List<double[]>();

            foreach (Tuple<double, double[]> tuple in _tuples)
            {
                rowsValuesList.Add(tuple.Item2);
            }

            return rowsValuesList;
        }

        private Topology GetSettings()
        {
            LayersCount = Convert.ToInt32(LayersCountTextBox.Text);
            Epoch = Convert.ToInt32(EpochTextBox.Text);
            NeurounsCount = Convert.ToInt32(NeuronsCountTextBox.Text);

            Layers = new int[LayersCount];
            for (int i = 0; i < LayersCount; i++)
            {
                Layers[i] = NeurounsCount;
            }

            LearningRate = Convert.ToDouble(LearningRateTextBox.Text);

            return new Topology(_dataset.ColumnsCount, OutputCount, LearningRate, Layers);
        }

        private void GetResultsNeuron(double learningRate, int[] layers, int epoch)
        {
            double probability = NeuronMethod(OutputCount, learningRate, layers, epoch);

            OutputDiagnosisNeuralTextBlock.Text = probability > 0.5
                ? "Диагноз: не болен психическим расстройством или заболеванием"
                : "Диагноз: болен психическим(и) расстройством или заболеванием(ями)";

            AccuracyNeuralTextBlock.Text = "Точность: " + (GetAccuracy() * 100).ToString("N2");
        }

        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {
            Complete();
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            GetSettings();

            GetResultsNeuron(LearningRate, Layers, Epoch);
        }
    }
}