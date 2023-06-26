using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseTouch : MonoBehaviour
{
    public GameObject FingerController = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Touch touch;
        if (Input.touchCount < 1 || ((touch = Input.GetTouch(0)).phase != TouchPhase.Began))
        {
            return;
        }
        FingerController.GetComponent<FingerState>().Release();
    }
}
