﻿using StockFishBlazorChess.Data;
using StockFishBlazorChess.Game;
using StockFishBlazorChess.Pieces;
using StockFishBlazorChess.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Data;
using System.Xml;
using StockFishBlazorChess.Handlers;

namespace StockFishBlazorChess.Components.Pages
{
    public partial class Game
    {
        [Inject]
        private ILocalStorageService localStorage { get; set; } = default!;

        [Inject] 
        private IDialogService dialogService { get; set; } = default!;

        [Inject]
		private NavigationManager navigationManager { get; set; } = default!;

        [Inject]
        private IUserHandler userHandler { get; set; } = default!;

        private ChessGameService chessGameService = default!;
        private StockfishService stockfishService = default!;

        [Parameter]
        public string gameName { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            chessGameService = new ChessGameService();
            stockfishService = new StockfishService();
            Stockfish stockfish = new Stockfish(stockfishService);
            stockfishService.startEngine();
            string bestMove = stockfish.getBestMove();
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
            chessGameService.movePiece(piece, gameName);
            if (chessGameService.checkForCheckmate())
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
            
        }

        private async Task joinGame()
        {
            // Retrieve the unique GUID from local storage
            string? uniqueGuid = await localStorage.GetItemAsync<string>("uniqueGuid");

            if (string.IsNullOrEmpty(uniqueGuid))
            {
                navigationManager.NavigateTo("/");
                return;
            }

            // Join the new game
            Dictionary<string, List<string>> connectedPlayers = userHandler.getConnectedPlayers();
            Dictionary<string, MatchInfo> matchInfos = userHandler.getMatchInfos();

            // Check if there are already connected players for the game
            if (connectedPlayers.ContainsKey(gameName))
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
            if (connectedPlayers[gameName].Count > 0 && !connectedPlayers[gameName].Contains(uniqueGuid))
            {
                navigationManager.NavigateTo("/");
                return;
            }
            // Check if the current player is already connected to the game
            if (connectedPlayers[gameName].Contains(uniqueGuid))
            {
                // The player is refreshing or renavigating
                // Update the chessboard, pieces list, and player turn
                chessGameService.chessBoard.board = userHandler.getMatchInfoBoard(gameName);
                chessGameService.pieceChanges = userHandler.getMatchInfoMoves(gameName);
                chessGameService.piecesOnBoard = chessGameService.chessBoard.board.Cast<Piece>().ToList();
                bool isWhitePlayer = connectedPlayers[gameName].First() == uniqueGuid;

                chessGameService.player.IsMyTurn = isWhitePlayer == matchInfos[gameName].isWhiteTurn;
                chessGameService.player.isWhitePlayer = isWhitePlayer;
                chessGameService.whiteTurn = matchInfos[gameName].isWhiteTurn;
                StateHasChanged();
                chessGameService._container.Refresh();
            }
        }
        
        private void handleNewPlayer(Dictionary<string, List<string>> connectedPlayers, Dictionary<string, MatchInfo> matchInfos, string uniqueGuid)
        {
            // First player to connect to the game
            // Create new entries for connected players and match information
            connectedPlayers.Add(gameName, new List<string>() { uniqueGuid });
            matchInfos.Add(gameName, new MatchInfo());
            chessGameService.player.IsMyTurn = true;
            chessGameService.player.isWhitePlayer = true;
            chessGameService.ableToMove = true;
        }

        // Implementation of the IDisposable interface to perform cleanup when the object is disposed
        public async void Dispose()
        {
            // Update the match information with the current state of the chessboard
            userHandler.setMatchInfoBoard(gameName, chessGameService.chessBoard.board);
            userHandler.setMatchInfoMoves(gameName, chessGameService.pieceChanges, chessGameService.whiteTurn);
        }
    }
}