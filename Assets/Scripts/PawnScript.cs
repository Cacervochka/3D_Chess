using UnityEngine;
using System.Collections.Generic;

public class PawnScript : ChessPiece
{
    public override List<Vector2Int> GetValidMoves(ChessPiece[,] board, List<Move> moveHistory)
    {
        List<Vector2Int> validMoves = new List<Vector2Int>();

        int direction = IsWhite ? 1 : -1;
        int targetY = BoardPosition.y + direction;

        if (targetY >= 0 && targetY < 8)
        {
            if (board[BoardPosition.x, targetY] == null)
            {
                validMoves.Add(new Vector2Int(BoardPosition.x, targetY));

                if (isFirstMove)
                {
                    int doubleY = BoardPosition.y + (2 * direction);
                    if (doubleY >= 0 && doubleY < 8 && board[BoardPosition.x, doubleY] == null)
                    {
                        validMoves.Add(new Vector2Int(BoardPosition.x, doubleY));
                    }
                }
            }

            int[] xOffsets = { -1, 1 };

            foreach (int xOffset in xOffsets)
            {
                int targetX = BoardPosition.x + xOffset;

                if (targetX >= 0 && targetX < 8)
                {
                    ChessPiece diagonalPiece = board[targetX, targetY];
                    if (diagonalPiece != null && diagonalPiece.IsWhite != IsWhite)
                    {
                        validMoves.Add(new Vector2Int(targetX, targetY));
                    }


                    if (moveHistory != null && moveHistory.Count > 0)
                    {
                        ChessPiece sidePiece = board[targetX, BoardPosition.y];
                        Move lastMove = moveHistory[moveHistory.Count - 1];

                        if (sidePiece != null && 
                            sidePiece.PieceType == ChessPieceType.Pawn && 
                            sidePiece.IsWhite != IsWhite)
                        {
                            if (lastMove.Piece == sidePiece)
                            {
                                if (lastMove.From.y == BoardPosition.y + (direction * 2))
                                {
                                    validMoves.Add(new Vector2Int(targetX, targetY));
                                }
                            }
                        }
                    }
                }
            }
        }

        return validMoves;
    }

    public override bool isValidMove(Vector2Int from, Vector2Int to, ChessPiece[,] board, int direction)
    {
        return false;
    }    
}
