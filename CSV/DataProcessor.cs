using System;

namespace CSV
{
    public class DataProcessor
    {
        public string? ChunkName { get; set; }

        public List<string>? Chunk {  get; set; }

        public Dictionary<string, int> GenderCounts = [];

        public void ProcessChunk()
        {
            if(Chunk == null)
                throw new ArgumentNullException(nameof(Chunk));

            foreach (var chunk in Chunk)
            {
                if (string.IsNullOrEmpty(chunk))
                    continue;

                string[] values = chunk.Split(',');

                if(values.Length >= 5)
                {
                    string gender = values[4].Trim().ToLower();

                    if (GenderCounts.ContainsKey(gender))
                    {
                        GenderCounts[gender]++;
                    }
                    else
                    {
                        GenderCounts.Add(gender, 1);
                    }
                }
            }

            //This is simulating delay just for example
            Random random = new();
            Thread.Sleep(100 * random.Next(2, 5));
        }
    }
}
