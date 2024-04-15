using StockFishBlazorChess.Data;
using StockFishBlazorChess.Game;
using StockFishBlazorChess.Pieces;
using StockFishBlazorChess.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Data;
using System.Xml;
using StockFishBlazorChess.Utilities;
using StockFishBlazorChess.Interfaces;

namespace StockFishBlazorChess.Components.Pages
{
    public partial class Game: IDisposable
    {
        [Inject]
        private ILocalStorageService localStorage { get; set; } = default!;

        [Inject] 
        private IDialogService dialogService { get; set; } = default!;

        [Inject]
		private NavigationManager navigationManager { get; set; } = default!;
        [Inject]
        private IUserHandler userHandler { get; set; }
        [Inject]
        private IMatchManager matchManager { get; set; }

        [Parameter]
        public string difficulty { get; set; } = string.Empty;


        private ChessGameService chessGameService = new ChessGameService();
        private StockfishService stockfishService = new StockfishService();
        private Stockfish stockfish = default!;
        private string? uniqueGuid;

        protected override async Task OnInitializedAsync()
        {
            stockfishService.startEngine();
            stockfish = new Stockfish(stockfishService);
            stockfish.setDifficulty(Convert.ToInt32(difficulty));
            // Retrieve the unique GUID from local storage
            uniqueGuid = await localStorage.GetItemAsync<string>("uniqueGuid");
            //string bestMove = stockfish.getNextMove();
            int b = 0;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            // Create a new hub connection for the chess game
            if (firstRender)
            {
                await joinGame();
            }
        }


        private async void pieceUpdated(MudItemDropInfo<Piece> piece)
        {
            chessGameService.movePiece(piece, difficulty);
            if (chessGameService.checkForCheckmate())
            {
                gameEndDialog();
                return;
            }
            stockfishResponse();
        }

        private void stockfishResponse()
        {
            string boardFEN = ChessNotationConverter.convertBoardToFEN(chessGameService.chessBoard.board, chessGameService.whiteTurn);
            string test = stockfish.getNextMove(boardFEN);
            Piece movedPiece = chessGameService._container.Items.First(x => x.Position == test.Split(',')[0]);
            MudItemDropInfo<Piece> piece = new MudItemDropInfo<Piece>(movedPiece, test.Split(',')[1], -1);
            chessGameService.movePiece(piece, difficulty);
            if (chessGameService.checkForCheckmate())
            {
                gameEndDialog();
            }
        }

        private async void gameEndDialog()
        {
            bool? result = await dialogService.ShowMessageBox(
                    "Sakkmatt",
                    "later",
                    yesText: "Exit!", cancelText: "Again");

            if (result.HasValue && result.Value)
            {
                // Perform necessary actions for exiting or starting a new game
                // InitGame();
                //StateHasChanged();
            }
        }

        private async Task joinGame()
        {
            if (string.IsNullOrEmpty(uniqueGuid))
            {
                navigationManager.NavigateTo("/");
                return;
            }

            string key = uniqueGuid + difficulty;

            // Join the new game
            Dictionary<string, List<string>> connectedPlayers = userHandler.getConnectedPlayers();
            Dictionary<string, MatchInfo> matchInfos = matchManager.getMatchInfos();

            // Check if there are already connected players for the game
            if (connectedPlayers.ContainsKey(key))
            {
                handleExistingPlayer(connectedPlayers, matchInfos, uniqueGuid);
            }
            else
            {
                handleNewPlayer(connectedPlayers, matchInfos, uniqueGuid);
            }
        }
        private void handleExistingPlayer(Dictionary<string, List<string>> connectedPlayers, Dictionary<string, MatchInfo> matchInfos, string uniqueGuid)
        {
            string key = uniqueGuid + difficulty;

            if (connectedPlayers[key].Count > 0 && !connectedPlayers[key].Contains(uniqueGuid))
            {
                navigationManager.NavigateTo("/");
                return;
            }
            // Check if the current player is already connected to the game
            if (connectedPlayers[key].Contains(uniqueGuid))
            {
                // The player is refreshing or renavigating
                // Update the chessboard, pieces list, and player turn
                chessGameService.chessBoard.board = matchManager.getMatchInfoBoard(key);
                chessGameService.pieceChanges = matchManager.getMatchInfoMoves(key);
                chessGameService.piecesOnBoard = chessGameService.chessBoard.board.Cast<Piece>().ToList();
                bool isWhitePlayer = connectedPlayers[key].First() == uniqueGuid;

                chessGameService.player.IsMyTurn = isWhitePlayer == matchInfos[key].isWhiteTurn;
                chessGameService.player.isWhitePlayer = isWhitePlayer;
                chessGameService.whiteTurn = matchInfos[key].isWhiteTurn;
                StateHasChanged();
                chessGameService._container.Refresh();
            }
        }
        
        private void handleNewPlayer(Dictionary<string, List<string>> connectedPlayers, Dictionary<string, MatchInfo> matchInfos, string uniqueGuid)
        {
            // First player to connect to the game
            // Create new entries for connected players and match information
            string key = uniqueGuid + difficulty;
            connectedPlayers.Add(key, [uniqueGuid]);
            matchInfos.Add(key, new MatchInfo());
            chessGameService.player.IsMyTurn = true;
            chessGameService.player.isWhitePlayer = true;
            chessGameService.ableToMove = true;
        }

        // Implementation of the IDisposable interface to perform cleanup when the object is disposed
        public async void Dispose()
        {
            // Update the match information with the current state of the chessboard
            matchManager.setMatchInfoBoard(uniqueGuid + difficulty, chessGameService.chessBoard.board, chessGameService.whiteTurn);
            matchManager.setMatchInfoMoves(uniqueGuid + difficulty, chessGameService.pieceChanges, chessGameService.whiteTurn);
        }
    }
}