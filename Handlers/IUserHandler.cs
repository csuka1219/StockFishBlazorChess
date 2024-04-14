using StockFishBlazorChess.Data;
using StockFishBlazorChess.Pieces;

namespace StockFishBlazorChess.Handlers
{
    public interface IUserHandler
    {
        List<string> getConnectedPlayerKeys();
        void setMatchInfoMoves(string gameName, List<PieceChange> pieceChanges, bool isWhiteTurn);
        void setMatchInfoBoard(string gameName, Piece[,] board);
        Piece[,] getMatchInfoBoard(string gameName);
        List<PieceChange> getMatchInfoMoves(string gameName);
        Dictionary<string, List<string>> getConnectedPlayers();
        Dictionary<string, MatchInfo> getMatchInfos();
    }
}
