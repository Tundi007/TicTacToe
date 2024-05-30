namespace Connect4;

class MyUI
{

    private static string gameInfo_String = "";

    /// This function is a general interface that displays a menu and awaits user input
    public static bool UserInterface_Function(string menu_String, string firstOption_String, string secondOption_String, bool pointer_Bool, out bool output_Bool, out bool exit_Bool)
    {
        // Initialize outputs

        exit_Bool = false;

        output_Bool = false;

        // Clear console
        Console.Clear();

        // Display menu
        System.Console.WriteLine(menu_String);

        // Display pointer
        if (!pointer_Bool) System.Console.Write("->");

        System.Console.WriteLine(firstOption_String);

        if (pointer_Bool) System.Console.Write("->");

        System.Console.WriteLine(secondOption_String);

        // Await user input
        switch (Console.ReadKey(true).Key)
        {
            // If enter is pressed, clear console and return true

            case ConsoleKey.Enter:
                Console.Clear();
                return true;

            // If escape is pressed, set exit_Bool to true
            case ConsoleKey.Escape:            
                exit_Bool = true;
                break;

            // If up or down arrow is pressed, set output_Bool to true
            case ConsoleKey.UpArrow:
               output_Bool = true;
               break;

            case ConsoleKey.DownArrow:
                output_Bool = true;
                break;

            // If any other key is pressed, do nothing
            default:
                break;

        }

        // Return false
        return false;

    }

    /// This function is a general interface that displays a game menu and awaits user input
    public static (int,int) GameInterface_Function(int[,] menuItems_ArrayString2D, int lastRow_Int, int lastColumn_Int, int player_Int)
    {
        // Initialize variables
        string error_String = "";
        int menuPointerColumn_Int = lastColumn_Int;
        int menuPointerRow_Int = lastRow_Int;
        string symbol_string = "O";
        string player_String = "1";

        // Set symbol and player string based on player
        if(player_Int == 2)
        {
            (symbol_string, player_String) = ("X","2");
        }

        // Clear player string if single player mode
        if(Game.singlePlayer_Bool)
            player_String = "";

        // Create hint string
        string hint_String =
            $"Player{player_String}'s Turn ({symbol_string})\nUse Arrow Keys To Navigate, \"Enter\" To Select, \"Escape\": Main Menu)";

        // Display game menu
        while (true)
        {
            Console.Clear();          
            System.Console.WriteLine(gameInfo_String);
            ShowMenu_Function(menuItems_ArrayString2D, menuPointerRow_Int, menuPointerColumn_Int);
            if(!string.IsNullOrWhiteSpace(error_String))
                System.Console.WriteLine(error_String);
            error_String = "";
            System.Console.WriteLine(hint_String);

            // Await user input
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.LeftArrow:
                {
                    if(menuPointerColumn_Int < 1) break;
                    menuPointerColumn_Int--;
                }break;

                case ConsoleKey.RightArrow:
                {
                    if (menuPointerColumn_Int > 1) break;
                    menuPointerColumn_Int++;
                }break;

                case ConsoleKey.UpArrow:
                {
                    if(menuPointerRow_Int < 1) break;
                    menuPointerRow_Int--;
                }break;

                case ConsoleKey.DownArrow:
                {
                    if (menuPointerRow_Int > 1) break;
                    menuPointerRow_Int++;
                }break;

                case ConsoleKey.Enter:
                {
                    if(menuItems_ArrayString2D[menuPointerRow_Int,menuPointerColumn_Int] == 0)
                        return (menuPointerRow_Int,menuPointerColumn_Int);
                    else
                        error_String = "Please Select An Empty Space!";
                }break;

                case ConsoleKey.A: return(-3,-3);

                case ConsoleKey.Escape:
                {
                    bool pointer_Bool = false;
                    bool mainMenu_Bool = false;

                    // Display confirmation for exiting to main menu
                    while (!MyUI.UserInterface_Function($"All Progress Will Be Lost\nProceed?", "No", "Yes", pointer_Bool, out bool valid_Bool, out bool exit_Bool))
                    {            
                        if(exit_Bool)
                            return (-2,-2);
                        if(valid_Bool)
                            (mainMenu_Bool,pointer_Bool) =
                                (!mainMenu_Bool,!pointer_Bool);
                    }

                    if(mainMenu_Bool)
                        return (-1,-1);
                }break;

                default:
                    error_String = "Undefined Input";
                    break;

            }

        }

    }

    /// Sets the game info string to the provided
    public static void GameInfo_Function(string info_String)
    {        
        // Set the game info string to the provided string
        gameInfo_String = info_String;
    }

    /// Displays a menu given a 2D array of menu items and the coordinates of the pointer
    public static void ShowMenu_Function(int[,] menuItems_ArrayString2D, int menuPointerRow_Int, int menuPointerColumn_Int)
    {
        // Print each row of the menu
        System.Console.WriteLine();
        for (int rowNumber_Int = 0; rowNumber_Int < 3 ; rowNumber_Int++)
        {
            // Print each column of the row
            for (int columnNumber_Int = 0; columnNumber_Int < 3; columnNumber_Int++)
            {
                // If the current cell is the menu pointer
                if(rowNumber_Int == menuPointerRow_Int && columnNumber_Int == menuPointerColumn_Int)
                {
                    // Print the appropriate symbol
                    if(menuItems_ArrayString2D[rowNumber_Int,columnNumber_Int] == 1)
                        System.Console.Write("[O]");
                    else if (menuItems_ArrayString2D[rowNumber_Int,columnNumber_Int] == 2)
                        System.Console.Write("[X]");
                    else
                        System.Console.Write("[-]");
                }
                else
                {
                    // Print the appropriate symbol
                    if (menuItems_ArrayString2D[rowNumber_Int,columnNumber_Int] == 1)
                        System.Console.Write(" O ");
                    else if (menuItems_ArrayString2D[rowNumber_Int,columnNumber_Int] == 2)
                        System.Console.Write(" X ");
                    else
                        System.Console.Write(" - ");
                }
            }
            // Print a new line after each row
            System.Console.WriteLine();
        }
    }

}