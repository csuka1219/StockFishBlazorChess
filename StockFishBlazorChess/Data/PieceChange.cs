namespace StockFishBlazorChess.Data
{
    public class PieceChange
    {
        public (int row, int col) fromMove { get; set; }
        public (int row, int col) toMove { get; set; }
        public int movedPieceValue { get; set; }
        public int hitPiece { get; set; }

        public PieceChange((int, int) fromMove, (int, int) toMove, int movedPiece, int hitPiece)
        {
            this.fromMove = fromMove;
            this.toMove = toMove;
            this.movedPieceValue = movedPiece;
            this.hitPiece = hitPiece;
        }
    }
}
