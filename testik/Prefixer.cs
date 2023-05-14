using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testik
{
    public class Prefixer
    {
        public void FindPrefix(string path)
        {
            StreamReader reader = new(path);
            StreamWriter writer = new(path.Insert(path.Length - 4, "_output"));

            Dictionary<string, int> map = new Dictionary<string, int>();
            Dictionary<char, int> spotreba = new Dictionary<char, int>();
            SortedDictionary<int, string> mapTest = new SortedDictionary<int, string>(new DuplicateKeyComparer<int>());
            List<string> capacity = new();
            int numberOfTasks = Convert.ToInt32(reader.ReadLine());
            int addMe = 0;
            for (int i = 0; i < numberOfTasks; i++) // pro kazde zadani
            {
                mapTest.Clear();
                spotreba.Clear();
                int tuha = Convert.ToInt32(reader.ReadLine()); // pocet tuhy
                List<string> words = new();
                int numberOfWords = Convert.ToInt32(reader.ReadLine());
                for (int j = 0; j < numberOfWords; j++) //nactu vsechna slovicka
                {
                    words.Add(reader.ReadLine());
                }
                int numberOfLetters = Convert.ToInt32(reader.ReadLine());
                for (int j = 0; j < numberOfLetters; j++) //nactu vsechny spotřeby na pismenka
                {
                    string[] line = reader.ReadLine().Split(' ');
                    spotreba.Add(Convert.ToChar(line[0]), Convert.ToInt32(line[1]));
                }
                int start = 1;
                Dictionary<int,string> prefixes = new();

                CompareInfo info = CultureInfo.InvariantCulture.CompareInfo;

                words.Sort();

                for (int j = 0; j < numberOfWords; j++) //projedu kazde slovicko
                {
                    if (start > words[j].Length)
                        continue;
                    string checkMe = words[j].Substring(0, start);
                    if(j != numberOfWords - 1 && info.IsPrefix(words[j+1], checkMe)) // nasledující slovo má stejný prefix --> zvetsim checkMe a zkousim dal
                    {
                        start++;
                        j--;
                    }
                    else //nasla jsem jedinecny prefix
                    {
                        if(prefixes.Count == 0) //nemam ho s cim kontrolovat
                        {
                            prefixes.Add(addMe, checkMe);
                            start = 1;
                            addMe++;
                            continue;
                        }
                        else
                        {
                            if (info.IsPrefix(prefixes[addMe - 1], checkMe) == false)
                            {
                                prefixes.Add(addMe, checkMe);
                                addMe++;
                                start = 1;
                            }
                            else // pokud takový prefix už existuje, pojedu toto slovo znova ale s větším prefixem
                            {
                                start++;
                                j--;
                            }
                        }
                        
                    }
                }
                foreach (string item in prefixes.Values) // dopocitam mnozstvi tuhy pro kazdy prefix
                {
                    int spotrebaTuhy = 0;
                    foreach (char item2 in item)
                    {
                        spotrebaTuhy += spotreba[item2];
                    }
                    mapTest.Add(spotrebaTuhy, item); // sortedDictionary -> sesortovano podle mnozstvi tuhy
                }
                int usedTuha = 0;
                capacity.Clear(); //prefixy, co se vlezou
                foreach (var item in mapTest) // projedu seznam prefixů a kontroluji, jak moc se vleze
                {
                    if ((usedTuha + item.Key) <= tuha)
                    {
                        usedTuha += item.Key;
                        capacity.Add(item.Value);
                    }
                    else
                    {
                        break;
                    }
                }
                writer.WriteLine(capacity.Count);
                Console.WriteLine(capacity.Count);
                if (capacity.Count > 0)
                {
                    foreach (var item in capacity)
                    {
                        writer.WriteLine(item);
                        Console.WriteLine(item);
                    }
                }               
            }
            writer.Close();
            reader.Close();
        }
    }
}
