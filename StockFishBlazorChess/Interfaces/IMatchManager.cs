using StockFishBlazorChess.Data;
using StockFishBlazorChess.Pieces;

namespace StockFishBlazorChess.Interfaces
{
    public interface IMatchManager
    {
        void setMatchInfoMoves(string key, List<PieceChange> pieceChanges, bool isWhiteTurn);
        void setMatchInfoBoard(string key, Piece[,] board, bool isWhiteTurn);
        Piece[,] getMatchInfoBoard(string key);
        List<PieceChange> getMatchInfoMoves(string key);
        Dictionary<string, MatchInfo> getMatchInfos();
    }
}
