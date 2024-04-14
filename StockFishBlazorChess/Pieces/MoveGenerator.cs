using StockFishBlazorChess.Data;

namespace StockFishBlazorChess.Pieces
{
    public static class MoveGenerator
    {
        public static bool[,] generateDiagonalMoves(int row, int col, Color color, Piece[,] board, bool[,] availableMoves)
        {
            const int boardSize = 8;
            bool whiteTurn = color == Color.White;
            int distance;

            // Top-left diagonal
            for (distance = 1; row - distance >= 0 && col - distance >= 0; distance++)
            {
                if (!isValidMove(row - distance, col - distance, whiteTurn, board, availableMoves))
                    break;
            }

            // Top-right diagonal
            for (distance = 1; row - distance >= 0 && col + distance < boardSize; distance++)
            {
                if (!isValidMove(row - distance, col + distance, whiteTurn, board, availableMoves))
                    break;
            }

            // Bottom-left diagonal
            for (distance = 1; row + distance < boardSize && col - distance >= 0; distance++)
            {
                if (!isValidMove(row + distance, col - distance, whiteTurn, board, availableMoves))
                    break;
            }

            // Bottom-right diagonal
            for (distance = 1; row + distance < boardSize && col + distance < boardSize; distance++)
            {
                if (!isValidMove(row + distance, col + distance, whiteTurn, board, availableMoves))
                    break;
            }

            return availableMoves;
        }

        public static bool[,] generateStraightMoves(int row, int col, Color color, Piece[,] board, bool[,] availableMoves)
        {
            const int boardSize = 8;
            bool whiteTurn = color == Color.White;

            // Upwards movement
            for (int distance = row - 1; distance >= 0; distance--)
            {
                if (!isValidMove(distance, col, whiteTurn, board, availableMoves))
                    break;
            }

            // Downwards movement
            for (int b = row + 1; b < boardSize; b++)
            {
                if (!isValidMove(b, col, whiteTurn, board, availableMoves))
                    break;
            }

            // Leftwards movement
            for (int b = col - 1; b >= 0; b--)
            {
                if (!isValidMove(row, b, whiteTurn, board, availableMoves))
                    break;
            }

            // Rightwards movement
            for (int b = col + 1; b < boardSize; b++)
            {
                if (!isValidMove(row, b, whiteTurn, board, availableMoves))
                    break;
            }

            return availableMoves;
        }

        public static bool[,] generateHorseMoves(int row, int col, Color color, Piece[,] board, bool[,] availableMoves)
        {
            bool whiteTurn = color == Color.White;
            checkAndSetMove(row - 2, col - 1, whiteTurn, board, availableMoves);
            checkAndSetMove(row - 2, col + 1, whiteTurn, board, availableMoves);
            checkAndSetMove(row + 2, col - 1, whiteTurn, board, availableMoves);
            checkAndSetMove(row + 2, col + 1, whiteTurn, board, availableMoves);
            checkAndSetMove(row - 1, col - 2, whiteTurn, board, availableMoves);
            checkAndSetMove(row + 1, col - 2, whiteTurn, board, availableMoves);
            checkAndSetMove(row - 1, col + 2, whiteTurn, board, availableMoves);
            checkAndSetMove(row + 1, col + 2, whiteTurn, board, availableMoves);
            return availableMoves;
        }

        private static void checkAndSetMove(int row, int column, bool blackTurn, Piece[,] board, bool[,] availableMovesTable)
        {
            bool isValidMove = false;

            if (row >= 0 && row < 8 && column >= 0 && column < 8)
            {
                int pieceValue = board[row, column].PieceValue;

                if (blackTurn)
                {
                    isValidMove = (pieceValue == 0 || pieceValue > 10);
                }
                else
                {
                    isValidMove = (pieceValue < 10);
                }
            }

            if (isValidMove)
            {
                availableMovesTable[row, column] = true;
            }
        }

        private static bool isValidMove(int row, int column, bool whiteTurn, Piece[,] board, bool[,] availableMoves)
        {
            int pieceValue = board[row, column].PieceValue;
            bool isValid;
            if (whiteTurn)
            {
                isValid = pieceValue > 10 || pieceValue == 0;
            }
            else
            {
                isValid = pieceValue < 10;
            }

            if (isValid)
            {
                availableMoves[row, column] = true;
                return pieceValue == 0;
            }

            return false;
        }

    }
}
