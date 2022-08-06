using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNApproximation
{
    static class myFile
    {
        static public void Write(double[,] y, double[,] ey, int type)
        {
            /////////////////////////
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            //before your loop
            string[] filePath = {   @"C:\WorkPlace\University-3-5\labs\МРЗИС\Kursovoi\Irishka\EdDataset.csv",
                                    @"C:\WorkPlace\University-3-5\labs\МРЗИС\Kursovoi\Irishka\TestDataset.csv" };
            var csv = new StringBuilder();

            for (int i = 0; i < ey.GetLength(0); i++)
            {
                var first =  ey[i, 0].ToString();
                var second = ey[i, 1].ToString();
                var third =  ey[i, 2].ToString();
                var fourth =  y[i, 0].ToString();
                var fifth =   y[i, 1].ToString();
                var sixth =   y[i, 2].ToString();

                var newLine = string.Format($"{first};{second};{third};{fourth};{fifth};{sixth}");
                csv.AppendLine(newLine);
            }

            //after your loop
            File.WriteAllText(filePath[type], csv.ToString());
        }
    }
}
