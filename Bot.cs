using System.Security.Cryptography;
namespace Connect4;

class Bot
{

    private static bool upgradedBot_Bool = false;

    private static bool clumsyBot_Bool = false;

    private static bool visibleAlgorithm_Bool = false;

    private static int playerID_Int = 1;

    private static int botID_Int = 2;

    public static (int,int) Bot_Function()
    {

        int bestCost_Int = -10000;

        int bestColumnMove_Int = -1;

        int bestRowMove_Int = -1;

        List<int[]> botMaxPieces_Int = [];

        int[] playerMaxPieces_Int = new int[3];

        for (int elementColumn_Int = 0; elementColumn_Int < 3; elementColumn_Int++)
        {

            botMaxPieces_Int.Add(new int[2]);

            for (int elementRow_Int = 0; elementRow_Int < 3; elementRow_Int++)
            {

                botMaxPieces_Int[elementColumn_Int][0] = -100;

                botMaxPieces_Int[elementColumn_Int][1] = elementRow_Int;

                if(GameBoard.GameBoardStatus_Function()[elementRow_Int,elementColumn_Int] == 0)
                {

                    playerMaxPieces_Int[elementColumn_Int] = -100;

                    int[,] botBoard_2dArrayInt = GameBoard.GameBoardStatus_Function();

                    botBoard_2dArrayInt[elementRow_Int,elementColumn_Int] = botID_Int;
                    
                    int number_Int;

                    if(upgradedBot_Bool)
                        number_Int = UpgradedMax_Function(botBoard_2dArrayInt, botID_Int);
                    else
                        number_Int = Max_Function(botBoard_2dArrayInt, botID_Int);

                    if(botMaxPieces_Int[elementColumn_Int][0] < number_Int)
                    {

                        botMaxPieces_Int[elementColumn_Int][0] = number_Int;
                        
                        botMaxPieces_Int[elementColumn_Int][1] = elementRow_Int;
                    
                    }
                
                    if(botMaxPieces_Int[elementColumn_Int][0] == 100)
                    {

                        ShowSteps(botBoard_2dArrayInt,100,0,elementColumn_Int, elementRow_Int);
                    
                        return (elementColumn_Int,elementRow_Int);
                        
                    }


                    for (int playerColumn_Int = 0; playerColumn_Int < 3; playerColumn_Int++)
                    {

                        for (int playerRow_Int = 0; playerRow_Int < 3; playerRow_Int++)
                        {

                            if(botBoard_2dArrayInt[playerRow_Int,playerColumn_Int] == 0)
                            {

                                int min_Int;

                                int[,] playerBoard_2dArrayInt = botBoard_2dArrayInt;

                                playerBoard_2dArrayInt[playerRow_Int,playerColumn_Int] = playerID_Int;

                                if(upgradedBot_Bool)
                                    min_Int = UpgradedMax_Function(playerBoard_2dArrayInt, playerID_Int);
                                else
                                    min_Int = Max_Function(playerBoard_2dArrayInt, playerID_Int);

                                if(min_Int == 100)
                                    min_Int = 50;

                                if(playerMaxPieces_Int[elementColumn_Int] < min_Int)
                                    playerMaxPieces_Int[elementColumn_Int] = min_Int;

                                if(visibleAlgorithm_Bool)
                                    ShowSteps(playerBoard_2dArrayInt,botMaxPieces_Int[elementColumn_Int][0],min_Int,-1,-1);

                            }

                        }
                        
                    }

                    if(clumsyBot_Bool)
                    {

                        playerMaxPieces_Int[elementColumn_Int] += RandomNumberGenerator.GetInt32(99,101);

                        playerMaxPieces_Int[elementColumn_Int] *= RandomNumberGenerator.GetInt32(2,5);

                        botMaxPieces_Int[elementColumn_Int][0] += RandomNumberGenerator.GetInt32(99,101);

                        botMaxPieces_Int[elementColumn_Int][0] *= RandomNumberGenerator.GetInt32(2,5);

                    }

                }

            }

        }

        for (int index_Int = 0; index_Int < 3; index_Int++)
        {

            if(bestCost_Int < botMaxPieces_Int[index_Int][0] - playerMaxPieces_Int[index_Int])
            {

                bestColumnMove_Int = index_Int;

                bestRowMove_Int = botMaxPieces_Int[index_Int][1];

                bestCost_Int = botMaxPieces_Int[index_Int][0] - playerMaxPieces_Int[index_Int];
                
            }

        }

        return (bestColumnMove_Int ,bestRowMove_Int);
    
    }

    private static int Max_Function(int[,] max_2dArrayInt, int ID_Int)
    {

        int highestCount_Int = 0;

        (int[][]rows_2DArrayInt,
            int[][]columns_2DArrayInt,
                int[]diagonal_ArrayInt,
                    int[]reverseDiagonal_ArrayInt) =
                        GameBoard.VectorGenerator_Function(max_2dArrayInt);

        if(upgradedBot_Bool){

            highestCount_Int =
                UpgradedVectorElementCount_Function(
                    diagonal_ArrayInt,
                        ID_Int);

            int mirroredDiagonal_Int = 
                UpgradedVectorElementCount_Function(
                    reverseDiagonal_ArrayInt,
                        ID_Int);

            if(highestCount_Int < mirroredDiagonal_Int)
                highestCount_Int = mirroredDiagonal_Int;
            
        }

        for(int index_Int = 0; index_Int < 3; index_Int++)
        {   

            int countRow_Int =
                VectorElementCount_Function(
                    rows_2DArrayInt[index_Int],
                        ID_Int);

            if(highestCount_Int < countRow_Int)
                highestCount_Int = countRow_Int;

            int countColumn_Int =
                VectorElementCount_Function(
                    columns_2DArrayInt[index_Int],
                        ID_Int);

            if(highestCount_Int < countColumn_Int)
                highestCount_Int = countColumn_Int;

        }

        return highestCount_Int;

    }
    
    private static int UpgradedMax_Function(int[,] max_2dArrayInt, int ID_Int)
    {

        int highestCount_Int = 0;

        (int[][]rows_2DArrayInt,
            int[][]columns_2DArrayInt,
                int[]diagonal_ArrayInt,
                    int[]reverseDiagonal_ArrayInt) =
                        GameBoard.VectorGenerator_Function(max_2dArrayInt);

        int diagonal_int =
            UpgradedVectorElementCount_Function(
                diagonal_ArrayInt,
                    ID_Int);

        int mirroredDiagonal_Int = 
            UpgradedVectorElementCount_Function(
                reverseDiagonal_ArrayInt,
                    ID_Int);

        if(highestCount_Int < mirroredDiagonal_Int)
            highestCount_Int = mirroredDiagonal_Int;

        if(highestCount_Int < diagonal_int)
            highestCount_Int = diagonal_int;

        for (int index_Int = 0; index_Int < 3; index_Int++)
        {

            int countRow_Int = 
                UpgradedVectorElementCount_Function(
                    rows_2DArrayInt[index_Int],
                        ID_Int);

            if (highestCount_Int < countRow_Int)
                highestCount_Int = countRow_Int;

            int countColumn_Int =
                UpgradedVectorElementCount_Function(
                    columns_2DArrayInt[index_Int],
                        ID_Int);

            if (highestCount_Int < countColumn_Int)
                highestCount_Int = countColumn_Int;

        }            

        return highestCount_Int;

    }

    private static int VectorElementCount_Function(int[] array_FloatVector, int elementID_Int)
    {

        List<float> list_FloatList = [..array_FloatVector];

        int highestCount_Int = 0;

        while(list_FloatList.Count > 1)
        {

            list_FloatList = list_FloatList.SkipWhile(x => x != elementID_Int).ToList();

            int temp_Int = list_FloatList.TakeWhile(x => x == elementID_Int && x != 0).Count();

            if(temp_Int > highestCount_Int)
                highestCount_Int = temp_Int;

            list_FloatList = list_FloatList.Skip(temp_Int).ToList();

        }

        return highestCount_Int;
    
    }
    
    private static int UpgradedVectorElementCount_Function(int[] array_FloatVector, int elementID_Int)
    {

        List<float> list_FloatList = [..array_FloatVector];

        int highestCount_Int = 0;

        if(list_FloatList.FindAll(x => x != elementID_Int && x != 0).Count > 0)
            return 0;

        if(list_FloatList.FindAll(x => x == elementID_Int && x != 0).Count == 3)
            return 100;

        if(list_FloatList.FindAll(x => x == elementID_Int || x == 0).Count == 3)
        {

            while(list_FloatList.Count > 1)
            {

                list_FloatList = list_FloatList.SkipWhile(x => x == 0).ToList();

                int temp1_Int = list_FloatList.TakeWhile(x => x == elementID_Int && x != 0).Count();

                if(temp1_Int > highestCount_Int)
                    highestCount_Int = temp1_Int;

                list_FloatList = list_FloatList.Skip(temp1_Int).ToList();

            }

            return highestCount_Int;

        }else
            return 0;
    
    }

    public static string BotSet_Function(bool difficulty_Bool, bool default_Bool, int bot_Int, int player_Int)
    {

        string botInfo_String = "Bot: ";

        if((player_Int, bot_Int) == (0 , 0))
        {
            
            if(upgradedBot_Bool)
                botInfo_String += "[Advanced]";
            else
                botInfo_String += "[Normal]";

            if(clumsyBot_Bool)
                botInfo_String += "[Clumsy]";

            return botInfo_String;

        }

        if(default_Bool)
        {

            playerID_Int = player_Int;

            botID_Int = bot_Int;

            clumsyBot_Bool = false;
        
            upgradedBot_Bool = false;

            botInfo_String += "[Normal]";

            return botInfo_String;

        }

        playerID_Int = player_Int;

        botID_Int = bot_Int;

        if(difficulty_Bool)
        {

            botInfo_String += "[Advanced]";

            clumsyBot_Bool = false;
        
            upgradedBot_Bool = true;
            
        }
        else
        {

            botInfo_String += "[Clumsy]";
            
            clumsyBot_Bool = true;

            upgradedBot_Bool = false;
            
        }

        return botInfo_String;
    
    }

    public static string Visiblity_Function()
    {

        visibleAlgorithm_Bool = !visibleAlgorithm_Bool;

        string botInfo_String = "Bot: ";
            
        if(upgradedBot_Bool)
            botInfo_String += "[Advanced]";
        else if(clumsyBot_Bool)
            botInfo_String += "[Clumsy]";
        else
            botInfo_String += "[Normal]";

        if(visibleAlgorithm_Bool)
            botInfo_String += "[Visible]";
        else
            botInfo_String += "[Hidden]";

        return botInfo_String;
    
    }

    private static void ShowSteps(int[,] board_2dArrayInt, int botPoints_Int, int playerPoints_Int, int columnNumber_Int, int rowNumber_Int)
    {

        Console.Clear();

        if(botPoints_Int == 100)
        {

            for (int i = 0; i < 4; i++)
            {

                System.Console.Write("\n\n\n\n\n\n\n\n");

                Thread.Sleep(10);

                Console.Clear();
                        
                MyUI.ShowMenu_Function(board_2dArrayInt,columnNumber_Int, rowNumber_Int);

                System.Console.WriteLine($"Check Mate!");

                Thread.Sleep(500);

                Console.Clear();                
                
            }

            return;

        }
        
        MyUI.ShowMenu_Function(board_2dArrayInt,-1,-1);

        System.Console.WriteLine($"You vs Bot: {playerPoints_Int} - {botPoints_Int} [Points]");

        Thread.Sleep(120);


    }

}