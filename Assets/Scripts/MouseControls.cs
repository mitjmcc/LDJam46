using UnityEngine;
using Luminosity.IO;
using UnityEngine.SceneManagement;

public class MouseControls : MonoBehaviour
{
    public float pickupDistance = 2.0f;
    public float holdDistance = 2.0f;
    public LayerMask pickupMask;

    public bool isHoldingSomething = false;
    private GameObject heldObject;

    private Camera camera;

    float rotationSpeed = 10f;
    float heldObjectHeight = 0.01f;

    private static MouseControls _instance;
    private static object m_Lock = new object();

    public static MouseControls Instance
    {
        get
        {

            lock (m_Lock)
            {
                if (_instance == null)
                {
                    // Search for existing instance.
                    _instance = (MouseControls)FindObjectOfType(typeof(MouseControls));

                    // Create new instance if one doesn't already exist.
                    if (_instance == null)
                    {
                        // Need to create a new GameObject to attach the singleton to.
                        var singletonObject = new GameObject();
                        _instance = singletonObject.AddComponent<MouseControls>();
                        singletonObject.name = typeof(MouseControls).ToString() + " (Singleton)";

                        // Make instance persistent.
                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return _instance;
            }
        }
    }

    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        if(!isHoldingSomething)
        {
            GrabObject();
        }
        else
        {
            MoveObject();

            if(Input.GetMouseButton(1))
            {
                float XaxisRotation = InputManager.GetAxis("Mouse X") * rotationSpeed;
                float YaxisRotation = InputManager.GetAxis("Mouse Y") * rotationSpeed;

                heldObject.transform.Rotate(Vector3.down, XaxisRotation);
                heldObject.transform.Rotate(Vector3.right, YaxisRotation);
            }
        }

        if (InputManager.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void GrabObject()
    {
        RaycastHit hitInfo;
        Ray mousePosition = camera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(mousePosition, out hitInfo, pickupDistance, pickupMask))
        {
            if(Input.GetMouseButtonDown(0))
            {
                hitInfo.collider.GetComponent<Rigidbody>().isKinematic = true;
                heldObject = hitInfo.collider.gameObject;
                isHoldingSomething = true;
            }
        }
    }

    private void MoveObject()
    {
        float planeY = 0;
        Plane plane = new Plane(Vector3.up, Vector3.up * planeY); // ground plane

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Input.GetMouseButton(1))
        {
            float distance; // the distance from the ray origin to the ray intersection of the plane
            if(plane.Raycast(ray, out distance))
            {
                if (heldObjectHeight >= 0)
                    heldObjectHeight += InputManager.GetAxis("Mouse ScrollWheel");
                else
                    heldObjectHeight = 0;
                heldObject.transform.position = ray.GetPoint(distance) - camera.transform.forward * 1.3f + Vector3.up * heldObjectHeight; // distance along the ray
            }

            if(Input.GetMouseButtonDown(0))
            {
                if (!heldObject.GetComponent<Piece>().IsMounted)
                {
                    heldObject.GetComponent<Rigidbody>().isKinematic = false;
                }
                heldObject = null;
                isHoldingSomething = false;
                heldObjectHeight = 0f;
            }
        }
    }
}
