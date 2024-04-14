using StockFishBlazorChess.Services;

namespace StockFishBlazorChess.Game
{
    public class Stockfish
    {
        private StockfishService stockfishService;

        public Stockfish(StockfishService stockfishService)
        {
            this.stockfishService = stockfishService;
        }

        public string getBestMove()
        {
            stockfishService.sendCommand("go depth 5");
            string response;
            while ((response = stockfishService.readLine()) != null)
            {
                if (response.StartsWith("bestmove"))
                {
                    return response.Split(' ')[1]; // Assuming format "bestmove e2e4 ponder e7e5"
                }
            }
            return null;
        }
    }
}
