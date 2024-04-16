using StockFishBlazorChess.Pieces;
using MudBlazor;
using MudBlazor.Extensions;
using System.Text;
using StockFishBlazorChess.Data;
using StockFishBlazorChess.Utilities;
using StockFishBlazorChess.Interfaces;

namespace StockFishBlazorChess.Services
{
    public class UserHandler : IUserHandler
    {
        private Dictionary<string, List<string>> connectedPlayers = new Dictionary<string, List<string>>();

        public void addConnectedPlayer(string key, string uniqueGuid)
        {
            connectedPlayers.Add(key, [uniqueGuid]);
        }
        public void removeConnectedPlayer(string key)
        {
            connectedPlayers.Remove(key);
        }
        public Dictionary<string, List<string>> getConnectedPlayers()
        {
            return connectedPlayers;
        }

    }
}