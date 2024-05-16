using MathNet.Numerics.LinearAlgebra;

namespace Connect4;

class Game
{

    private static int lastColumn_Int = 0;

    private static int player1_Int;

    private static int player2_Int;

    private static bool botFirst_Bool;

    private static bool singlePlayer_Bool;

    public static bool sameRules_Bool = false;

    private static string error_String = "";

    private static string botInfo_String = "";

    public static (int,int) GetTeams_Function()
    {
    
        return(player1_Int,player2_Int);
    
    }
    
    private static void SideSelect_Function()
    {

        bool pointer_Bool = false;

        string singlePlayer_String = "";

        if(!singlePlayer_Bool)
            singlePlayer_String = "First Player, ";

        while(!MyUI.UserInterface_Function($"{singlePlayer_String}Select Your Side (Use Up/Down Arrow Keys, Escape To Exit):", "X", "O", pointer_Bool,out bool valid_Bool, out bool exit_Bool))
        {

            if(exit_Bool)
                PrematureExit_Function();

            if(valid_Bool)
                (player1_Int, player2_Int,pointer_Bool) = (player2_Int, player1_Int,!pointer_Bool);

        }

        pointer_Bool = false;

        while(!MyUI.UserInterface_Function("Who Goes First?:", "You", "Bot", pointer_Bool,out bool valid_Bool, out bool exit_Bool))
        {

            if(exit_Bool)
                PrematureExit_Function();

            if(valid_Bool)
                (botFirst_Bool, pointer_Bool) = (!botFirst_Bool, !pointer_Bool);

        }        

    }

    private static void GameMode_Function()
    {

        bool pointer_Bool = false;

        while(!MyUI.UserInterface_Function("Select Game Mode (Use Up/Down Arrow Keys, Escape To Exit):", "PvE (Single Player)", "PvP (Couch Play)", pointer_Bool,out bool valid_Bool, out bool exit_Bool))
        {

            if(exit_Bool)
                PrematureExit_Function();

            if(valid_Bool)
                (singlePlayer_Bool, pointer_Bool) = (!singlePlayer_Bool, !pointer_Bool);

        }

        if(singlePlayer_Bool)
        {

            pointer_Bool = false;

            bool customConfig_Bool = true;

            SideSelect_Function();
        
            while(!MyUI.UserInterface_Function("Game Configurations (Use Up/Down Arrow Keys, Escape To Exit):", "AddOn", "Default", pointer_Bool,out bool valid_Bool, out bool exit_Bool))
            {

                if(exit_Bool)
                    PrematureExit_Function();

                if(valid_Bool)
                    (customConfig_Bool, pointer_Bool) = (!customConfig_Bool, !pointer_Bool);

            }

            if(customConfig_Bool)
                BotAddon_Function(false);
            else
                botInfo_String = Bot.BotSet_Function(false, true, player2_Int, player1_Int);

        }

    }

    private static void BotAddon_Function(bool sameBot_Bool)
    {

        if(sameBot_Bool)
            return;
            
        bool pointer_Bool = false;

        bool botDifficulty_Bool = true;

        while (!MyUI.UserInterface_Function($"Select Bot Configuration:", "Upgraded", "Dumb", pointer_Bool, out bool valid_Bool, out bool exit_Bool))
        {            

            if (exit_Bool) Game.PrematureExit_Function();

            if(valid_Bool)
                (botDifficulty_Bool,pointer_Bool) = (!botDifficulty_Bool,!pointer_Bool);

        }

        botInfo_String = Bot.BotSet_Function(botDifficulty_Bool, false, player2_Int, player1_Int);
    
    }

    public static void Load_Function()
    {

        if(!sameRules_Bool)
        {
            
            lastColumn_Int = 0;

            player1_Int = 1;

            player2_Int = 2;

            botFirst_Bool = false;

            singlePlayer_Bool = true;

            botInfo_String = "";
            
        }

        error_String = "";

        if(Program.initialStart_Bool)
        {

            string loading_String = "[                    ]";

            bool loading_Bool = true;

            for (int loading_Int = 2; loading_Int < 23; loading_Int++)
            {            

                Console.Clear();            

                System.Console.Write("Loading");

                System.Console.Write(loading_String);

                if(loading_Int == 22)System.Console.WriteLine("100%");
                else System.Console.WriteLine((int)((double)loading_Int/23*100)+"%");

                loading_String = loading_String[..(loading_Int-1)] + "-" + loading_String[(loading_Int)..];

                if(loading_Int == 4)Thread.Sleep(200);

                if(loading_Bool)
                {

                    if(loading_Int%1==0)Thread.Sleep(1);

                    if(loading_Int%3==0)Thread.Sleep(30);

                    if(loading_Int%5==0)Thread.Sleep(100);

                    if(loading_Int%6==0)Thread.Sleep(200);

                    if(loading_Int%7==0)Thread.Sleep(300);

                    if(loading_Int%10==0)
                    {

                        Thread.Sleep(400);

                        loading_Bool = false;

                    }

                }else Thread.Sleep(5);

            }

            Thread.Sleep(200);
        
        }

        Console.Clear();

        if(!sameRules_Bool)
            GameMode_Function();

        Game_Function();

    }

    private static void Game_Function()
    {

        GameBoard.GameBoardReset_Function();
    
        error_String = "";

        if(singlePlayer_Bool)
            while (true)
            {

                if(botFirst_Bool)
                    Action_Function(Bot.Bot_Function(), player2_Int);
                else if(!PlayerTurn_Function(player1_Int))
                    break;
                
                if(CheckGoal_Function())
                    break;

                if(botFirst_Bool)
                {

                    if(!PlayerTurn_Function(player1_Int))
                        break;

                }else
                    Action_Function(Bot.Bot_Function(), player2_Int);

                if(CheckGoal_Function())
                    break;

            }
        else
            while (true)
            {

                if(!PlayerTurn_Function(player1_Int))
                    break;
                
                if(CheckGoal_Function())
                    break;

                if(!PlayerTurn_Function(player2_Int))
                    break;

                if(CheckGoal_Function())
                    break;

            }
    
    }

    private static bool PlayerTurn_Function(int player_Int)
    {

        while (true)
        {

            int elementColumn_Int = MyUI.GameInterface_Function(error_String, GameBoard.GameBoardStatus_Function(), player_Int, lastColumn_Int,botInfo_String);

            error_String = "";

            lastColumn_Int = elementColumn_Int;

            if(elementColumn_Int == -1)
                PrematureExit_Function();

            if(elementColumn_Int == -2)
                return false;

            if(elementColumn_Int > 9)
            {

                botInfo_String = Bot.Visiblity_Function();

                lastColumn_Int = elementColumn_Int - 10;
                
            }
            else if(Action_Function(elementColumn_Int, player_Int))                
                return true;
            else
                error_String = $"Can't Place There (Column: {elementColumn_Int + 1})";
        }

    }

    private static bool Action_Function(int elementColumn_Int, int ID_Int)
    {
        
        if(GameBoard.ElementValidColumn_Function(
            GameBoard.GameBoardStatus_Function(), elementColumn_Int, out int elementRow_Int))
            if(GameBoard.ElementPlace_Function(elementRow_Int, elementColumn_Int, ID_Int))
                return true;

        return false;
    
    }
    
    private static bool CheckGoal_Function()
    {

        int winner_int = -1;

        for (int corner_Int = 1; corner_Int < 5; corner_Int++)
        {

            if(winner_int == player2_Int | winner_int == player1_Int)
                break;

            GameBoard.SubMatrix_Function(GameBoard.GameBoardStatus_Function(),
                corner_Int, out Matrix<float> fourByFour_SingleMatrix);

            Matrix<float> mirror_SingleMatrix = Matrix<float>.Build.Dense(4,4,0);

            for (int i = 0; i < 4; i++)
            {

                mirror_SingleMatrix[3-i,i] = 1;
                
            }

            for (int ID_Int = 1; ID_Int < 3; ID_Int++)
            {

                if(winner_int == player2_Int | winner_int == player1_Int)
                    break;

                if(fourByFour_SingleMatrix.Diagonal().ToList().All(x => x == ID_Int))
                {

                    winner_int = ID_Int;

                    break;

                }

                if(fourByFour_SingleMatrix.Multiply(mirror_SingleMatrix).Diagonal().ToList().All(x => x == ID_Int))
                {

                    winner_int = ID_Int;

                    break;

                }

                for (int index_Int = 0; index_Int < 4; index_Int++)
                {

                    if(fourByFour_SingleMatrix.Row(index_Int).ToList().All(x => x == ID_Int))
                    {

                        winner_int = ID_Int;

                        break;

                    }

                    if(fourByFour_SingleMatrix.Column(index_Int).ToList().All(x => x == ID_Int))
                    {

                        winner_int = ID_Int;

                        break;

                    }
                    
                }

            }

        }

        if(winner_int == -1 & GameBoard.GameBoardStatus_Function().Find(x => x == 0) == null)
        {

            Console.Clear();
            
            System.Console.Write("Tied, Game Over!");

            MyUI.ShowMenu_Function(GameBoard.GameBoardStatus_Function(), -1);

            System.Console.WriteLine("Press Anything To Continue");

            Console.ReadKey();

            return true;

        }

        if(winner_int == player1_Int)
        {

            string player_String = "";

            Console.Clear();

            if(singlePlayer_Bool)
                player_String = "You Won!";
            else
                player_String = "Player 1 Won!";
            
            System.Console.Write(player_String);

            MyUI.ShowMenu_Function(GameBoard.GameBoardStatus_Function(), -1);

            System.Console.WriteLine("Press Anything To Continue");

            Console.ReadKey();

            return true;
            
        }

        if(winner_int == player2_Int)
        {

            string player_String = "";

            Console.Clear();

            if(singlePlayer_Bool)
                player_String = "You Lost, Better Luck Next Time!";
            else
                player_String = "Player 2 Won!";
            
            System.Console.Write(player_String);

            System.Console.Write("You Lost, Better Luck Next Time!");

            MyUI.ShowMenu_Function(GameBoard.GameBoardStatus_Function(), -1);

            System.Console.WriteLine("Press Anything To Continue");

            Console.ReadKey();

            return true;
        
        }

        return false;

    }

    public static bool Rematch_Function()
    {

        bool option_Bool = false;

        bool pointer_Bool = false;

        while(!MyUI.UserInterface_Function("Rematch? (Use Up/Down Arrow Keys, Escape To Exit)", "No", "Yes", pointer_Bool,out bool valid_Bool, out bool exit_Bool))
        {

            if(exit_Bool)
                PrematureExit_Function();

            if(valid_Bool)
                (option_Bool,pointer_Bool) = (!option_Bool,!pointer_Bool);

        }

        if(option_Bool)
        {

            pointer_Bool = false;

            sameRules_Bool = false;

            while(!MyUI.UserInterface_Function("Select One (Use Up/Down Arrow Keys, Escape To Exit)", "New Game", "Same Game", pointer_Bool,out bool valid_Bool, out bool exit_Bool))
            {

                if(exit_Bool)
                    PrematureExit_Function();

                if(valid_Bool)
                    (sameRules_Bool,pointer_Bool) = (!sameRules_Bool,!pointer_Bool);

            }
            
        }

        return option_Bool;

    }

    private static void PrematureExit_Function()
    {

        Console.Clear();

        System.Console.WriteLine("Have A Nice Day!");

        Thread.Sleep(300);

        Environment.Exit(0);

    }

}