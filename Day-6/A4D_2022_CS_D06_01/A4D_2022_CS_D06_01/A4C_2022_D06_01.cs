// Advent for code - 2022 - Day 6 - R. Bruce Pitt, 2022/12/06

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace A4D_2022_CS_D06_01
{
    internal class A4C_2022_D06_01
    {
        public class SOLUTION
        {
            String _filePath = "";
            int startOfTx = 0;
            int startOfMsg = 0;
            String rxChars = "";

            public SOLUTION(String filePath)
            {
                _filePath = filePath;
                ReadInputForRadioTx();
                startOfTx = FindStart(4);   // Start of packet marker is 4
                startOfMsg = FindStart(14) + 1; // Start of message marker is 14
                                                // The next character is the start of message (+1)
            }

            private void ReadInputForRadioTx()
            {
                StreamReader inHandle = new StreamReader(_filePath);
                String line = "";
                int lineCounter = 0;

                while ((line = inHandle.ReadLine()) != null)
                {
                    if (line != "")
                    {
                        rxChars = line;
                    }
                    lineCounter++;
                }
            }

            private int FindStart(int uniqueChars)
            {
                // We do this backwards because this tells us the number of stacks/piles
                // and the stack lettering spacing
                String lastSet = "";
                int inputPos = 0;
                Boolean found = false;

                while ((!found) && (inputPos < rxChars.Length))
                {
                    char nextChar = rxChars[inputPos];
                    if (lastSet.Length >= 1)
                    {
                        int fourPos = lastSet.IndexOf(nextChar);
                        if (fourPos == -1)
                        {
                            lastSet += nextChar;
                            if (lastSet.Length == uniqueChars)   // Found
                            {
                                found = true;
                                Console.WriteLine("Size:[" + uniqueChars + "] Last Set: " + lastSet);
                                return (inputPos+1);
                            }
                        }
                        else
                        {
                            lastSet = lastSet.Substring(fourPos + 1);
                            lastSet += nextChar.ToString();
                            // lastSet = lastSet.Substring(fourPos);
                            // lastSet = nextChar.ToString();
                        }
                    }
                    else
                    {
                        lastSet += nextChar;
                    }
                    inputPos++;
                }

                Console.WriteLine("ERROR: Size:[" + uniqueChars + "] Last Set: " + lastSet);
                return (inputPos);
            }

            public int GetStart()
            {
                return startOfTx;
            }

            public int GetStart2()
            {
                return startOfMsg-1;
            }
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Syntax: A4D_2022_D06_01 <input_data_file_path>");
                return;
            }

            String inputPath = args[0];
            if (!File.Exists(inputPath))
            {
                Console.WriteLine("File " + inputPath + " does not exist");
                return;
            }

            SOLUTION d6 = new SOLUTION(inputPath);
            int getStart_part1 = d6.GetStart();
            int getStart_part2 = d6.GetStart2();

            Console.WriteLine("The solution to Part1 is : " + getStart_part1.ToString());
            Console.WriteLine("The solution to Part2 is : " + getStart_part2.ToString());

            Console.WriteLine("\n\nPress any key...");
            Console.ReadKey();
        }
    }
}
