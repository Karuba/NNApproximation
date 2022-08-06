using System;

namespace NNApproximation
{
    class NeuralNetwork
    {
        const int OutputLayerSize = 3;
        const double k = 1;
        private double[,] w1, w2, y1, y2;
        private double[] T1,T2;
        private double[,] x, t;
        private double[,] error1, error2;
        private double alpha, Em, Es;
        private uint epoch;
        private double activationOutputFunction(double S)
        {
            return k*S;
        }
        private double activationHiddenFunction(int form, int i)
        {
            return 1 / (1 + Math.Pow(Math.E, -S(form, i)));
        }
        private double dHiddenFunction(int form, int i)
        {
            return y1[form, i] * (1 - y1[form, i]);
        }
        private double dOutputFunction()
        {
            return 1;
        }
        public NeuralNetwork(in int hiddenCount, in double alpha, in double Em, in double[,] x, in double[,] standart)
        {            
            this.alpha = alpha;
            this.Em = Em;
            this.x = x;
            this.t = standart;
            y1 = new double[standart.GetLength(0), hiddenCount];
            y2 = new double[standart.GetLength(0), OutputLayerSize];
            error1 = new double[standart.GetLength(0), hiddenCount];
            error2 = new double[standart.GetLength(0), OutputLayerSize];         
            T1 = new double[hiddenCount];
            T2 = new double[y2.GetLength(1)];
            w1 = new double[x.GetLength(1), hiddenCount];
            w2 = new double[hiddenCount, y2.GetLength(1)];

            InitializationWT(hiddenCount);  
        }
        void InitializationWT(int hiddenCount)
        {
            Random rnd = new Random();
            for (int i = 0; i < x.GetLength(1); i++)
            {
                for (int j = 0; j < hiddenCount; j++)
                {
                    w1[i, j] = (float)rnd.NextDouble();
                }
            }
            for (int i = 0; i < hiddenCount; i++)
            {
                T1[i] = (float)rnd.NextDouble();
                for (int j = 0; j < y2.GetLength(1); j++)
                {
                    w2[i, j] = (float)rnd.NextDouble();
                    T2[j] = (float)rnd.NextDouble();
                }
            }
        }
        public void startApproximationCycle()
        {
            do
            {
                Es = 0;
                Epoch();
                epoch++;
                Console.WriteLine($"Epoch: {epoch}, E = {Es}");
            } while (Em < Es);
            Console.WriteLine($"Epoch: {epoch}, E = {Es}");
        }
        private double outputY(int form, int j)
        {
            double sumW2 = 0;
            for (int i = 0; i < y1.GetLength(1); i++)
            {
                sumW2 += w2[i, j] * hiddenY(form, i);
            }
            return activationOutputFunction(sumW2 - T2[j]);
        }
        private double hiddenY(int form, int i)
        {
            return y1[form, i] = activationHiddenFunction(form, i);
        }
        private double S(int form, int i)
        {
            double sum = 0;
            for (int k = 0; k < x.GetLength(1); k++)
            {
                sum += w1[k, i] * x[form, k];
            }
            return sum - T1[i];
        }        
        private double outputLayerError(int form, int j)
        {
            return y2[form, j] - t[form, j];
        }
        private double hiddenLayerError(int form, int i)
        {
            double sumError = 0;
            for (int j = 0; j < y2.GetLength(1); j++)
            {
                sumError += error2[form, j] * dOutputFunction() * w2[i, j];
            }
            return sumError;
        }
        private void changeOutputW(int form)
        {
            for (int i = 0; i < y1.GetLength(1); i++)
            {
                for (int j = 0; j < y2.GetLength(1); j++)
                {
                    w2[i, j] = w2[i, j] - alpha * error2[form, j] * dOutputFunction() * y1[form, i];
                }
            }
        }
        private void changeHiddenW(int form)
        {
            for (int k = 0; k < x.GetLength(1); k++)
            {
                for (int i = 0; i < y1.GetLength(1); i++)
                {
                    w1[k, i] = w1[k, i] - alpha * error1[form, i] * dHiddenFunction(form, i) * x[form, k];
                }
            }
        }
        private void changeOutputT(int form)
        {
            for (int j = 0; j < y2.GetLength(1); j++)
            {
                T2[j] = T2[j] + alpha * error2[form, j] * dOutputFunction();
            }
        }
        private void changeHiddenT(int form)
        {
            for (int i = 0; i < y1.GetLength(1); i++)
            {
                T1[i] = T1[i] + alpha * error1[form, i] * dHiddenFunction(form, i);
            }
        }
        private double E(int form)
        {
            double sum = 0;
            for (int j = 0; j < y2.GetLength(1); j++)
            {
                sum += Math.Pow((y2[form, j] - t[form, j]), 2);
            }
            return sum;
        }
        private void Epoch()
        {
            for (int form = 0; form < t.GetLength(0); form++)
            {
                for (int j = 0; j < y2.GetLength(1); j++)
                {
                    y2[form, j] = outputY(form, j);
                    error2[form, j] = outputLayerError(form, j);
                }
                for (int i = 0; i < y1.GetLength(1); i++)
                {
                    error1[form, i] = hiddenLayerError(form, i);
                }
                Es += E(form);
            }
            for (int form = 0; form < t.GetLength(0); form++)
            {
                changeOutputW(form);
                changeHiddenW(form);
                changeOutputT(form);
                changeHiddenT(form);
            }
            Es *= 0.5;
        }
        public double[,] show_resultOutput()
        {
            for (int form = 0; form < t.GetLength(0); form++)
            {
                Console.WriteLine($"Epoch: {epoch}, Em: {Em}, E: {E(form)}");
                Console.WriteLine("t\t \tout y");
                for (int j = 0, i = 0; j < 1; j++)
                {
                    if (i < y1.GetLength(1))
                    {
                        Console.WriteLine($"{t[form, j]}\t {y2[form, j]}");
                        i++;
                    }
                    else
                    {
                        Console.WriteLine($"{t[form, j]}\t \t {y2[form, j]}");
                    }
                }
            }
            return y2;
        }
        public double[,] functionPrediction(double[,] x2)
        {
            x = x2;
            double[,] prediction = new double[x2.GetLength(0), 3];

            for (int form = 0; form < x.GetLength(0); form++)
            {
                for (int j = 0; j < prediction.GetLength(1); j++)
                {
                    prediction[form, j] += outputY(form, j);
                }
            }
            return prediction;
        }
    }
}
