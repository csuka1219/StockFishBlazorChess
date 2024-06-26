﻿using StockFishBlazorChess.Data;
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
using StockFishBlazorChess.Rules;
using StockFishBlazorChess.Components.Dialogs;

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

        private async void pieceUpdated(MudItemDropInfo<Piece> piece)
        {
            bool isPromotion = Promotion.isPromotion(piece);
            char pieceValueChar = '_';

            if (isPromotion)
            {
                pieceValueChar = await selectPromotionPiece();
            }

            chessGameService.movePiece(piece);

            if (isPromotion)
            {
				Promotion.performPromotion(chessGameService, piece.Item!, pieceValueChar);
				await InvokeAsync(StateHasChanged);
			}

			// If the last move was not a check, check for a stalemate.
			// Otherwise, check for checkmate.
			if (!chessGameService.pieceChanges.Last().isCheck && chessGameService.checkForStalemate())
            {
                await InvokeAsync(() => gameEndDialog("Stalemate!", true));
                return;
            }
            else if (chessGameService.checkForCheckmate())
            {
                chessGameService.pieceChanges.Last().isCheckmate = true;
                await InvokeAsync(() => gameEndDialog("Checkmate", false));
                return;
            }
            _ = Task.Run(() => { stockfishMove(); });
        }

		private async Task<char> selectPromotionPiece()
		{
			var dialog = await dialogService.ShowAsync<PromotionDialog>("Promotion");
			var result = await dialog.Result;
			string pieceStrValue = result.Data.ToString()!;
            return char.Parse(pieceStrValue);
		}

		private async void stockfishMove()
        {
            string boardFEN = ChessNotationConverter.convertBoardToFEN(chessGameService.chessBoard.board, chessGameService.whiteTurn);
            string nextMove = stockfish.getNextMove(boardFEN);
            Piece movedPiece = chessGameService._container.Items.First(x => x.Position == nextMove.Split(',')[0]);
            MudItemDropInfo<Piece> piece = new MudItemDropInfo<Piece>(movedPiece, nextMove.Split(',')[1], -1);

            chessGameService.movePiece(piece);

            if (nextMove.Length > 6) 
            {
                // if nextMove length is bigger than 6 then there is a promotion value on the last character
                // for example "a4,a6,q"
                Promotion.performPromotion(chessGameService, piece.Item!, nextMove.Last());
            }

            await InvokeAsync(StateHasChanged);
            chessGameService._container.Refresh();

            if (!chessGameService.pieceChanges.Last().isCheck && chessGameService.checkForStalemate())
            {
                chessGameService.pieceChanges.Last().isCheckmate = true;
                await InvokeAsync(() => gameEndDialog("Stalemate!", true));
                unSubcribeGame();
                return;
            }
            else if (chessGameService.checkForCheckmate())
            {
                chessGameService.pieceChanges.Last().isCheckmate = true;
                await InvokeAsync(() => gameEndDialog("Checkmate", false));
                unSubcribeGame();
                return;
            }
        }

        private async void gameEndDialog(string title, bool isStalemate)
        {
            if (isStalemate)
            {
                await dialogService.ShowMessageBox(
                        title,
                        "Draw, well played!"
                        );
            }
            else
            {
                await dialogService.ShowMessageBox(
                        title,
                        chessGameService.player.IsMyTurn ?
                        "Unfortunately, you have lost. Better luck next time!"
                        :
                        "Congratulations, you won the game!"
                        );
            }

        }

        private void joinGame()
        {
            if (string.IsNullOrEmpty(uniqueGuid))
            {
                navigationManager.NavigateTo("/");
                return;
            }

            Dictionary<string, List<string>> connectedPlayers = userHandler.getConnectedPlayers();

            // Check if the game is already exist
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
            if (connectedPlayers[gameKey].Count > 0 && !connectedPlayers[gameKey].Contains(uniqueGuid))
            {
                navigationManager.NavigateTo("/");
                return;
            }
            // Check if the current player is already connected to the game
            if (connectedPlayers[gameKey].Contains(uniqueGuid))
            {
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
            userHandler.addConnectedPlayer(gameKey, uniqueGuid);

            matchManager.addMatchInfo(gameKey);

            chessGameService.player.IsMyTurn = isWhiteSide;
            chessGameService.player.isWhitePlayer = isWhiteSide;
            chessGameService.ableToMove = true;
        }

        private void playAgainClick()
        {
            unSubcribeGame();
            navigationManager.NavigateTo(navigationManager.Uri, forceLoad: true);
        }
        private void giveUpClick()
        {
            unSubcribeGame();
            navigationManager.NavigateTo("/");
        }

        private void unSubcribeGame()
        {
            matchManager.removeMatchInfo(gameKey);
            userHandler.removeConnectedPlayer(gameKey);
        }
        private void exitClick()
        {
            navigationManager.NavigateTo("/");
        }

        public void Dispose()
        {
            stockfishService.stopEngine();
            if (!matchManager.getMatchInfos().ContainsKey(gameKey)) return;
            matchManager.setMatchInfoBoard(gameKey, chessGameService.chessBoard.board, chessGameService.whiteTurn);
            matchManager.setMatchInfoMoves(gameKey, chessGameService.pieceChanges, chessGameService.whiteTurn);

			GC.SuppressFinalize(this);
		}
    }
}