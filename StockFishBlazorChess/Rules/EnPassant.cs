using MudBlazor.Extensions;
using StockFishBlazorChess.Pieces;
using StockFishBlazorChess.Utilities;

namespace StockFishBlazorChess.Rules
{    public static class EnPassant
    {
        public static bool isEnPassant(Piece piece, int newRow, int newCol)
        {
            int enPassantRow = piece.Color == Color.White ? 2 : 5;
            return piece is Pawn && newRow == enPassantRow && newCol != piece.getPositionTuple().col;
        }

        public static void performEnPassant(Piece[,] board, Piece piece, int row, int col, IEnumerable<Piece> list)
        {
            int forwardDirection = piece.Color == Color.White ? -1 : 1;
            int enPassantRow = row - forwardDirection;

            if (board[enPassantRow, col] is Pawn enPassantPawn && enPassantPawn.ableToEnPassant)
            {
                Piece capturedPawn = list.First(x => x == enPassantPawn);
                capturedPawn.Position = null;
                board[enPassantRow, col] = new EmptyPiece();
            }
        }

        public static string getEnPassantAvailability(Piece[,] board)
        {
            foreach (Piece piece in board)
            {
                if (piece is not Pawn) continue;

                (int row, int col) = piece.getPositionTuple();

				if (piece.As<Pawn>()!.ableToEnPassant && ((col>0 && board[row, col-1] is Pawn && board[row, col-1].Color != piece.Color) || (col<7&&board[row, col + 1] is Pawn && board[row, col + 1].Color != piece.Color)))
                {
					int direction = piece.Color == Color.White ? 2 : -2;
					return ChessNotationConverter.convertCoordinateToAlgebraicNotation($"{row+direction}{col}");
				}
            }

            return "-";
        }
    }
}
