using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondStep
{
    class NN
    {
        private double[,] w1;
        private double[,] w2;
        private double[,] y1;
        private double[,] y2;
        private double[] T1;
        private double[] T2;
        private double[,] x;
        private double[] t;
        private double[,] error1;
        private double[,] error2;
        private double a, Em;
        private int L;
        private uint epoch;
        private double Es;
        public NN(in int hidCount, in double a, in double Em, in double[,] x, in double[] t)
        {
            this.a = a;
            this.Em = Em;
            this.x = x;
            this.t = t;

            int r = x.GetLength(0);//3
            int r2 = x.GetLength(1);//30

            y1 = new double[t.GetLength(0), hidCount];
            y2 = new double[t.GetLength(0), 1];
            error1 = new double[t.GetLength(0), hidCount];
            error2 = new double[t.GetLength(0), 1];

            L = x.GetLength(1);

            Random rnd = new Random();
            T1 = new double[hidCount];
            T2 = new double[t.GetLength(0)];
            w1 = new double[x.GetLength(1), hidCount];
            w2 = new double[hidCount, t.GetLength(0)];

            for (int i = 0; i < x.GetLength(1); i++)
            {
                for (int j = 0; j < hidCount; j++)
                {
                    w1[i, j] = (float)rnd.NextDouble();
                }
            }
            for (int i = 0; i < hidCount; i++)
            {
                T1[i] = (float)rnd.NextDouble();
                for (int j = 0; j < t.GetLength(0); j++)
                {
                    w2[i, j] = (float)rnd.NextDouble();
                    T2[j] = (float)rnd.NextDouble();
                }
            }
            //need Cycle
            do
            {
                Es = 0;
                lifeCycle();
                epoch++; 
                Console.WriteLine($"Epoch: {epoch}, E = {Es}");                                
            } while (Em < Es);
            Console.WriteLine($"Epoch: {epoch}, E = {Es}");
        }
        private double outY(int obraz, int j)
        {
            double sumW2 = 0;
            for (int i = 0; i < y1.GetLength(1); i++)
            {
                sumW2 += w2[i, j] * hidY(obraz, i);
            }
            return sumW2 - T2[j];
        }
        private double hidY(int obraz, int i)
        {
            return (1 / (1 + Math.Pow(Math.E, -S(obraz, i))));
        }
        private double S(int obraz, int i)
        {
            double sum = 0;
            for (int k = 0; k < x.GetLength(1); k++)
            {
                sum += w1[k, i] * x[obraz, k];
            }
            return sum - T1[i];
        }
        private double dFhid(int obraz, int i)
        {
            return y1[obraz, i] * (1 - y1[obraz, i]);
        }
        private double dFout()
        {
            return 1;
        }
        private double outError(int obraz, int j)
        {
            return y2[obraz, j] - t[obraz];
        }
        private double hidError(int obraz, int i)//<-- rewrite// mb ready <-- ???
        {
            double sumError = 0;
            for (int j = 0; j < y2.GetLength(1); j++)
            {
                sumError += error2[obraz, j] * dFout() * w2[i, j]; // dFhid(i) -- dFout() ???
            }
            return sumError;
        }
        private void chOutW(int obraz)//<-- mb ready
        {
            for (int i = 0; i < y1.GetLength(1); i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    w2[i, j] = w2[i, j] - a * error2[obraz, j] * dFout() * y1[obraz, i];
                }
            }
        }
        private void chHidW(int obraz)//<-- mb ready
        {
            for (int k = 0; k < x.GetLength(1); k++)
            {
                for (int i = 0; i < y1.GetLength(1); i++)
                {
                    w1[k, i] = w1[k, i] - a * error1[obraz, i] * dFhid(obraz, i) * x[obraz, k];
                }
            }
        }
        private void chOutT(int obraz)
        {
            for (int j = 0; j < 1; j++)
            {
                T2[j] = T2[j] + a * error2[obraz, j] * dFout();
            }
        }
        private void chHidT(int obraz)
        {
            for (int i = 0; i < y1.GetLength(1); i++)
            {
                T1[i] = T1[i] + a * error1[obraz, i] * dFhid(obraz, i);
            }
        }
        private double E(int obraz)////////////////////
        {
            double sum = 0;
            for (int j = 0; j < y2.GetLength(1); j++)
            {
                sum += Math.Pow((y2[obraz, j] - t[obraz]), 2);
            }
            return 0.5 * sum;
            //double sum = 0;
            //for (int k = 0; k < L; k++)
            //{
            //    for (int j = 0; j < y2.GetLength(1); j++)
            //    {
            //        sum += Math.Abs(y2[obraz, j] - t[obraz]);
            //    }
            //}
            //return sum;
        }
        private void lifeCycle()
        {
            for (int obraz = 0; obraz < t.GetLength(0); obraz++)
            {
                for (int j = 0; j < 1; j++)
                {
                    y2[obraz, j] = outY(obraz, j);
                    error2[obraz, j] = outError(obraz, j);
                }
                for (int i = 0; i < y1.GetLength(1); i++)
                {
                    y1[obraz, i] = hidY(obraz, i);
                    error1[obraz, i] = hidError(obraz, i);
                }
                Es += E(obraz);
            }
            for (int obraz = 0; obraz < t.GetLength(0); obraz++)
            {
                chOutW(obraz);
                chHidW(obraz);
                chOutT(obraz);
                chHidT(obraz);
            }
        }
        public void show(int type)
        {
            for (int obraz = 0; obraz < t.GetLength(0); obraz++)
            {
                Console.WriteLine($"Epoch: {epoch}, Em: {Em}, E: {E(obraz)}");
                Console.WriteLine("t\t \tout y");
                for (int j = 0, i = 0; j < 1; j++)
                {
                    if (i < y1.GetLength(1))
                    {
                        Console.WriteLine($"{t[obraz]}\t {y2[obraz, j]}");
                        i++;
                    }
                    else
                    {
                        Console.WriteLine($"{t[obraz]}\t \t {y2[obraz, j]}");
                    }
                }

                //for (int g = 0; g < x.GetLength(1); g++)
                //{
                //    string ch = x[obraz, g] == 1 ? "#" : " ";
                //    Console.Write($"{ch}");
                //    if (g % 6 == 0)
                //    {
                //        Console.WriteLine();
                //    }
                //}
                //Console.WriteLine();
            }
            //запись в файл
            string txt = "";
            for (int i = 0; i < t.GetLength(0); i++)
            {
                txt += t[i].ToString() + "\n";
            }
            txt += "\n";
            for (int i = 0; i < x.GetLength(0); i++)
            {
                txt += y2[i,0].ToString() + "\n";
            }
            IO io = new IO(txt, type);
        }
        public void genNext(double[,] x2, double[] t2, int type)
        {
            string txt = "";

            x = x2;
            //запись в файл
            for (int i = 0; i < t2.GetLength(0); i++)
            {
                 txt += t2[i].ToString() + "\n";  
            }
            txt += "\n";
            for (int i = 0; i < x.GetLength(0); i++)
            {
                txt += outY(i, 0).ToString() + "\n";
            }

            IO io = new IO(txt, type);
        }
    }
}
