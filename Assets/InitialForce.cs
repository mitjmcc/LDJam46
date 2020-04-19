using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialForce : MonoBehaviour
{
    public Vector3 force;

    void Awake()
    {
        GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
    }
}
