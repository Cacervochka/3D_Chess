using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public enum ChessPieceType
{
    None = 0,
    Pawn,
    Knight,
    Bishop,
    Rook,
    Queen,
    King
}
[System.Serializable]


public struct Move
{
    public ChessPiece Piece { get; private set; }
    public Vector2Int From { get; private set; }
    public Vector2Int To { get; private set; }

    public Move(ChessPiece piece, Vector2Int from, Vector2Int to)
    {
        Piece = piece;
        From = from;
        To = to;
    }

}

public class ChessBoard : MonoBehaviour
{
    [SerializeField] private GameObject pawnPrefab;
    [SerializeField] private GameObject rookPrefab;
    [SerializeField] private GameObject bishopPrefab;
    [SerializeField] private GameObject queenPrefab;
    [SerializeField] private GameObject kingPrefab;
    [SerializeField] private GameObject knightPrefab;

    [SerializeField] private Material whiteMaterial;
    [SerializeField] private Material blackMaterial;

    public List<Move> moveHistory;

    private ChessPiece[,] board = new ChessPiece[8, 8];

    private ChessPiece selectedPiece;
    
    void Start()
    {
        SpawnAllPieces();
        moveHistory = new List<Move>();
    }

    void SpawnAllPieces()
    {
        for (int x = 0; x < 8; x++)
        {
            SpawnSinglePiece(pawnPrefab, new Vector2Int(x, 1), true, ChessPieceType.Pawn);
        }

        for (int x = 0; x < 8; x++)
        {
            SpawnSinglePiece(pawnPrefab, new Vector2Int(x, 6), false, ChessPieceType.Pawn);
        }

        SpawnSinglePiece(bishopPrefab, new Vector2Int(2,0), true, ChessPieceType.Bishop);
        SpawnSinglePiece(bishopPrefab, new Vector2Int(5,0), true, ChessPieceType.Bishop);
        SpawnSinglePiece(bishopPrefab, new Vector2Int(2,7), false, ChessPieceType.Bishop);
        SpawnSinglePiece(bishopPrefab, new Vector2Int(5,7), false, ChessPieceType.Bishop);

        SpawnSinglePiece(knightPrefab, new Vector2Int(1,0), true, ChessPieceType.Knight);
        SpawnSinglePiece(knightPrefab, new Vector2Int(6,0), true, ChessPieceType.Knight);
        SpawnSinglePiece(knightPrefab, new Vector2Int(1,7), false, ChessPieceType.Knight);
        SpawnSinglePiece(knightPrefab, new Vector2Int(6,7), false, ChessPieceType.Knight);

        SpawnSinglePiece(rookPrefab, new Vector2Int(0,0), true, ChessPieceType.Rook);
        SpawnSinglePiece(rookPrefab, new Vector2Int(7,0), true, ChessPieceType.Rook);
        SpawnSinglePiece(rookPrefab, new Vector2Int(0,7), false, ChessPieceType.Rook);
        SpawnSinglePiece(rookPrefab, new Vector2Int(7,7), false, ChessPieceType.Rook);

        SpawnSinglePiece(queenPrefab, new Vector2Int(4,0), true, ChessPieceType.Queen);
        SpawnSinglePiece(queenPrefab, new Vector2Int(4,7), false, ChessPieceType.Queen);

        SpawnSinglePiece(kingPrefab, new Vector2Int(3,0), true, ChessPieceType.King);
        SpawnSinglePiece(kingPrefab, new Vector2Int(3,7), false, ChessPieceType.King);
    }

    void SpawnSinglePiece(GameObject objectPrefab,Vector2Int gridPos,bool isWhite, ChessPieceType type)    
    {   
        
        Quaternion rotation;
        if(type!=ChessPieceType.Knight){
            if(isWhite){
                rotation=Quaternion.Euler(-90, 0, 90);
            }else
            {
                rotation=Quaternion.Euler(-90, 0, -90);
            }
        }
        else
        {
            if(isWhite){
                rotation=Quaternion.Euler(-90, 0, 0);
            }else
            {
                rotation=Quaternion.Euler(-90, 0, 180);
            }
        }
        Vector3 worldPos = new Vector3((float)gridPos.x+0.5f, 0.0f, (float)gridPos.y+0.5f);

        GameObject pieceObject = Instantiate(objectPrefab, worldPos, rotation, transform);
        ChessPiece piece = pieceObject.GetComponent<ChessPiece>();

        Material matToApply = isWhite ? whiteMaterial : blackMaterial;
        piece.Init(gridPos, isWhite, matToApply,type);

        board[gridPos.x, gridPos.y] = piece;
    }

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Debug.Log($"Mouse position: {mousePosition}");
            HandleClick(mousePosition);
        }
    }

    private void HandleClick(Vector2 mousePosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        
        
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector2Int gridPos = GetGridPosition(hitInfo.point);
            Debug.Log($"Hit info point: {hitInfo.point}");

            if (selectedPiece == null)
            {
                SelectPiece(gridPos);
            }
            else
            {
                TryMovePiece(selectedPiece, gridPos);
            }
        }
    }

    private void SelectPiece(Vector2Int pos)
    {
        ChessPiece piece = board[pos.x, pos.y];

        if (piece != null)
        {
            selectedPiece = piece;
            Debug.Log($"Выбрана фигура {piece.PieceType} на позиция {pos}");
            
            //Make possible moves visible
        }
    }

    private void TryMovePiece(ChessPiece piece, Vector2Int targetPos)
    {
        List<Vector2Int> validMoves = piece.GetValidMoves(board,moveHistory);

        if (validMoves.Contains(targetPos))
        {
            MakeMove(piece, targetPos);
        }
        else
        {
            Debug.Log("Недопустимый ход!");
        }

        selectedPiece = null;
    }

    private void MakeMove(ChessPiece piece, Vector2Int newPos)
    {
        Vector2Int oldPos = piece.BoardPosition;

        if(board[newPos.x,newPos.y]!=null)
        {
            ChessPiece pieceVisibility=board[newPos.x,newPos.y];
            pieceVisibility.GetComponent<Renderer>().enabled = false;        
        }

        board[oldPos.x, oldPos.y] = null;
        board[newPos.x, newPos.y] = piece;

        piece.transform.position = new Vector3(newPos.x + 0.5f, 0f, newPos.y + 0.5f);

        moveHistory.Add(new Move(piece, oldPos, newPos));

        piece.SetPosition(newPos);
    }

    private Vector2Int GetGridPosition(Vector3 worldPoint)
    {
        int x = Mathf.FloorToInt(worldPoint.x);

        int y = Mathf.FloorToInt(worldPoint.z);
        Debug.Log($"X:{x}  Y: {y}");
        return new Vector2Int(Mathf.Clamp(x, 0, 7), Mathf.Clamp(y, 0, 7));
    }
}
