// Advent for code - 2022 - Day 5 - R. Bruce Pitt, 2022/12/05

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace A4D_2022_CS_D05_01
{
    internal class A4D_2022_D05_01
    {
        public class SOLUTION
        {
            String _filePath = "";
            const int stackFlatFileSpacing = 4; // [B] [L] [Q] [W] [S] [L] [J] [W] [Z]
            int stacksFound = 0;
            Stack<String> stacksRawInput = new Stack<String>();
            List<String> movesFound = new List<String>();

            Stack<String> [] stacks = null;
            Stack<String> [] stacksPart2 = null;

            public SOLUTION(String filePath)
            {
                _filePath = filePath;
                ReadInputForStackAndMoves();
                ProcessInitialStacks();
                RunMoves();
                RunMovesPart2();
            }

            private void ReadInputForStackAndMoves()
            {
                StreamReader inHandle = new StreamReader(_filePath);
                String line = "";
                int lineCounter = 0;
                Boolean inputStackInfo = true;

                while ((line = inHandle.ReadLine()) != null)
                {
                    if (line != "")
                    {
                        if (inputStackInfo)
                        {
                            stacksRawInput.Push(line);
                        }
                        else
                        {
                            movesFound.Add(line);
                        }
                    }
                    else if (inputStackInfo)
                    {
                        inputStackInfo = false;
                    }
                    lineCounter++;
                }
            }

            private void ProcessInitialStacks()
            {
                // We do this backwards because this tells us the number of stacks/piles
                // and the stack lettering spacing

                String stackCount = stacksRawInput.Pop();
                stackCount = stackCount.Replace("  ", " ");
                stackCount = stackCount.Trim();
                String[] stacksNums = stackCount.Split(' ');
                // eliminate the empties
                List<String> actualNums = new List<String>();
                for (int i = 0; i < stacksNums.Length; i++)
                {
                    if (stacksNums[i] != "")
                        actualNums.Add(stacksNums[i]);
                }
                stacksFound = actualNums.Count + 1; // Leaving zero empty

                // Create out Stacks
                stacks = new Stack<String>[stacksFound];
                stacksPart2 = new Stack<String>[stacksFound];
                for (int i = 0; i < stacksFound; i++)
                {
                    stacks[i] = new Stack<String>();
                    stacksPart2[i] = new Stack<String>();
                }

                // These will be from the bottom up
                while (stacksRawInput.Count > 0)
                {
                    String stacksInfo = stacksRawInput.Pop();
                    int ofs = 0;
                    for (int j = 1; j < stacksFound; j++)
                    {
                        String thisStackStg = stacksInfo.Substring(ofs, (stackFlatFileSpacing - 1));
                        thisStackStg = thisStackStg.Trim();
                        if (thisStackStg != "")
                        {
                            int itemNamePos = thisStackStg.IndexOf("[");
                            stacks[j].Push(thisStackStg[itemNamePos + 1].ToString());
                            stacksPart2[j].Push(thisStackStg[itemNamePos + 1].ToString());
                        }
                        ofs += stackFlatFileSpacing;
                    }
                }
            }

            private void RunMoves()
            {
                for (int i = 0; i < movesFound.Count; i++)
                {
                    // move 3 from 5 to 2
                    String thisMove = movesFound[i];
                    String [] components = thisMove.Split(' ');
                    int itemsToMove = Convert.ToInt32(components[1]);
                    int srcStack = Convert.ToInt32(components[3]);
                    int destStack = Convert.ToInt32(components[5]);

                    for (int itemsLoop = 0; itemsLoop < itemsToMove; itemsLoop++)
                    {
                        String item2move = stacks[srcStack].Pop();
                        stacks[destStack].Push(item2move);
                    }
                }
            }

            private void RunMovesPart2()
            {
                for (int i = 0; i < movesFound.Count; i++)
                {
                    // move 3 from 5 to 2
                    String thisMove = movesFound[i];
                    String[] components = thisMove.Split(' ');
                    int itemsToMove = Convert.ToInt32(components[1]);
                    int srcStack = Convert.ToInt32(components[3]);
                    int destStack = Convert.ToInt32(components[5]);

                    // Part 2 change - crane works with items in same order.
                    Stack<String> cranePull = new Stack<String>();
                    for (int itemsLoop = 0; itemsLoop < itemsToMove; itemsLoop++)
                        cranePull.Push(stacksPart2[srcStack].Pop());

                    // Now put on the other stack
                    for (int itemsLoop = 0; itemsLoop < itemsToMove; itemsLoop++)
                        stacksPart2[destStack].Push(cranePull.Pop());
                }
            }

            public String GetFinalTops()
            {
                String stacksTop = "";

                for (int i = 1; i < stacksFound; i++)
                {
                    stacksTop += stacks[i].Peek();
                }

                return stacksTop;
            }

            public String GetFinalTopsPart2()
            {
                String stacksTop = "";

                for (int i = 1; i < stacksFound; i++)
                {
                    stacksTop += stacksPart2[i].Peek();
                }

                return stacksTop;
            }
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Syntax: A4D_2022_D05_01 <input_data_file_path>");
                return;
            }

            String inputPath = args[0];
            if (!File.Exists(inputPath))
            {
                Console.WriteLine("File " + inputPath + " does not exist");
                return;
            }

            SOLUTION d5 = new SOLUTION(inputPath);
            String getFinalTopPackages = d5.GetFinalTops();
            String getFinalTopPackagesPart2 = d5.GetFinalTopsPart2();

            Console.WriteLine("The solution to Part1 is : " + getFinalTopPackages);
            Console.WriteLine("The solution to Part2 is : " + getFinalTopPackagesPart2);

            Console.WriteLine("\n\nPress any key...");
            Console.ReadKey();
        }
    }
}
