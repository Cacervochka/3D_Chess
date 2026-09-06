using System.Collections.Generic;
using UnityEngine;

public abstract class  ChessPiece : MonoBehaviour
{    
    public Vector2Int BoardPosition;
    [SerializeField] private MeshRenderer meshRenderer;
    public bool IsWhite;

    public virtual void Init(Vector2Int position, bool isWhite, Material pieceMaterial)
    {
        BoardPosition=position;
        IsWhite=isWhite;
        meshRenderer.material = pieceMaterial;
    }

    public abstract List<Vector2Int> GetValidMoves(ChessPiece[,] board);
}
