using StockFishBlazorChess.Data;
using StockFishBlazorChess.Pieces;

namespace StockFishBlazorChess.Interfaces
{
    public interface IUserHandler
    {
        public void addConnectedPlayer(string key, string uniqueGuid);
        public void removeConnectedPlayer(string key);
        Dictionary<string, List<string>> getConnectedPlayers();
    }
}
