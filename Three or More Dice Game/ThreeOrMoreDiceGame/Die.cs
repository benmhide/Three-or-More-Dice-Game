using System;

namespace ThreeOrMoreDiceGame
{
    // Start of the Die class
    public class Die
	{
		// Die class fields for:
		// Random Number Generator, Number on Top of the Die, Number of Die Faces and Die Colour
		#region Die Class Fields
		private static readonly Random randomNumberGenerator = new Random();
		private static readonly object sync = new object();
		private int numberOnTop;
		private int numberOfFaces;
		private string dieColour;
        #endregion

        // Die class contructor - Constructor is detailed below
        #region Die Class Constructor
        // Default Die constructor - Sets the die class fields for the constructed die
        // Passed parameters for the number of faces per die, and die (player) colour
        public Die(int numOfFaces, string colour)
		{
			SetNumberOfFaces(numOfFaces);
			SetDieColour(colour);
		}
        #endregion

        // Die class methods -  Methods are detailed below
        #region Die Class Methods
        // Rolls the dice and returns the number on top of the die
        public int RollDie()
		{
			lock (sync)
				numberOnTop = randomNumberGenerator.Next(1, GetNumberOfFaces() + 1);
            return numberOnTop;
		}


        // Returns the number on top of the die
		public int GetNumberOnTop()
		{ return numberOnTop; }


        // Sets the number of faces on the die
		public void SetNumberOfFaces(int faces)
		{ numberOfFaces = faces; }


        // Returns the number of faces in the die
		public int GetNumberOfFaces()
		{ return numberOfFaces; }


        // Sets the die colour (the die colour matches the player colour)
		public void SetDieColour(string colour)
		{ dieColour = colour; }


        // Returns the die colour (the die colour matches the player colour)
        public string GetDieColour()
		{ return dieColour; } 
		#endregion
	}
    // End of the Die class
}
