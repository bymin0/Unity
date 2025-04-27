using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Domino : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasFallen = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    public void Activate()
    {
        if (rb != null)
            rb.isKinematic = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Domino other = collision.collider.GetComponent<Domino>();
        if (other != null && other.rb.isKinematic)
            other.Activate();
    }
}
