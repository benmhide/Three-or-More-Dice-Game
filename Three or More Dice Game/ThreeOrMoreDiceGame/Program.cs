using System;
using System.Runtime.InteropServices;

/* Three-or-More Dice game - Players take turns rolling all five/seven/nine die which are either six/eight/twelve sided and scoring for
 * three-of-a-kind or better. If a player only has two-of-a-kind, they may re-throw the remaining dice in an attempt to improve 
 * the matching dice values. The player may choose to roll all die at once and get double points for scoring dice, however this means
 * they can't re-roll any die if they only have two-of-a-kind. If no matching numbers are rolled, a player scores 0. A player wins when they 
 * have reached the target winning score.
 * 
 * The options of the game inculde: Choosing the number of die per player, choosing the number of faces per die, choosing to play against 
 * another player or the computer, choosing the winning score value and choosing to play either to reach a winning score or after a set
 * number of rounds.
 * 
 * Each player will be able to view their turn/game/session statistics at the end of each turn/game which includes: Totals for dice rolls per
 * turn, averages for dice rolls per turn, total averages for all die rolled in the game, face value count per turn/game, the win count,
 * loss count, draw count and game count, aswell as the results history from the last game.
 * 
 * Created by Ben Hide for the CMP1127M Programming and Data Structures: Assessment 2 - Games Computing BSc first year
 */


namespace ThreeOrMoreDiceGame
{
    // Start of the Program class
	class Program
	{
		// Maximises the console window on start up
		#region Maximise Window
		[DllImport("kernel32.dll", ExactSpelling = true)]
		private static extern IntPtr GetConsoleWindow();
		private static IntPtr ThisConsole = GetConsoleWindow();
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
		private const int HIDE = 0;
		private const int MAXIMIZE = 3;
		private const int MINIMIZE = 6;
		private const int RESTORE = 9;
		#endregion

		// Entry point for the Three-Or-More Dice Game
		#region Program Main Method
		public static void Main()
		{
			// Maximises the console window on start up
			#region Maximise Window
			Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
			ShowWindow(ThisConsole, MAXIMIZE);
            #endregion

            // Start a new Game
            #region Instaniate New Game
            Game game = new Game();
            #endregion

            // Loop the game untill either player 1 or player 2 has a wining score
            #region Game Loop
            while (game.GetPlayer(0).GetPlayerScore() < game.GetWinningScore() && game.GetPlayer(1).GetPlayerScore() < game.GetWinningScore())
			{
				// Loop throught the number of players 
				#region Loop Through Players
				while (game.GetPlayer(0).GetPlayerTurn() || game.GetPlayer(1).GetPlayerTurn())
				{
                    // Check to see if either player has won before allowing the next player to take their turn
                    if (game.GetPlayer(0).GetPlayerScore() >= game.GetWinningScore() || game.GetPlayer(1).GetPlayerScore() >= game.GetWinningScore() || game.GetRounds().Count == 0)
                    {
                        // Check the players scores to see if either player has won the game before continuing or starting a new game
                        #region Check For Winner/Start New Game
                        // Check the players scores to see if they have won the game and diaplys result if a player has won
                        game.CheckForWinner();


                        // Calls the start new game method which allows the user to restart a new game or quit the application
                        game.StartNewGame();

                        break;
                        #endregion
                    }

                    // Else continue the game    
                    else
                    {
                        // Displays which players turn it is currently and the current player scores
                        #region Display Player Turn and Scores
                        game.DisplayScoresPlayerTurn();
                        #endregion

                        // Sets the current round of the game
                        #region Set Game Round
                        game.SetRound();
                        #endregion


                        // Loop throught the number of dice/player rolls
                        #region Player Dice Rolls
                        // Variable to hold the return value for the RollAllDice method
                        //(method allows the player to choose if they want to roll all dice or roll die indivdually)
                        bool rollAllDice = game.RollAllDice(game.GetCurrentPlayer());

                        // Allows the player to either roll all thier dice at once or one at a time
                        Console.ForegroundColor = game.GetPlayer(game.GetCurrentPlayer()).GetPlayerConsoleColour();
                        game.RollAllDieOrRollIndivdually(rollAllDice);
                        #endregion


                        // Check the dice results for matching scoring dice and for pairs if no scoring matches are found
                        // Set the results array equal to the dice values from the last round of dice rolls
                        // Sets the Dice score values from the last round of dice rolls
                        #region Check for Pairs, Set Player Results and Dice Face-Value Counts
                        // Set the initial die rolls results
                        game.SetInitialResults(game.GetCurrentPlayer());

                        // Checks initial die rolls results for winning score and pairs of dice (also check if all dice rolled or one at a time)
                        game.CheckForPairs(game.GetCurrentPlayer(), rollAllDice);

                        // Set the final die rolls results (after any re-rolls)
                        game.GetPlayer(game.GetCurrentPlayer()).GetStatistics().AddResultsToHistory(game.GetPlayerResults());

                        // Set the dice score values for the current player
                        game.SetDiceScoreValue();
                        #endregion


                        // Show the results and statistics from the last last round of dice rolls
                        #region Display Player Results

                        // Displays the results from the last round of die rolls for the player
                        game.DisplayPlayerResults(game.GetCurrentPlayer());

                        // Set and show the average die value from the last round of die rolls
                        game.GetPlayer(game.GetCurrentPlayer()).GetStatistics().SetAverageDieValue(game.GetFinalResults());
                        Console.WriteLine("Die value average from round results: {0}\n", game.GetPlayer(game.GetCurrentPlayer()).GetStatistics().GetAverageDieValue());

                        // Set the average die values history list for the current player
                        game.GetPlayer(game.GetCurrentPlayer()).GetStatistics().SetAverageDieValuesResults(game.GetPlayer(game.GetCurrentPlayer()).GetStatistics().GetAverageDieValue());

                        // Set and show the total dice value from the last round of die rolls
                        game.GetPlayer(game.GetCurrentPlayer()).GetStatistics().SetTotalDiceValue(game.GetFinalResults());
                        Console.WriteLine("Dice total from round results: {0}\n", game.GetPlayer(game.GetCurrentPlayer()).GetStatistics().GetTotalDiceValue());

                        // Set the total dice values history list for the current player
                        game.GetPlayer(game.GetCurrentPlayer()).GetStatistics().SetTotalDiceValuesResults(game.GetPlayer(game.GetCurrentPlayer()).GetStatistics().GetTotalDiceValue());

                        // Set the dice face count values for the last round of die rolls
                        game.GetPlayer(game.GetCurrentPlayer()).GetStatistics().SetDiceFaceValuesCount(game.GetFinalResults());

                        // Show the dice face value counts from the last round (number of number matches from dice rolled)
                        game.DisplayPlayerFaceCounts(game.GetCurrentPlayer());
                        #endregion


                        // Check the results of the last round of dice rolls and add points to
                        // the player score if they have a winning dice roll
                        #region Check Player Results
                        game.CheckResults(game.GetCurrentPlayer(), game.GetDiceScoreValues(), rollAllDice);
                        #endregion

                        

                        // After each player rolls their dice the current game player is swapped to the next player
                        // Sets players turn to true or false and sets the currnt player value for the game
                        #region Swap The Player Turns
                        game.SwapPlayerTurns();
                        #endregion
                    }
                }
                #endregion
            } 
			#endregion
		} 
		#endregion
	}
    // End of the Program class
}
