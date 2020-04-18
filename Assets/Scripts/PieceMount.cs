using UnityEngine;

public class PieceMount : MonoBehaviour
{
    public int pieceID;

    private bool pieceConnected = false;

    void OnTriggerEnter(Collider thing)
    {
        var piece = thing.GetComponent<Piece>();
        if (piece == null) return;
        if (!pieceConnected && piece.pieceID == pieceID)
        {
            print("Hey that's the right piece");
            pieceConnected = true;
            piece.transform.SetParent(this.transform);
            piece.transform.localPosition = Vector3.up;
            piece.GetComponent<Rigidbody>().isKinematic = true;
            piece.gameObject.layer = 0;
        }
    }
}
