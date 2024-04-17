namespace StockFishBlazorChess.Data
{
    public class PieceChange
    {
        public (int row, int col) fromMove { get; set; }
        public (int row, int col) toMove { get; set; }
        public int movedPieceValue { get; set; }
        public int hitPiece { get; set; }
        public bool isCastling { get; set; }
        public bool isEnpassant { get; set; }

        public PieceChange((int, int) fromMove, (int, int) toMove, int movedPiece, int hitPiece, bool isCastling = false, bool isEnpassant =  false)
        {
            this.fromMove = fromMove;
            this.toMove = toMove;
            this.movedPieceValue = movedPiece;
            this.hitPiece = hitPiece;
            this.isCastling = isCastling;
            this.isEnpassant = isEnpassant;
        }
    }
}
