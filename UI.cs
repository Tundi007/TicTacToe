namespace Connect4;

class MyUI
{

    public static bool UserInterface_Function(string menu_String, string firstOption_String, string secondOption_String, bool pointer_Bool, out bool output_Bool, out bool exit_Bool)
    {

        exit_Bool = false;

        output_Bool = false;

        Console.Clear();

        System.Console.WriteLine(menu_String);

        if (!pointer_Bool) System.Console.Write("->");

        System.Console.WriteLine(firstOption_String);

        if (pointer_Bool) System.Console.Write("->");

        System.Console.WriteLine(secondOption_String);

        switch (Console.ReadKey(true).Key)
        {

            case ConsoleKey.Enter:
                Console.Clear();
                return true;

            case ConsoleKey.Escape:                
                exit_Bool = true;
                break;

            case ConsoleKey.UpArrow:
                output_Bool = true;
                break;

            case ConsoleKey.DownArrow:
                output_Bool = true;
                break;

            default:
                break;

        }

        return false;

    }

    public static (int,int) GameInterface_Function(string error_String, int[,] gameBoard_SingleMatrix,int player_Int, string botInfo_String)
    {

        return KeyMenu_Function(error_String, gameBoard_SingleMatrix, player_Int, botInfo_String);

    }

    private static (int,int) KeyMenu_Function(string error_String, int[,] menuItems_ArrayString2D, int player_Int, string botInfo_String)
    {

        int menuPointerColumn_Int = 0;
        
        int menuPointerRow_Int = 0;

        string player_string = "O";

        if(player_Int == 2)
            player_string = "X";

        string hint_String =
            $"Player{player_Int}'s Turn ({player_string})\nUse Arrow Keys To Navigate, \"Enter\" To Select, \"Escape\": Main Menu ())";

        while (true)
        {

            Console.Clear();

            if(!string.IsNullOrWhiteSpace(botInfo_String))
                System.Console.WriteLine(botInfo_String);            

            ShowMenu_Function(menuItems_ArrayString2D, menuPointerColumn_Int, menuPointerRow_Int);

            if(!string.IsNullOrWhiteSpace(error_String))
                System.Console.WriteLine(error_String);

            error_String = "";

            System.Console.WriteLine(hint_String);

            switch (Console.ReadKey(true).Key)
            {

                case ConsoleKey.Enter:
                {
                    if(menuItems_ArrayString2D[menuPointerRow_Int,menuPointerColumn_Int] == 0)
                        return (menuPointerRow_Int,menuPointerColumn_Int);
                    else
                        error_String = "Please Select An Empty Space!";
                    
                }break;

                case ConsoleKey.A:
                    return (10 + menuPointerColumn_Int, menuPointerRow_Int);

                case ConsoleKey.Escape:
                {

                    bool pointer_Bool = false;

                    bool mainMenu_Bool = false;

                    while (!MyUI.UserInterface_Function($"All Progress Will Be Lost\nProceed?", "No", "Yes", pointer_Bool, out bool valid_Bool, out bool exit_Bool))
                    {            

                        if(exit_Bool)
                            return (-1,-1);

                        if(valid_Bool)
                            (mainMenu_Bool,pointer_Bool) =
                                (!mainMenu_Bool,!pointer_Bool);

                    }

                    if(mainMenu_Bool)
                        return (-2,-2);
                    
                    break;
                    
                }

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

                default:
                    error_String = "Undefined Input";
                    break;

            }

        }

    }

    public static void ShowMenu_Function(int[,] menuItems_ArrayString2D, int menuPointerColumn_Int, int menuPointerRow_Int)
    {

        System.Console.WriteLine();

        for (int rowNumber_Int = 0; rowNumber_Int < 3 ; rowNumber_Int++)
        {

            for (int columnNumber_Int = 0; columnNumber_Int < 3; columnNumber_Int++)
            {

                if(columnNumber_Int == menuPointerColumn_Int & rowNumber_Int == menuPointerRow_Int)
                {

                    System.Console.Write("[ ]");

                }else
                if (menuItems_ArrayString2D[rowNumber_Int, columnNumber_Int] == 1)
                {

                    System.Console.Write(" X ");

                }else
                if (menuItems_ArrayString2D[rowNumber_Int, columnNumber_Int] == 2)
                {

                    System.Console.Write(" O ");

                }else
                if (menuItems_ArrayString2D[rowNumber_Int, columnNumber_Int] == 0)
                {

                    System.Console.Write(" - ");

                }

            }

            System.Console.WriteLine();

        }

    }

}