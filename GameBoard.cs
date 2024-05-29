namespace Connect4;

class GameBoard
{

    private static int[,] gameBoard_2DArrayInt = new int[3,3];

    public static void GameBoardReset_Function()
    {
    
        gameBoard_2DArrayInt = new int[3,3];

        for (int index_Int = 0; index_Int < 3; index_Int++)
        {

            gameBoard_2DArrayInt[index_Int,0] = 0;

            gameBoard_2DArrayInt[index_Int,1] = 0;

            gameBoard_2DArrayInt[index_Int,2] = 0;
            
        }
    
    }

    public static int[,] GameBoardStatus_Function()
    {
    
        return gameBoard_2DArrayInt;
    
    }

    public static void CopyGameBoard_Function(int[,] board_2DArrayInt, out int[,] boardCopy_2DArray)
    {

        boardCopy_2DArray = new int[3,3];

        for (int i = 0; i < 3; i++)
        {

            for (int j = 0; j < 3; j++)
            {

                boardCopy_2DArray[i,j] = board_2DArrayInt[3,3];
                
            }            

        }
    
    }

    public static void ElementPlace_Function(int elementRow_Int,int elementColumn_Int, int elementValue_Int)
    {

        gameBoard_2DArrayInt[elementRow_Int,elementColumn_Int] = elementValue_Int;
    
    }

    public static (int[][],int[][],int[],int[]) VectorGenerator_Function(int[,] matrix_2DArrayInt)
    {
        int[][] rows_ArrayInt = new int[3][];

        int[][] columns_ArrayInt = new int[3][];

        int[] diagonal_ArrayInt = new int[3];

        int[] reverseDiagonal_ArrayInt = new int[3];

        for (int index_Int = 0; index_Int < 3; index_Int++)
        {

            rows_ArrayInt[index_Int] = new int[3];

            columns_ArrayInt[index_Int] = new int[3];

            for (int index2_int = 0; index2_int < 3; index2_int++)
            {

                rows_ArrayInt[index_Int][index2_int] = matrix_2DArrayInt[index_Int,index2_int];

                columns_ArrayInt[index_Int][index2_int] = matrix_2DArrayInt[index2_int,index_Int];
                
            }

            diagonal_ArrayInt[index_Int] = matrix_2DArrayInt[index_Int,index_Int];

            reverseDiagonal_ArrayInt[index_Int] = matrix_2DArrayInt[index_Int,2-index_Int];
            
        }

        return(rows_ArrayInt,columns_ArrayInt,diagonal_ArrayInt,reverseDiagonal_ArrayInt);
    
    }

}