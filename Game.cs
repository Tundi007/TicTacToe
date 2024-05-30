using System.Linq.Expressions;

namespace Connect4;

class Game
{

    private static int lastColumn_Int = 0;

    private static int lastRow_Int = 0;

    private static int player1_Int;

    private static int player2_Int;

    private static bool botFirst_Bool;

    public static bool singlePlayer_Bool;

    private static readonly bool sameRules_Bool = false;

    /// Load function is responsible for setting up the game rules and starting the game.
    public static void Load_Function()
    {
        // If the game is not being played with the same rules, reset the game variables.
        if (!sameRules_Bool)
        {
            lastRow_Int = 0;
            lastColumn_Int = 0;
            player1_Int = 1;
            player2_Int = 2;
            botFirst_Bool = false;
            singlePlayer_Bool = true;
        }

        // If the game is being started for the first time, display a loading screen.
        if (Program.initialStart_Bool)
        {
            string loading_String = "[                    ]";
            bool loading_Bool = true;

            // Loop through the loading animation.
            for (int loading_Int = 2; loading_Int < 23; loading_Int++)
            {
                Console.Clear();            
                System.Console.Write("Loading");
                System.Console.Write(loading_String);

                if (loading_Int == 22) System.Console.WriteLine("100%");
                else System.Console.WriteLine((int)((double)loading_Int / 23 * 100) + "%");

                loading_String = loading_String[..(loading_Int - 1)] + "-" + loading_String[(loading_Int)..];

                if (loading_Int == 4) Thread.Sleep(200);

                if (loading_Bool)
                {
                    if (loading_Int % 1 == 0) Thread.Sleep(1);
                    if (loading_Int % 3 == 0) Thread.Sleep(30);
                    if (loading_Int % 5 == 0) Thread.Sleep(100);
                    if (loading_Int % 6 == 0) Thread.Sleep(200);
                    if (loading_Int % 7 == 0) Thread.Sleep(300);
                    if (loading_Int % 10 == 0)
                    {
                        Thread.Sleep(400);
                        loading_Bool = false;
                    }
                }
                else Thread.Sleep(5);
            }

            Thread.Sleep(200);
        }

        Console.Clear();

        // If the game is not being played with the same rules, display the game mode selection menu.
        if (!sameRules_Bool)
            GameMode_Function();

        // Start the game.
        Game_Function();
    }

    /// Function that handles the game mode selection menu.
    private static void GameMode_Function()
    {
        // Initialize variables
        bool pointer_Bool = false;
        string player1_String = "Player 1";
        string player2_String = "Player 2";

        // Game mode selection loop
        while (!MyUI.UserInterface_Function("Select Game Mode:", "PvE (Single Player)", "PvP (Couch Play)", pointer_Bool,
            out bool valid_Bool, out bool exit_Bool))
        {
            // If exit is triggered, exit the game
            if (exit_Bool)
                PrematureExit_Function();

            // If valid input is triggered, toggle single player mode
            if (valid_Bool)
                (singlePlayer_Bool, pointer_Bool) = (!singlePlayer_Bool, !pointer_Bool);
        }

        // If single player mode is selected, update player names
        if (singlePlayer_Bool)
        {
            player1_String = "You";
            player2_String = "Bot";
        }

        // First player selection loop
        pointer_Bool = false;
        while (!MyUI.UserInterface_Function("Who Is First?", player1_String, player2_String, pointer_Bool,
            out bool valid_Bool, out bool exit_Bool))
        {
            // If exit is triggered, exit the game
            if (exit_Bool)
                PrematureExit_Function();

            // If valid input is triggered, toggle player IDs
            if (valid_Bool)
                (player1_Int, player2_Int, pointer_Bool) = (player2_Int, player1_Int, !pointer_Bool);
        }

        // If single player mode is selected, initialize the bot
        if (singlePlayer_Bool)
        {
            pointer_Bool = false;
            bool alphaBeta_Bool = true;

            // Bot configuration selection loop
            while (!MyUI.UserInterface_Function("Minimax Configurations:", "Pruning", "Vanilla", pointer_Bool,
                out bool valid_Bool, out bool exit_Bool))
            {
                // If exit is triggered, exit the game
                if (exit_Bool)
                    PrematureExit_Function();

                // If valid input is triggered, toggle bot configurations
                if (valid_Bool)
                    (alphaBeta_Bool, pointer_Bool) = (!alphaBeta_Bool, !pointer_Bool);
            }

            // Initialize the bot
            Bot.BotInitialization_Function(alphaBeta_Bool, player1_Int, player2_Int);
        }
    }

    /// The main game loop that handles the game logic.
    private static void Game_Function()
    {
        // Reset the game board
        GameBoard.GameBoardReset_Function();

        // If single player mode is selected, start the game loop
        if (singlePlayer_Bool)
        {
            // Game loop for single player mode
            while (true)
            {
                // If it's the bot's turn, make a move
                if (botFirst_Bool)
                {
                    (int row_int, int column_Int) = Bot.Bot_Function();

                    // Place the bot's element on the game board
                    if(!GameBoard.ElementPlace_Function(row_int, column_Int, player2_Int))
                        break;
                }
                else // If it's the player's turn, let them make a move
                    if (!PlayerTurn_Function(player1_Int))
                        break;

                // Check if the game is over
                if (CheckGoal_Function())
                    break;

                // If it's the bot's turn, let them make a move
                if(botFirst_Bool)
                {
                    if (!PlayerTurn_Function(player1_Int))
                        break;
                }
                else // If it's the player's turn, let them make a move
                {
                    (int row_int, int column_Int) = Bot.Bot_Function();

                    // Place the bot's element on the game board
                    if (!GameBoard.ElementPlace_Function(row_int, column_Int, player2_Int))
                        break;
                }

                // Check if the game is over
                if (CheckGoal_Function())
                    break;
            }
        }
        else // If multiplayer mode is selected, start the game loop
        {
            // Game loop for multiplayer mode
            while (true)
            {
                // Let the first player make a move
                if (!PlayerTurn_Function(player1_Int))
                    break;

                // Check if the game is over
                if (CheckGoal_Function())
                    break;

                // Let the second player make a move
                if (!PlayerTurn_Function(player2_Int))
                    break;

                // Check if the game is over
                if (CheckGoal_Function())
                    break;
            }
        }

    }

    /// Handles the player's turn in the game loop.
    private static bool PlayerTurn_Function(int playerID_Int)
    {
        // Game loop for the player's turn
        while (true)
        {
            // Get the element row and column chosen by the player
            (int elementRow_Int, int elementColumn_Int) = MyUI.GameInterface_Function(
                GameBoard.GameBoardStatus_Function(), lastRow_Int, lastColumn_Int, playerID_Int);

            // If the player has chosen a valid element, update the last row and column
            if (elementColumn_Int > -1)
            {
                lastColumn_Int = elementColumn_Int;
                lastRow_Int = elementRow_Int;
            }

            // If the player has chosen to exit the game, exit the game
            if (elementColumn_Int == -2)
                PrematureExit_Function();

            // If the player has chosen to end the match, end the match
            if (elementColumn_Int == -1)
                return false;

            // If the player has chosen to initialize the bot, initialize the bot
            if (elementColumn_Int == -3)
                Bot.BotInitialization_Function(false, -1, -1);

            // If the player has chosen a valid location to place their element, place the element and end the player's turn
            if (GameBoard.ElementPlace_Function(elementRow_Int, elementColumn_Int, playerID_Int))
                return true;
            else // If the player has chosen an invalid location, display an error message                
                System.Console.WriteLine("Invalid Move!");

            // Wait for one second before asking the player for another move
            Thread.Sleep(1000);
        }
    }

    /// Checks if the game is over and returns true if it is.
    private static bool CheckGoal_Function()
    {
        // Initialize variable to store the winner's ID. Default value is -1, which means no winner yet.
        int winner_int = -1;

        // Loop through all the player IDs (1 and 2)
        for (int ID_Int = 1; ID_Int < 3; ID_Int++)
        {
            // If a winner has already been found, break the loop
            if (winner_int == player2_Int || winner_int == player1_Int)
                break;

            // Check if the board is filled with player's ID in reverse diagonal
            if (GameBoard.GameBoardSubStatus_Function(new int[1,1], 3, -1).All(element => element == ID_Int))
            {
                winner_int = ID_Int;
                break;
            }

            // Check if the board is filled with player's ID in diagonal
            if (GameBoard.GameBoardSubStatus_Function(new int[1,1], 2, -1).All(element => element == ID_Int))
            {
                winner_int = ID_Int;
                break;
            }

            // Check if the board is filled with player's ID in rows and columns
            for (int index_Int = 0; index_Int < 3; index_Int++)
            {
                if (GameBoard.GameBoardSubStatus_Function(new int[1,1], 0, index_Int).All(element => element == ID_Int))
                {
                    winner_int = ID_Int;
                    break;
                }

                if (GameBoard.GameBoardSubStatus_Function(new int[1,1], 1, index_Int).All(element => element == ID_Int))
                {
                    winner_int = ID_Int;
                    break;
                }
            }
        }

        // If no winner has been found and the board is full, it is a tie
        if (winner_int == -1 && !GameBoard.GameBoardSubStatus_Function(new int[1,1], 4, -1).Any(element => element == 0))
        {
            Console.Clear();
            System.Console.Write("Tied, Game Over!");
            MyUI.ShowMenu_Function(GameBoard.GameBoardStatus_Function(), -1, -1);
            System.Console.WriteLine("Press Anything To Continue");
            Console.ReadKey();
            return true;
        }

        // If player 1 won, display the winner message and return true
        if (winner_int == player1_Int)
        {
            string player_String = "";
            Console.Clear();
            if (singlePlayer_Bool)
                player_String = "You Won!";
            else
                player_String = "Player 1 Won!";
            System.Console.Write(player_String);
            MyUI.ShowMenu_Function(GameBoard.GameBoardStatus_Function(), -1, -1);
            System.Console.WriteLine("Press Anything To Continue");
            Console.ReadKey();
            return true;
        }

        // If player 2 won, display the winner message and return true
        if (winner_int == player2_Int)
        {
            string player_String = "";
            Console.Clear();
            if (singlePlayer_Bool)
                player_String = "You Lost, Better Luck Next Time!";
            else
                player_String = "Player 2 Won!";
            System.Console.Write(player_String);
            MyUI.ShowMenu_Function(GameBoard.GameBoardStatus_Function(), -1, -1);
            System.Console.WriteLine("Press Anything To Continue");
            Console.ReadKey();
            return true;
        }

        // If no winner has been found, return false
        return false;
    }

    /// Function to check if the player wants to rematch.
    public static bool Rematch_Function()
    {
        // Initialize option_Bool to true and pointer_Bool to false
        bool option_Bool = true;
        bool pointer_Bool = false;

        // Loop until the player selects an option or exits the function
        while(!MyUI.UserInterface_Function("Rematch? (Use Up/Down Arrow Keys, Escape To Exit)", "Yes", "No", pointer_Bool, out bool valid_Bool, out bool exit_Bool))
        {
            // If the player has exited the function, call the PrematureExit_Function
            if (exit_Bool)
                PrematureExit_Function();

            // If the player has selected a valid option, toggle the option_Bool and pointer_Bool
            if (valid_Bool)
                (option_Bool, pointer_Bool) = (!option_Bool, !pointer_Bool);
        }

        // Return the value of option_Bool
        return option_Bool;
    }

    /// Function to handle a premature exit from the game.
    private static void PrematureExit_Function()
    {
        // Clear the console
        Console.Clear();

        // Display a message
        System.Console.WriteLine("Have A Nice Day!");

        // Wait for 300 milliseconds
        Thread.Sleep(300);

        // Terminate the program
        Environment.Exit(0);
    }

}