using System;
using System.Threading;

namespace ThreeOrMoreDiceGame
{
    // Start of the Player class
    class Player
	{
        // Player class fields for:
        // Name, Colour, Turn, Score, Dice Array,
        // Console Colour, Die Array and Statistics
		#region Player Class Fields
		private string playerName;
        private int playerScore;
        private string playerColour;
        private bool playerTrun;
        ConsoleColor consoleColour;

        private static readonly Random playerAI = new Random();
        private static readonly object sync = new object();

        private Die[] dice;
        private Statistics playerStats;
        #endregion

        // Player class Constructor - Constructor is detailed below
        #region Player Class Constructor
        // Default Player Constructor - Sets the player class fields for the constructed player
        // Passed parameters for the number of faces per die, the number of die per player, player name and player colour
        public Player(int numOfDie, string name, string colour, int dieFaces)
		{
			dice = new Die[numOfDie];
            playerStats = new Statistics(dieFaces);

			SetPlayerName(name);
			SetPlayerColour(colour);
			SetPlayerConsoleColour((ConsoleColor)Enum.Parse(typeof(ConsoleColor), colour));
			SetPlayerScore(0);
            SetPlayerTurn(false);
		}
        #endregion

        // Player class methods -  Methods are detailed below
        #region Player Class Methods
        // Sets the player name
        public void SetPlayerName(string name)
        { playerName = name; }


        // Returns player name
        public string GetPlayerName()
		{ return playerName; }


        // Sets the player colour 
        public void SetPlayerColour(string colour)
        { playerColour = colour; }


        // Returns the player colour
        public string GetPlayerColour()
		{ return playerColour; }


        // Sets the players console colour
        public void SetPlayerConsoleColour(ConsoleColor colour)
        { consoleColour = colour; }


        // Returns the players console colour
        public ConsoleColor GetPlayerConsoleColour()
		{ return consoleColour; }


        // Sets the player score
        public void SetPlayerScore(int score)
        { playerScore += score; }


        // Returns the player score
        public int GetPlayerScore()
		{ return playerScore; }


        // Sets the player turn
        public void SetPlayerTurn(bool turn)
        { playerTrun = turn; }


        // Returns the player turn
        public bool GetPlayerTurn()
        { return playerTrun; }


        // Sets the player dice with the number of die faces
        public void SetDie(int numOfFaces)
        {
            for (int index = 0; index < dice.Length; index++)
                dice[index] = new Die(numOfFaces, GetPlayerColour());
        }


        // Returns the player die from the die array by index
        public Die GetDie(int dieIndex)
		{ return dice[dieIndex]; }


        // Returns the player dice array
        public Die[] GetDice()
        { return dice; }


        // Resets the player dice array (called when a new game is started with new settings)
        public void ResetDice(int numOfDie)
        { dice = new Die[numOfDie]; }


        // Returns a random number after a short delay which selects options in the game for an AI player
        public int AIPlayerChoice()
        {
            Random timeDelay = new Random();
            int milliseconds = timeDelay.Next(1000, 1500);
            Thread.Sleep(milliseconds);
            int AIChoice;

            lock (sync)
                AIChoice = playerAI.Next(1, 3);
            return AIChoice;
        }


        // Rolls all the dice and returns the player dice
		public Die[] RollAllDice()
		{
			for (int index = 0; index < dice.Length; index++)
				dice[index].RollDie();
			return dice;
		} 


        // Returns the player statistics 
        public Statistics GetStatistics()
        { return playerStats; }
		#endregion
	}
    // End of the Player class
}
