using System.Collections.ObjectModel;

namespace Connect4;

class GameBoard
{
    private static int[,]gameBoard_2DArrayInt = new int[3,3];

    /// Resets the game board for a new match.
    public static void GameBoardReset_Function()
    {
        // Create a new game board with all elements set to zero
        gameBoard_2DArrayInt = new int[3,3];
    }

    /// Returns the current state of the game board.
    public static int[,] GameBoardStatus_Function()
    {
        // Returns the current state of the game board.
        // The value of each element represents the ID of the player who made the move.
        // The ID of the player is 0 for no player, 1 for the first player, 2 for the second player.

        return gameBoard_2DArrayInt;
    }

    /// A sub-status is a 1D array that represents either a row, column, diagonal, reverse diagonal or all of the elements of the game board.
    public static int[] GameBoardSubStatus_Function(int[,] targetBoard_2DArrayInt, int sub_Int, int index_int)
    {
        // If the targetBoard_2DArrayInt argument is null, use the current game board.
        if (targetBoard_2DArrayInt.Length == 1)
            targetBoard_2DArrayInt = gameBoard_2DArrayInt.Clone() as int[,];

        // Return the sub-status based on the type of sub-status.
        return sub_Int switch
        {
            // Row sub-status
            0 => [targetBoard_2DArrayInt[index_int, 0], targetBoard_2DArrayInt[index_int, 1], targetBoard_2DArrayInt[index_int, 2]],
            // Column sub-status
            1 => [targetBoard_2DArrayInt[0, index_int], targetBoard_2DArrayInt[1, index_int], targetBoard_2DArrayInt[2, index_int]],
            // Diagonal sub-status
            2 => [targetBoard_2DArrayInt[0, 0], targetBoard_2DArrayInt[1, 1], targetBoard_2DArrayInt[2, 2]],
            // Other diagonal sub-status
            3 => [targetBoard_2DArrayInt[0, 2], targetBoard_2DArrayInt[1, 1], targetBoard_2DArrayInt[2, 0]],
            // Entire board sub-status
            4 => [
                                targetBoard_2DArrayInt[0,0], targetBoard_2DArrayInt[0,1], targetBoard_2DArrayInt[0,2],
                    targetBoard_2DArrayInt[1,0], targetBoard_2DArrayInt[1,1], targetBoard_2DArrayInt[1,2],
                    targetBoard_2DArrayInt[2,0], targetBoard_2DArrayInt[2,1], targetBoard_2DArrayInt[2,2]
                            ],
            _ => new int[3],
        };
    }

    /// Places an element on the game board.
    public static bool ElementPlace_Function(int row_Int, int column_Int, int playerID_Int)
    {
        // Check if the position is out of bounds or already occupied
        if (row_Int > 2 || row_Int < 0 || column_Int > 2 || column_Int < 0 || gameBoard_2DArrayInt[row_Int, column_Int] != 0)
        {
            System.Console.WriteLine("Cant put there");

            Console.ReadKey();

            return false;
        }

        // Place the element on the game board
        gameBoard_2DArrayInt[row_Int, column_Int] = playerID_Int;

        return true;
    }

}