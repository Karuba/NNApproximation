using System;
using System.IO;

namespace SecondStep
{
    class IO
    {
        string[] path = new string[6];
        public IO(string txt, int i)
        {
            string curPath = Directory.GetCurrentDirectory();
            path[0] = curPath + @"\educationX.txt";
            path[1] = curPath + @"\testX.txt";
            path[2] = curPath + @"\educationY.txt";
            path[3] = curPath + @"\testY.txt";
            path[4] = curPath + @"\educationZ.txt";
            path[5] = curPath + @"\testZ.txt";
            try
            {
                using (StreamWriter sw = new StreamWriter(path[i], false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(txt);
                }
                Console.WriteLine("Запись выполнена");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
        }
    }
}
