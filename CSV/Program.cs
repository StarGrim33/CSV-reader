
namespace CSV
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region DataCSV
            File.WriteAllText(Shared.FilePath,
                "Chandra Stoll', 'marietta.ewing@phases.com', '0528 Hillbrook Avenue', 'Olathe', 'Mauritius', 'Female'" +
                "\r\nDennis Ortega', 'joleensimonson@yahoo.com', '9016 Hargreaves Avenue', 'Ontario', 'Kyrgyzstan', 'Male'" +
                "\r\nCarlton Pickens', 'ayana42517@accessing.krasnodar.su', '3426 Bridge Street', 'Normal', 'Bhutan', 'Male'" +
                "\r\nVonnie Lanier', 'chelsie_pulido@skiing.shingu.fukuoka.jp', '1549 Whitby Lane', 'Gilbert', 'Mali', 'Male'" +
                "\r\nNoelle Mancuso', 'latoyaquick78929@lodging.com', '1900 Melling Circle', 'Cary', 'Bosnia Herzegovina', 'Male'" +
                "\r\nAnnita Mckenna', 'birdie6@variables.com', '6114 Back Road', 'College Station', 'Sierra Leone', 'Female'" +
                "\r\nMilly Forsyth', 'evelyne-griswold@yahoo.com', '5166 Brandle Street', 'Virginia Beach', 'Burundi', 'Male'" +
                "\r\nRickie Perkins', 'marya8168@yahoo.com', '5585 Ravenwood Lane', 'Duluth', 'Kiribati', 'Male'" +
                "\r\nBrigitte Guerin', 'lashell.stuart60474@gmail.com', '5734 Newport', 'Concord', 'Norway', 'Female'" +
                "\r\nHelene Elrod', 'katheryn04393@gmail.com', '7582 Coal', 'Appleton', 'Albania', 'Female'");
            #endregion


            //Reading data from file
            using (StreamReader sr = new(Shared.FilePath))
            {
                string? line;
                int lineNumbers = 0;
                int threadCount = 0;

                List<Thread> threads = [];
                List<string> lines = [];
                Semaphore semaphore = new(Shared.MaxConcurency, Shared.MaxConcurency);

                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line)) continue;

                    lineNumbers++;
                    lines.Add(line);

                    if (lineNumbers % Shared.ChunkSize == 0)
                    {
                        threadCount++;

                        List<string> chunkCopy = lines.Select(temp => temp).ToList();
                        string chunkName = $"Chunk {threadCount}";

                        Thread thread = new(() =>
                        {
                            //Wait for the semaphore to be available
                            semaphore.WaitOne();

                            try
                            {
                                InvokeDataProcessor(chunkName, chunkCopy);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                throw;
                            }
                            finally
                            {
                                semaphore.Release();
                            }
                        });

                        threads.Add(thread);
                        thread.Start();
                        lines.Clear();
                    }
                }

                if (lines.Count > 0)
                {
                    threadCount++;
                    string chunkName = $"Chunk {threadCount}";

                    Thread thread = new(() =>
                    {
                        InvokeDataProcessor(chunkName, lines);
                    });

                    threads.Add(thread);
                    thread.Start();
                    lines.Clear();
                }

                foreach (Thread thread in threads)
                {
                    thread.Join();
                }

            };

            Console.WriteLine("\nAll CSV lines processed");
            Console.ReadKey();
        }


        private static void InvokeDataProcessor(string chunkName, List<string> chunkCopy)
        {
            DataProcessor processor = new()
            {
                ChunkName = chunkName,
                Chunk = chunkCopy
            };

            Console.WriteLine($"Processing: {processor.ChunkName} of size {chunkCopy.Count}");

            processor.ProcessChunk();

            try
            {
                Shared.Mutex.WaitOne();
                {
                    Console.WriteLine($"\nProcessed: {processor.ChunkName} of size {chunkCopy.Count}");
                    Console.WriteLine($"{processor.ChunkName}");

                    foreach (var gender in processor.GenderCounts)
                    {
                        Console.WriteLine($"{gender.Key}: {gender.Value}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                Shared.Mutex.ReleaseMutex();
            }
        }
    }
}
