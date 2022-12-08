// Advent for code - 2022 - Day 6 - R. Bruce Pitt, 2022/12/06

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace A4C_2022_CS_D07_01
{
    internal class A4C_2022_D07_01
    {
        public class FILENODE
        {
            public String fileName;
            public UInt64 fileSize;
            public String parentDir;
            public int dirLevel;

            public FILENODE(String fileName, UInt64 fileSize, String parentDir, int dirLevel)
            {
                this.fileName = fileName;
                this.fileSize = fileSize;
                this.parentDir = parentDir;
                this.dirLevel = dirLevel;
            }

            public FILENODE(String fileName, String fileSizeStg, String parentDir, int dirLevel)
            {
                this.fileName = fileName;
                this.fileSize = Convert.ToUInt64(fileSizeStg);
                this.parentDir = parentDir;
                this.dirLevel = dirLevel;
            }

        }

        public class DIRNODE
        {
            public String dirName;
            public UInt64 myFileSizes;      // sum of my file sizes
            public UInt64 subsFileSizes;    // sum of all sub-directory file sizes
            public String parentDir;
            public int myDirLevel;
            public String breadCrumb;       // Dirs down to here "/" separator

            public Dictionary<String, FILENODE> myFiles = new Dictionary<String, FILENODE>();
            public Dictionary<String, DIRNODE> subDirs = new Dictionary<String, DIRNODE>();

            public DIRNODE(String name, String parent, int dirLevel, String breadCrumb)
            {
                dirName = name;
                parentDir = parent;
                myDirLevel = dirLevel;
                breadCrumb = breadCrumb + "/" + name;
            }

            public UInt64 getMyDirSize()
            {
                return (myFileSizes + subsFileSizes);
            }
        }

        public class SOLUTION
        {
            String _filePath = "";
            List<String> commandLog = new List<string>();
            int commandLogPos = 0;

            DIRNODE topNode = null;

            UInt64 partOneAnswer = 0;
            String partTwoAnswerDirName = "";
            UInt64 partTwoAnswerDirSize = ulong.MaxValue;

            public SOLUTION(String filePath)
            {
                _filePath = filePath;
                ReadInputForCommandLog();
                BuildDirFileHeirachy();
                CalculateSizingAcrossHeirachy();
                DeterminePartOneAnswer();
                DeterminePartTwoAnswer();
            }

            private void ReadInputForCommandLog()
            {
                StreamReader inHandle = new StreamReader(_filePath);
                String line = "";
                int lineCounter = 0;

                while ((line = inHandle.ReadLine()) != null)
                {
                    if (line != "")
                    {
                        commandLog.Add(line);
                    }
                    lineCounter++;
                }
            }

            private void processCommandLog(String dirName, ref DIRNODE parentNode, int dirLevel, String breadCrumb)
            {
                // We are now in a directory and we are going to look for specific log entries
                // which are commands "$" <cd>, <ls> and then command results
                // ls: "<size> <filename>", "dir <dirname>" 
                dirLevel++;

                // Loop until we see a "cd .." which means we are done at this level
                while (true)
                {
                    if (commandLogPos >= commandLog.Count)
                        return;

                    // Get command information
                    String cmdLog = commandLog[commandLogPos];
                    commandLogPos++;
                    String[] cmdItems = cmdLog.Split(' ');

                    switch (cmdItems[0])
                    {
                        case "$":
                            if (cmdItems[1] == "cd")
                            {
                                // moved up or down
                                if (cmdItems[2] == "..")
                                    return;
                                // Just in case the commands are out of order (no ls, prior to cd)
                                DIRNODE thisSubDir = null;
                                if (!parentNode.subDirs.ContainsKey(cmdItems[2]))
                                {
                                    thisSubDir = new DIRNODE(cmdItems[2], parentNode.dirName, dirLevel, parentNode.breadCrumb);
                                    parentNode.subDirs.Add(cmdItems[2], thisSubDir);
                                }
                                else
                                {
                                    thisSubDir = parentNode.subDirs[cmdItems[2]];
                                }

                                processCommandLog(cmdItems[2], ref thisSubDir, dirLevel, parentNode.breadCrumb);
                            }
                            break;
                        case "dir":
                            if (!parentNode.subDirs.ContainsKey(cmdItems[1]))
                            {
                                DIRNODE thisSubDir = new DIRNODE(cmdItems[1], parentNode.dirName, dirLevel, parentNode.breadCrumb);
                                parentNode.subDirs.Add(cmdItems[1], thisSubDir);
                            }
                            break;
                        default:    // should be a number
                            int result = 0;
                            if (int.TryParse(cmdItems[0],out result))  // Is a number, therefore a file
                            {
                                FILENODE thisFile = new FILENODE(cmdItems[1].Trim(), Convert.ToUInt64(result), parentNode.dirName, dirLevel);
                                parentNode.myFiles.Add(cmdItems[1].Trim(), thisFile);
                            }
                            break;
                    }
                }
            }

            private void BuildDirFileHeirachy()
            {
                // Seed the first call - should be $ cd /
                String cmdLog = commandLog[commandLogPos];
                commandLogPos++;
                String [] cmdItems = cmdLog.Split(' ');
                if (cmdItems[0].Trim() == "$") // Command
                {
                    String cmd = cmdItems[1].Trim();
                    if (cmd == "cd")
                    {
                        String subDirName = cmdItems[2].Trim();
                        topNode = new DIRNODE(subDirName, "", 0, "");

                        processCommandLog(subDirName, ref topNode, 0, topNode.breadCrumb);
                    }
                }
            }

            private UInt64 CaculateDirSizingRentrant(ref DIRNODE dirNode)
            {
                // Setup our sums
                UInt64 totalFileSize = 0;
                UInt64 totalSubDirSize = 0;

                // First we calculate any local files
                for (int i = 0; i < dirNode.myFiles.Count; i++)
                {
                    FILENODE thisFile = dirNode.myFiles.ElementAt(i).Value;
                    totalFileSize += thisFile.fileSize;
                }

                dirNode.myFileSizes = totalFileSize;

                // Now we run down the directory list and sum the response we get
                for (int i = 0; i < dirNode.subDirs.Count; i++)
                {
                    DIRNODE thisNode = dirNode.subDirs.ElementAt(i).Value;
                    UInt64 thisSubDirSize = CaculateDirSizingRentrant(ref thisNode);
                    totalSubDirSize += thisSubDirSize;

                }

                dirNode.subsFileSizes = totalSubDirSize;
                return (totalFileSize + totalSubDirSize);
            }

            private void CalculateSizingAcrossHeirachy()
            {
                // Seed into reentant function
                UInt64 totalSize = CaculateDirSizingRentrant(ref topNode);
            }

            private UInt64 CalculateSizingUnderThreshold(ref DIRNODE nextNode, UInt64 threshold)
            {
                // Drop down through our directories until there are no directories, then
                // as each call returns, calculate the summation of the size if below
                // the threshold. Ignore all files, Only directories. But we have to hit bottom 
                // first.

                UInt64 sumAtThisLevel = 0;
                if (nextNode.getMyDirSize() < threshold)
                {
                    sumAtThisLevel = nextNode.getMyDirSize();
                }

                for (int i = 0; i < nextNode.subDirs.Count; i++)
                {
                    DIRNODE nextSubDir = nextNode.subDirs.ElementAt(i).Value;
                    sumAtThisLevel += CalculateSizingUnderThreshold(ref nextSubDir, threshold);
                }

                return (sumAtThisLevel);
            }

            private void DeterminePartOneAnswer()
            {
                // Find all of the directories with a total size of at most 100000.
                // What is the sum of the total sizes of those directories?

                // To begin, find all of the directories with a total size of at most 100000,
                // then calculate the sum of their total sizes. In the example above,
                // these directories are a and e; the sum of their total sizes is 95437
                // (94853 + 584). (As in this example, this process can count files more than once!)

                UInt64 totalUnderThreshold = CalculateSizingUnderThreshold(ref topNode, 100000);
                partOneAnswer = totalUnderThreshold;
            }

            private void FindDirClosestToThisSize(ref DIRNODE nextNode, UInt64 neededAmount)
            {
                if (nextNode.getMyDirSize() >= neededAmount)
                {
                    if (nextNode.getMyDirSize() < partTwoAnswerDirSize)
                    {
                        partTwoAnswerDirSize = nextNode.getMyDirSize();
                        partTwoAnswerDirName = nextNode.dirName;
                    }
                }

                for (int i = 0; i < nextNode.subDirs.Count; i++)
                {
                    DIRNODE nextSubDir = nextNode.subDirs.ElementAt(i).Value;
                    FindDirClosestToThisSize(ref nextSubDir, neededAmount);
                }
            }

            private void DeterminePartTwoAnswer()
            {
                // The total disk space available to the filesystem is 70000000. To run the update,
                // you need unused space of at least 30000000. You need to find a directory you can
                // delete that will free up enough space to run the update.

                // Current used total space from topNode
                UInt64 curUsed = topNode.getMyDirSize();
                UInt64 maxAvai      = 70000000;
                UInt64 updNeeded    = 30000000;
                UInt64 curAvai      = maxAvai - curUsed;
                UInt64 needed4Upd   = updNeeded - curAvai;

                FindDirClosestToThisSize(ref topNode, needed4Upd);
            }

            public UInt64 GetPartOneAnswer()
            {
                return partOneAnswer;
            }

            public UInt64 GetPartTwoAnswer()
            {
                return partTwoAnswerDirSize;
            }

            public String GetPartTwoDirName()
            {
                return partTwoAnswerDirName;
            }

            public void DumpADir(ref DIRNODE dirNode)
            {
                // Output my dir first
                String tabs = "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t".Substring(0, dirNode.myDirLevel);
                Console.WriteLine(tabs + " " + dirNode.dirName + "[" + (dirNode.subsFileSizes + dirNode.myFileSizes).ToString() + "]:");
                // Then my Files
                for (int i = 0; i < dirNode.myFiles.Count; i++)
                {
                    Console.WriteLine(tabs + "\t" + dirNode.myFiles.ElementAt(i).Value.fileName + " Size:" + dirNode.myFiles.ElementAt(i).Value.fileSize.ToString());
                }

                // Then the subdirs
                for (int i = 0; i < dirNode.subDirs.Count; i++)
                {
                    DIRNODE subDirNode = dirNode.subDirs.ElementAt(i).Value;
                    DumpADir(ref subDirNode);
                }
            }

            public void DumpFileSystem()
            {
                DumpADir(ref topNode);
            }

        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Syntax: A4D_2022_D07_01 <input_data_file_path>");
                return;
            }

            String inputPath = args[0];
            if (!File.Exists(inputPath))
            {
                Console.WriteLine("File " + inputPath + " does not exist");
                return;
            }

            SOLUTION d7 = new SOLUTION(inputPath);
            d7.DumpFileSystem();
            UInt64 get_part1 = d7.GetPartOneAnswer();
            UInt64 get_part2 = d7.GetPartTwoAnswer();
            String get_part2_name = d7.GetPartTwoDirName();

            Console.WriteLine("The solution to Part1 is : " + get_part1.ToString());
            Console.WriteLine("The solution to Part2 is : " + get_part2.ToString() + " using dir:" + get_part2_name);

            Console.WriteLine("\n\nPress any key...");
            Console.ReadKey();
        }
    }
}
