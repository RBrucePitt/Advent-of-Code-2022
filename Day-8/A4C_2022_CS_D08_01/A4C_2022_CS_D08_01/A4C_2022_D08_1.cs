// Advent for code - 2022 - Day 8 - R. Bruce Pitt, 2022/12/08

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;


namespace A4C_2022_CS_D08_01
{
    internal class A4C_2022_D08_1
    {
        public class TREE
        {
            public int x_pos = -1;
            public int y_pos = -1;
            public int tree_height = -1; // 0 to 9
            public bool seen_from_top = false;
            public bool seen_from_bot = false;
            public bool seen_from_right = false;
            public bool seen_from_left = false;
            public bool seen = false;

            public int scenic_top = 0;
            public int scenic_bot = 0;
            public int scenic_right = 0;
            public int scenic_left = 0;
            public int scenic_score = 0;

            public TREE()
            {

            }

            public TREE(int x, int y, int height)
            {
                add(x, y, height);
            }

            public void add(int x, int y, int h)
            {
                x_pos = x;
                y_pos = y;
                tree_height = h;
            }
           
            public void setSeenFromTop()
            {
                seen_from_top = true;
                seen = true;
            }

            public void setSeenFromBot()
            {
                seen_from_bot = true;
                seen = true;
            }

            public void setSeenFromRight()
            {
                seen_from_right = true;
                seen = true;
            }

            public void setSeenFromLeft()
            {
                seen_from_left = true;
                seen = true;
            }

            public void setScenicToTop(int score)
            {
                scenic_top = score;
            }

            public void setScenicToBot(int score)
            {
                scenic_bot = score;
            }

            public void setScenicToRight(int score)
            {
                scenic_right = score;
            }

            public void setScenicToLeft(int score)
            {
                scenic_left = score;
            }

            public void calcScenicScore()
            {
                scenic_score = scenic_top * scenic_bot * scenic_right * scenic_left;
            }
        }

        public class SOLUTION
        {
            String _filePath = "";
            List<String> treeMap = new List<string>();
            TREE[,] trees = null;
            int x_tree_count = 0;
            int y_tree_count = 0;

            int partOneAnswer = 0;
            int partTwoAnswer = 0;
            int partTwoTreeX = -1;
            int partTwoTreeY = -1;

            public SOLUTION(String filePath)
            {
                _filePath = filePath;
                ReadInputForTreeMap();
                BuildTreeMap();
                ProcessFromTop();
                ProcessFromBot();
                ProcessFromRight();
                ProcessFromLeft();
                CountVisibleTrees();
                CalculateScenicFromEachTree();
                CalculateScenicScores();
                FindBestScore();
            }

            private void MarkTheOuterEdges()
            {
                // Now mark all of the edges "seen" - Top and Bot
                for (int x = 0; x < x_tree_count; x++)
                {
                    //trees[0, x] = new TREE(0, x);
                    trees[0, x].setSeenFromTop();
                    //trees[(y_tree_count-1), x] = new TREE((y_tree_count - 1), x);
                    trees[(y_tree_count - 1), x].setSeenFromBot();
                }

                // Mark the left and rights
                for (int y = 0; y < y_tree_count; y++)
                {
                    //trees[y, 0] = new TREE(y, 0);
                    trees[y, 0].setSeenFromLeft();
                    //trees[y, (x_tree_count - 1)] = new TREE(y, (x_tree_count - 1));
                    trees[y, (x_tree_count - 1)].setSeenFromLeft();
                }
            }

            private void BuildTreeMap()
            {
                x_tree_count = treeMap.ElementAt(0).Length;
                y_tree_count = treeMap.Count();

                trees = new TREE[y_tree_count, x_tree_count];

                // Now we are going to process the balance of the map
                for (int y = 0; y < treeMap.Count(); y++)
                {
                    String inputTreeData = treeMap.ElementAt(y);
                    for (int x = 0; x < inputTreeData.Length; x++)
                    {
                        String heightStg = inputTreeData.Substring(x, 1);
                        int height = Convert.ToInt32(heightStg);
                        trees[y, x] = new TREE(x, y, height);
                    }
                }

                MarkTheOuterEdges();
            }

            private void ReadInputForTreeMap()
            {
                StreamReader inHandle = new StreamReader(_filePath);
                String line = "";
                int lineCounter = 0;

                while ((line = inHandle.ReadLine()) != null)
                {
                    if (line != "")
                    {
                        treeMap.Add(line);
                    }
                    lineCounter++;
                }
            }
            private void ProcessFromTop()
            {
                // So we will go cross as x, then down for each Y keeping the last tree height
                // to determine if it can be see. At the start of the top, we always fill last
                // height with first x value across that row.

                for (int x = 0; x < x_tree_count; x++)
                {
                    int savedTreeHeight = trees[0, x].tree_height;
                    for (int y = 1; y < y_tree_count; y++)
                    {
                        if (trees[y, x].tree_height > savedTreeHeight)
                        {
                            trees[y, x].setSeenFromTop();
                            savedTreeHeight = trees[y, x].tree_height;
                        }
                    }
                }
            }

            private void ProcessFromBot()
            {
                for (int x = 0; x < x_tree_count; x++)
                {
                    int savedTreeHeight = trees[(y_tree_count-1), x].tree_height;
                    for (int y = (y_tree_count - 2); y >= 0; y--)
                    {
                        if (trees[y, x].tree_height > savedTreeHeight)
                        {
                            trees[y, x].setSeenFromBot();
                            savedTreeHeight = trees[y, x].tree_height;
                        }
                    }
                }
            }

            private void ProcessFromLeft()
            {
                for (int y = 0; y < y_tree_count; y++)
                {
                    int savedTreeHeight = trees[y, 0].tree_height;
                    for (int x = 1; x < x_tree_count; x++)
                    {
                        if (trees[y, x].tree_height > savedTreeHeight)
                        {
                            trees[y, x].setSeenFromLeft();
                            savedTreeHeight = trees[y, x].tree_height;
                        }
                    }
                }
            }

            private void ProcessFromRight()
            {
                for (int y = 0; y < y_tree_count; y++)
                {
                    int savedTreeHeight = trees[y, (x_tree_count-1)].tree_height;
                    for (int x = (x_tree_count-2); x >= 0; x--)
                    {
                        if (trees[y, x].tree_height > savedTreeHeight)
                        {
                            trees[y, x].setSeenFromLeft();
                            savedTreeHeight = trees[y, x].tree_height;
                        }
                    }
                }
            }

            private void CountVisibleTrees()
            {
                int sumOfVisibleTrees = 0;
                for (int x = 0; x < x_tree_count; x++)
                {
                    for (int y = 0; y < y_tree_count; y++)
                    {
                        if (trees[y, x].seen)
                            sumOfVisibleTrees++;
                    }
                }

                partOneAnswer = sumOfVisibleTrees;
            }

            private void MarkScenicValuesOuterTrees()
            {
                for (int x = 0; x < x_tree_count; x++)
                {
                    trees[0, x].setScenicToTop(0);
                    trees[(y_tree_count -1), x].setScenicToBot(0);
                }
                for (int y = 0; y < x_tree_count; y++)
                {
                    trees[y,0].setScenicToLeft(0);
                    trees[y, (x_tree_count-1)].setScenicToRight(0);

                }
            }

            private int ProcessScenicToTop(int y, int x)
            {
                // From our current position, count trees that we can see until
                // we either hit a y==0 (which we count if tree), or a tree is 
                // equal or greater height than us.

                int myHeight = trees[y, x].tree_height;
                int treeCount = 0;
                while (y > 0)
                {
                    y--;
                    int thisTreeHeight = trees[y, x].tree_height;
                    if (thisTreeHeight != 0)
                    {
                        treeCount++;
                        if (thisTreeHeight >= myHeight)
                            break;
                    }
                    else // view distance - not trees
                    {
                        treeCount++;
                    }
                }
                return (treeCount);
            }

            private int ProcessScenicToBot(int y, int x)
            {
                // From our current position, count trees that we can see until
                // we either hit a y==(total-1) (which we count if tree), or a tree is 
                // equal or greater height than us.

                int myHeight = trees[y, x].tree_height;
                int treeCount = 0;
                while (y < (y_tree_count-1))
                {
                    y++;
                    int thisTreeHeight = trees[y, x].tree_height;
                    if (thisTreeHeight != 0)
                    {
                        treeCount++;
                        if (thisTreeHeight >= myHeight)
                            break;
                    }
                    else // view distance - not trees
                    {
                        treeCount++;
                    }
                }
                return (treeCount);
            }

            private int ProcessScenicToLeft(int y, int x)
            {
                // From our current position, count trees that we can see until
                // we either hit a x==0 (which we count if tree), or a tree is 
                // equal or greater height than us.

                int myHeight = trees[y, x].tree_height;
                int treeCount = 0;
                while (x > 0)
                {
                    x--;
                    int thisTreeHeight = trees[y, x].tree_height;
                    if (thisTreeHeight != 0)
                    {
                        treeCount++;
                        if (thisTreeHeight >= myHeight)
                            break;
                    }
                    else // view distance - not trees
                    {
                        treeCount++;
                    }
                }
                return (treeCount);
            }

            private int ProcessScenicToRight(int y, int x)
            {
                // From our current position, count trees that we can see until
                // we either hit a x==(x_count-1) (which we count if tree), or a tree is 
                // equal or greater height than us.

                int myHeight = trees[y, x].tree_height;
                int treeCount = 0;
                while (x < (x_tree_count-1))
                {
                    x++;
                    int thisTreeHeight = trees[y, x].tree_height;
                    if (thisTreeHeight != 0)
                    {
                        treeCount++;
                        if (thisTreeHeight >= myHeight)
                            break;
                    }
                    else // view distance - not trees
                    {
                        treeCount++;
                    }
                }
                return (treeCount);
            }


            private void ProcessScenicFromThisTree(int y, int x)
            {
                // if we are a side (top, bot, left, right), then just skip
                if ((x == 0) || (y == 0) || (x == (x_tree_count - 1)) || (y == (y_tree_count - 1)))
                    return;

                // No Tree - No View
                if (trees[y, x].tree_height == 0)
                    return;

                trees[x, y].setScenicToTop(ProcessScenicToTop(y, x));
                trees[x, y].setScenicToBot(ProcessScenicToBot(y, x));
                trees[x, y].setScenicToLeft(ProcessScenicToLeft(y, x));
                trees[x, y].setScenicToRight(ProcessScenicToRight(y, x));
            }

            private void CalculateScenicFromEachTree()
            {
                // Mark the outer trees because they all have a score of 0.
                // But we will still need to proess
                MarkScenicValuesOuterTrees();

                for (int x = 0; x < x_tree_count; x++)
                {
                    for (int y = 0; y < y_tree_count; y++)
                    {
                        ProcessScenicFromThisTree(y, x);
                    }
                }
            }

            private void CalculateScenicScores()
            {
                for (int x = 0; x < x_tree_count; x++)
                {
                    for (int y = 0; y < y_tree_count; y++)
                    {
                        trees[y, x].calcScenicScore();
                    }
                }
            }

            private void FindBestScore()
            {
                int bestScore = 0;
                int bestX = -1;
                int bestY = -1;

                for (int x = 0; x < x_tree_count; x++)
                {
                    for (int y = 0; y < y_tree_count; y++)
                    {
                        trees[y, x].calcScenicScore();
                        if (trees[y,x].scenic_score > bestScore)
                        {
                            bestScore = trees[y, x].scenic_score;
                            bestX = x;
                            bestY = y;
                        }
                    }
                }

                partTwoAnswer = bestScore;
                partTwoTreeX = bestX;
                partTwoTreeY = bestY;
            }

            public int GetPartOneAnswer()
            {
                return partOneAnswer;
            }

            public int GetPartTwoAnswer()
            {
                return partTwoAnswer;
            }

            public void DumpVisibleForest()
            {
                for (int y = 0; y < y_tree_count; y++)
                {
                    for (int x = 0; x < x_tree_count; x++)
                    {
                        if (trees[y, x].seen)
                            Console.Write(trees[y, x].tree_height.ToString());
                        else
                            Console.Write("-");
                    }
                    Console.Write("\n");
                }
            }

            public void NonDumpVisibleForest()
            {
                for (int y = 0; y < y_tree_count; y++)
                {
                    for (int x = 0; x < x_tree_count; x++)
                    {
                        if (!trees[y, x].seen)
                            Console.Write(trees[y, x].tree_height.ToString());
                        else
                            Console.Write("-");
                    }
                    Console.Write("\n");
                }
            }
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Syntax: A4D_2022_D08_01 <input_data_file_path>");
                return;
            }

            String inputPath = args[0];
            if (!File.Exists(inputPath))
            {
                Console.WriteLine("File " + inputPath + " does not exist");
                return;
            }

            SOLUTION d8 = new SOLUTION(inputPath);
            //d8.DumpForest();
            int get_part1 = d8.GetPartOneAnswer();
            int get_part2 = d8.GetPartTwoAnswer();

            Console.WriteLine("The solution to Part1 is : " + get_part1.ToString());
            Console.WriteLine("The solution to Part2 is : " + get_part2.ToString());

            Console.WriteLine("\n\nPress any key...");
            Console.ReadKey();

            Console.WriteLine("Visible Forrest: ");
            d8.DumpVisibleForest();
            Console.WriteLine("\n\nPress any key...");
            Console.ReadKey();

            Console.WriteLine("Non Visible Forrest: ");
            d8.DumpVisibleForest();
            Console.WriteLine("\n\nPress any key...");
            Console.ReadKey();
        }
    }
}
