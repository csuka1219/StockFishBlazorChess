using StockFishBlazorChess.Data;

namespace StockFishBlazorChess.Pieces
{
    public class Queen : Piece
    {
        public Queen(Color color, int pieceValue, string? icon, string? position) : base(color, pieceValue, icon, position)
        {
        }
        public override bool[,] calculatePossibleMoves(Piece[,] board, bool[,] availableMoves)
        {
            (int row, int col) = this.getPositionTuple();
            availableMoves = MoveGenerator.generateDiagonalMoves(row, col, this.Color, board, availableMoves);
            availableMoves = MoveGenerator.generateStraightMoves(row, col, this.Color, board, availableMoves);
            return base.calculatePossibleMoves(board, availableMoves);
        }
        public override bool[,] getCheckPositions(Piece[,] board, bool[,] checkArray)
        {
            checkArray = this.calculatePossibleMoves(board, checkArray);
            return base.getCheckPositions(board, checkArray);
        }

        public override string getAlgebraicNotation()
        {
            return Color == Color.White ? "Q" : "q";
        }
    }
}
