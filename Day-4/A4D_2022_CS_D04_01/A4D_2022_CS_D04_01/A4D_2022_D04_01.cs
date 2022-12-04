// Advent for code - 2022 - Day 4 - R. Bruce Pitt, 2022/12/04

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace A4D_2022_CS_D04_01
{
    internal class A4D_2022_D04_01
    {
        public class ELFSECTION
        {
            public int low_range = 0;
            public int high_range = 0;

            public ELFSECTION(String sectionInfo)
            {
                // 2-4
                String [] range = sectionInfo.Split('-');
                low_range = Convert.ToInt32(range[0]);
                high_range = Convert.ToInt32(range[1]);
            }

            public int inMine(ref ELFSECTION otherElf)
            {
                if ((otherElf.low_range >= low_range) && (otherElf.high_range <= high_range))
                    return 1;
                return 0;
            }

            public int inOther(ref ELFSECTION otherElf)
            {
                if ((low_range >= otherElf.low_range) && (high_range <= otherElf.high_range))
                    return 1;
                return 0;
            }

            public int anyOverlap(ref ELFSECTION otherElf)
            {
                if (low_range <= otherElf.high_range)
                {
                    if (high_range >= otherElf.low_range)
                    {
                        return 1;
                    }
                }

                return 0;
            }
        }

        public class ELFPAIRS
        {
            public ELFSECTION elf1;
            public ELFSECTION elf2;

            public int oneInTheOther = 0;
            public int someOverlap = 0;

            public ELFPAIRS(String pairString)
            {
                // 2-4,6-8
                String[] pairs = pairString.Split(',');
                elf1 = new ELFSECTION(pairs[0]);
                elf2 = new ELFSECTION(pairs[1]);

                if (elf1.inMine(ref elf2) == 1)
                {
                    oneInTheOther = 1;
                }
                else if (elf2.inMine(ref elf1) == 1)
                {
                    oneInTheOther = 1;
                }
                else if (elf1.anyOverlap(ref elf2) == 1)
                { 
                    someOverlap = 1;
                }
                else if (elf2.anyOverlap(ref elf1) == 1)
                {
                    someOverlap = 1;
                }
            }
        }

        public class SOLUTION
        {
            String _filePath = "";
            int totalOverlap = 0;
            int totalAnyOverlap = 0;
            List<ELFPAIRS> foundPairs = new List<ELFPAIRS> ();

            public SOLUTION(String filePath)
            {
                _filePath = filePath;
                ReadInputIntoPairs();
                ScanPairs();
            }

            private void ReadInputIntoPairs()
            {
                StreamReader inHandle = new StreamReader(_filePath);
                String line = "";
                int lineCounter = 0;

                while ((line = inHandle.ReadLine()) != null)
                {
                    if (line != "")
                    {
                        ELFPAIRS newPair = new ELFPAIRS(line);
                        foundPairs.Add(newPair);
                    }
                    lineCounter++;
                }
            }

            private void ScanPairs()
            {
                for (int i = 0; i < foundPairs.Count; i++)
                {
                    ELFPAIRS pair = foundPairs[i];
                    totalOverlap += pair.oneInTheOther;
                    totalAnyOverlap += pair.someOverlap;
                }
            }

            public int GetTotalOverlap()
            {
                return totalOverlap;
            }

            public int GetTotalAnyOverlap()
            {
                return totalAnyOverlap+totalOverlap;
            }

        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Syntax: A4D_2022_D04_01 <input_data_file_path>");
                return;
            }

            String inputPath = args[0];
            if (!File.Exists(inputPath))
            {
                Console.WriteLine("File " + inputPath + " does not exist");
                return;
            }

            SOLUTION d4 = new SOLUTION(inputPath);
            int totalOverlappingPairs = d4.GetTotalOverlap();
            int totalAnyOverlappingPairs = d4.GetTotalAnyOverlap();


            Console.WriteLine("The solution to Part1 is : " + totalOverlappingPairs.ToString());
            Console.WriteLine("The solution to Part2 is : " + totalAnyOverlappingPairs.ToString());

            Console.WriteLine("\n\nPress any key...");
            Console.ReadKey();
        }
    }
}
