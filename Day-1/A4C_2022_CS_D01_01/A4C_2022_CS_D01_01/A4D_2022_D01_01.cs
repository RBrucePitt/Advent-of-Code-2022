// Advent for code - 2022 - Day 1 - R. Bruce Pitt, 2022/12/03

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace A4C_2022_CS_D01_01
{
    internal class A4D_2022_D01_01
    {
        public class SOLUTION
        {
            String _filePath = "";
            int elfCounter = 0;
            int elfWithMostCalories = 0;
            int mostCaloriesCarried = 0;
            int topThreeTotal = 0;

            // Elfs are incremented as blank lines found
            Dictionary<int, int> calorieCounter = new Dictionary<int, int>();
            Dictionary<int, List<int>> caloriesPerElf = new Dictionary<int, List<int>>();
            List<int> sortedCalories = null;

            public SOLUTION(String filePath)
            {
                _filePath = filePath;
                ReadInputIntoDictionary();
                FindTopCount();
                FindTopThreeCount();
            }

            private void ReadInputIntoDictionary()
            {
                StreamReader inHandle = new StreamReader(_filePath);
                String line = "";
                int lineCounter = 0;

                while ((line = inHandle.ReadLine()) != null)
                {
                    if ((lineCounter == 0) && (elfCounter == 0))
                        elfCounter++;
                    if (line != "")
                    {
                        int thisCalorieCount = Convert.ToInt32(line);
                        if (!calorieCounter.ContainsKey(elfCounter))
                        {
                            calorieCounter.Add(elfCounter, thisCalorieCount);
                        }
                        else
                        {
                            calorieCounter[elfCounter] += thisCalorieCount;
                        }
                    }
                    else    // Line is blank, see if it is empty at the top
                    {
                        if (elfCounter != 0)
                            elfCounter++;
                    }

                    lineCounter++;
                }
            }

            private void FindTopCount()
            {
                int elfWithMost = 0;
                int mostCalories = 0;

                for (int i = 0; i < calorieCounter.Count; i++)
                {
                    int elfId = calorieCounter.ElementAt(i).Key;
                    int elfCalories = calorieCounter.ElementAt(i).Value;
                    if (elfCalories > mostCalories)
                    {
                        elfWithMost = elfId;
                        mostCalories = elfCalories;
                    } // Probably should check equal in case multiple elves have same
                }

                elfWithMostCalories = elfWithMost;
                mostCaloriesCarried = mostCalories;
            }

            public void FindTopThreeCount()
            {
                // Reverse the Dictionary

                for (int i = 0; i < calorieCounter.Count; i++)
                {
                    int elfId = calorieCounter.ElementAt(i).Key;
                    int elfCalories = calorieCounter.ElementAt(i).Value;
                    if (caloriesPerElf.ContainsKey(elfCalories))
                    {
                        caloriesPerElf[elfCalories].Add(elfId);
                    }
                    else
                    {
                        List<int> elves = new List<int>();
                        elves.Add(elfId);
                        caloriesPerElf.Add(elfCalories, elves);
                    }
                }

                sortedCalories = new List<int>(caloriesPerElf.Keys);
                sortedCalories.Sort();
                sortedCalories.Reverse();

                int elfFoundCounter = 0;
                int idx = 0;
                topThreeTotal = 0;
                while (elfFoundCounter < 3)
                {
                    int calories = sortedCalories[idx];
                    int elvesAtThisCalorieCount = caloriesPerElf[calories].Count;
                    if (elfFoundCounter == 2)
                        elvesAtThisCalorieCount = 1;
                    topThreeTotal += calories * elvesAtThisCalorieCount;
                    elfFoundCounter += elvesAtThisCalorieCount;
                    idx++;
                }
            }

            public int GetMostCalories()
            {
                return mostCaloriesCarried;
            }

            public int GetWhoCarriesTheCalories()
            {
                return elfWithMostCalories;
            }

            public int GetTopThreeTotal()
            {
                return topThreeTotal;
            }

            public void dumpElfCaloriesByElf()
            {
                for (int i = 0; i < calorieCounter.Count; i++)
                {
                    int elfId = calorieCounter.ElementAt(i).Key;
                    int elfCalories = calorieCounter.ElementAt(i).Value;
                    Console.WriteLine((i+1).ToString() + ": Elf: " + elfId.ToString() + " Calories: " + elfCalories.ToString());
                }
            }
            public void dumpCaloriesAndTheirElves()
            {
                for (int i = 0; i < sortedCalories.Count; i++)
                {
                    int elfCalories = sortedCalories.ElementAt(i);
                    String elves = "";
                    for (int j = 0; j < caloriesPerElf[elfCalories].Count; j++)
                    {
                        elves += " Elf: " + caloriesPerElf[elfCalories].ElementAt(j);
                    }
                    String elfTitle = " Elf:";
                    if (caloriesPerElf[elfCalories].Count > 1)
                    {
                        elfTitle = " Elves:";
                    }
                    Console.WriteLine((i + 1).ToString() + ": Calories: " + elfCalories.ToString() + " " + elfTitle + elves);
                }
            }
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Syntax: A4D_2022_D01_01 <input_data_file_path>");
                return;
            }

            String inputPath = args[0];
            if (!File.Exists(inputPath))
            {
                Console.WriteLine("File " + inputPath + " does not exist");
                return;
            }

            SOLUTION d1 = new SOLUTION(inputPath);
            int count = d1.GetMostCalories();
            int who = d1.GetWhoCarriesTheCalories();
            int topThreeTotal = d1.GetTopThreeTotal();

            Console.WriteLine("The solution to Part1 is : " + count.ToString() + " is carried by " + who.ToString());
            Console.WriteLine("The solution to Part2 is : " + topThreeTotal.ToString());
            Console.WriteLine("The Elves each Carry:");
            d1.dumpElfCaloriesByElf();
            Console.WriteLine("\nThe Calories each elf is carrying is:");
            d1.dumpCaloriesAndTheirElves();
            Console.WriteLine("\n\nPress any key...");
            Console.ReadKey();
        }
    }
}
