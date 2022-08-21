using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertToCSV
{
    static class CSVConverter
    {
        public static void GenerateCSV()
        {
            //Get all lines from path
            string newPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\"));
            string filePath = newPath + "Files\\access.log";
            string csvfilePath = newPath + "Files\\report.csv";
            List<string> lines = System.IO.File.ReadAllLines(filePath).Skip(4).ToList();

            //Filter all lines
            List<List<string>> filteredLines = new List<List<string>>();
            int count = 0;
            foreach (var line in lines)
            {
                List<string> words = line.Split(' ').ToList();
                if (words.Count() < 8)
                {
                    continue;
                }
                int i = words[2].IndexOf('.', words[2].IndexOf('.') + 1);
                if (i < 0 || words[2].Length < i)
                {
                    continue;
                }
                string w = words[2].Substring(0, i);
                if (words[8].Equals("GET") && words[7].Equals("80") && !w.Equals("207.114"))
                {
                    filteredLines.Add(words);
                }
                count++;
            }

            //Write to CSV
            Dictionary<string, int> value = new Dictionary<string, int>();
            value = filteredLines.GroupBy(x => x[2]).ToDictionary(kvp => kvp.Key, kvp => kvp.Count()).OrderByDescending(x => x.Value).ToDictionary(y => y.Key, y => y.Value);
            var result = string.Join(Environment.NewLine, value.Select(x => string.Join(",", x.Value, x.Key)));
            File.WriteAllText(csvfilePath, result);
        }
    }
}
