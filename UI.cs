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

    public static (int,int) GameInterface_Function(string error_String, int[,] gameBoard_SingleMatrix,int player_Int, int lastColumn_Int, int lastRow_Int, string botInfo_String)
    {

        return KeyMenu_Function(error_String, gameBoard_SingleMatrix, player_Int, lastColumn_Int , lastRow_Int, botInfo_String);

    }

    private static (int,int) KeyMenu_Function(string error_String, int[,] menuItems_ArrayString2D, int player_Int, int lastColumn_Int, int lastRow_Int,string botInfo_String)
    {

        List<int>[] available_ListInt = new List<int>[3];

        int menuPointerColumn_Int = 0;
        
        int menuPointerRow_Int = 0;

        int listColumn_Int = 0;
        
        int listRow_Int = 0;

        string player_string = "O";

        if(player_Int == 2)
            player_string = "X";

            for (int row_int = 0; row_int < 3; row_int++)
            {

                for (int column_Int = 0; column_Int < 3; column_Int++)
                {

                    if (menuItems_ArrayString2D[row_int, column_Int] == 0)
                        available_ListInt[row_int].Add(column_Int);
                                        
                }
                
            }

        for (int index_Int = 0; index_Int < 3; index_Int++)
        {

            if(available_ListInt[index_Int].Count != 0)
            {

                (menuPointerRow_Int, menuPointerColumn_Int) =
                    (0,available_ListInt[index_Int].ElementAt(0));

                listRow_Int = menuPointerRow_Int;

                break;

            }

        }

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
                    return (menuPointerColumn_Int,menuPointerRow_Int);

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

                    if(listColumn_Int < 1) break;

                    menuPointerColumn_Int = available_ListInt[listRow_Int].ElementAt(listColumn_Int--);

                }break;

                case ConsoleKey.RightArrow:
                {

                    if (listColumn_Int >= available_ListInt[listRow_Int].Count-1) break;

                    menuPointerColumn_Int = available_ListInt[listRow_Int].ElementAt(listColumn_Int++);

                }break;

                case ConsoleKey.UpArrow:
                {

                    if(listRow_Int < 1) break;

                    if(available_ListInt[listRow_Int-1].Count == 0)
                        if(listRow_Int == 0) break;
                        else
                        if(available_ListInt[listRow_Int-2].Count == 0) break;
                        else
                            listRow_Int = 0;

                    if(available_ListInt[listRow_Int-1].Count - 1 < listColumn_Int)
                        listColumn_Int = available_ListInt[listRow_Int-1].Count - 1;
 
                    menuPointerColumn_Int = available_ListInt[listRow_Int--].ElementAt(listColumn_Int);

                }break;

                case ConsoleKey.DownArrow:
                {

                    if (menuPointerRow_Int >= 2) break;

                    if(available_ListInt[listRow_Int+1].Count == 0)
                        if(listRow_Int == 2) break;
                        else
                        if(available_ListInt[listRow_Int+2].Count == 0) break;
                        else
                            listRow_Int = 2;

                    if(available_ListInt[listRow_Int].Count - 1 < listColumn_Int)
                        listColumn_Int = available_ListInt[listRow_Int-1].Count - 1;
 
                    menuPointerColumn_Int = available_ListInt[listRow_Int++].ElementAt(listColumn_Int);

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