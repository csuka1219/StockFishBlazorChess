using StockFishBlazorChess.Data;
using StockFishBlazorChess.Game;
using StockFishBlazorChess.Pieces;
using MudBlazor;
using StockFishBlazorChess.Rules;

namespace StockFishBlazorChess.Services
{

    public class ChessGameService
    {
        public Chessboard chessBoard = new Chessboard();
        public Player player = new Player();
        public IEnumerable<Piece> piecesOnBoard = new List<Piece>();
        public MudDropContainer<Piece> _container = default!;
        public List<PieceChange> pieceChanges = new List<PieceChange>();

        public bool[,] availableMoves = new bool[8, 8];
        public bool whiteTurn = true;
        public bool ableToMove = false;

        private bool lastTurn = true;
        private bool isCheckmate = false;
        private bool dragEnded = true;
        private string lastposition = "";
        public ChessGameService()
        {
            piecesOnBoard = chessBoard.board.Cast<Piece>().ToList();
        }

        public bool canMovePiece(Piece selectedPiece, string destination)
        {
            if (isValidGameState(selectedPiece))
            {
                return false;
            }

            resetDragStateIfNeeded(selectedPiece);

			(int row, int col) = parsePosition(destination);

            bool[,] checkArray = getCheckArray();

            if (dragEnded)
            {
                if (selectedPiece is King)
                {
                    availableMoves = selectedPiece.calculatePossibleMoves(chessBoard.board, availableMoves, checkArray);
                }
                else
                {
                    availableMoves = selectedPiece.calculatePossibleMoves(chessBoard.board, availableMoves);
                }

                dragEnded = false;
                lastposition = selectedPiece.Position!;
                lastTurn = whiteTurn;
                removeInvalidMoves(selectedPiece);
            }

            return availableMoves[row, col];
        }

        public void movePiece(MudItemDropInfo<Piece> piece)
        {
            (int newRow, int newCol) = parsePosition(piece.DropzoneIdentifier);

            bool isCastling = false;
            bool isEnpassant = false;

            bool isHitPiece = piecesOnBoard.Any(p => p.PieceValue != piece.Item!.PieceValue && p.Position == piece.DropzoneIdentifier);
            int hitpieceValue = 0;
            if (isHitPiece)
            {
                // Clear the position of the captured piece
                Piece hitPiece = piecesOnBoard.First(p => p.Position == piece.DropzoneIdentifier);
                hitPiece.Position = null;
                hitpieceValue = hitPiece.PieceValue;
            }
            else
            {
                isCastling = Castling.canPerformCastling(piece.Item!, newRow, newCol);
                isEnpassant = EnPassant.isEnPassant(piece.Item!, newRow, newCol);
            }

            (int oldRow, int oldCol) = piece.Item!.getPositionTuple();

            chessBoard.setPiece(newRow, newCol, piece.Item, piecesOnBoard, isCastling, isEnpassant);

            dragEnded = true;

            availableMoves = new bool[8, 8];

            whiteTurn = !whiteTurn;

            player.IsMyTurn = !player.IsMyTurn;

            bool isCheck = Check.checkChecker(chessBoard.board, whiteTurn);

            pieceChanges.Add(
                new PieceChange(
                    (oldRow, oldCol), 
                    (newRow, newCol), 
                    piece.Item.PieceValue, 
                    hitpieceValue, 
                    isCastling, 
                    isEnpassant,
                    isCheck)
                );
        }
        
        public bool checkForCheckmate()
        {
            isCheckmate = Checkmate.isCheckmate(chessBoard.board, !whiteTurn);
            ableToMove = !isCheckmate;
            return isCheckmate;
        }

        public bool checkForStalemate()
        {
            // stalemate meaning there is no available moves just like in checkmate, the difference is the king not in check
            // so we can use the isCheckmate function for check stalemate
            return Checkmate.isCheckmate(chessBoard.board, !whiteTurn);
        }

        private bool isValidGameState(Piece piece)
        {
            return (!ableToMove || !player.IsMyTurn || (!whiteTurn && piece.PieceValue < PieceConstants.blackPawnValue) || (whiteTurn && piece.PieceValue > PieceConstants.whiteKingValue));

		}

        private void resetDragStateIfNeeded(Piece piece)
        {
			if (whiteTurn == lastTurn && piece.Position != lastposition && !dragEnded)
			{
				availableMoves = new bool[8, 8];
				dragEnded = true;
			}
		}

		private bool[,] getCheckArray()
		{
			bool[,] checkArray = new bool[8, 8];
			foreach (Piece piece in chessBoard.board)
			{
				if (piece.Color == Pieces.Color.White != whiteTurn)
				{
					checkArray = piece.getCheckPositions(chessBoard.board, checkArray);
				}
			}
			return checkArray;
		}

		private static (int, int) parsePosition(string position)
		{
			int row = position[0] - '0';
			int col = position[1] - '0';
			return (row, col);
		}

		private void removeInvalidMoves(Piece piece)
        {
            (int row, int col) = piece.getPositionTuple();

            for (int newRow = 0; newRow < 8; newRow++)
            {
                for (int newCol = 0; newCol < 8; newCol++)
                {
                    // Check if the piece can move into the current cell
                    if (availableMoves[newRow, newCol])
                    {
                        Piece lastHitPiece = chessBoard.board[newRow, newCol];

                        // Move the piece to the new cell and update its position
                        chessBoard.board[newRow, newCol] = piece;
                        chessBoard.board[newRow, newCol].setPosition($"{row}{col}", true);
                        chessBoard.board[row, col].setPosition($"{newRow}{newCol}", true);

                        // Replace the original cell with an empty piece
                        chessBoard.board[row, col] = new EmptyPiece();

                        // Check if the move leads to a check for the player
                        if (whiteTurn && Check.checkChecker(chessBoard.board, whiteTurn))
                        {
                            // Mark the move as invalid
                            availableMoves[newRow, newCol] = false;
                        }

                        // Restore the original positions and pieces on the chessboard
                        chessBoard.board[newRow, newCol] = lastHitPiece;
                        chessBoard.board[newRow, newCol].setPosition($"{newRow}{newCol}", true);
                        chessBoard.board[row, col] = piece;
                        chessBoard.board[row, col].setPosition($"{row}{col}", true);
                    }
                }
            }
        }
        
    }

}
