using UnityEngine;
using System.Collections.Generic;

public class PieceMount : MonoBehaviour
{
    public int pieceID;
    public List<int> pieceIDs;
    private HashSet<Piece> connectedPieces;
    private bool otherPieceConnected = false;
    private Piece thisPiece;

    void Awake()
    {
        thisPiece = this.GetComponent<Piece>();
        if (pieceIDs.Count == 0) // I wanted to add multiple mount points but didn't want to break already set up puzzles
        {
            pieceIDs.Add(pieceID);
        }
        connectedPieces = new HashSet<Piece>();
    }

    void OnTriggerEnter(Collider thing)
    {
        var otherPiece = thing.GetComponent<Piece>();
        if (otherPiece == null || !thisPiece.IsMounted) return;
        foreach (int id in pieceIDs)
        {
            if (!otherPieceConnected && otherPiece.pieceID == id && MouseControls.Instance.isHoldingSomething)
            {


                otherPiece.IsMounted = true;
                otherPiece.transform.SetParent(this.transform);
                // otherPiece.transform.localPosition = Vector3.up;
                otherPiece.GetComponent<Rigidbody>().isKinematic = true;
                otherPiece.GetComponent<GlowObjectCmd>().enabled = false;
                otherPiece.gameObject.layer = 0;
                PuzzleChecker.Instance.CheckPuzzleComplete();
                connectedPieces.Add(otherPiece);
            }
        }
        if (connectedPieces.Count == pieceIDs.Count)
        {
            otherPieceConnected = true;
            print("All pieces connected ");
        }
    }
}
