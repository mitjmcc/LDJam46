using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleChecker : MonoBehaviour
{
    [SerializeField]
    private List<Piece> pieces;

    private int currentScene;

    private static PuzzleChecker _instance;
    private static object m_Lock = new object();

    public static PuzzleChecker Instance
    {
        get
        {

            lock (m_Lock)
            {
                if (_instance == null)
                {
                    // Search for existing instance.
                    _instance = (PuzzleChecker)FindObjectOfType(typeof(PuzzleChecker));

                    // Create new instance if one doesn't already exist.
                    if (_instance == null)
                    {
                        // Need to create a new GameObject to attach the singleton to.
                        var singletonObject = new GameObject();
                        _instance = singletonObject.AddComponent<PuzzleChecker>();
                        singletonObject.name = typeof(PuzzleChecker).ToString() + " (Singleton)";

                        // Make instance persistent.
                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return _instance;
            }
        }
    }

    void Awake()
    {
        currentScene = int.Parse(SceneManager.GetActiveScene().name);
    }

    public void CheckPuzzleComplete()
    {
        if (_instance != null)
        {
            int completePieces = 0;
            foreach(Piece p in pieces)
            {
                if (p.IsMounted)
                {
                    completePieces++;
                }
            }
            if (completePieces == pieces.Count)
            {
                print("Puzzle Complete!");
                SceneManager.LoadScene((currentScene + 1).ToString());
            }
        }
    }
}
