using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV
{
    public class Shared
    {
        public static Mutex Mutex { get; set; }

        public static string FilePath {  get; set; }

        public static int ChunkSize { get; set; }

        public static int MaxConcurency { get; set; }

        static Shared()
        {
            FilePath = @"C:\Users\prosk\OneDrive\Документы\GitHub\CSV\CSV\bin\Debug\net8.0\data.csv";
            GetRowsNumber();
            MaxConcurency = 3;
            Mutex = new Mutex();
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
