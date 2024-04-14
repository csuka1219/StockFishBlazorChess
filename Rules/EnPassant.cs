using StockFishBlazorChess.Pieces;

namespace StockFishBlazorChess.Rules
{    public static class EnPassant
    {
        public static bool isEnPassant(Piece piece, int row, int col)
        {
            int enPassantRow = piece.Color == Color.White ? 2 : 5;
            return piece is Pawn && row == enPassantRow && col != piece.getPositionTuple().col;
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
    }
}
