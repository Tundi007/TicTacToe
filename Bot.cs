namespace Connect4;

class Bot
{

    private static bool alphaBeta_Bool = false;

    private static int playerID_Int;
    
    private static int botID_Int;

    private static bool miniMaxShow_Bool = false;

    /// Returns the coordinates of the best move for the bot to make.
    public static (int, int) Bot_Function()
    {
        // Initialize variables
        int selectedRow_Int = -1;
        int selectedColumn_Int = -1;
        int[,] gameBoard_2DArrayInt = GameBoard.GameBoardStatus_Function().Clone()as int[,];
        int[,] scoreBoard_2DArrayInt = {{-10000, -10000, -10000}, {-10000, -10000, -10000}, {-10000, -10000, -10000}};
        int maxScore_Int = -10000;
        bool alphaBetaPruning_Bool = false;

        // Iterate over the board for bot's turn
        for (int botRow_Int = 0; botRow_Int < 3; botRow_Int++)
        {
            for (int botColumn_Int = 0; botColumn_Int < 3; botColumn_Int++)
            {
                int minScore_Int = 10000;

                // Check if the current position is empty
                if(gameBoard_2DArrayInt[botRow_Int,botColumn_Int] == 0)
                {
                    int[,] botBoard_2DArrayInt = gameBoard_2DArrayInt.Clone()as int[,];
                    botBoard_2DArrayInt[botRow_Int,botColumn_Int] = botID_Int;

                    // Iterate over the board for player's turn
                    for (int playerRow_Int = 0; playerRow_Int < 3; playerRow_Int++)
                    {
                        for (int playerColumn_Int = 0; playerColumn_Int < 3; playerColumn_Int++)
                        {
                            int[,] playerBoard_2DArrayInt = botBoard_2DArrayInt.Clone()as int[,];
                            if(playerBoard_2DArrayInt[playerRow_Int,playerColumn_Int] == 0)
                            {
                                playerBoard_2DArrayInt[playerRow_Int,playerColumn_Int] = playerID_Int;
                                int score_Int = Score_Function(playerBoard_2DArrayInt);

                                if(score_Int == 100)
                                    return (botRow_Int,botColumn_Int);

                                if(score_Int < minScore_Int)
                                    minScore_Int = score_Int;

                                if(alphaBeta_Bool & minScore_Int <= maxScore_Int)
                                    alphaBetaPruning_Bool = true;

                                if(miniMaxShow_Bool)
                                {
                                    Console.Clear();
                                    System.Console.WriteLine($"[{botRow_Int},{botColumn_Int}]");
                                    MyUI.ShowMenu_Function(playerBoard_2DArrayInt,-1,-1);
                                    System.Console.WriteLine("score: " + score_Int + " | MinScore: " + minScore_Int + " | MaxScore: " + maxScore_Int);
                                    Console.ReadKey();
                                }
                            }

                            if(alphaBetaPruning_Bool)
                                break;
                        }

                        if(alphaBetaPruning_Bool)
                            break;
                    }

                    if(!alphaBetaPruning_Bool)
                        scoreBoard_2DArrayInt[botRow_Int,botColumn_Int] = minScore_Int;
                    else
                        alphaBetaPruning_Bool = false;

                    if(maxScore_Int < minScore_Int)
                        maxScore_Int = minScore_Int;

                    if(miniMaxShow_Bool)
                    {
                        Console.Clear();
                        System.Console.WriteLine("Placement Score Marix: (-10000 = [Not Calculated Yet] or [Impossible Placement] or [Pruned])");
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                System.Console.Write(scoreBoard_2DArrayInt[i,j] + " , ");
                            }
                            System.Console.WriteLine();
                        }
                        Console.ReadKey();
                    }
                }
            }
        }

        int maximumScore_Int = -10000;

        // Find the best move
        for (int row_Int = 0; row_Int < 3; row_Int++)
        {
            for (int column_Int = 0; column_Int < 3; column_Int++)
            {
                if(scoreBoard_2DArrayInt[row_Int,column_Int] != -10000)
                    if(scoreBoard_2DArrayInt[row_Int,column_Int] > maximumScore_Int)
                        (selectedRow_Int,selectedColumn_Int,maximumScore_Int) = (row_Int,column_Int,scoreBoard_2DArrayInt[row_Int,column_Int]);
            }
        }

        if(miniMaxShow_Bool){
            Console.Clear();
            System.Console.WriteLine("Placement: [" + selectedRow_Int + "," + selectedColumn_Int + "]");
            Console.ReadKey();
        }

        return(selectedRow_Int,selectedColumn_Int);
    }

    /// Calculates the score of a given game board.
    private static int Score_Function(int[,] board_ListArrayInt)
    {
        // Calculate the score of the main diagonal
        int score_Int = Score_Function([..GameBoard.GameBoardSubStatus_Function(board_ListArrayInt,2,-1)]);

        // Calculate the score of the secondary diagonal
        int scoreTemp_Int = Score_Function([..GameBoard.GameBoardSubStatus_Function(board_ListArrayInt,3,-1)]);

        // If either diagonal is a win, return 100
        if (scoreTemp_Int == 300 || score_Int == 300)
            return 100;

        // Add the scores of the diagonals to the total score
        score_Int += scoreTemp_Int;

        // Calculate the score of each row and column and add it to the total score
        for (int index_Int = 0; index_Int < 3; index_Int++)
        {
            scoreTemp_Int = Score_Function([..GameBoard.GameBoardSubStatus_Function(board_ListArrayInt,0,index_Int)]);

            // If a row is a win, return 100
            if (scoreTemp_Int == 300)
                return 100;

            score_Int += scoreTemp_Int;

            scoreTemp_Int = Score_Function([..GameBoard.GameBoardSubStatus_Function(board_ListArrayInt,1,index_Int)]);

            // If a column is a win, return 100
            if (scoreTemp_Int == 300)
                return 100;

            score_Int += scoreTemp_Int;
        }

        return score_Int;

    }
    
    /// Calculates the score of a given array based on the number of occurrences of the bot and player IDs.
    private static int Score_Function(List<int> array_ListInt)
    {
        // Initialize highest count to 0
        int highestCount_Int = 0;

        // Check if the array contains both bot and player IDs
        if (array_ListInt.Any(element => element == botID_Int) && array_ListInt.Any(element => element == playerID_Int))
            return 0;

        // Sort the array
        array_ListInt.Sort();

        // Remove leading zeros from the array
        array_ListInt = array_ListInt.SkipWhile(element => element == 0).ToList();

        // If array is empty, return 0
        if (array_ListInt.Count == 0)
            return 0;

        
        if (array_ListInt[0] == botID_Int) // If the first element of the array is the bot ID, set highest count to the count of the array
            highestCount_Int = array_ListInt.Count;        
        else // If the first element of the array is the player ID, set highest count to the negative count of the array
        if (array_ListInt[0] == playerID_Int)
            highestCount_Int = -array_ListInt.Count;

        
        if (highestCount_Int == 1 || highestCount_Int == -2) // If highest count is 1 or -2, increment or decrement it
            highestCount_Int += highestCount_Int == 1 ? 1 : -1;
        else // If highest count is 3 or -3, multiply it by 100 or 10 respectively
        if (highestCount_Int == 3 || highestCount_Int == -3)
            highestCount_Int *= highestCount_Int == 3 ? 100 : 10;
        else if (highestCount_Int == 2)
            highestCount_Int += 2;

        // Return the highest count
        return highestCount_Int;
    }

    /// Initializes the bot based on the provided alphaBetaSet, player ID, and bot ID.
    public static void BotInitialization_Function(bool alphaBetaSet_Bool, int player_Int, int bot_Int)
    {
        // If player ID is -1, toggle miniMaxShow_Bool
        if (player_Int == -1)
            miniMaxShow_Bool = !miniMaxShow_Bool;
        else
        {
            // Set player ID and bot ID
            playerID_Int = player_Int;
            botID_Int = bot_Int;

            // Set alphaBeta_Bool based on alphaBetaSet_Bool
            alphaBeta_Bool = alphaBetaSet_Bool;
        }

        string info_String = "";

        
        if (alphaBetaSet_Bool) // If alphaBetaSet_Bool is true, add "MiniMax: [Pruning]" to info_String
            info_String += "MiniMax: [Pruning]";
        else // If alphaBetaSet_Bool is false, add "MiniMax: [Vanilla]" to info_String            
            info_String += "MiniMax: [Vanilla]";

        // If miniMaxShow_Bool is true, add " [Step By Step]" to info_String
        if (miniMaxShow_Bool)
            info_String += " [Step By Step]";
        
        // Call GameInfo_Function with info_String
        MyUI.GameInfo_Function(info_String);
    }

}