using System;
using System.Collections.Generic;
using System.Threading;

namespace ThreeOrMoreDiceGame
{
    // Start of the Game class
    public class Game
	{
        // Game class fields for:
        // Player Array, Number of Players, Number of Die (per player), Number of Rolls per Turn, The Game Mode, The Number of Rounds
        // Number of Faces (per die), Winning Score, Player Colours, Current Player, Points Scores for matches, Round Queue,
        // Random Player to Start (and sync object), Initial Results Array, Final Results Array and Dice Score Values Array
        // (Also inculdes Booleans to control program flow for user input in conditional statements within class methods and if an
        // AI player has been selected to play against) 
        #region Game Class Fields
        internal Player[] players;
        private Queue<int> roundQueue;
        private static Random firstplayer = new Random();
        private static readonly object sync = new object();

        private bool isNumOfPlayers = false;
		private bool isNumOfDie = false;
		private bool isNumOfDieFaces = false;
		private bool isWinningScore = false;
        private bool isScoreOrRounds = false;
        private bool isRoundsNum = false;
        private bool isPlayerName = false;
        private bool isRestartGame = false;
        private bool isPlayerAI = false;

        private int numOfPlayers;
		private int numOfDie;
		private int numOfDieFaces;
		private int numOfRolls;
		private int winningScore;
        private int gameMode;                   
        private int numOfRounds;                
        private int currentPlayer;
        private string gameType;
        private string[] playerColours = new string[] { "Yellow", "Red", "Cyan" };

        private int[] scorePoints = new int[] { 0, 0, 0, 3, 6, 9, 12, 15, 18, 21 };
        private int[] initialResults;
        private int[] finalResults;
        private int[] diceScoreValues;
        #endregion

        // Game class constructors - Constructors are detailed below
        #region Game Class Constructor
        // Defualt Game Constructor - Sets the game class fields for the constructed game
        // No parameters passed to the constructor, game class fields are set within the constructor
        public Game()
		{
            // Called to to set the game settings for the the dice, players and other game settings
            GameSettings();

            // Generates the rounds to be played in the game by the players (one round is one turn per player)
            GenerateRounds();

            // Initialises all the players, die and other game settings
            #region Initialises The Game
            // Sets the instaniates the player array to the size of the number of players in the game
            players = new Player[GetNumberOfPlayers()];

            // Initialises the size of thr initial results array (size is equal to the number of die per player)
            initialResults = new int[numOfDie];

            // Initialises the size of the final results array (size is equal to the number of die per player)
            finalResults = new int[numOfDie];

            // Initialises the size of the dice score values array (size is equal to the number of faces per die)
            diceScoreValues = new int[numOfDieFaces];

            // Sets the players of the game (instaniates new players from the Player class)
            SetPlayers(GetNumberOfDie(), isPlayerAI);

            // Sets the the Dice array objects (calls the GetNumberOfDieFaces method)
            SetDice(GetNumberOfDieFaces());

            // Sets the number of rolls per player turn, the same as the number of dice per player (calls the GetNumberOfDie method)
            SetNumberOfRolls(GetNumberOfDie());
            #endregion
        }
        #endregion

        // Game class methods -  Methods are detailed below
        #region Game Class Methods
        // Returns the Player array for the current game
        internal Player[] GetPlayers()
		{ return players; }


        // Returns the player in the Player array at the specified index for the current game
        internal Player GetPlayer(int playerNum)
		{ return players[playerNum]; }


        // Sets the number of players in the game
		public void SetNumberOfPlayers(int playerCount)
		{ numOfPlayers = playerCount; }


        // Returns the number of players in the game
		public int GetNumberOfPlayers()
		{ return numOfPlayers; }


        // Sets the number of die per player in the game
		public void SetNumberOfDie(int dieCount)
		{ numOfDie = dieCount; }


        // Returns the number of die per player in the game
        public int GetNumberOfDie()
		{ return numOfDie; }


        // Sets the number of die rolls per player in the game (die rolls is equal to the number of die per player)
        public void SetNumberOfRolls(int dieCount)
		{ numOfRolls = dieCount; }


        // Returns the number of die rolls per player in the game
		public int GetNumberOfRolls()
		{ return numOfRolls; }


        // Sets the number of die faces per die in the game
		public void SetNumberOfDieFaces(int dieFaceCount)
		{ numOfDieFaces = dieFaceCount; }


        // Returns the number of die faces per die in the game
		public int GetNumberOfDieFaces()
		{ return numOfDieFaces; }


        // Sets the winning score value for the game
		public void SetWinningScore(int winScore)
		{ winningScore = winScore; }


        // Returns the winning score value for the game
		public int GetWinningScore()
		{ return winningScore; }
        

        // Sets the current player of the game
        public void SetCurrentPlayer(int player)
        { currentPlayer = player; }


        // Returns the current player of the game
        public int GetCurrentPlayer()
        { return currentPlayer; }


        // Initialises the the number of rounds in the game
        public void SetNumberOfRounds(int rounds)
        { numOfRounds += rounds; }


        // Returns the number of rounds in the game
        public int GetNumberOfRounds()
        { return numOfRounds; }


        // Sets the round in the game (dequeues each round once its been played)
        public void SetRound()
        { roundQueue.Dequeue(); }


        // Returns the current round in the game
        public int GetRound()
        { return roundQueue.Peek(); }


        // Initialises the name/type of game mode for the game
        public void SetGameType(string mode)
        { gameType = mode; }


        // Returns the name/type of game mode for the game
        public string GetGameType()
        { return gameType; }


        // Generates the rounds in the game
        public void GenerateRounds()
        {
            roundQueue = new Queue<int>(numOfRounds * 2);

            for (int round = 0; round < numOfRounds * 2; round++)
                roundQueue.Enqueue((round / 2) + 1);
        }


        // Returns the queue of intgers for the player rounds in the game
        public Queue<int> GetRounds()
        { return roundQueue; }


        // Initialises the dice faces for the players
        public void SetDice(int numOfDieFaces)
        {
            // Loops through the players in the game and initialises the number of die faces per die
            for (int player = 0; player < players.Length; player++)
                players[player].SetDie(numOfDieFaces);
        }


        // Initialises the dice roll score values
        public void InitialiseDiceScoreValues(int dieFaces)
        {
            for (int scoreIndex = 0; scoreIndex < dieFaces; scoreIndex++)
                diceScoreValues[scoreIndex] = 0;
        }


        // Sets the dice score values for the last set of die rolls
        public void SetDiceScoreValue()
        {
            // Initailise all the dice score values for each die per die face to 0
            InitialiseDiceScoreValues(GetNumberOfDieFaces());

            // Sets the dice score values from the current player die rolls
            #region Set Dice Score Values
            for (int resultsIndex = 0; resultsIndex < initialResults.Length; resultsIndex++)
            {
                switch (initialResults[resultsIndex])
                {
                    case 1:
                        diceScoreValues[0]++;
                        break;
                    case 2:
                        diceScoreValues[1]++;
                        break;
                    case 3:
                        diceScoreValues[2]++;
                        break;
                    case 4:
                        diceScoreValues[3]++;
                        break;
                    case 5:
                        diceScoreValues[4]++;
                        break;
                    case 6:
                        diceScoreValues[5]++;
                        break;
                    case 7:
                        diceScoreValues[6]++;
                        break;
                    case 8:
                        diceScoreValues[7]++;
                        break;
                    case 9:
                        diceScoreValues[8]++;
                        break;
                    case 10:
                        diceScoreValues[9]++;
                        break;
                    case 11:
                        diceScoreValues[10]++;
                        break;
                    case 12:
                        diceScoreValues[11]++;
                        break;
                    default:
                        break;
                }
            }
            #endregion
        }


        // Returns the dice score values for the last set of die rolls
        public int[] GetDiceScoreValues()
        { return diceScoreValues; }


        // Initialises the players turn for the game (call once per game to decide which player goes first)
        // Returns a boolean array with the values representing which player will start
        public bool[] GetPlayersTurn()
        {
            int playerTurn;
            bool[] playersTurn = new bool[2] { false, false };

            // Random number determines the player to start the game
            lock (sync)
                playerTurn = firstplayer.Next(1, 3);

            // Sets player one to go first
            if (playerTurn == 1)
                playersTurn[0] = true;

            // Sets player two to go first
            else if (playerTurn == 2)
                playersTurn[1] = true;

            // Returns the player turns as a boolean array
            return playersTurn;
        }


        // Initialises the players for the game - Sets the players names and instaniates the players
        public void SetPlayers(int numOfDie, bool playerAI)
		{
            bool[] turn = GetPlayersTurn();
			string playerName = "";

            // Loop through the players to set their names and colours
            for (int playerIndex = 0; playerIndex < players.Length; playerIndex++)
			{
                // User input to select player name and then assigned player colour
                #region Set Player Names and Colours
                while (!isPlayerName)
                {
                    try
                    {
                        // Set console colour to that of the players console colours
                        if (playerIndex == 0)
                            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), playerColours[0]);
                        else if (playerIndex == 1)
                            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), playerColours[1]);

                        // If selected to play against an AI oponent set the first players name to COMPUTER
                        if (playerAI)
                        {
                            playerName = "COMPUTER";
                            playerAI = false;
                        }
                        else
                        {
                            Console.WriteLine("Player {0} please enter your name now: ", playerIndex + 1);

                            // User input for players name, check for exceptions
                            playerName = Console.ReadLine().ToUpper();
                        }

                        if (playerName.Length <= 1)
                            throw new Exception("Name is not valid.");

                        else
                        {
                            for (int nameIndex = 0; nameIndex < playerName.Length; nameIndex++)
                                if (!char.IsLetter(playerName[nameIndex]))
                                    throw new Exception("Name is not valid.");
                        }
                        Console.WriteLine("Player {0} name has been set to: {1}\nPress enter to continue ... ", playerIndex + 1, playerName);
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    }
                    catch (Exception name)
                    {
                        Console.WriteLine(name.Message + " Press enter to try again now ...");
                        Console.ReadLine();
                        Console.Clear();
                    }
                }
                #endregion

                // Instaniate the players for a new game inculding their names, colours die and sets player turn
                #region Instaniate The Players/Initialises Thier Turns
                // Instaniate the new player and set up their dice and colours
                players[playerIndex] = new Player(numOfDie, playerName, playerColours[playerIndex], GetNumberOfDieFaces());

                // Initialise the player turns to true or flase
                players[playerIndex].SetPlayerTurn(turn[playerIndex]); 
                #endregion
            }

            // Initialises the current game player, sets which player will start the game
            #region Initialise The Current Game Player
            if (turn[0])
                SetCurrentPlayer(0);
            if (turn[1])
                SetCurrentPlayer(1);
            #endregion

            // Display the game rules to the user (calls the GameRules method)
            #region Game Rules
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("**********************************************************************************************");
            Console.WriteLine("The games rules which have been selected are: {0}\nPress enter to continue ... ", GameRules());
            Console.WriteLine("**********************************************************************************************");
            Console.ReadLine();
            Console.Clear();
            #endregion
        }


        // Initialises the players for a new game - after a game has already been played
        public void SetPlayers(int numOfDie, bool playerAI, string[] playerNames)
        {
            string playerName = "";

            // If starting a new game againt the computer check if the previous game was against the computer
            // If the previous game wasn't against the computer instaniate a new computer oponent
            #region Set Computer Player
            if (playerNames[0] != "COMPUTER")
            {
                if (isPlayerAI)
                {
                    // Instaniate a new computer player (previous game will have been versus a human player)
                    players[0] = new Player(numOfDie, "COMPUTER", playerColours[0], numOfDieFaces);
                }
                    
            }
            #endregion

            // Else If starting a new game againt another player (human) check if the previous game was against the computer
            // If the previous game was against the computer instaniate a human player oponent
            #region Set Human Player
            else if (playerNames[0] == "COMPUTER")
            {
                if (!isPlayerAI)
                {
                    while (!isPlayerName)
                    {
                        try
                        {
                            // Set console colour to that of the players console colours
                            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), playerColours[0]);


                            Console.WriteLine("Player 1 please enter your name now: ");

                            // User input for players name, check for exceptions
                            playerName = Console.ReadLine().ToUpper();

                            if (playerName.Length <= 1)
                                throw new Exception("Name is not valid.");

                            else
                            {
                                for (int nameIndex = 0; nameIndex < playerName.Length; nameIndex++)
                                    if (!char.IsLetter(playerName[nameIndex]))
                                        throw new Exception("Name is not valid.");
                            }
                            Console.WriteLine("Player 1 name has been set to: {0}\nPress enter to continue ... ", playerName);
                            Console.ReadLine();
                            Console.Clear();

                            // Instaniate a new player for the game (previous game will have been versus a computer player)
                            players[0] = new Player(numOfDie, playerName, playerColours[0], numOfDieFaces);
                            break;
                        }
                        catch (Exception name)
                        {
                            Console.WriteLine(name.Message + " Press enter to try again now ...");
                            Console.ReadLine();
                            Console.Clear();
                        }
                    }
                }
            }
            #endregion

            // Loop through the players to set their names and colours
            #region Set Players Names/Dice
            for (int player = 0; player < players.Length; player++)
            {
                #region Sets Names/Dice
                // Instaniate the new player and set up their dice and colours
                players[player].SetPlayerName(GetPlayer(player).GetPlayerName());

                // Resets the players dice for a new game
                players[player].ResetDice(numOfDie);

                // Sets the players die face count for the players die
                players[player].SetDie(numOfDieFaces);
                #endregion
            }
            #endregion

            // Display the game rules to the user (calls the GameRules method)
            #region Game Rules
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("**********************************************************************************************");
            Console.WriteLine("The games rules which have been selected are: {0}\nPress enter to continue ... ", GameRules());
            Console.WriteLine("**********************************************************************************************");
            Console.ReadLine();
            Console.Clear();
            #endregion
        }


        // Sets the initial results for the player dice rolls (before re-rolls take place)
        public void SetInitialResults(int player)
        {
            for (int resultsIndex = 0; resultsIndex < players[player].GetDice().Length; resultsIndex++)
                initialResults[resultsIndex] = players[player].GetDice()[resultsIndex].GetNumberOnTop();
        }


        // Sets the final results for the player dice rolls (after re-rolls take place)
        public void SetFinalResults()
        {
            for (int resultsIndex = 0; resultsIndex < finalResults.Length; resultsIndex++)
                finalResults[resultsIndex] = initialResults[resultsIndex];
        }


        // Returns the final dice roll results (after re-rolls take place)
        public int[] GetFinalResults()
        { return finalResults; }


        // Returns the last round of dice rolls for the player to add to the player statistics
        public int[] GetPlayerResults()
        {
            int[] results = new int[GetFinalResults().Length];

            for (int result = 0; result < results.Length; result++)
            { results[result] = GetFinalResults()[result]; }

            return results;
        }


        // Player chooses to roll all dice at once or one at a time returns true if player
        // wants to roll alll dice or false for one at a time
        public bool RollAllDice(int player)
        {
            bool isRollAllDice = false;
            bool rollAllDice = false;

            // PLayer chooses if they want to roll all dice or one at a time
            #region Player Options To Roll All Dice
            while (!isRollAllDice)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Would you like to roll all dice at once? or one at a time?\n");
                    Console.WriteLine("(Any points scored rolling all dice at once will be doubled, however if you don't score rolling all die at once");
                    Console.WriteLine("you won't be able to improve your score if you only have matching pair(s) of dice. If you roll die one at a time ");
                    Console.WriteLine("you can try to improve your score by rolling non-matching die if you have one or more pairs of matching die).\n");
                    Console.WriteLine("Option 1: Yes - All at once");
                    Console.WriteLine("Option 2: No - One at a time");

                    int rollAll;

                    // If the players name is computer (e.g. an AI player) call the AI player choice method
                    if (GetPlayer(player).GetPlayerName() == "COMPUTER")
                        // Set the roll all die options for the AI player
                        rollAll = GetPlayer(player).AIPlayerChoice();

                    else
                        // User input for number of roll all dice options
                        rollAll = Convert.ToInt32(Console.ReadLine());

                    if (rollAll == 1)
                    { rollAllDice = true; break; }

                    else if (rollAll == 2)
                    { rollAllDice = false; break; }

                    if (rollAll < 1 || rollAll > 2)
                        throw new Exception("option is not valid.");
                }

                catch (Exception allDie)
                {
                    Console.WriteLine(allDie.Message + " Press enter to try again now ...");
                    Console.ReadLine();
                    Console.Clear();
                    DisplayScoresPlayerTurn();
                }
            }
            Console.Clear(); 
            #endregion
            return rollAllDice;
        }


        // Player rolls thier dice depending on the returned value of the RollAllDice method
        public void RollAllDieOrRollIndivdually(bool rollAllDice)
        {
            // If the player wants to roll all dice
            if (rollAllDice)
            {
                // Roll all die at once
                #region Roll All Die
                // Player rolls all dice at once
                GetPlayer(GetCurrentPlayer()).RollAllDice();

                // Display if player is going to roll all die
                Console.WriteLine("Player {0} - {1} rolled all die ...\n", GetCurrentPlayer() + 1, GetPlayer(GetCurrentPlayer()).GetPlayerName());

                // Display the results from rolling all dice
                for (int die = 0; die < GetNumberOfRolls(); die++)
                    Console.WriteLine("Player {0} - {1} rolled a ... {2}", GetCurrentPlayer() + 1, GetPlayer(GetCurrentPlayer()).GetPlayerName(), GetPlayer(GetCurrentPlayer()).GetDie(die).GetNumberOnTop());
                #endregion
            }

            // Else if the player rolls dice indivdually
            else if (!rollAllDice)
            {
                // Roll all die one at a time
                #region Roll Die Individually
                // Display if player is going to roll die one at a time
                Console.WriteLine("Player {0} - {1} to roll die one at a time...\n", GetCurrentPlayer() + 1, GetPlayer(GetCurrentPlayer()).GetPlayerName());

                for (int die = 0; die < GetNumberOfRolls(); die++)
                {
                    // Player rolls each die one at a time
                    Console.Write("Player {0} - {1} rolled a ... {2} ...",
                        GetCurrentPlayer() + 1, GetPlayer(GetCurrentPlayer()).GetPlayerName(), GetPlayer(GetCurrentPlayer()).GetDie(die).RollDie());

                    // If the players name is not COMPUTER player rolls the die one at a time else computer rolls  die
                    // (sleep after each roll for the computer)
                    if (GetPlayer(GetCurrentPlayer()).GetPlayerName() != "COMPUTER")
                    { Console.Write("\tPress enter ..."); Console.ReadLine(); }
                    else
                    { Thread.Sleep(1000); Console.WriteLine(); }
                }
                #endregion
            }
        }


        // Checks for pairs in the players results and and allows the player to roll again non matching die 
        // Die can only be rolled again once and only if the player has a matching pair, no scoring match
        // and didn't roll all dice at once
        public void CheckForPairs(int player, bool rolledAllDice)
        {
            int[] scoreValues = new int[numOfDieFaces];
            bool playerScored = false;
            bool playerGotPair = false;

            // If the player didn't roll all dice check for pairs of dice
            if (!rolledAllDice)
            {
                // Loop through the results array and count the amount of die with the same face value
                // Add each die face value to the scoreValues array
                #region Count Die Faces For Turn
                for (int resultsIndex = 0; resultsIndex < initialResults.Length; resultsIndex++)
                {
                    switch (initialResults[resultsIndex])
                    {
                        case 1:
                            scoreValues[0]++;
                            break;
                        case 2:
                            scoreValues[1]++;
                            break;
                        case 3:
                            scoreValues[2]++;
                            break;
                        case 4:
                            scoreValues[3]++;
                            break;
                        case 5:
                            scoreValues[4]++;
                            break;
                        case 6:
                            scoreValues[5]++;
                            break;
                        case 7:
                            scoreValues[6]++;
                            break;
                        case 8:
                            scoreValues[7]++;
                            break;
                        case 9:
                            scoreValues[8]++;
                            break;
                        case 10:
                            scoreValues[9]++;
                            break;
                        case 11:
                            scoreValues[10]++;
                            break;
                        case 12:
                            scoreValues[11]++;
                            break;
                        default:
                            break;
                    }
                } 
                #endregion

                // Loop through the scoreValues array if any values are greater than 2 the played has scored
                // points on their first roll (no re-rolls required for the turn)
                // If the player has scored set the playedScore to true so that the player cant re-roll any die
                #region Check For Winning Rolls
                for (int scoreIndex = 0; scoreIndex < scoreValues.Length; scoreIndex++)
                    if (scoreValues[scoreIndex] > 2)
                        playerScored = true;
                #endregion

                //If the player hasn't scored on their first attemp check to see if they have any matching pairs
                //If the player has a matching pair set the playerGotPair to true
                if (!playerScored)
                {
                    // Check for matching pairs of die
                    #region Check For Pairs Of Dice
                    for (int scoreIndex = 0; scoreIndex < scoreValues.Length; scoreIndex++)
                        if (scoreValues[scoreIndex] == 2)
                            playerGotPair = true;
                    #endregion

                    // If the player got 1 or more pairs set all the results which are not a matching pair to 0
                    if (playerGotPair)
                    {
                        // Set results index of non-matching pairs of die to 0 (results index with a value of 0 will be re-rolled)
                        #region Reset Non-Matching Dice results
                        for (int scoreIndex = 0; scoreIndex < scoreValues.Length; scoreIndex++)
                            if (scoreValues[scoreIndex] == 1)
                                for (int resultsIndex = 0; resultsIndex < initialResults.Length; resultsIndex++)
                                    if (initialResults[resultsIndex] == scoreIndex + 1)
                                        initialResults[resultsIndex] = 0;
                        #endregion

                        // Loop through the reuslts array and re-roll any die which now have a 0 value
                        // i.e. dies which are not a matching pair
                        #region Re-Roll Dice
                        for (int resultsIndex = 0; resultsIndex < initialResults.Length; resultsIndex++)
                        {
                            if (initialResults[resultsIndex] == 0)
                            {
                                Console.Write("\nPlayer {0} - {1} roll die {2} again ...",
                                       player + 1, GetPlayer(player).GetPlayerName(), resultsIndex + 1);

                                if (GetPlayer(GetCurrentPlayer()).GetPlayerName() != "COMPUTER")
                                { Console.Write("\tPress enter ...\n"); Console.ReadLine(); }

                                else
                                { Thread.Sleep(2000); Console.WriteLine(); }

                                Console.WriteLine("Player {0} - {1} rolled a ... {2} ...",
                                       player + 1, GetPlayer(player).GetPlayerName(), GetPlayer(player).GetDie(resultsIndex).RollDie());

                                initialResults[resultsIndex] = GetPlayer(player).GetDie(resultsIndex).GetNumberOnTop();
                            }
                        } 
                        #endregion
                    }
                }
            }

            // Sets the players final results from the turn after any re-rolls have been made
            SetFinalResults();
        }


        // Checks the players results for a winning score
		public void CheckResults(int player, int[] playerDieScores, bool isRolledAllDice)
		{
            // Modifier for points score
            int pointsModifier = 1;

            // Loop through the players die scores array and check for winning matches
            // If a winning match is found add the score value to the players score
            // If the player rolled all dice at once double thier round score
            for (int score = 0; score < playerDieScores.Length; score++)
            {
                // If the player rolled all dice at once set the points modifier to double their score
                // if they have a scoring dice roll from the last round of rolls
                #region Set For Points Modified
                if (isRolledAllDice)
                    pointsModifier = 2;

                else if (!isRolledAllDice)
                    pointsModifier = 1;
                #endregion

                // Award the player points depending on the dice rolls from the last round of dice rolls
                Console.ForegroundColor = ConsoleColor.White;
                #region Add Points To The Players Score
                switch (playerDieScores[score])
                {
                    case 3:
                        players[player].SetPlayerScore(scorePoints[3] * pointsModifier);
                        Console.WriteLine("\nPlayer {0} - {1} - scored: {2} points!!",
                            player + 1, GetPlayer(player).GetPlayerName(), scorePoints[3] * pointsModifier);
                        break;
                    case 4:
                        players[player].SetPlayerScore(scorePoints[4] * pointsModifier);
                        Console.WriteLine("\nPlayer {0} - {1} - scored: {2} points!!",
                            player + 1, GetPlayer(player).GetPlayerName(), scorePoints[4] * pointsModifier);
                        break;
                    case 5:
                        players[player].SetPlayerScore(scorePoints[5] * pointsModifier);
                        Console.WriteLine("\nPlayer {0} - {1} - scored: {2} points!!",
                            player + 1, GetPlayer(player).GetPlayerName(), scorePoints[5] * pointsModifier);
                        break;
                    case 6:
                        players[player].SetPlayerScore(scorePoints[6] * pointsModifier);
                        Console.WriteLine("\nPlayer {0} - {1} - scored: {2} points!!",
                            player + 1, GetPlayer(player).GetPlayerName(), scorePoints[6] * pointsModifier);
                        break;
                    case 7:
                        players[player].SetPlayerScore(scorePoints[7] * pointsModifier);
                        Console.WriteLine("\nPlayer {0} - {1} - scored: {2} points!!",
                            player + 1, GetPlayer(player).GetPlayerName(), scorePoints[7] * pointsModifier);
                        break;
                    case 8:
                        players[player].SetPlayerScore(scorePoints[8] * pointsModifier);
                        Console.WriteLine("\nPlayer {0} - {1} - scored: {2} points!!",
                            player + 1, GetPlayer(player).GetPlayerName(), scorePoints[8] * pointsModifier);
                        break;
                    case 9:
                        players[player].SetPlayerScore(scorePoints[9] * pointsModifier);
                        Console.WriteLine("\nPlayer {0} - {1} - scored: {2} points!!",
                            player + 1, GetPlayer(player).GetPlayerName(), scorePoints[9] * pointsModifier);
                        break;
                    default:
                        break;
                }
            }
            #endregion

            // Displays the players score once calculated
            #region Display Score
            Console.WriteLine("\nPlayer {0} - {1} - score: {2}\n", GetCurrentPlayer() + 1, GetPlayer(GetCurrentPlayer()).GetPlayerName(), GetPlayer(GetCurrentPlayer()).GetPlayerScore());
            Console.Write("Press enter to continue ...");
            Console.ReadLine();
            Console.Clear(); 
            #endregion
        }


        // Check to see if either player has won the game, if so set the statistics for each player
        // and display the game results
        public void CheckForWinner()
        {
            Console.ForegroundColor = ConsoleColor.White;

            // Sets/calculates the players statistics for the last game played. Checks for the game winner
            #region Calculate/Display Winner And Player Stats
            // Set the player games values
            GetPlayer(0).GetStatistics().SetGames(1);
            GetPlayer(1).GetStatistics().SetGames(1);

            Console.WriteLine("**********************************************************************************************");
            // Player one wins the game
            if (GetPlayer(0).GetPlayerScore() >= GetWinningScore() || GetPlayer(0).GetPlayerScore() > GetPlayer(1).GetPlayerScore())
            {
                Console.WriteLine("PLAYER {0} - {1} - WINS THE GAME WITH A SCORE OF: {2}", 1, GetPlayer(0).GetPlayerName(), GetPlayer(0).GetPlayerScore());

                GetPlayer(0).SetPlayerTurn(false);
                GetPlayer(0).GetStatistics().SetWins(1);
                GetPlayer(1).GetStatistics().SetLosses(1);
            }

            // Player two wins the game   
            else if (GetPlayer(1).GetPlayerScore() >= GetWinningScore() || GetPlayer(1).GetPlayerScore() > GetPlayer(0).GetPlayerScore())
            {
                Console.WriteLine("PLAYER {0} - {1} - WINS THE GAME WITH A SCORE OF: {2}", 2, GetPlayer(1).GetPlayerName(), GetPlayer(1).GetPlayerScore());

                GetPlayer(1).SetPlayerTurn(false);
                GetPlayer(1).GetStatistics().SetWins(1);
                GetPlayer(0).GetStatistics().SetLosses(1);
            }

            // The game is a draw
            else if (GetPlayer(0).GetPlayerScore() == GetPlayer(1).GetPlayerScore())
            {
                Console.WriteLine("THE IS A DRAW - PLAYER {0} - {1} SCORE: {2} - PLAYER {3} - {4} SCORE: {5}", 1, GetPlayer(0).GetPlayerName(), GetPlayer(0).GetPlayerScore(), 2, GetPlayer(1).GetPlayerName(), GetPlayer(1).GetPlayerScore());

                GetPlayer(0).SetPlayerTurn(false);
                GetPlayer(1).SetPlayerTurn(false);
                GetPlayer(0).GetStatistics().SetDraws(1);
                GetPlayer(1).GetStatistics().SetDraws(1);
            }
            Console.WriteLine("**********************************************************************************************");
            #endregion


            // Displays the game stats for each player and the reuslts for the last game
            #region Display The Game Results
            // Displays the current games results history for the players
            for (int player = 0; player < GetPlayers().Length; player++)
            {
                Console.ForegroundColor = GetPlayer(player).GetPlayerConsoleColour();

                Console.WriteLine("**********************************************************************************************");
                Console.WriteLine("Player 1 - {0} - Game Results:\nScore: {1}\n", GetPlayer(player).GetPlayerName(), GetPlayer(player).GetPlayerScore());
                

                GetPlayer(player).GetStatistics().AveragesTotalAndFaceCountDisplay();
                GetPlayer(player).GetStatistics().ResultsHistoryDisplay();
                Console.WriteLine("Statistics History: {0}", GetPlayer(player).GetStatistics().DisplayStatistics());
                Console.WriteLine("**********************************************************************************************");
            }

            Console.ForegroundColor = ConsoleColor.White;
            #endregion
        }


        // Swap the player turns (after each round of die rolls)
        public void SwapPlayerTurns()
        {
            // If its player ones turn swap to player two and vice versa
            #region Swap Turns
            if (GetPlayer(0).GetPlayerTurn())
            {
                GetPlayer(0).SetPlayerTurn(false);
                GetPlayer(1).SetPlayerTurn(true);
                SetCurrentPlayer(1);
            }
            else if (GetPlayer(1).GetPlayerTurn())
            {
                GetPlayer(0).SetPlayerTurn(true);
                GetPlayer(1).SetPlayerTurn(false);
                SetCurrentPlayer(0);
            } 
            #endregion
        }


        // Displays the which players turn it is and the current scores for the players
        public void DisplayScoresPlayerTurn()
        {
            // Displays which players turn it is currently and set the Console foreground colour to
            // that of the current players colour
            Console.ForegroundColor = ConsoleColor.White;

            if (GetGameType() == "SCORE")
                Console.WriteLine("Player {0} - {1} has: {2} points \tPlayer {3} - {4} has: {5} points\t\tPoints Target: {6}\t\tRound Limit: {7}/{8}\n",
                    1, GetPlayer(0).GetPlayerName(), GetPlayer(0).GetPlayerScore(),
                    2, GetPlayer(1).GetPlayerName(), GetPlayer(1).GetPlayerScore(),
                    GetWinningScore(), GetRound(), GetNumberOfRounds());

            else if (GetGameType() == "ROUNDS")
                Console.WriteLine("Player {0} - {1} has: {2} points \tPlayer {3} - {4} has: {5} points\t\tRound: {6}/{7}\t\tScore Limit: {8}\n",
                    1, GetPlayer(0).GetPlayerName(), GetPlayer(0).GetPlayerScore(),
                    2, GetPlayer(1).GetPlayerName(), GetPlayer(1).GetPlayerScore(),
                    GetRound(), GetNumberOfRounds(), GetWinningScore());


            Console.ForegroundColor = GetPlayer(GetCurrentPlayer()).GetPlayerConsoleColour();

            Console.WriteLine("Player {0} - {1} to roll ... ",
                GetCurrentPlayer() + 1, GetPlayer(GetCurrentPlayer()).GetPlayerName());
        }


        // Display the results from the current game for the selected player
        public void DisplayPlayerResults(int player)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\n{0}S results were: [ ", GetPlayer(player).GetPlayerName());

            for (int results = 0; results < GetFinalResults().Length; results++)
                Console.Write(GetFinalResults()[results] + " ");
            Console.WriteLine("]\n");
        }


        // Display the die face counts from the current game for the selected player
        public void DisplayPlayerFaceCounts(int player)
        {
            for (int faceScores = 0; faceScores < GetDiceScoreValues().Length; faceScores++)
                Console.WriteLine("Number of {0}'s: {1}", (faceScores + 1), GetDiceScoreValues()[faceScores]);
        }


        // Initialises the settings for the game
        private void GameSettings()
        {
            // Loops until the user has selected a valid option for number of players in the game
            #region Select Oponent
            while (!isNumOfPlayers)
            {
                try
                {
                    Console.WriteLine("Please select the number of players from the options below: ");
                    Console.WriteLine("Option 1: Player VS Computer");
                    Console.WriteLine("Option 2: Player VS Player");

                    // User input for opponent options
                    int playerCount = Convert.ToInt32(Console.ReadLine());

                    // Sets the AI player of the game (if Player VS Computer is selected)
                    if (playerCount == 1)
                        isPlayerAI = true;

                    else
                        isPlayerAI = false;

                    // Sets the number of players in the game
                    SetNumberOfPlayers(2);

                    if (playerCount < 1 || playerCount > 2)
                        throw new Exception("Please select either options 1 or 2.");

                    // Break out of the loop
                    break;
                }
                catch (Exception players)
                {
                    Console.WriteLine(players.Message + " Press enter to try again now ....");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
            Console.Clear();
            #endregion

            // Loops until the user has selected a valid option for number of die per player in the game
            #region Select Number of Dice
            while (!isNumOfDie)
            {
                try
                {
                    Console.WriteLine("Please select the number of die per player from the options below: ");
                    Console.WriteLine("Option 1: 5 Die");
                    Console.WriteLine("Option 2: 7 Die");
                    Console.WriteLine("Option 3: 9 Die");

                    // User input for number of die options
                    int dieCount = Convert.ToInt32(Console.ReadLine());

                    // Sets the number of die per player in the game
                    if (dieCount == 1)
                        SetNumberOfDie(5);
                    else if (dieCount == 2)
                        SetNumberOfDie(7);
                    else if (dieCount == 3)
                        SetNumberOfDie(9);

                    if (dieCount < 1 || dieCount > 3)
                        throw new Exception("Please select either options 1, 2 or 3");

                    // Break out of the loop
                    break;
                }
                catch (Exception dice)
                {
                    Console.WriteLine(dice.Message + " Press enter to try again now ....");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
            Console.Clear();
            #endregion

            // Loops until the user has selected a valid option for number of faces per die in the game
            #region Select Number of Die Faces
            while (!isNumOfDieFaces)
            {
                try
                {
                    Console.WriteLine("Please select the number of faces per die from the options below: ");
                    Console.WriteLine("Option 1: 6  Faces");
                    Console.WriteLine("Option 2: 8  Faces");
                    Console.WriteLine("Option 3: 12 Faces");

                    // User input for number of die faces options
                    int dieFaceCount = Convert.ToInt32(Console.ReadLine());

                    // Sets the number of faces per die in the game
                    if (dieFaceCount == 1)
                        SetNumberOfDieFaces(6);
                    else if (dieFaceCount == 2)
                        SetNumberOfDieFaces(8);
                    else if (dieFaceCount == 3)
                        SetNumberOfDieFaces(12);

                    if (dieFaceCount < 1 || dieFaceCount > 3)
                        throw new Exception("Please select either options 1, 2 or 3");

                    // Break out of the loop
                    break;
                }
                catch (Exception faces)
                {
                    Console.WriteLine(faces.Message + " Press enter to try again now ....");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
            Console.Clear();
            #endregion

            // Loops until the user has selected a valid game mode - either 'Round Play' or 'Score Play'
            #region Select Game Mode
            while (!isScoreOrRounds)
            {
                try
                {
                    Console.WriteLine("Please select to 'Round Play' or 'Score Play': ");
                    Console.WriteLine("('Round Play' - players take turns untill all rounds are complete, highest score wins)");
                    Console.WriteLine("('Score Play' - players take turns untill one player reaches the winning score)");
                    Console.WriteLine("Option 1: Round Play");
                    Console.WriteLine("Option 2: Score Play");

                    // User input for game mode options
                    gameMode = Convert.ToInt32(Console.ReadLine());

                    // Sets the game mode of the game
                    if (gameMode == 1)
                        SetGameType("ROUNDS");

                    else if (gameMode == 2)
                        SetGameType("SCORE");

                    if (gameMode < 1 || gameMode > 2)
                        throw new Exception("Please select either options 1 or 2");

                    break;
                }

                catch (Exception mode)
                {
                    Console.WriteLine(mode.Message + " Press enter to try again now ....");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
            Console.Clear();
            #endregion

            // Loops until the user has selected the valid options for the game mode selected
            #region Select Game Mode Options
            SetNumberOfRounds(- GetNumberOfRounds());

            if (GetGameType() == "SCORE")
            {
                // Options to play to a winning score
                #region Score Play Game
                while (!isWinningScore)
                {
                    try
                    {
                        Console.WriteLine("Please select the winning score target from the options below: ");
                        Console.WriteLine("Option 1: 15 Points");
                        Console.WriteLine("Option 2: 25 Points");
                        Console.WriteLine("Option 3: 50 Points");

                        // User input for winning score options
                        int winScore = Convert.ToInt32(Console.ReadLine());

                        // Sets the winning score in the game
                        if (winScore == 1)
                            SetWinningScore(15);
                        else if (winScore == 2)
                            SetWinningScore(25);
                        else if (winScore == 3)
                            SetWinningScore(50);

                        // Sets the number of rounds for the game (set to a maximum number of rounds of 100)
                        // Since 'Score Play' has been selected this will ensure that the game does not stop
                        // due to a player reaching the final rounds e.g. a round limit 
                        SetNumberOfRounds(50);


                        if (winScore < 1 || winScore > 3)
                            throw new Exception("Please select either options 1, 2 or 3");

                        // Break out of the loop
                        break;
                    }
                    catch (Exception winValue)
                    {
                        Console.WriteLine(winValue.Message + " Press enter to try again now ....");
                        Console.ReadLine();
                        Console.Clear();
                    }
                }
                Console.Clear();
            }
            #endregion

            else if (GetGameType() == "ROUNDS")
            {
                // Option to play to a set number of rounds
                #region Round Play Game
                while (!isRoundsNum)
                {
                    try
                    {
                        Console.WriteLine("Please select the number of rounds in the game: ");
                        Console.WriteLine("Option 1: 5 rounds");
                        Console.WriteLine("Option 2: 10 rounds");
                        Console.WriteLine("Option 3: 15 rounds");

                        // User input for the round options
                        int rounds = Convert.ToInt32(Console.ReadLine());

                        // Sets the number of rounds in the game
                        if (rounds == 1)
                            SetNumberOfRounds(5);
                        else if (rounds == 2)
                            SetNumberOfRounds(10);
                        else if (rounds == 3)
                            SetNumberOfRounds(15);

                        // Sets the winning score for the game (set to a maximum score of 500)
                        // Since 'Round Play' has been selected this will ensure that the game does not stop
                        // due to a player reaching a winning score e.g a score limit
                        SetWinningScore(250);

                        if (rounds < 1 || rounds > 3)
                            throw new Exception("Please select either options 1 or 2");

                        break;
                    }

                    catch (Exception round)
                    {
                        Console.WriteLine(round.Message + " Press enter to try again now ....");
                        Console.ReadLine();
                        Console.Clear();
                    }
                }
                Console.Clear();
                #endregion
            }
            #endregion
        }


        // Returns the selected game rules as a string
        public string GameRules()
        {
            // Conditional statement to set the games rules versus string
            #region Display Opponent
            string versus = "";
            if (isPlayerAI)
                versus = "Computer Vs Player";
            else if (!isPlayerAI)
                versus = "Player Vs Player"; 
            #endregion

            // Return the string which lists the chosen game rules
            if (GetGameType() == "SCORE")
                return string.Format("\n\nPlayers: {0} // Dice per Player: {1} // Faces per Die: {2} // Winning Score: {3} // (Round Limit: {4})\n", versus, GetNumberOfDie(), GetNumberOfDieFaces(), GetWinningScore(), GetNumberOfRounds());
            else
                return string.Format("\n\nPlayers: {0} // Dice per Player: {1} // Faces per Die: {2} // Number of Rounds: {3} // (Score Limit: {4})\n",
                    versus, GetNumberOfDie(), GetNumberOfDieFaces(), GetNumberOfRounds(), GetWinningScore());

        }


        // Called when the game has ended, either starts a new game or quits the application
        public void StartNewGame()
        {
            // Options for restarting the game
            #region Restart Game Options
            while (!isRestartGame)
            {
                try
                {
                    Console.WriteLine("Would you like to play again?: ");
                    Console.WriteLine("Option 1: Yes play again");
                    Console.WriteLine("Option 2: No quit");

                    // User input for play again options
                   int playAgian = Convert.ToInt32(Console.ReadLine());

                    Console.Clear();

                    // If the player wants to play again start a new game
                    if (playAgian == 1)
                    #region Play Again
                    {
                        // String array to hold the players names from the last game played
                        string[] playerNames = new string[] { GetPlayers()[0].GetPlayerName(), GetPlayers()[1].GetPlayerName() };
                        bool isSettings = false;

                        while (!isSettings)
                        {
                            try
                            {
                                Console.WriteLine("Would you like to keep the same game settings or choose new game settings?");
                                Console.WriteLine("Option 1: Yes same games settings");
                                Console.WriteLine("Option 2: No new game settings");

                                // User input for play again options
                                int gameSettings = Convert.ToInt32(Console.ReadLine());

                                if (gameSettings == 1)
                                #region Initialise Game (Same Settings)
                                {
                                    NewGameHelper();
                                    Console.Clear();
                                } 
                                #endregion 

                                else if (gameSettings == 2)
                                #region Initialise Game (Different Settings)
                                {
                                    Console.Clear();
                                    NewGameNewSettingsHelper(playerNames);
                                    Console.Clear();
                                } 
                                #endregion

                                if (gameSettings < 1 || gameSettings > 2)
                                    throw new Exception("option is not valid.");
                                break;
                            }
                            catch (Exception settings)
                            {
                                Console.WriteLine(settings.Message + " Press enter to try again now ....");
                                Console.ReadLine();
                                Console.Clear();
                            }
                        }
                    } 
                    #endregion

                    //  Else quit the game
                    else if (playAgian == 2)
                    #region Quit Game
                    {
                        Console.WriteLine("Quiting ... Press enter to quit ...");
                        Console.ReadLine();
                        Environment.Exit(0);
                    } 
                    #endregion

                    else if (playAgian < 1 || playAgian > 2)
                        throw new Exception("Please select either options 1 or 2.");
                    break;
                }
                catch (Exception restart)
                {
                    Console.WriteLine(restart.Message + " Press enter to try again now ....");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
            Console.Clear();
            #endregion
        }


        // Called to help generate a new game and to reset the player scores, statistics and turn
        private void NewGameHelper()
        {
            bool[] turn = GetPlayersTurn();
            
            // Generates the rounds to be played in the game by the players (one round is one turn per player)
            GenerateRounds();

            // Reset each player in the game
            #region Reset Players
            for (int player = 0; player < players.Length; player++)
            {
                GetPlayers()[player].SetPlayerScore(-GetPlayers()[player].GetPlayerScore());
                GetPlayers()[player].SetPlayerTurn(turn[player]);
                GetPlayers()[player].GetStatistics().ResetGameHistory(numOfDieFaces);
            }

            // Reset the current player of the game
            if (GetPlayer(0).GetPlayerTurn())
                SetCurrentPlayer(0);
            if (GetPlayer(1).GetPlayerTurn())
                SetCurrentPlayer(1); 
            #endregion
        }


        // Called to help generate a new game and to reset the player scores, statistics and turn
        private void NewGameNewSettingsHelper(string[] playerNames)
        {
            // Called to to set the game settings for the the dice, players and other game settings
            GameSettings();

            // Initialises all the players, die and other game settings
            #region Initialises The Game
            // Initialises the size of the initial results array (size is equal to the number of die per player)
            initialResults = new int[numOfDie];

            // Initialises the size of the final results array (size is equal to the number of die per player)
            finalResults = new int[numOfDie];

            // Initialises the size of the dice score values array (size is equal to the number of faces per die)
            diceScoreValues = new int[numOfDieFaces];

            // Sets the players of the game (passed the current player names)
            SetPlayers(GetNumberOfDie(), isPlayerAI, playerNames);

            // Sets the the Dice array objects (calls the GetNumberOfDieFaces method)
            SetDice(GetNumberOfDieFaces());

            // Sets the number of rolls per player turn, the same as the number of dice per player (calls the GetNumberOfDie method)
            SetNumberOfRolls(GetNumberOfDie());

            // Reset each player in the game and all thier statistics, score and turns
            NewGameHelper();
            #endregion
        }
        #endregion
    }
    // End of the Game class
}