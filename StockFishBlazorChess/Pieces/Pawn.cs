using StockFishBlazorChess.Data;
using MudBlazor.Extensions;

namespace StockFishBlazorChess.Pieces
{
    public class Pawn : Piece
    {
        public bool ableToEnPassant { get; set; }
        public Pawn(Color color, int pieceValue, string? icon, string? position, bool ableToEnPassant = false) : base(color, pieceValue, icon, position)
        {
            this.ableToEnPassant = ableToEnPassant;
        }

        public override bool[,] calculatePossibleMoves(Piece[,] board, bool[,] availableMoves)
        {
            (int row, int col) = getPositionTuple();
            if (row == 7 || row == 0)
                return availableMoves;

            bool isWhiteTurn = Color == Color.White;
            int forwardDirection = isWhiteTurn ? -1 : 1;
            int startingRow = isWhiteTurn ? 6 : 1;
            int enPassantRow = isWhiteTurn ? 3 : 4;

            int leftDiagonalCol = col - 1;
            int rightDiagonalCol = col + 1;

            if (isValidMove(board, row + forwardDirection, col))
            {
                markMove(availableMoves, row + forwardDirection, col);
            }

            if (isValidCapture(board, row + forwardDirection, leftDiagonalCol, isWhiteTurn))
            {
                markMove(availableMoves, row + forwardDirection, leftDiagonalCol);
            }

            if (isValidCapture(board, row + forwardDirection, rightDiagonalCol, isWhiteTurn))
            {
                markMove(availableMoves, row + forwardDirection, rightDiagonalCol);
            }

            if (row == startingRow)
            {
                int doubleMoveRow = row + 2 * forwardDirection;
                if (isValidMove(board, doubleMoveRow, col) && board[row + forwardDirection, col].PieceValue == 0)
                {
                    markMove(availableMoves, doubleMoveRow, col);
                }
            }

            handleEnPassant(board, availableMoves, row, col, forwardDirection, enPassantRow);

            return base.calculatePossibleMoves(board, availableMoves);
        }

        private static bool isValidMove(Piece[,] board, int row, int col)
        {
            if (row >= 0 && row < 8 && col >= 0 && col < 8)
            {
                if (board[row, col].PieceValue == 0)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool isValidCapture(Piece[,] board, int row, int col, bool isWhiteTurn)
        {
            if (row >= 0 && row < 8 && col >= 0 && col < 8)
            {
                int pieceValue = board[row, col].PieceValue;

                if (isWhiteTurn && pieceValue > 10 && pieceValue != 0)
                {
                    return true;
                }
                else if (!isWhiteTurn && pieceValue < 10 && pieceValue != 0)
                {
                    return true;
                }
            }
            return false;
        }
        private void handleEnPassant(Piece[,] board, bool[,] availableMoves, int row, int col, int forwardDirection, int enPassantRow)
        {
            if (row == enPassantRow)
            {
                int leftCol = col - 1;
                int rightCol = col + 1;

                checkEnPassantCapture(board, availableMoves, row, leftCol, forwardDirection);
                checkEnPassantCapture(board, availableMoves, row, rightCol, forwardDirection);
            }
        }

        private void checkEnPassantCapture(Piece[,] board, bool[,] availableMoves, int row, int col, int forwardDirection)
        {
            if (col >= 0 && col < 8 && board[row, col] is Pawn && board[row, col].As<Pawn>()!.ableToEnPassant)
            {
                markMove(availableMoves, row + forwardDirection, col);
            }
        }

        private static void markMove(bool[,] availableMoves, int row, int col)
        {
            availableMoves[row, col] = true;
        }


        public override bool[,] getCheckPositions(Piece[,] board, bool[,] checkArray)
        {
            bool isWhite = this.Color == Color.White;
            int direction = isWhite ? 1 : -1;

            (int i, int j) = this.getPositionTuple();
            if (i == 7 || i == 0) return base.getCheckPositions(board, checkArray);

            int leftDiagonalCol = j - 1;
            int rightDiagonalCol = j + 1;
            if (leftDiagonalCol >= 0)
            {
                checkArray[i - direction, leftDiagonalCol] = true;
            }

            if (rightDiagonalCol < 8)
            {
                checkArray[i - direction, rightDiagonalCol] = true;
            }

            return base.getCheckPositions(board, checkArray);
        }

        public override string getAlgebraicNotation()
        {
            return Color == Color.White ? "P" : "p";
        }

        public override void setPosition(string? position)
        {
            int newRow = int.Parse(position![..1]);
            var oldRow = this.getPositionTuple().row;
            if (oldRow == (this.Color == Color.White ? 6 : 1) && newRow == (this.Color == Color.White ? 4 : 3))
            {
                this.ableToEnPassant = true;
            }
            else
            {
                this.ableToEnPassant = false;
            }

            base.setPosition(position);
        }
    }
}
