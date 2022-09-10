using ClosedXML.Excel;
using MentalHealthAnalysis.Test;
using System;
using System.Collections.Generic;

namespace MentalHealthAnalysis
{
    public class Dataset
    {
        private readonly string _xlsxPath;
        private readonly DatasetSettings _settings;

        public int RowsCount => _settings.RowsCount;
        public int ColumnsCount => _settings.ColumnsCount;

        public List<Question> Questions { get; set; }

        public Dataset(string xlsxPath, DatasetSettings settings)
        {
            _xlsxPath = xlsxPath;
            _settings = settings;

            Questions = (List<Question>)GetQuestions(_settings.ColumnsCount);
        }

        private Question GetQuestion(int column, int worksheetNumber = 1)
        {
            XLWorkbook workbook = new XLWorkbook(_xlsxPath);
            IXLWorksheet worksheet = workbook.Worksheets.Worksheet(worksheetNumber);

            List<Answer> answers = new List<Answer>();

            string questionText = worksheet.Cell(1, column).GetValue<string>();

            for (int row = 2; row <= _settings.RowsCount + 1; row++)
            {
                string textAnswer = worksheet.Cell(row, column).GetValue<string>();
                Answer answer = new Answer(textAnswer);

                answers.Add(answer);
            }

            Question question = new Question(questionText, answers);

            return question;
        }

        private IEnumerable<Question> GetQuestions(int columnsCount, int worksheetNumber = 1)
        {
            List<Question> questions = new List<Question>();

            for (int column = 1; column <= columnsCount; column++)
            {
                Question question = GetQuestion(column, worksheetNumber);

                questions.Add(question);
            }

            return questions;
        }

        public List<Tuple<double, double[]>> Normalize()
        {
            List<Tuple<double, double[]>> normalizeQuestions = new List<Tuple<double, double[]>>();

            List<double[]> standardAnswers = new List<double[]>();
            double[] resultAnswers = new double[RowsCount];

            for (int i = 0; i < RowsCount; i++)
            {
                standardAnswers.Add(new double[ColumnsCount]);
            }

            for (int index = 0; index < Questions.Count; index++)
            {
                int indexStandardAnswer = 0;

                foreach (Answer answer in Questions[index].Answers)
                {
                    standardAnswers[indexStandardAnswer].SetValue(GetNormalizedValue(answer.Text), index);

                    indexStandardAnswer++;
                }
            }

            int indexResultAnswer = 0;
            foreach (Answer answer in Questions[ColumnsCount - 1].Answers)
            {
                resultAnswers[indexResultAnswer] = GetNormalizedValue(answer.Text);

                indexResultAnswer++;
            }

            for (int i = 0; i < RowsCount; i++)
            {
                Tuple<double, double[]> tuple = new Tuple<double, double[]>(resultAnswers[i], standardAnswers[i]);

                normalizeQuestions.Add(tuple);
            }

            return normalizeQuestions;
        }

        private double GetNormalizedValue(string text)
        {
            switch (text)
            {
                case "Да":
                    return 0;
                case "Нет":
                    return 1;
                case "Не знаю":
                    return 0.5;
                case "Очень легко":
                    return 1;
                case "Легко":
                    return 0.8;
                case "Ни легко, ни трудно":
                    return 0.5;
                case "Немного трудно":
                    return 0.2;
                case "Трудно":
                    return 0;
                default:
                    return 0;
            }
        }

        public double[] GetNormalizedUserAnswers(IEnumerable<Answer> userAnswers)
        {
            List<Answer> answersList = (List<Answer>)userAnswers;

            double[] normilizedRow = new double[answersList.Count];

            for (int i = 0; i < ColumnsCount - 1; i++)
            {
                normilizedRow[i] = GetNormalizedValue(answersList[i].Text);
            }

            return normilizedRow;
        }
    }
}
