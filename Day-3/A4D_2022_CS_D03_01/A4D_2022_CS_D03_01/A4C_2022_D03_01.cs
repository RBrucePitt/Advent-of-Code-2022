// Advent for code - 2022 - Day 3 - R. Bruce Pitt, 2022/12/03

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace A4D_2022_CS_D03_01
{
    internal class A4C_2022_D03_01
    {
        public class RUCKSACK
        {
            public String allComparements = "";
            public String rightComparment = "";
            public String leftComparment = "";
            public int priority = 0;
            public String sameItem = "";
            public String badgeItem = "";
            public int badgePriority = 0;

            public RUCKSACK(String fillInfo)
            {
                allComparements = fillInfo;
                int len = fillInfo.Length;
                rightComparment = fillInfo.Substring(0, len/2);
                leftComparment = fillInfo.Substring(len/2);
            }

            public String whatTheSame()
            {
                String retValue = "";

                for (int i = 0; i < rightComparment.Length; i++)
                {
                    String thisChar = rightComparment.Substring(i, 1);
                    if (leftComparment.IndexOf(thisChar) != -1)
                    {
                        retValue = thisChar;
                        break;
                    }
                }

                sameItem = retValue;

                return retValue;
            }

            public void setPriority(int value)
            {
                priority = value;
            }

            public void setBadgeItem(String item)
            {
                badgeItem = item;
            }

            public void setBadgePriority(int value)
            {
                badgePriority = value;
            }
        }

        public class SOLUTION
        {
            String _filePath = "";
            int totalPriority = 0;
            int totalBadgePriority = 0;

            List<String> sackContents = new List<String>();
            List<RUCKSACK> sackInfo = new List<RUCKSACK>();

            String priorityValue = "0abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            public SOLUTION(String filePath)
            {
                _filePath = filePath;
                ReadInputIntoList();
                ScanSacks();
            }

            private void ReadInputIntoList()
            {
                StreamReader inHandle = new StreamReader(_filePath);
                String line = "";
                int lineCounter = 0;

                while ((line = inHandle.ReadLine()) != null)
                {
                    if (line != "")
                        sackContents.Add(line);
                    lineCounter++;
                }
            }

            private void ScanSacks()
            {
                for (int i = 0; i < sackContents.Count; i++)
                {
                    RUCKSACK sack = new RUCKSACK(sackContents[i]);
                    String oopsItems = sack.whatTheSame();
                    int priority = priorityValue.IndexOf(oopsItems);
                    totalPriority += priority;
                    sack.setPriority(priority);
                    sackInfo.Add(sack);
                }
            }

            public void Scan4Badges()
            {
                for (int i = 0; i < sackContents.Count; i += 3)
                {
                    RUCKSACK sack1 = sackInfo[i];
                    RUCKSACK sack2 = sackInfo[i+1];
                    RUCKSACK sack3 = sackInfo[i+2];

                    String badgeItemFound = "";
                    for (int j = 0; j < sack1.allComparements.Length; j++)
                    {
                        String checkItem = sack1.allComparements.Substring(j, 1);
                        if (sack2.allComparements.IndexOf(checkItem) != -1)
                        {
                            if (sack3.allComparements.IndexOf(checkItem) !=-1)
                            {
                                badgeItemFound = checkItem;
                                break;
                            }
                        }
                    }

                    sackInfo[i].setBadgeItem(badgeItemFound);
                    sackInfo[i+1].setBadgeItem(badgeItemFound);
                    sackInfo[i+2].setBadgeItem(badgeItemFound);

                    int priority = priorityValue.IndexOf(badgeItemFound);
                    totalBadgePriority += priority;

                    sackInfo[i].setBadgePriority(priority);
                    sackInfo[i+1].setBadgePriority(priority);
                    sackInfo[i+2].setBadgePriority(priority);
                }
            }

            public int GetTotalPriority()
            {
                return totalPriority;
            }

            public int GetTotalBadgePriority()
            {
                return totalBadgePriority;
            }

        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Syntax: A4D_2022_D03_01 <input_data_file_path>");
                return;
            }

            String inputPath = args[0];
            if (!File.Exists(inputPath))
            {
                Console.WriteLine("File " + inputPath + " does not exist");
                return;
            }

            SOLUTION d3 = new SOLUTION(inputPath);
            int totalPriority = d3.GetTotalPriority();
            d3.Scan4Badges();
            int totalBadgePriority = d3.GetTotalBadgePriority();

            Console.WriteLine("The solution to Part1 is : " + totalPriority.ToString());
            Console.WriteLine("The solution to Part2 is : " + totalBadgePriority.ToString());

            Console.WriteLine("\n\nPress any key...");
            Console.ReadKey();
        }
    }
}
