using System.Collections.Generic;
using UnityEngine;

public abstract class  ChessPiece : MonoBehaviour
{    
    public Vector2Int BoardPosition;
    [SerializeField] private MeshRenderer meshRenderer;
    public bool IsWhite;
    public bool isFirstMove;
    public ChessPieceType PieceType;
    
    public virtual void Init(Vector2Int position, bool isWhite, Material pieceMaterial, ChessPieceType pieceType)
    {
        PieceType=pieceType;
        BoardPosition=position;
        IsWhite=isWhite;
        meshRenderer.material = pieceMaterial;
        isFirstMove = true;
    }

    public ChessPieceType getPieceType()
    {
        return PieceType;
    }

    public void SetPosition(Vector2Int position)
    {
        BoardPosition=position;
    }

    public abstract List<Vector2Int> GetValidMoves(ChessPiece[,] board, List<Move> moveHistory);

    public abstract bool isValidMove(Vector2Int from,Vector2Int to, ChessPiece[,] board, int direction);
}
