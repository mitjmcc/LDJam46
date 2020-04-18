using UnityEngine;

public class MouseControls : MonoBehaviour
{
    public float pickupDistance = 2.0f;
    public float holdDistance = 2.0f;
    public LayerMask pickupMask;

    private bool isHoldingSomething = false;
    private GameObject heldObject;

    private Camera camera;

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
                hitInfo.collider.transform.position = newPosition;

                heldObject = hitInfo.collider.gameObject;
                isHoldingSomething = true;
            }
        }
    }

    private void MoveObject()
    {
        Vector3 newPosition = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, holdDistance));
        // newPosition += camera.transform.forward * holdDistance;
        heldObject.transform.position = newPosition;

        if(Input.GetMouseButtonUp(0))
        {
            heldObject.GetComponent<Rigidbody>().isKinematic = false;
            heldObject = null;
            isHoldingSomething = false;
        }
    }
}
