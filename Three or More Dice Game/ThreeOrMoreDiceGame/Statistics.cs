using System;
using System.Collections.Generic;

namespace ThreeOrMoreDiceGame
{
    // Start of the Statistics class
    class Statistics
    {
        // Statistics class fields for:
        // Number of Wins, Number of Losses, Number of Draws, Games Played, Win Percentage, Loss Percentage, Average Die Values,
        // Total Die Values, Count of the Die Face Values and Lists to hold the History of the Average Die Values,
        // Total Die Values and Game Results
        #region Statistics Class Fields
        private double wins;
        private double losses;
        private double games;
        private double draws;

        private double winPercentage;
        private double lossPercentage;
        private double drawPercentage;

        private double averageDieValue;
        private double averageDieValuesAllRolls;
        private int totalDieValue;
        private int[] diceScoreValuesCount;
        
        List<double> averageDieValuesHistory;
        List<int> totalDieValuesHistory;
        List<int[]> resultsHistory;
        #endregion


        // Statistics class constructors - Constructors are detailed below
        #region Game Class Constructor
        // Defualt Statistics Constructor - Sets the statistics class fields for the constructed statistics for the player
        // Number of dice faces parameter passed to the constructor, used to set the diceScoreValuesCount array size
        public Statistics(int dieFaces)
        {
            averageDieValuesHistory = new List<double>();
            totalDieValuesHistory = new List<int>();
            diceScoreValuesCount = new int[dieFaces];
            resultsHistory = new List<int[]>();

            wins = 0;
            losses = 0;
            draws = 0;
            games = 0;

            winPercentage = 0;
            lossPercentage = 0;
            drawPercentage = 0;

            averageDieValue = 0;
            averageDieValuesAllRolls = 0;
            totalDieValue = 0;
    }
        #endregion

        // Statistics class methods -  Methods are detailed below
        #region Player Class Methods
        // Resets the statistics game history from the previous game
        public void ResetGameHistory(int dieFaces)
        {
            averageDieValuesHistory = new List<double>();
            totalDieValuesHistory = new List<int>();
            diceScoreValuesCount = new int[dieFaces];
            resultsHistory = new List<int[]>();
        }


        // Sets the games count for the player
        public void SetGames(int game)
        { games += game; }


        // Returns the games count for the player
        public double GetGames()
        { return games; }


        // Sets the wins for the player
        public void SetWins(int win)
        { wins += win; }


        // Returns the win count for the player
        public double GetWins()
        { return wins; }


        // Sets the losses for the player
        public void SetLosses(int loss)
        { losses += loss; }


        // Returns the loss count for the player
        public double GetLosses()
        { return losses; }


        // Sets the draws for the player
        public void SetDraws(int draw)
        { draws += draw; }


        // Returns the draw count for the player
        public double GetDraws()
        { return draws; }


        // Calculates the win percentage for the player
        public void CalculateWinPercentage()
        { winPercentage = System.Math.Round((wins / games) * 100, 2); }


        // Returns the win percentage for the player
        public double GetWinPercentage()
        { return winPercentage; }


        // Calculates the draw percentage for the player
        public void CalculateDrawPercentage()
        { drawPercentage = System.Math.Round((draws / games) * 100, 2); }


        // Returns the draw percentage for the player
        public double GetDrawPercentage()
        { return drawPercentage; }


        // Calculates the loss percentage for the player
        public void CalculateLossPercentage()
        { lossPercentage = System.Math.Round((losses / games) * 100 , 2); }


        // Returns the loss percentage for the player
        public double GetLossPercentage()
        { return lossPercentage; }


        // Sets the average die value for the last round of die rolls for the player
        public void SetAverageDieValue(int[] results)
        {
            double average = 0;

            for (int index = 0; index < results.Length; index++)
                average += results[index];
            averageDieValue = average / results.Length;
        }


        // Returns the average die value for the last round of die rolls for the player
        public double GetAverageDieValue()
        { return System.Math.Round(averageDieValue, 2); }


        // Sets the average die value for the game of all die rolls for the player
        public void SetAverageDieValuesAllRolls()
        {
            // Calculates the average die value for all die rolls from the last game (all final die rolls for each player)
            #region Calculate Average For All Rolls
            double average = 0;

            for (int historyIndex = 0; historyIndex < GetResultsHistory().Count; historyIndex++)
                for (int resultsIndex = 0; resultsIndex < GetResultsHistory()[historyIndex].Length; resultsIndex++)
                    average += GetResultsHistory()[historyIndex][resultsIndex];

            if (GetResultsHistory().Count >= 1)
                averageDieValuesAllRolls = average / (GetResultsHistory().Count * GetResultsHistory()[0].Length);
            else
                averageDieValuesAllRolls = 0;
            #endregion
        }


        // Returns the average die value for the game of all die rolls for the player
        public double GetAverageDieValuesAllRolls()
        { return System.Math.Round(averageDieValuesAllRolls, 2); }


        // Set the total die value for the last round of die rolls for the player
        public void SetTotalDiceValue(int[] results)
        {
            int total = 0;

            for (int index = 0; index < results.Length; index++)
                total += results[index];
            totalDieValue = total;
        }


        // Returns the total die value for the last round of die rolls for the player
        public int GetTotalDiceValue()
        { return totalDieValue; }


        // Sets the dice face value count for the last round of die rolls for the player
        public void SetDiceFaceValuesCount(int[] results)
        {
            for (int index = 0; index < results.Length; index++)
            {
                switch (results[index])
                {
                    case 1:
                        diceScoreValuesCount[0]++;
                        break;
                    case 2:
                        diceScoreValuesCount[1]++;
                        break;
                    case 3:
                        diceScoreValuesCount[2]++;
                        break;
                    case 4:
                        diceScoreValuesCount[3]++;
                        break;
                    case 5:
                        diceScoreValuesCount[4]++;
                        break;
                    case 6:
                        diceScoreValuesCount[5]++;
                        break;
                    case 7:
                        diceScoreValuesCount[6]++;
                        break;
                    case 8:
                        diceScoreValuesCount[7]++;
                        break;
                    case 9:
                        diceScoreValuesCount[8]++;
                        break;
                    case 10:
                        diceScoreValuesCount[9]++;
                        break;
                    case 11:
                        diceScoreValuesCount[10]++;
                        break;
                    case 12:
                        diceScoreValuesCount[11]++;
                        break;
                    default:
                        break;
                }
            }
        }


        // Returns the die face value count for the last round of die rolls for the player
        public int[] GetDiceFaceValuesCount()
        { return diceScoreValuesCount; }


        // Adds the last average die value to the average die values history list
        public void SetAverageDieValuesResults(double averageDieVal)
        { averageDieValuesHistory.Add(averageDieVal); }


        // Returns the list of average die values for the last game
        public List<double> GetAverageDieValuesResults()
        { return averageDieValuesHistory; }


        // Adds the last total dice value to the average die values history list
        public void SetTotalDiceValuesResults(int totalDiceVal)
        { totalDieValuesHistory.Add(totalDiceVal); }


        // Returns the list of total die values for the last game
        public List<int> GetDiceTotalValuesResults()
        { return totalDieValuesHistory; }


        // Adds the results from the last set of die rolls to the results history list
        public void AddResultsToHistory(int[] results)
        { resultsHistory.Add(results); }


        // Returns the results history list for the last game played
        public List<int[]> GetResultsHistory()
        { return resultsHistory; }


        // Returns a string which is used to display the statistics from the last game
        public string DisplayStatistics()
        {
            // Calculates the players win/loss/draw percentage for all games played during the current session
            CalculateWinPercentage();
            CalculateLossPercentage();
            CalculateDrawPercentage();

            // Returns a string which shows the players session statistics history
            return string.Format("\nGames: {0}\nWins: {1}\nLosses: {2}\nDraws: {3}\nWin Percentage: {4:G}%\nLoss Percentage: {5:G}%\nDraw Percentage: {6:G}%", GetGames(), GetWins(), GetLosses(), GetDraws(), GetWinPercentage(), GetLossPercentage(), GetDrawPercentage());
        } 


        // Displays the average, total and die face count statistics from the last game
        public void AveragesTotalAndFaceCountDisplay()
        {
            Console.ForegroundColor = ConsoleColor.White;
            // Sets the average die values from all the rolls/turns in the game
            SetAverageDieValuesAllRolls();

            // Displays the player die averages from the last game
            #region Display Player Die Averages
            Console.WriteLine("Die averages from each turn of the last game: ");
            if (GetAverageDieValuesResults().Count >= 1)
                for (int averagesIndex = 0; averagesIndex < GetAverageDieValuesResults().Count; averagesIndex++)
                    Console.WriteLine("Round {0}: {1}", averagesIndex + 1, GetAverageDieValuesResults()[averagesIndex]);
            else
                Console.WriteLine("Round 1: 0");
            Console.WriteLine();
            #endregion

            // Displays the player die averagse for all turns from the last game
            #region Display Player Die Averages For All Turns
            Console.WriteLine("Die average from all rounds of the last game: ");
            Console.WriteLine("Averages Total: {0}", GetAverageDieValuesAllRolls());
            Console.WriteLine();
            #endregion

            // Displays the player dice totals from the last game
            #region Display Dice Totals
            Console.WriteLine("Dice totals from the last game: ");
            if (GetAverageDieValuesResults().Count >= 1)
                for (int totalsIndex = 0; totalsIndex < GetAverageDieValuesResults().Count; totalsIndex++)
                    Console.WriteLine("Round {0}: {1}", totalsIndex + 1, GetDiceTotalValuesResults()[totalsIndex]);
            else
                Console.WriteLine("Round 1: 0");
            Console.WriteLine();
            #endregion

            // Display the player dice face value count form the last game 
            #region Display Dice Fave Value Counts
            Console.WriteLine("Die face counts from the last game: ");
            if (GetDiceFaceValuesCount().Length >= 1)
                for (int faceCountIndex = 0; faceCountIndex < GetDiceFaceValuesCount().Length; faceCountIndex++)
                    Console.WriteLine("Number of {0}'s: {1}", faceCountIndex + 1, GetDiceFaceValuesCount()[faceCountIndex]);
            else
                Console.WriteLine("No die rolled");
            Console.WriteLine(); 
            #endregion
        }


        // Displays the results history of the player from the last game
        public void ResultsHistoryDisplay()
        {
            Console.WriteLine("Results from the last game:");

            // Displays the players result history from the last game (all final die rolls for each player)
            #region Display Player Result History
            for (int historyIndex = 0; historyIndex < GetResultsHistory().Count; historyIndex++)
            {
                Console.Write("Round {0}: [ ", historyIndex + 1);
                for (int resultsIndex = 0; resultsIndex < GetResultsHistory()[historyIndex].Length; resultsIndex++)
                    Console.Write(GetResultsHistory()[historyIndex][resultsIndex] + " ");

                Console.Write("]\n");
            } 
            #endregion
            Console.WriteLine();
        }
        #endregion
    }
    // End of the Statistics class
}
