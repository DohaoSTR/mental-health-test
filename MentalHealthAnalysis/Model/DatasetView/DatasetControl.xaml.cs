using MentalHealthAnalysis.Test;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Controls;

namespace MentalHealthAnalysis.DatasetView
{
    /// <summary>
    /// Interaction logic for DatasetControl.xaml
    /// </summary>
    public partial class DatasetControl : UserControl
    {
        private readonly ObservableCollection<Question> _questionsList;

        public DatasetControl(Dataset dataset)
        {
            InitializeComponent();

            _questionsList = new ObservableCollection<Question>(dataset.Questions);

            DataTable dataTable = SetDataGridColumn(dataset.RowsCount, dataset.ColumnsCount);

            TestDataGrid.ItemsSource = dataTable.DefaultView;
        }

        private DataTable SetDataGridColumn(int rowsCount, int columnsCount)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add(new DataColumn("№"));

            for (int columnNumber = 0; columnNumber < columnsCount; columnNumber++)
            {
                DataColumn column = new DataColumn(_questionsList[columnNumber].Text);

                dataTable.Columns.Add(column);
            }

            for (int rowNumber = 0; rowNumber < rowsCount; rowNumber++)
            {
                string[] row = new string[columnsCount + 1];
                row[0] = (rowNumber + 1).ToString();

                for (int columnNumber = 0; columnNumber < columnsCount; columnNumber++)
                {
                    row[columnNumber + 1] = _questionsList[columnNumber].Answers[rowNumber].Text;
                }

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
    }
}