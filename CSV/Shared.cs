using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV
{
    public class Shared
    {
        public static object LockObject { get; set; }

        public static string FilePath {  get; set; }

        public static int ChunkSize { get; set; }

        static Shared()
        {
            FilePath = @"C:\Users\prosk\OneDrive\Документы\GitHub\CSV\CSV\bin\Debug\net8.0\data.csv";
            GetRowsNumber();
            LockObject = new object();
        }

        private static void GetRowsNumber()
        {
            using StreamReader reader = new(FilePath);
            string line;
            ChunkSize = 0;

            while ((line = reader.ReadLine()) != null)
            {
                ChunkSize++;
            }

            Console.WriteLine("Rows number: " + ChunkSize);
            Console.ReadKey();
        }
    }
}
