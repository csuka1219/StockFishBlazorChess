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
    public partial class Game : IDisposable
    {
        [Inject]
        private ILocalStorageService localStorage { get; set; } = default!;

        [Inject]
        private IDialogService dialogService { get; set; } = default!;

        [Inject]
        private NavigationManager navigationManager { get; set; } = default!;
        [Inject]
        private IUserHandler userHandler { get; set; } = default!;
        [Inject]
        private IMatchManager matchManager { get; set; } = default!;

        [Parameter]
        public string side { get; set; } = string.Empty;
        [Parameter]
        public string difficulty { get; set; } = string.Empty;


        private readonly ChessGameService chessGameService = new ChessGameService();
        private readonly StockfishService stockfishService = new StockfishService();
        private Stockfish stockfish = default!;
        private string? uniqueGuid;
        private string gameKey = string.Empty;
        private bool isWhiteSide;

        protected override async Task OnInitializedAsync()
        {
            stockfish = new Stockfish(stockfishService);

            stockfishService.startEngine();

            stockfish.setDifficulty(Convert.ToInt32(difficulty));

            // Retrieve the unique GUID from local storage
            uniqueGuid = await localStorage.GetItemAsync<string>("uniqueGuid");
            gameKey = uniqueGuid + difficulty;
            isWhiteSide = string.Equals(side, "white");
        }
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                joinGame();
            }
            base.OnAfterRender(firstRender);
        }

        private void pieceUpdated(MudItemDropInfo<Piece> piece)
        {
            chessGameService.movePiece(piece, difficulty);
            if (chessGameService.checkForCheckmate())
            {
                gameEndDialog();
                return;
            }
            stockfishMove();
        }

        private void stockfishMove()
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
                    "CheckMate!",
                    chessGameService.player.IsMyTurn ?
                    "Unfortunately, you have lost. Better luck next time!"
                    :
                    "Congratulations, you won the game!",
                    cancelText: "Close");

            if (result.HasValue && result.Value)
            {
                // Perform necessary actions for exiting or starting a new game
                // InitGame();
                //StateHasChanged();
            }
        }

        private void joinGame()
        {
            if (string.IsNullOrEmpty(uniqueGuid))
            {
                navigationManager.NavigateTo("/");
                return;
            }

            // Join the new game
            Dictionary<string, List<string>> connectedPlayers = userHandler.getConnectedPlayers();

            // Check if there are already connected players for the game
            if (connectedPlayers.ContainsKey(gameKey))
            {
                handleExistingPlayer(connectedPlayers, uniqueGuid);
            }
            else
            {
                handleNewPlayer(uniqueGuid);
                if (!isWhiteSide)
                {
                    stockfishMove();
                    chessGameService._container.Refresh();
                }
            }
        }
        private void handleExistingPlayer(Dictionary<string, List<string>> connectedPlayers, string uniqueGuid)
        {
            Dictionary<string, MatchInfo> matchInfos = matchManager.getMatchInfos();

            if (connectedPlayers[gameKey].Count > 0 && !connectedPlayers[gameKey].Contains(uniqueGuid))
            {
                navigationManager.NavigateTo("/");
                return;
            }
            // Check if the current player is already connected to the game
            if (connectedPlayers[gameKey].Contains(uniqueGuid))
            {
                // The player is refreshing or renavigating
                // Update the chessboard, pieces list, and player turn
                chessGameService.chessBoard.board = matchManager.getMatchInfoBoard(gameKey);
                chessGameService.pieceChanges = matchManager.getMatchInfoMoves(gameKey);
                chessGameService.piecesOnBoard = chessGameService.chessBoard.board.Cast<Piece>().ToList();

                chessGameService.player.IsMyTurn = true;
                chessGameService.player.isWhitePlayer = isWhiteSide;
                chessGameService.whiteTurn = isWhiteSide;
                chessGameService.ableToMove = true;
                StateHasChanged();
                chessGameService._container.Refresh();
            }
        }

        private void handleNewPlayer(string uniqueGuid)
        {
            // First player to connect to the game
            // Create new entries for connected players and match information
            userHandler.addConnectedPlayer(gameKey, uniqueGuid);

            matchManager.addMatchInfo(gameKey);

            chessGameService.player.IsMyTurn = isWhiteSide;
            chessGameService.player.isWhitePlayer = isWhiteSide;
            chessGameService.ableToMove = true;
        }

        private void playAgainClick()
        {
            matchManager.removeMatchInfo(gameKey);
            userHandler.removeConnectedPlayer(gameKey);
            navigationManager.NavigateTo(navigationManager.Uri, forceLoad: true);
        }
        private void giveUpClick()
        {
            matchManager.removeMatchInfo(gameKey);
            userHandler.removeConnectedPlayer(gameKey);
            navigationManager.NavigateTo("/");
        }
        private void exitClick()
        {
            navigationManager.NavigateTo("/");
        }

        // Implementation of the IDisposable interface to perform cleanup when the object is disposed
        public void Dispose()
        {
            // Update the match information with the current state of the chessboard
            stockfishService.stopEngine();
            if (!matchManager.getMatchInfos().ContainsKey(gameKey)) return;
            matchManager.setMatchInfoBoard(gameKey, chessGameService.chessBoard.board, chessGameService.whiteTurn);
            matchManager.setMatchInfoMoves(gameKey, chessGameService.pieceChanges, chessGameService.whiteTurn);
        }
    }
}