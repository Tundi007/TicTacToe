using MathNet.Numerics.LinearAlgebra;
namespace Connect4;

class GameBoard
{

    private static Matrix<float> gameBoard_FloatMatrix = Matrix<float>.Build.Dense(5,5,0);

    public static void GameBoardReset_Function()
    {
    
        Matrix<float>.Build.Dense(5,5,0).CopyTo(gameBoard_FloatMatrix);
    
    }

    public static Matrix<float> GameBoardStatus_Function()
    {
    
        return gameBoard_FloatMatrix;
    
    }

    public static bool ElementPlace_Function(int elementRow_Int,int elementColumn_Int, int elementValue_Int)
    {
        
        if(elementRow_Int < 5 & elementRow_Int > -1 & elementColumn_Int < 5 & elementColumn_Int > -1)
        {

            gameBoard_FloatMatrix[elementRow_Int,elementColumn_Int] = elementValue_Int;

            return true;

        }

        return false;
    
    }

    public static bool ElementValidColumn_Function(Matrix<float>matrix_FloatMatrix, int column_Int, out int output_Int)
    {

        output_Int = -1;

        if(column_Int < 0 & column_Int >= matrix_FloatMatrix.Column(column_Int).Count)
            return false;

        if(matrix_FloatMatrix.Column(column_Int).Contains(0))
        {
            
            output_Int = matrix_FloatMatrix.Column(column_Int).ToList().IndexOf(0);

            return true;

        }

        return false;
    
    }

    public static bool SubMatrix_Function(Matrix<float> matrix_FloatMatrix,int subCorner_Int,out Matrix<float> output_FloatMatrix)
    {
        
        int columnIndex_Int;

        int rowIndex_Int;

        output_FloatMatrix = Matrix<float>.Build.Dense(4,4);

        switch (subCorner_Int)
        {

            case 1:
                (rowIndex_Int, columnIndex_Int) = (0, 0);
                break;

            case 2:
                (rowIndex_Int, columnIndex_Int) = (1, 0);
                break;

            case 3:
                (rowIndex_Int, columnIndex_Int) = (0, 1);
                break;

            case 4:
                (rowIndex_Int, columnIndex_Int) = (1, 1);
                break;

            default:
                return false;

        }

        matrix_FloatMatrix.SubMatrix(rowIndex_Int, 4, columnIndex_Int, 4).CopyTo(output_FloatMatrix);

        return true;
    
    }

}