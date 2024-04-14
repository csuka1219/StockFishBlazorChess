using StockFishBlazorChess.Data;

namespace StockFishBlazorChess.Pieces
{
    public class Rook : Piece
    {
        public bool ableToCastling { get; set; }
        public Rook(Color color, int pieceValue, string? icon, string? position, bool ableToCastling = true) : base(color, pieceValue, icon, position)
        {
            this.ableToCastling = ableToCastling;
        }

        public override bool[,] calculatePossibleMoves(Piece[,] board, bool[,] availableMoves)
        {
            (int row, int col) = this.getPositionTuple();
            availableMoves = MoveGenerator.generateStraightMoves(row, col, this.Color, board, availableMoves);
            return base.calculatePossibleMoves(board, availableMoves);
        }

        public override bool[,] checkForStale(Piece[,] board, bool[,] staleArray)
        {
            staleArray = this.calculatePossibleMoves(board, staleArray);
            return base.checkForStale(board, staleArray);
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
            return Color == Color.White ? "R" : "r";
        }
    }
}
