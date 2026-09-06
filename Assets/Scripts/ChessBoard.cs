using UnityEngine;

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

    private ChessPiece[,] board = new ChessPiece[8, 8];
    
    void Start()
    {
        SpawnAllPieces();
    }

    void SpawnAllPieces()
    {
        for (int x = 0; x < 8; x++)
        {
            SpawnSinglePiece(pawnPrefab, new Vector2Int(x, 1), true);
        }

        for (int x = 0; x < 8; x++)
        {
            SpawnSinglePiece(pawnPrefab, new Vector2Int(x, 6), false);
        }
    }

    void SpawnSinglePiece(GameObject objectPrefab,Vector2Int gridPos,bool isWhite)    
    {   
        
        Quaternion rotation;
        if(isWhite){
            rotation=Quaternion.Euler(-90, 0, 90);
        }else
        {
            rotation=Quaternion.Euler(-90, 0, -90);
        }
        Vector3 worldPos = new Vector3((float)gridPos.x+0.5f, 0.0f, (float)gridPos.y+0.5f);

        GameObject pieceObject = Instantiate(objectPrefab, worldPos, rotation, transform);
        ChessPiece piece = pieceObject.GetComponent<ChessPiece>();

        Material matToApply = isWhite ? whiteMaterial : blackMaterial;
        piece.Init(gridPos, isWhite, matToApply);

        board[gridPos.x, gridPos.y] = piece;
    }
}
