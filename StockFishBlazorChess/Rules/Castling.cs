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
            StringBuilder sb = new StringBuilder();

            if (canCastleKingSide(Color.White, board))
            {
                sb.Append('K');
            }
            if (canCastleQueenSide(Color.White, board))
            {
                sb.Append('Q');
            }

            if (canCastleKingSide(Color.Black, board))
            {
                sb.Append('k');
            }
            if (canCastleQueenSide(Color.Black, board))
            {
                sb.Append('q');
            }

            return sb.Length > 0 ? sb.ToString() : " ";
        }


        private static bool canCastleKingSide(Pieces.Color color, Piece[,] board)
        {
            int row = (color == Color.White) ? 7 : 0;

            Piece king = board[row, 4];
            Piece rook = board[row, 7];

            if (king is King && rook is Rook && king.Color == color && rook.Color == color)
            {
                return king.As<King>()!.ableToCastling && rook.As<Rook>()!.ableToCastling;
            }

            return false;
        }

        private static bool canCastleQueenSide(Color color, Piece[,] board)
        {
            int row = (color == Color.White) ? 7 : 0;

            Piece king = board[row, 4];
            Piece rook = board[row, 0];

            if (king is King && rook is Rook && king.Color == color && rook.Color == color)
            {
                return king.As<King>()!.ableToCastling && rook.As<Rook>()!.ableToCastling;
            }

            return false;
        }

        public static void setCastlingAvailability(Piece[,] board, string castlingAvailability)
        {
            string lowerCastling = castlingAvailability.ToLower();
            if (lowerCastling.Contains('k') || lowerCastling.Contains('q'))
            {
                char[] availableCastling = castlingAvailability.ToCharArray();

                foreach (char castling in availableCastling)
                {
                    // Determine the color based on the case of the character (upper case for white, lower case for black)
                    Color color = char.IsUpper(castling) ? Color.White : Color.Black;

                    switch (char.ToLower(castling))
                    {
                        case 'k':
                            setKingCastlingAvailability(board, color, true);
                            break;
                        case 'q':
                            setQueenCastlingAvailability(board, color, true);
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

        private static void setQueenCastlingAvailability(Piece[,] board, Color color, bool ableToCastle)
        {
            int row = (color == Color.White) ? 7 : 0;

            int col = 7;

            if (board[row, col] is Rook && board[row, col].Color == color)
            {
                board[row, col].As<Rook>()!.ableToCastling = ableToCastle;
            }
        }

        private static void setKingCastlingAvailability(Piece[,] board, Color color, bool ableToCastle)
        {
            int row = (color == Color.White) ? 7 : 0;

            int col = 4;

            if (board[row, col] is King && board[row, col].Color == color)
            {
                board[row, col].As<King>()!.ableToCastling = ableToCastle;
            }
        }

    }
}
