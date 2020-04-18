using UnityEngine;

public class MouseControls : MonoBehaviour
{
    public float pickupDistance = 2.0f;
    public float holdDistance = 2.0f;
    public LayerMask pickupMask;

    private bool isHoldingSomething = false;
    private GameObject heldObject;

    private Camera camera;

    float rotationSpeed = 10f;
    float heldObjectHeight = 0f;

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
                float XaxisRotation = Input.GetAxis("Mouse X") * rotationSpeed;
                float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSpeed;

                heldObject.transform.Rotate(Vector3.down, XaxisRotation);
                heldObject.transform.Rotate(Vector3.right, YaxisRotation);
            }
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
                heldObjectHeight += Input.GetAxis("Mouse ScrollWheel");
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
