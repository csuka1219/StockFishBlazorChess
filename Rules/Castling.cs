using StockFishBlazorChess.Game;
using StockFishBlazorChess.Pieces;
using MudBlazor.Extensions;
using System.Text;

namespace StockFishBlazorChess.Data
{
    public static class Castling
    {
        public static void castling(Chessboard chessBoard, string position)
        {
            // Perform castling based on the given position
            switch (position)
            {
                case "72":
                    // Perform castling on the queen's side for white
                    chessBoard.board[7, 0].setPosition("73");
                    Piece rook = chessBoard.board[7, 0];
                    chessBoard.board[7, 0] = new EmptyPiece();
                    chessBoard.board[7, 3] = rook;
                    break;
                case "76":
                    // Perform castling on the king's side for white
                    chessBoard.board[7, 7].setPosition("75");
                    rook = chessBoard.board[7, 7];
                    chessBoard.board[7, 7] = new EmptyPiece();
                    chessBoard.board[7, 5] = rook;
                    break;
                case "02":
                    // Perform castling on the queen's side for black
                    chessBoard.board[0, 0].setPosition("03");
                    rook = chessBoard.board[0, 0];
                    chessBoard.board[0, 0] = new EmptyPiece();
                    chessBoard.board[0, 3] = rook;
                    break;
                case "06":
                    // Perform castling on the king's side for black
                    chessBoard.board[0, 7].setPosition("05");
                    rook = chessBoard.board[0, 7];
                    chessBoard.board[0, 7] = new EmptyPiece();
                    chessBoard.board[0, 5] = rook;
                    break;
            }
        }

        public static string getCastlingAvailability(Piece[,] board)
        {
            // StringBuilder to store the castling availability
            StringBuilder sb = new StringBuilder();

            // Check if castling is available for the white side (King's side and Queen's side)
            if (canCastleKingSide(Color.White, board))
            {
                sb.Append('K'); // Append 'K' to indicate castling availability for the white King's side
            }
            if (canCastleQueenSide(Color.White, board))
            {
                sb.Append('Q'); // Append 'Q' to indicate castling availability for the white Queen's side
            }

            // Check if castling is available for the black side (King's side and Queen's side)
            if (canCastleKingSide(Color.Black, board))
            {
                sb.Append('k'); // Append 'k' to indicate castling availability for the black King's side
            }
            if (canCastleQueenSide(Color.Black, board))
            {
                sb.Append('q'); // Append 'q' to indicate castling availability for the black Queen's side
            }

            // If no castling is available, return "-"
            return sb.Length > 0 ? sb.ToString() : "-";
        }


        private static bool canCastleKingSide(Pieces.Color color, Piece[,] board)
        {
            // Determine the row of the king and rook based on the color
            int row = (color == Color.White) ? 7 : 0;

            // Check if the king and rook are in their initial positions
            Piece king = board[row, 4];
            Piece rook = board[row, 7];

            // Check if the pieces are of the correct type (king and rook) and have the correct color
            if (king is King && rook is Rook && king.Color == color && rook.Color == color)
            {
                // Check if both the king and rook are able to perform castling
                return king.As<King>().ableToCastling && rook.As<Rook>().ableToCastling;
            }

            // Castling is not possible
            return false;
        }

        private static bool canCastleQueenSide(Color color, Piece[,] board)
        {
            // Determine the row of the king and rook based on the color
            int row = (color == Color.White) ? 7 : 0;

            // Check if the king and rook are in their initial positions
            Piece king = board[row, 4];
            Piece rook = board[row, 0];

            // Check if the pieces are of the correct type (king and rook) and have the correct color
            if (king is King && rook is Rook && king.Color == color && rook.Color == color)
            {
                // Check if both the king and rook are able to perform castling
                return king.As<King>().ableToCastling && rook.As<Rook>().ableToCastling;
            }

            // Castling is not possible
            return false;
        }

        public static void setCastlingAvailability(Piece[,] board, string castlingAvailability)
        {
            // Check if castling availability is not empty ("-")
            if (castlingAvailability != "-")
            {
                // Convert the castling availability string into an array of individual characters
                char[] availableCastling = castlingAvailability.ToCharArray();

                // Iterate over each castling option
                foreach (char castling in availableCastling)
                {
                    // Determine the color based on the case of the character (upper case for white, lower case for black)
                    Color color = char.IsUpper(castling) ? Color.White : Color.Black;

                    // Check the type of castling based on the lowercase character
                    switch (char.ToLower(castling))
                    {
                        case 'k':
                            // Set king-side castling availability for the specified color to true
                            setKingCastlingAvailability(board, color, true);
                            break;
                        case 'q':
                            // Set queen-side castling availability for the specified color to true
                            setQueenCastlingAvailability(board, color, true);
                            break;
                        default:
                            // Set castling availability for a specific rook based on the character
                            setRookCastlingAvailability(board, color, castling, true);
                            break;
                    }
                }
            }
        }
        public static bool canPerformCastling(Piece piece, int row, int col)
        {
            if (piece is King king && king.ableToCastling)
            {
                List<string> castlingPositions = new List<string> { "72", "76", "02", "06" };
                string position = $"{row}{col}";
                return castlingPositions.Contains(position);
            }

            return false;
        }


        private static void setRookCastlingAvailability(Piece[,] board, Color color, char rookFEN, bool ableToCastle)
        {
            int row = (color == Color.White) ? 7 : 0;

            int col = Char.ToLower(rookFEN) - 'a';

            // Check if the piece at the specified position is a rook and has the correct color
            if (board[row, col] is Rook && board[row, col].Color == color)
            {
                // Set the ability to castle for the rook to the specified value
                board[row, col].As<Rook>().ableToCastling = ableToCastle;
            }
        }

        private static void setQueenCastlingAvailability(Piece[,] board, Color color, bool ableToCastle)
        {
            int row = (color == Color.White) ? 7 : 0;

            int col = 7;

            // Check if the piece at the specified position is a rook and has the correct color
            if (board[row, col] is Rook && board[row, col].Color == color)
            {
                // Set the ability to castle for the rook to the specified value
                board[row, col].As<Rook>().ableToCastling = ableToCastle;
            }
        }

        private static void setKingCastlingAvailability(Piece[,] board, Color color, bool ableToCastle)
        {
            int row = (color == Color.White) ? 7 : 0;

            int col = 4;

            // Check if the piece at the specified position is a king and has the correct color
            if (board[row, col] is King && board[row, col].Color == color)
            {
                // Set the ability to castle for the king to the specified value
                board[row, col].As<King>().ableToCastling = ableToCastle;
            }
        }

    }
}
