using StockFishBlazorChess.Data;
using StockFishBlazorChess.Pieces;

namespace StockFishBlazorChess.Interfaces
{
    public interface IMatchManager
    {
        public void addMatchInfo(string key);
        public void removeMatchInfo(string key);
        void setMatchInfoMoves(string key, List<PieceChange> pieceChanges, bool isWhiteTurn);
        void setMatchInfoBoard(string key, Piece[,] board, bool isWhiteTurn);
        Piece[,] getMatchInfoBoard(string key);
        List<PieceChange> getMatchInfoMoves(string key);
        Dictionary<string, MatchInfo> getMatchInfos();
    }
}
