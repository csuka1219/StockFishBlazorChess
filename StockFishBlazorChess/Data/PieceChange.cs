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
        public bool isCheck { get; set; }
        public bool isCheckmate { get; set; }

        public PieceChange((int, int) fromMove, (int, int) toMove, int movedPiece, int hitPiece, bool isCastling, bool isEnpassant, bool isCheck)
        {
            this.fromMove = fromMove;
            this.toMove = toMove;
            this.movedPieceValue = movedPiece;
            this.hitPiece = hitPiece;
            this.isCastling = isCastling;
            this.isEnpassant = isEnpassant;
            this.isCheck = isCheck;
            this.isCheckmate = isCheckmate;
        }
    }
}
