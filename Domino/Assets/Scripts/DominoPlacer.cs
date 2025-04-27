using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DominoPlacer : MonoBehaviour
{
    public GameObject dominoPrefab;
    public float gridSize = .8f;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void OnPlaceDomino(InputAction.CallbackContext context)
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (!IsInsideGround(hit.point)) return;

            Vector3 snappedPos = GetSnappedPosition(hit.point);
            Instantiate(dominoPrefab, snappedPos, Quaternion.identity);
        }
    }

    Vector3 GetSnappedPosition(Vector3 rawpos)
    {
        float x = Mathf.Round(rawpos.x / gridSize) * gridSize;
        float z = Mathf.Round(rawpos.z / gridSize) * gridSize;

        return new Vector3(x, 0, z);
    }

    bool IsInsideGround(Vector3 pos)
    {
        float halfSize = 25f;
        return pos.x >= -halfSize && pos.x <= halfSize && pos.z >= -halfSize && pos.z <= halfSize;
    }
}
