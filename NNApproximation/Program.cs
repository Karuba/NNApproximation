using System;

namespace NNApproximation
{
    class Program
    {
        static void Main(string[] args)
        {
            const int hiddenCount = 7;
            const double alpha = 0.00035;
            const double Em = 0.1;
            const int formLearnNum = 500;
            const int formLearnSize = 1;
            const int formTestNum = 400;
            Runge_Kutta r_k = new Runge_Kutta(formLearnNum, formTestNum);
            Dataset dataset = new Dataset(r_k.x, r_k.y, r_k.z, formLearnNum, formTestNum, formLearnSize);

            NeuralNetwork neuralNetwork = new NeuralNetwork(hiddenCount, alpha, Em, dataset.getApproximationDataset(), dataset.getStandartDataset());
            neuralNetwork.startApproximationCycle();
            myFile.Write(neuralNetwork.show_resultOutput(), dataset.getStandartDataset(), 0);
            myFile.Write(neuralNetwork.functionPrediction(dataset.getForecastingDataset()), dataset.getStandartForecastingDataset(), 1);
        }
    }
}