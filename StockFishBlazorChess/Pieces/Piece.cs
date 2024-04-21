
namespace StockFishBlazorChess.Pieces
{
    public abstract class Piece
    {
        public readonly Color Color;
        public int PieceValue { get; init; }
        public string? Icon { get; set; }
        public string? Position { get; set; }

        public Piece(Color color, int pieceValue, string? icon, string? position)
        {
            Color = color;
            PieceValue = pieceValue;
            Icon = icon;
            Position = position;
        }
        public virtual bool[,] calculatePossibleMoves(Piece[,] board, bool[,] availableMoves)
        {
            return availableMoves;
        }
        public virtual bool[,] calculatePossibleMoves(Piece[,] board, bool[,] availableMoves, bool[,] checkArray)
        {
            return availableMoves;
        }

        public virtual bool[,] getCheckPositions(Piece[,] board, bool[,] staleArray)
        {
            return staleArray;
        }

        public virtual void setPosition(string? position)
        {
            this.Position = position;
        }
        public virtual void setPosition(string? position, bool simulate)
        {
            this.Position = position;
        }

        public (int row, int col) getPositionTuple()
        {
            int row = int.Parse(this.Position![..1]);
            int col = int.Parse(this.Position!.Substring(1, 1));
            return (row, col);
        }

        public virtual string getAlgebraicNotation()
        {
            return "0";
        }
    }
}
