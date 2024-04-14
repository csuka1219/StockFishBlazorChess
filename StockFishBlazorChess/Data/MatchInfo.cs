namespace StockFishBlazorChess.Data
{
    public class MatchInfo
    {
        public List<PieceChange> pieceChanges { get; set; }
        public string boardInfo { get; set; }
        public bool isWhiteTurn { get; set; }

        public MatchInfo()
        {
            pieceChanges = new List<PieceChange>();
            boardInfo = string.Empty;
            isWhiteTurn = true;
        }
    }
}
