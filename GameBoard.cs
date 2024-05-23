namespace Connect4;

class GameBoard
{

    private static int[,] gameBoard_2DArrayInt = new int[3,3];

    public static void GameBoardReset_Function()
    {
    
        gameBoard_2DArrayInt = new int[3,3];
    
    }

    public static int[,] GameBoardStatus_Function()
    {
    
        return gameBoard_2DArrayInt;
    
    }

    public static bool ElementPlace_Function(int elementRow_Int,int elementColumn_Int, int elementValue_Int)
    {
        
        if(elementRow_Int < 5 & elementRow_Int > -1 & elementColumn_Int < 5 & elementColumn_Int > -1)
        {

            gameBoard_2DArrayInt[elementRow_Int,elementColumn_Int] = elementValue_Int;

            return true;

        }

        return false;
    
    }

    public static bool ElementValidColumn_Function(int[,]matrix_2DArrayInt, int row_Int,int column_Int)
    {

        if(matrix_2DArrayInt[row_Int,column_Int] == 0)
            return true;

        return false;
    
    }

    public static (int[],int[]) _Function()
    {

        int[] columns_ArrayInt = new int[3];

        int[] rows_ArrayInt = new int[3];

        


        
        return(rows_ArrayInt,columns_ArrayInt);
    
    }

}