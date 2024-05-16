using System.Security.Cryptography;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
namespace Connect4;

class Bot
{

    private static bool upgradedBot_Bool = false;

    private static bool clumsyBot_Bool = false;

    private static bool visibleAlgorithm_Bool = false;

    private static int playerID_Int = 1;

    private static int botID_Int = 2;

    public static int Bot_Function()
    {

        int bestCost_Int = -10000;

        int bestMove_Int = -1;

        int[] botMaxPieces_Int = new int[5];

        int[] playerMaxPieces_Int = new int[5];

        for (int elementColumn_Int = 0; elementColumn_Int < 5; elementColumn_Int++)
        {

            botMaxPieces_Int[elementColumn_Int] = -100;

            if(GameBoard.ElementValidColumn_Function(GameBoard.GameBoardStatus_Function(), elementColumn_Int, out int elementRow_Int))
            {

                playerMaxPieces_Int[elementColumn_Int] = -100;

                Matrix<float> botBoard_FloatMatrix = Matrix<float>.Build.DenseOfMatrix(GameBoard.GameBoardStatus_Function());

                Vector<float> elementVector_FloatVector = Vector<float>.Build.Dense(5);

                botBoard_FloatMatrix.
                    Column(elementColumn_Int).CopyTo(elementVector_FloatVector);

                elementVector_FloatVector[elementRow_Int] = botID_Int;

                botBoard_FloatMatrix.SetColumn(elementColumn_Int,elementVector_FloatVector);

                if(upgradedBot_Bool)
                    botMaxPieces_Int[elementColumn_Int] = UpgradedMax_Function(botBoard_FloatMatrix, botID_Int);
                else
                    botMaxPieces_Int[elementColumn_Int] = Max_Function(botBoard_FloatMatrix, botID_Int);
            
                if(botMaxPieces_Int[elementColumn_Int] == 100)
                {

                    ShowSteps(botBoard_FloatMatrix,100,0,elementColumn_Int);
                
                    return elementColumn_Int;
                    
                }


                for (int playerColumn_Int = 0; playerColumn_Int < 5; playerColumn_Int++)
                {

                    if(GameBoard.ElementValidColumn_Function(botBoard_FloatMatrix, playerColumn_Int, out int playerRow_Int))
                    {

                        int min_Int = 0;

                        Matrix<float> playerBoard_FloatMatrix = Matrix<float>.Build.DenseOfMatrix(botBoard_FloatMatrix);

                        playerBoard_FloatMatrix[playerRow_Int,playerColumn_Int] = playerID_Int;

                        if(upgradedBot_Bool)
                            min_Int = UpgradedMax_Function(playerBoard_FloatMatrix, playerID_Int);
                        else
                            min_Int = Max_Function(playerBoard_FloatMatrix, playerID_Int);

                        if(min_Int == 100)
                            min_Int = 50;

                        if(playerMaxPieces_Int[elementColumn_Int] < min_Int)
                            playerMaxPieces_Int[elementColumn_Int] = min_Int;

                        if(visibleAlgorithm_Bool)
                            ShowSteps(playerBoard_FloatMatrix,botMaxPieces_Int[elementColumn_Int],min_Int,-1);

                    }
                    
                }

            }

            if(clumsyBot_Bool)
            {

                playerMaxPieces_Int[elementColumn_Int] += RandomNumberGenerator.GetInt32(99,101);

                playerMaxPieces_Int[elementColumn_Int] *= RandomNumberGenerator.GetInt32(2,5);

                botMaxPieces_Int[elementColumn_Int] += RandomNumberGenerator.GetInt32(99,101);

                botMaxPieces_Int[elementColumn_Int] *= RandomNumberGenerator.GetInt32(2,5);

            }

        }

        for (int index_Int = 0; index_Int < 5; index_Int++)
        {

            if(bestCost_Int < botMaxPieces_Int[index_Int] - playerMaxPieces_Int[index_Int])
            {

                bestMove_Int = index_Int;

                bestCost_Int = botMaxPieces_Int[index_Int] - playerMaxPieces_Int[index_Int];
                
            }

        }

        System.Console.WriteLine(bestMove_Int);

        return bestMove_Int;
    
    }

    private static int Max_Function(Matrix<float> max_FloatMatrix, int ID_Int)
    {

        int highestCount_Int = 0;

        if(upgradedBot_Bool){

            Matrix<float> mirror_SingleMatrix = Matrix<float>.Build.Dense(5,5,0);

            for (int i = 0; i < 5; i++)
            {

                mirror_SingleMatrix[4-i,i] = 1;
                
            }

            highestCount_Int = UpgradedVectorElementCount_Function(max_FloatMatrix.Diagonal(), ID_Int);

            int mirroredDiagonal_Int = UpgradedVectorElementCount_Function(max_FloatMatrix.Multiply(mirror_SingleMatrix).Diagonal(), ID_Int);

            if (highestCount_Int < mirroredDiagonal_Int)
                highestCount_Int = mirroredDiagonal_Int;
            
        }

        for (int index_Int = 0; index_Int < 5; index_Int++)
        {

            int countRow_Int = VectorElementCount_Function(max_FloatMatrix.Row(index_Int), ID_Int);

            if (highestCount_Int < countRow_Int)
                highestCount_Int = countRow_Int;

            int countColumn_Int = VectorElementCount_Function(max_FloatMatrix.Column(index_Int), ID_Int);

            if (highestCount_Int < countColumn_Int)
                highestCount_Int = countColumn_Int;

        }

        return highestCount_Int;

    }
    
    private static int UpgradedMax_Function(Matrix<float> max_FloatMatrix, int ID_Int)
    {

        int highestCount_Int = 0;

        int corner1_Int = 0;

        int corner2_Int = 0;

        for (int repeat_Int = 0; repeat_Int < 4; repeat_Int++)
        {

            Matrix<float> copy_FloatMatrix = Matrix<float>.Build.DenseOfMatrix(max_FloatMatrix.SubMatrix(corner1_Int,4,corner2_Int,4));

            if(corner2_Int == 1)
                corner1_Int--;
            else
            if(corner1_Int == 1)
                corner2_Int++;
            else
                corner1_Int++;

            Matrix<float> mirror_SingleMatrix = Matrix<float>.Build.Dense(4,4,0);

            for (int i = 0; i < 4; i++)
            {

                mirror_SingleMatrix[3-i,i] = 1;
                
            }

            int diagonal_int = UpgradedVectorElementCount_Function(copy_FloatMatrix.Diagonal(), ID_Int);

            int mirroredDiagonal_Int = UpgradedVectorElementCount_Function(copy_FloatMatrix.Multiply(mirror_SingleMatrix).Diagonal(), ID_Int);

            if(highestCount_Int < mirroredDiagonal_Int)
                highestCount_Int = mirroredDiagonal_Int;

            if(highestCount_Int < diagonal_int)
                highestCount_Int = diagonal_int;

            for (int index_Int = 0; index_Int < 4; index_Int++)
            {

                int countRow_Int = UpgradedVectorElementCount_Function(copy_FloatMatrix.Row(index_Int), ID_Int);

                if (highestCount_Int < countRow_Int)
                    highestCount_Int = countRow_Int;

                int countColumn_Int = UpgradedVectorElementCount_Function(copy_FloatMatrix.Column(index_Int), ID_Int);

                if (highestCount_Int < countColumn_Int)
                    highestCount_Int = countColumn_Int;

            }
            
        }

        return highestCount_Int;

    }

    private static int VectorElementCount_Function(Vector<float> array_FloatVector, int elementID_Int)
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
    
    private static int UpgradedVectorElementCount_Function(Vector<float> array_FloatVector, int elementID_Int)
    {

        List<float> list_FloatList = [..array_FloatVector];

        int highestCount_Int = 0;

        if(list_FloatList.FindAll(x => x != elementID_Int && x != 0).Count > 0)
            return 0;

        if(list_FloatList.FindAll(x => x == elementID_Int && x != 0).Count == 4)
            return 100;

        if(list_FloatList.FindAll(x => x == elementID_Int || x == 0).Count == 4)
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

    private static void ShowSteps(Matrix<Single> board_FloatMatrix, int botPoints_Int, int playerPoints_Int, int columnNumber_Int)
    {

        Console.Clear();

        if(botPoints_Int == 100)
        {

            for (int i = 0; i < 4; i++)
            {

                System.Console.Write("\n\n\n\n\n\n\n\n");

                Thread.Sleep(10);

                Console.Clear();
                        
                MyUI.ShowMenu_Function(board_FloatMatrix,columnNumber_Int);

                System.Console.WriteLine($"Check Mate!");

                Thread.Sleep(500);

                Console.Clear();                
                
            }

            return;

        }
        
        MyUI.ShowMenu_Function(board_FloatMatrix,-1);

        System.Console.WriteLine($"You vs Bot: {playerPoints_Int} - {botPoints_Int} [Points]");

        Thread.Sleep(120);


    }

}