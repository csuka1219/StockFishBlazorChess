using StockFishBlazorChess.Data;
using StockFishBlazorChess.Pieces;

namespace StockFishBlazorChess.Interfaces
{
    public interface IUserHandler
    {
        Dictionary<string, List<string>> getConnectedPlayers();
    }
}
