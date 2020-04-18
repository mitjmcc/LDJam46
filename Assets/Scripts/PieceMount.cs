using UnityEngine;

public class PieceMount : MonoBehaviour
{
    public int pieceID;
    private bool otherPieceConnected = false;
    private Piece thisPiece;

    void Awake()
    {
        thisPiece = this.GetComponent<Piece>();
    }

    void OnTriggerEnter(Collider thing)
    {
        var otherPiece = thing.GetComponent<Piece>();
        if (otherPiece == null || !thisPiece.IsMounted) return;
        if (!otherPieceConnected && otherPiece.pieceID == pieceID)
        {
            print("Hey that's the right piece");
            otherPieceConnected = true;
            otherPiece.IsMounted = true;
            otherPiece.transform.SetParent(this.transform);
            otherPiece.transform.localPosition = Vector3.up;
            otherPiece.GetComponent<Rigidbody>().isKinematic = true;
            otherPiece.gameObject.layer = 0;
        }
    }
}
