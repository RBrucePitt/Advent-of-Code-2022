using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace A4C_2022_CS_D02_01
{
    internal class A4C_2022_D02_01
    {
        public class SOLUTION
        {
            String _filePath = "";
            int gamesPlayed = 0;
            int totalScore = 0;
            int strategyScore = 0;

            List<String> otherElfPlay = new List<String>();
            List<String> myPlay = new List<string>();
            List<int> gameResults = new List<int>();

            String [] itemsStg = new string[3]{ "Rock", "Paper", "Sizzors" };

            // Modifyable ROCK/PAPER/SIZZOR for input parameters
            String myModel = "XYZ"; // Pos 1 is Rock, Pos 2 is Paper, Pos 3 is Sissors
            String otherModel = "ABC";

            public SOLUTION(String filePath)
            {
                _filePath = filePath;
                ReadInputIntoList();
                PlayRockPaperSissors();
            }

            private void ReadInputIntoList()
            {
                StreamReader inHandle = new StreamReader(_filePath);
                String line = "";
                int lineCounter = 0;

                while ((line = inHandle.ReadLine()) != null)
                {
                    String[] play = line.Split(' ');
                    otherElfPlay.Add(play[0]);
                    myPlay.Add(play[1]);
                    lineCounter++;
                }
            }

            private int scoreFromPlay(String otherElfMove, String myPlayMove)
            {
                // The score for a single round is the score for the shape
                // you selected (1 for Rock, 2 for Paper, and 3 for Scissors)
                // plus the score for the outcome of the round (0 if you lost,
                // 3 if the round was a draw, and 6 if you won).

                // The first column is what your opponent is going to play:
                // A for Rock, B for Paper, and C for Scissors.
                // The second column, you reason, must be what you should play in response:
                // X for Rock, Y for Paper, and Z for Scissors

                int myPlayItem = myModel.IndexOf(myPlayMove) + 1;   // 1 is Rock, 2 is Paper, 3 is Sizzors
                int opponentItem = otherModel.IndexOf(otherElfMove) + 1; 
                int itemScore = myModel.IndexOf(myPlayMove) + 1;

                // Play Game
                // Rock (1) defeats Scissors (3), Scissors (3) defeats Paper (2), and Paper(2) defeats Rock (1).
                int gameScore = 0;
                if (opponentItem == myPlayItem)
                {
                    gameScore = 3;
                }
                else if (opponentItem == 1) // Rock
                {
                    if (myPlayItem == 2) // Paper
                        gameScore = 6;
                }
                else if (opponentItem == 2) // Paper
                {
                    if (myPlayItem == 3) // Sizzors
                        gameScore = 6;
                }
                else if (opponentItem == 3)
                {
                    if (myPlayItem == 1) // Rock
                        gameScore = 6;
                }

                return (itemScore + gameScore);
            }


            private void PlayRockPaperSissors()
            {
                for (int i = 0; i < otherElfPlay.Count; i++)
                {
                    String otherElfMove = otherElfPlay.ElementAt(i);
                    String myPlayMove = myPlay.ElementAt(i);
                    int score = scoreFromPlay(otherElfMove, myPlayMove);
                    totalScore += score;
                    gameResults.Add(score);
                    gamesPlayed++;
                }
            }

            public void PlayRPS_UsingWinLoseDraw()
            {
                for (int i = 0; i < otherElfPlay.Count; i++)
                {
                    String otherElfMove = otherElfPlay.ElementAt(i);
                    String myPlayStrategy = myPlay.ElementAt(i);
                    String myPlayMove = "";
                    switch (myPlayStrategy)
                    {
                        case "X": // Lose
                            if (otherElfMove == "A")
                                myPlayMove = myModel.ElementAt(2).ToString();
                            else if (otherElfMove == "B")
                                myPlayMove = myModel.ElementAt(0).ToString();
                            else
                                myPlayMove = myModel.ElementAt(1).ToString();
                            break;
                        case "Y": // Draw
                            myPlayMove = myModel.ElementAt(otherModel.IndexOf(otherElfMove)).ToString();
                            break;
                        case "Z": // Win
                            if (otherElfMove == "A")
                                myPlayMove = myModel.ElementAt(1).ToString();
                            else if (otherElfMove == "B")
                                myPlayMove = myModel.ElementAt(2).ToString();
                            else
                                myPlayMove = myModel.ElementAt(0).ToString();
                            break;
                    }

                    int score = scoreFromPlay(otherElfMove, myPlayMove);
                    strategyScore += score;
                }
            }

            public int GetTotalScore()
            {
                return totalScore;
            }

            public int GetTotalStrategyScore()
            {
                return strategyScore;
            }

            public int GetGamesPlayed()
            {
                return gamesPlayed;
            }

            public void ChangeModel(String newModel)
            {
                myModel = newModel;
                totalScore = 0;
                PlayRockPaperSissors();
            }

            public void dumpGames()
            {
                for (int i = 0; i < otherElfPlay.Count; i++)
                {
                    String oppPlayItem = otherElfPlay.ElementAt(i);
                    String myPlayItem = myPlay.ElementAt(i);
                    int score = gameResults.ElementAt(i);

                    String oppStg = itemsStg[otherModel.IndexOf(oppPlayItem)];
                    String myStg = itemsStg[myModel.IndexOf(oppPlayItem)];

                    Console.WriteLine((i + 1).ToString() + ": Opponent:" + oppStg + " MyPlay:" + myStg + " Score: " + score);
                }
            }
         }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Syntax: A4D_2022_D02_01 <input_data_file_path>");
                return;
            }

            String inputPath = args[0];
            if (!File.Exists(inputPath))
            {
                Console.WriteLine("File " + inputPath + " does not exist");
                return;
            }

            SOLUTION d2 = new SOLUTION(inputPath);
            int myScore_Default = d2.GetTotalScore();
            d2.PlayRPS_UsingWinLoseDraw();
            int myScore_Strategy = d2.GetTotalStrategyScore();
            d2.ChangeModel("XYZ");
            int myScore_M1 = d2.GetTotalScore();
            d2.ChangeModel("YZX");
            int myScore_M2 = d2.GetTotalScore();
            d2.ChangeModel("ZXY");
            int myScore_M3 = d2.GetTotalScore();

            Console.WriteLine("The solution to Part1 is : " + myScore_Default.ToString());
            Console.WriteLine("The solution to XYZ is : " + myScore_M1.ToString());
            Console.WriteLine("The solution to XZY is : " + myScore_M2.ToString());
            Console.WriteLine("The solution to ZXY is : " + myScore_M2.ToString());
            Console.WriteLine("The solution to Strategy is : " + myScore_Strategy.ToString());

            Console.WriteLine("\n\nPress any key...");
            Console.ReadKey();
        }
    }
}
