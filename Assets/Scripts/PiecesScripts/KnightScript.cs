using UnityEngine;
using System.Collections.Generic;

public class KnightScript : ChessPiece
{
    public override List<Vector2Int> GetValidMoves(ChessPiece[,] board, 
        List<Move> moveHistory)
    {

        List<Vector2Int> validMoves=new List<Vector2Int>();

        Vector2Int[] xyOffsets = new Vector2Int[]
        {
            new Vector2Int(2,1),
            new Vector2Int(1,2),
            new Vector2Int(-2,1),
            new Vector2Int(-1,2),
            new Vector2Int(2,-1),
            new Vector2Int(1,-2),
            new Vector2Int(-2,-1),
            new Vector2Int(-1,-2)
        };

        foreach (Vector2Int xyOffset in xyOffsets)
        {
            Vector2Int targetPos=BoardPosition+xyOffset;
            
            if(targetPos.x<8 && targetPos.x>=0 && targetPos.y<8 && targetPos.y>=0)
            {   
                if(board[targetPos.x,targetPos.y]==null){
                    validMoves.Add(targetPos);
                    targetPos+=xyOffset;
                }
                else
                {
                    ChessPiece sidePiece = board[targetPos.x, targetPos.y];
                    if(sidePiece.IsWhite!=IsWhite)
                    {
                        validMoves.Add(targetPos);
                    }
                }
            }
        }

        return validMoves;
    }

    
    public override bool isValidMove(Vector2Int from, Vector2Int to, 
        ChessPiece[,] board, int direction)
    {
        return false;
    }   
}
