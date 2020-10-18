using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoShadows : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Thanks to https://www.youtube.com/watch?v=tdfoj6aVWao
        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null)
            Debug.Log("Renderer is empty");
        GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        GetComponent<Renderer>().receiveShadows = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
