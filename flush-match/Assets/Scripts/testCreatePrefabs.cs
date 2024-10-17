using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testCreatePrefabs : MonoBehaviour
{
    public GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject obj = (GameObject) Instantiate(prefab);
            obj.transform.parent = this.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
