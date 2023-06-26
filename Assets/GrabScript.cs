using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabScript : MonoBehaviour
{

    public GameObject otherFinger = null;
    public GameObject controller = null;
    private GameObject touchingObj = null;
    private FingerState fs = null;

    private void Start()
    {
        fs = controller.GetComponent<FingerState>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Grabable") && touchingObj == null && !fs.IsHoldingObj())
        {
            touchingObj = other.gameObject;
            if (otherFinger.GetComponent<GrabScript>().TouchingSameObj(touchingObj))
            {
                GameObject temp = touchingObj;
                touchingObj = null;
                otherFinger.GetComponent<GrabScript>().touchingObj = null;
                controller.GetComponent<FingerState>().SetHeldObj(temp);
            }
        }
        else if(other.gameObject.CompareTag("Interactable") && touchingObj == null && !fs.IsHoldingObj())
        {
            touchingObj = other.gameObject;
            fs.SetInteract(touchingObj, gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == touchingObj)
        {
            touchingObj = null;
        }
    }

    public bool TouchingSameObj(GameObject otherFingerTouchObj) //Check if touching the same object
    {
        
        return touchingObj == otherFingerTouchObj;
    }

}