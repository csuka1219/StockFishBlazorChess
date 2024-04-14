using StockFishBlazorChess.Data;

namespace StockFishBlazorChess.Pieces
{
    public class Bishop : Piece
    {
        public Bishop(Color color, int pieceValue, string? icon, string? position) : base(color, pieceValue, icon, position)
        {
        }

        public override bool[,] calculatePossibleMoves(Piece[,] board, bool[,] availableMoves)
        {
            (int row, int col) = this.getPositionTuple();
            availableMoves = MoveGenerator.generateDiagonalMoves(row, col, this.Color, board, availableMoves);
            return base.calculatePossibleMoves(board, availableMoves);
        }

        public override bool[,] checkForStale(Piece[,] board, bool[,] staleArray)
        {
            staleArray = this.calculatePossibleMoves(board, staleArray);
            return base.checkForStale(board, staleArray);
        }

        public override string getFENRepresentation()
        {
            return Color == Color.White ? "B" : "b";
        }
    }
}
