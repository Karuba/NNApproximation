using System.Collections.Generic;

namespace NNApproximation
{
    class Dataset
    {
        int NumberOfForm, NumberOfTestForm, formLearnSize;
        private List<double> x, y, z;
        public Dataset(List<double> x, List<double> y, List<double> z, int NumberOfForm, int NumberOfTestForm, int formLearnSize)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.NumberOfForm = NumberOfForm;
            this.NumberOfTestForm = NumberOfTestForm;
            this.formLearnSize = formLearnSize;
        }
        public double[,] getApproximationDataset()
        {
            double[,] MultiLearnData = new double[NumberOfForm - 1, formLearnSize * 3];
            for (int i = 0; i < MultiLearnData.GetLength(0); i++)
            {
                for (int j = 0, it = 0; j < MultiLearnData.GetLength(1); j += 3, it++)
                {
                    MultiLearnData[i, j] = x[i + it];
                    MultiLearnData[i, j + 1] = y[i + it];
                    MultiLearnData[i, j + 2] = z[i + it];
                }
            }
            return MultiLearnData;
        }
        public double[,] getStandartDataset()
        {
            double[,] MultiLearnData = new double[NumberOfForm - 1, 3];
            for (int i = 0, j = formLearnSize; i < MultiLearnData.GetLength(0); i++, j++)
            {
                MultiLearnData[i, 0] = x[j];
                MultiLearnData[i, 1] = y[j];
                MultiLearnData[i, 2] = z[j];
            }
            return MultiLearnData;
        }
        public double[,] getForecastingDataset()
        {
            double[,] MultiTestData = new double[NumberOfTestForm, formLearnSize * 3];
            for (int i = 0, k = NumberOfForm - 1; i < MultiTestData.GetLength(0); i++, k++)
            {
                for (int j = 0, it = 0; j < MultiTestData.GetLength(1); j += 3, it++)
                {
                    MultiTestData[i, j] = x[k + it];
                    MultiTestData[i, j + 1] = y[k + it];
                    MultiTestData[i, j + 2] = z[k + it];
                }
            }
            return MultiTestData;
        }
        public double[,] getStandartForecastingDataset()
        {
            double[,] MultiTestData = new double[NumberOfTestForm, 3];
            for (int i = 0, k = NumberOfForm - 1 + formLearnSize; i < MultiTestData.GetLength(0); i++, k++)
            {
                MultiTestData[i, 0] = x[k];
                MultiTestData[i, 1] = y[k];
                MultiTestData[i, 2] = z[k];
            }
            return MultiTestData;
        }
    }
}
