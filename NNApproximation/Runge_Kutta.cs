using System;
using System.Collections.Generic;

namespace NNApproximation
{
    public class Runge_Kutta
    {
        const int M = 6, Q = 10;
        const double T = 1.5;
        const double h = 0.1;
        int NumberOfForm, NumberOfTestForm;
        public List<double> x, y, z;

        public Runge_Kutta(int NumberOfForm, int NumberOfTestForm)
        {
            this.NumberOfForm = NumberOfForm;
            this.NumberOfTestForm = NumberOfTestForm;
            x = new List<double> { 0.1 };
            y = new List<double> { 0.1 };
            z = new List<double> { 0.1 };

            calculation();
        }
        /// <summary>
        /// Производная функции X()
        /// </summary>
        /// <returns></returns>
        private double X(double x, double y, double z)
        {
            return (M * z * Math.Exp(-Math.Pow(z, 2)) - x) / T;
        }
        /// <summary>
        /// Производная функции Y()
        /// </summary>
        /// <returns></returns>
        private double Y(double x, double y, double z)
        {
            return x - z;
        }
        /// <summary>
        /// Производная функции Z()
        /// </summary>
        /// <returns></returns>
        private double Z(double x, double y, double z)
        {
            return y - z / Q;
        }
        /// <summary>
        /// Вычислить XYZ методом Рунге-Кутты 4-го порядка
        /// </summary>
        private void calculation()
        {
            double[,] k = new double[4, 3];
            for (int i = 0; i < NumberOfForm + NumberOfTestForm; i++)
            {
                k[0, 0] = X(x[i], y[i], z[i]);
                k[0, 1] = Y(x[i], y[i], z[i]);
                k[0, 2] = Z(x[i], y[i], z[i]);

                k[1, 0] = X(x[i] + k[0, 0] / 2 * h, y[i] + k[0, 1] / 2 * h, z[i] + k[0, 2] / 2 * h);
                k[1, 1] = Y(x[i] + k[0, 0] / 2 * h, y[i] + k[0, 1] / 2 * h, z[i] + k[0, 2] / 2 * h);
                k[1, 2] = Z(x[i] + k[0, 0] / 2 * h, y[i] + k[0, 1] / 2 * h, z[i] + k[0, 2] / 2 * h);

                k[2, 0] = X(x[i] + k[1, 0] / 2 * h, y[i] + k[1, 1] / 2 * h, z[i] + k[1, 1] / 2 * h);
                k[2, 1] = Y(x[i] + k[1, 0] / 2 * h, y[i] + k[1, 1] / 2 * h, z[i] + k[1, 1] / 2 * h);
                k[2, 2] = Z(x[i] + k[1, 0] / 2 * h, y[i] + k[1, 1] / 2 * h, z[i] + k[1, 1] / 2 * h);

                k[3, 0] = X(x[i] + k[2, 0] * h, y[i] + k[2, 1] * h, z[i] + k[2, 2] * h);
                k[3, 1] = Y(x[i] + k[2, 0] * h, y[i] + k[2, 1] * h, z[i] + k[2, 2] * h);
                k[3, 2] = Z(x[i] + k[2, 0] * h, y[i] + k[2, 1] * h, z[i] + k[2, 2] * h);

                x.Add(x[i] + (k[0, 0] + 2 * k[1, 0] + 2 * k[2, 0] + k[3, 0]) / 6 * h);
                y.Add(y[i] + (k[0, 1] + 2 * k[1, 1] + 2 * k[2, 1] + k[3, 1]) / 6 * h);
                z.Add(z[i] + (k[0, 2] + 2 * k[1, 2] + 2 * k[2, 2] + k[3, 2]) / 6 * h);

            }
        }

    }

}
