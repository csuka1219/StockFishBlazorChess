using StockFishBlazorChess.Services;
using StockFishBlazorChess.Utilities;

namespace StockFishBlazorChess.Game
{
    public class Stockfish
    {
        private StockfishService stockfishService;
        private readonly int depth = 15;
        private readonly int waitTime = 100; //milliseconds

        public Stockfish(StockfishService stockfishService)
        {
            this.stockfishService = stockfishService;
        }

        public void setDifficulty(int elo)
        {
            stockfishService.sendCommand($"setoption name UCI_LimitStrength value true");
            stockfishService.wait(waitTime);
            stockfishService.sendCommand($"setoption name UCI_Elo value {elo}");
        }

        public string getNextMove(string fenPosition)
        {
            stockfishService.sendCommand($"position fen {fenPosition}");
            stockfishService.wait(waitTime);
            stockfishService.sendCommand($"go depth {depth}");
            stockfishService.wait(waitTime);
            string response;
            while ((response = stockfishService.readLine()) != null)
            {
                if (response.StartsWith("bestmove"))
                {
                    response = response.Split(' ')[1]; // Assuming format "bestmove e2e4 ponder e7e5"
                    break;
                }
            }
            stockfishService.sendCommand($"position {fenPosition} moves {response}");
            return ChessNotationConverter.ConvertFenTo2DStringIndex(response);
        }

    }
}
