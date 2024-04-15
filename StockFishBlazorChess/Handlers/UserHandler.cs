using StockFishBlazorChess.Pieces;
using MudBlazor;
using MudBlazor.Extensions;
using System.Text;
using StockFishBlazorChess.Data;
using StockFishBlazorChess.Utilities;
using StockFishBlazorChess.Interfaces;

namespace StockFishBlazorChess.Handlers
{
    public class UserHandler : IUserHandler
    {
        private Dictionary<string, List<string>> connectedPlayers = new Dictionary<string, List<string>>();
        
        public Dictionary<string, List<string>> getConnectedPlayers()
        {
            return connectedPlayers;
        }
    }
}