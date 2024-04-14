namespace StockFishBlazorChess.Pieces
{
    public class EmptyPiece : Piece
    {
        public EmptyPiece() : base(Color.None, 0, null, null)
        {
        }

        public override void setPosition(string? position)
        {
            position = null;
            base.setPosition(position);
        }

        public override void setPosition(string? position, bool simulate)
        {
            position = null;
            base.setPosition(position, simulate);
        }
    }
}