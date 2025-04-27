using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterManager : MonoBehaviour
{
    public GameObject starterPrefab;
    public Transform startPoint;
    public Vector3 pushDirection = Vector3.right;
    public float pushForce = 5f;
    public float spawnHeight = 1f;

    public void SpawnStarter()
    {
        Vector3 spawnPos = startPoint.position;
        spawnPos.y = spawnHeight;

        GameObject starter = Instantiate(starterPrefab, spawnPos, Quaternion.identity);
        Rigidbody rb = starter.GetComponent<Rigidbody>();
        if (rb != null)
            rb.AddForce(pushDirection.normalized * pushForce, ForceMode.Impulse);
    }
}
