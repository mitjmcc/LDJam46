using UnityEngine;

public class MouseControls : MonoBehaviour
{
    public float pickupDistance = 2.0f;
    public float holdDistance = 2.0f;
    public LayerMask pickupMask;

    private bool isHoldingSomething = false;
    private GameObject heldObject;

    private Camera camera;

    private Vector3 xyplane = new Vector3(1f, 0f, 1f);

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

                Vector3 newPosition = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, holdDistance));
                newPosition += camera.transform.forward * holdDistance;

                hitInfo.collider.transform.position = Vector3.ProjectOnPlane(newPosition, Vector3.up);

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

        float distance; // the distance from the ray origin to the ray intersection of the plane
        if(plane.Raycast(ray, out distance))
        {
            heldObject.transform.position = ray.GetPoint(distance) - camera.transform.forward * 1.3f; // distance along the ray
        }

        if(Input.GetMouseButtonUp(0))
        {
            if (!heldObject.GetComponent<Piece>().IsMounted)
            {
                heldObject.GetComponent<Rigidbody>().isKinematic = false;
            }
            heldObject = null;
            isHoldingSomething = false;
        }
    }
}
