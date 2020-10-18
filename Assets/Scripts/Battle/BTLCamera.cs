using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLCamera : MonoBehaviour
{
    public Camera mainCamera;
    public Vector3 defaultPosition;

    private void Awake()
    {
        transform.position = defaultPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
