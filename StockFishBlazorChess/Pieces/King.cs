using MudBlazor.Extensions;

namespace StockFishBlazorChess.Pieces
{
    public class King : Piece
    {
        public bool ableToCastling { get; set; }

        public King(Color color, int pieceValue, string? icon, string? position, bool ableToCastling = true) : base(color, pieceValue, icon, position)
        {
            this.ableToCastling = ableToCastling;
        }

        public override bool[,] calculatePossibleMoves(Piece[,] board, bool[,] availableMoves, bool[,] checkArray)
        {
            (int row, int col) = this.getPositionTuple();
            int size = 8;

            // Check for valid moves in all directions
            checkMove(row, col - 1);
            checkMove(row, col + 1);
            checkMove(row - 1, col);
            checkMove(row + 1, col);
            checkMove(row - 1, col - 1);
            checkMove(row - 1, col + 1);
            checkMove(row + 1, col - 1);
            checkMove(row + 1, col + 1);

            // Check for additional moves if not in 'all' mode and castling conditions are met
            if (ableToCastling)
            {
                checkCastling();
            }

            void checkMove(int r, int c)
            {
                if (IsValidMove(r, c, board))
                {
                    availableMoves[r, c] = true;
                }
            }

            void checkCastling()
            {
                if (Color == Color.White)
                {
                    CheckCastlingForRook(7, 0, size - 1, 2, 3);
                    CheckCastlingForRook(7, 7, size - 1, 6, 5);
                }
                else
                {
                    CheckCastlingForRook(0, 0, 0, 2, 3);
                    CheckCastlingForRook(0, 7, 0, 6, 5);
                }
            }

            void CheckCastlingForRook(int rookRow, int rookCol, int row, int targetCol1, int targetCol2)
            {
                var rook = board[rookRow, rookCol];
                if (rook.GetType() == typeof(Rook) && rook.As<Rook>()!.ableToCastling && board[row, targetCol1].PieceValue == 0 && board[row, targetCol2].PieceValue == 0 && !checkArray[row, targetCol1] && !checkArray[row, targetCol2] && !checkArray[row, col])
                {
                    availableMoves[row, targetCol1] = true;
                }
            }

            return base.calculatePossibleMoves(board, availableMoves);
        }

		public override bool[,] calculatePossibleMoves(Piece[,] board, bool[,] availableMoves)
        {
            bool[,] checkArray = new bool[8, 8];
			(int row, int col) = this.getPositionTuple();
            checkArray[row, col] = true;
            return calculatePossibleMoves(board, availableMoves, checkArray);
        }


		private bool IsValidMove(int r, int c, Piece[,] board)
        {
            if (r >= 0 && r < 8 && c >= 0 && c < 8)
            {
                if (Color == Color.White)
                {
                    return board[r, c].PieceValue > 10 || board[r, c].PieceValue == 0;
                }
                else
                {
                    return board[r, c].PieceValue == 0 || board[r, c].PieceValue < 10;
                }
            }
            return false;
        }

        public override bool[,] getCheckPositions(Piece[,] board, bool[,] checkArray)
        {
            checkArray = this.calculatePossibleMoves(board, checkArray, new bool[8,8]);
            return base.getCheckPositions(board, checkArray);
        }


        public override void setPosition(string? position)
        {
            if (ableToCastling)
            {
                ableToCastling = false;
            }
            base.setPosition(position);
        }
        public override void setPosition(string? position, bool simulate)
        {
            base.setPosition(position);
        }

        public override string getFENRepresentation()
        {
            return Color == Color.White ? "K" : "k";
        }
    }
}
