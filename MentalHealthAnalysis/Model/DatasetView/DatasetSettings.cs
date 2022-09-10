namespace MentalHealthAnalysis
{
    public class DatasetSettings
    {
        public int RowsCount { get; private set; }

        public int ColumnsCount { get; private set; }

        public DatasetSettings(int rowsCount, int columnsCount)
        {
            RowsCount = rowsCount;
            ColumnsCount = columnsCount;
        }
    }
}