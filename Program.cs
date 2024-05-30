namespace Connect4;

class Program
{

    public static bool initialStart_Bool = true;

    /// Main function of the program. Handles the game loop and the initial start of the game.
    static void Main(string[] args)
    {
        // Main game loop
        do
        {
            // Load the game
            Game.Load_Function();

            // Reset the initial start flag
            initialStart_Bool = false;

        } while (Game.Rematch_Function()); // Loop until the user wants to play again

        // Clear the console
        Console.Clear();

        // Print farewell message
        System.Console.WriteLine("Have A Nice Day!");

        // Wait for 300 milliseconds
        Thread.Sleep(300);
    }

}