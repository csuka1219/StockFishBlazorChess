using StockFishBlazorChess.Data;
using StockFishBlazorChess.Interfaces;
using StockFishBlazorChess.Pieces;
using StockFishBlazorChess.Utilities;

namespace StockFishBlazorChess.Services
{
    public class MatchManager : IMatchManager
    {
        private Dictionary<string, MatchInfo> matchInfos = new Dictionary<string, MatchInfo>();

        public void addMatchInfo(string key)
        {
            matchInfos.Add(key, new MatchInfo());
        }

        public void removeMatchInfo(string key)
        {
            matchInfos.Remove(key);
        }

        public void setMatchInfoMoves(string key, List<PieceChange> pieceChanges, bool isWhiteTurn)
        {
            matchInfos[key].pieceChanges = new List<PieceChange>(pieceChanges);
            matchInfos[key].isWhiteTurn = isWhiteTurn;
        }

        public void setMatchInfoBoard(string key, Piece[,] board, bool isWhiteTurn)
        {
            string boardString = ChessNotationConverter.convertBoardToFEN(board, isWhiteTurn);
            matchInfos[key].boardInfo = boardString;
        }

        public Piece[,] getMatchInfoBoard(string key)
        {
            return ChessNotationConverter.convertFENToboard(matchInfos[key].boardInfo);
        }

        public List<PieceChange> getMatchInfoMoves(string key)
        {
            return matchInfos[key].pieceChanges;
        }


        public Dictionary<string, MatchInfo> getMatchInfos()
        {
            return matchInfos;
        }
    }
}
