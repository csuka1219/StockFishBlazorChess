using StockFishBlazorChess.Pieces;
using MudBlazor;
using MudBlazor.Extensions;
using System.Text;
using StockFishBlazorChess.Data;
using StockFishBlazorChess.Utilities;

namespace StockFishBlazorChess.Handlers
{
    public class UserHandler : IUserHandler
    {
        private Dictionary<string, List<string>> connectedPlayers = new Dictionary<string, List<string>>();
        private Dictionary<string, MatchInfo> matchInfos = new Dictionary<string, MatchInfo>();

        public List<string> getConnectedPlayerKeys()
        {
            return connectedPlayers.Where(cp => cp.Value.Count == 1).Select(cp => cp.Key).ToList();
        }

        public void setMatchInfoMoves(string gameName, List<PieceChange> pieceChanges, bool isWhiteTurn)
        {
            matchInfos[gameName].pieceChanges = new List<PieceChange>(pieceChanges);
            matchInfos[gameName].isWhiteTurn = isWhiteTurn;
        }

        public void setMatchInfoBoard(string gameName, Piece[,] board, bool isWhiteTurn)
        {
            string boardString = ChessNotationConverter.convertBoardToFEN(board, isWhiteTurn);
            matchInfos[gameName].boardInfo = boardString;
        }

        public Piece[,] getMatchInfoBoard(string gameName)
        {
            return ChessNotationConverter.convertStringToFEN(matchInfos[gameName].boardInfo);
        }

        public List<PieceChange> getMatchInfoMoves(string gameName)
        {
            return matchInfos[gameName].pieceChanges;
        }

        public Dictionary<string, List<string>> getConnectedPlayers()
        {
            return connectedPlayers;
        }

        public Dictionary<string, MatchInfo> getMatchInfos()
        {
            return matchInfos;
        }
    }
}