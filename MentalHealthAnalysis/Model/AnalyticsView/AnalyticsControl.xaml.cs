using MentalHealthAnalysis.Test;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Forms.DataVisualization.Charting;

namespace MentalHealthAnalysis.AnalyticsView
{
    /// <summary>
    /// Interaction logic for AnalyticsControl.xaml
    /// </summary>
    public partial class AnalyticsControl : UserControl
    {
        private Dataset _dataset;

        public AnalyticsControl(Dataset dataset)
        {
            InitializeComponent();

            _dataset = dataset;
        }

        private void GetResult_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            int columnNumber = Convert.ToInt32(ColumnNumberTextBox.Text);

            if (columnNumber < 0 || columnNumber >= 36)
            {
                MessageBox.Show("Столбца с таким номером не существует!");
            }

            string[] xValues = GetUniqueAnswers(_dataset.Questions, columnNumber);
            double[] yValues = GetYValues(xValues.Length, _dataset.Questions, columnNumber);

            WorkSpaceChart.Series.Clear();
            WorkSpaceChart.ChartAreas.Clear();
            WorkSpaceChart.Legends.Clear();
            WorkSpaceChart.Titles.Clear();

            WorkSpaceChart.ChartAreas.Add(new ChartArea());
            WorkSpaceChart.BackColor = Color.Transparent;

            WorkSpaceChart.Titles.Add(new Title("Данные по столбцу №" + columnNumber.ToString(), Docking.Top, new Font("Times new roman", 18), Color.Black));

            WorkSpaceChart.Series.Add(new Series("ColumnSeries")
            {
                ChartType = SeriesChartType.Pie
            });

            WorkSpaceChart.Legends.Add(new Legend("Legend2"));
            WorkSpaceChart.Series["ColumnSeries"].Legend = "Legend2";
            WorkSpaceChart.Series["ColumnSeries"].IsVisibleInLegend = true;

            WorkSpaceChart.Series["ColumnSeries"].Points.DataBindXY(xValues, yValues);

            WorkSpaceChart.ChartAreas[0].Area3DStyle.Enable3D = true;
            WorkSpaceChart.ChartAreas[0].BackColor = Color.Transparent;
        }

        private string[] GetUniqueAnswers(List<Question> questions, int columnNumber)
        {
            var uniqueList = questions[columnNumber].Answers.Select(k => k.Text).GroupBy(g => g).Select(i => i.Key);

            string[] uniqueArray = new string[uniqueList.Count()];

            int index = 0;
            foreach (string answer in uniqueList)
            {
                uniqueArray[index] = answer;

                index++;
            }

            return uniqueArray;
        }

        private double[] GetYValues(int answersCount, List<Question> questions, int columnNumber)
        {
            double[] yValues = new double[answersCount];

            foreach (Answer answer in questions[columnNumber].Answers)
            {
                yValues[GetAnswerIndex(answer.Text)] += 1;
            }

            return yValues;
        }

        private int GetAnswerIndex(string text)
        {
            switch (text)
            {
                case "Да":
                    return 0;
                case "Нет":
                    return 1;
                case "Не знаю":
                    return 2;
                case "Очень легко":
                    return 0;
                case "Легко":
                    return 1;
                case "Ни легко, ни трудно":
                    return 3;
                case "Немного трудно":
                    return 4;
                case "Трудно":
                    return 5;
                default:
                    return 0;
            }
        }
    }
}