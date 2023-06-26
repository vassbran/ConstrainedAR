using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Vuforia;

public class FingerState : MonoBehaviour
{
    //State Variables
    private bool emptyState = true;
    private bool transRotState = false;
    private bool scaleState = false;
    private bool interactState = false;

    //Fingers and held Object
    private GameObject heldObj = null;
    public GameObject Pointer = null;
    public GameObject Thumb = null;

    //Position and Rotation variables
    private Vector3 fingerMidPoint;
    private Vector3 fingerAxis;
    private Vector3 posOffset;
    private Quaternion rotOffset;
    private Quaternion rotUpdate;
    private bool centerGrab = false;

    //Scaling Variables
    private float baseScaleDist;
    private Vector3 baseScale;
    public float scaleRate = 1;
    public float interactionSpeed = 0.4f;
    //Release object variables
    public double releaseStartDistPercent = .25;
    public double releaseEndDistPercent = .25;
    private double initialGrabDist;
    private double releaseGrabDist;
    private bool minDistReached = false;

    //Interaction variables
    private Vector3 dir;
    private GameObject trackedFinger = null;


    // Update is called once per frame
    void Update()
    {
        //FingerInterpolate(); //Update fingers if marker detection is lost

        if (emptyState)
        {

        }
        else if (transRotState)
        { 
            UpdateHeldObjPosRot();
            CheckAndRelease();
        }
        else if (scaleState)
        {
            UpdateScale();
        }
        else if(interactState)
        {
            CheckRelativePos();
        }
        
    }
    private void FixedUpdate()
    {
        if (interactState)
            {
                UpdateInteractObjPos();
            }
    }



    void UpdateHeldObjPosRot()
    {
        //Get vector pointing from one finger to another
        fingerAxis.x = Thumb.transform.position.x - Pointer.transform.position.x;
        fingerAxis.y = Thumb.transform.position.y - Pointer.transform.position.y;
        fingerAxis.z = Thumb.transform.position.z - Pointer.transform.position.z;
        rotUpdate = Quaternion.LookRotation(fingerAxis, Thumb.transform.up);
        heldObj.transform.rotation = rotUpdate * rotOffset;

        //Get midpoint of the two fingers
        fingerMidPoint.x = Thumb.transform.position.x + (Pointer.transform.position.x - Thumb.transform.position.x) / 2;
        fingerMidPoint.y = Thumb.transform.position.y + (Pointer.transform.position.y - Thumb.transform.position.y) / 2;
        fingerMidPoint.z = Thumb.transform.position.z + (Pointer.transform.position.z - Thumb.transform.position.z) / 2;

        if (centerGrab)
        {
            heldObj.transform.position = fingerMidPoint;
        }
        else
        {
            heldObj.transform.position = fingerMidPoint - rotUpdate * posOffset;
        }
    }

    void UpdateInteractObjPos()
    {
        heldObj.GetComponent<Rigidbody>().velocity = dir;
    }

    void CheckRelativePos()
    {
        dir = (trackedFinger.transform.position - heldObj.transform.position).normalized * interactionSpeed;
    }

    public void SetHeldObj(GameObject newHeldObj)
    {
        if (heldObj == null && emptyState)
        {
            heldObj = newHeldObj;
            if(heldObj.GetComponent<Rigidbody>() != null)
            {
                heldObj.GetComponent<Rigidbody>().useGravity = false;
            }
            if(heldObj.CompareTag("RefEmpty"))
            {
                foreach (Transform child in heldObj.transform)
                {
                    if(child.gameObject.GetComponent<Rigidbody>() != null)
                    {
                        child.gameObject.GetComponent<Rigidbody>().useGravity = false;
                    } 
                }
            }
            
            Pointer.GetComponent<GrabScript>().enabled = false;
            Thumb.GetComponent<GrabScript>().enabled = false;

            GetOffsets(heldObj);
            emptyState = false;
            SetTransRotMode();
        }
    }
    public void SetHeldObj(GameObject newHeldObj, GameObject newHeldObjReference)
    {
        if (heldObj == null && emptyState)
        {
            heldObj = newHeldObjReference;
            if (heldObj.GetComponent<Rigidbody>() != null)
            {
                heldObj.GetComponent<Rigidbody>().useGravity = false;
            }
            if (heldObj.CompareTag("RefEmpty"))
            {
                foreach (Transform child in heldObj.transform)
                {
                    if (child.gameObject.GetComponent<Rigidbody>() != null)
                    {
                        child.gameObject.GetComponent<Rigidbody>().useGravity = false;
                    }
                }
            }

            Pointer.GetComponent<GrabScript>().enabled = false;
            Thumb.GetComponent<GrabScript>().enabled = false;

            GetOffsets(newHeldObjReference);
            emptyState = false;
            SetTransRotMode();
        }
    }

    public void SetInteract(GameObject newInteractObj, GameObject interactFinger)
    {
        if(heldObj == null && emptyState)
        {
            heldObj = newInteractObj;
            trackedFinger = interactFinger;

            Pointer.GetComponent<GrabScript>().enabled = false;
            Thumb.GetComponent<GrabScript>().enabled = false;

            emptyState = false;
            interactState = true;
            dir = (trackedFinger.transform.position - heldObj.transform.position).normalized * interactionSpeed;
        } 
    }
    void GetOffsets(GameObject heldObj)
    {
        fingerMidPoint.x = Thumb.transform.position.x + (Pointer.transform.position.x - Thumb.transform.position.x) / 2;
        fingerMidPoint.y = Thumb.transform.position.y + (Pointer.transform.position.y - Thumb.transform.position.y) / 2;
        fingerMidPoint.z = Thumb.transform.position.z + (Pointer.transform.position.z - Thumb.transform.position.z) / 2;
        fingerAxis.x = Thumb.transform.position.x - Pointer.transform.position.x;
        fingerAxis.y = Thumb.transform.position.y - Pointer.transform.position.y;
        fingerAxis.z = Thumb.transform.position.z - Pointer.transform.position.z;
        rotUpdate = Quaternion.LookRotation(fingerAxis, Thumb.transform.up);
        rotOffset = Quaternion.Inverse(rotUpdate) * heldObj.transform.rotation;
        posOffset = Quaternion.Inverse(rotUpdate) * (fingerMidPoint - heldObj.transform.position);

    }


    void CheckAndRelease()
    {
        if (Vector3.Distance(Thumb.transform.position, Pointer.transform.position) < releaseStartDistPercent * initialGrabDist && !minDistReached)
        {
            minDistReached = true;
            Debug.Log("minDistReached");
        }

        if (minDistReached && Vector3.Distance(Thumb.transform.position, Pointer.transform.position) > releaseEndDistPercent * initialGrabDist)
        {

            Release();
        }

    }

    public void Release()
    {
        heldObj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        heldObj = null;
        Pointer.GetComponent<GrabScript>().enabled = true;
        Thumb.GetComponent<GrabScript>().enabled = true;
        minDistReached = false;
        transRotState = false;
        interactState = false;
        scaleState = false;
        emptyState = true;
    }

    void UpdateScale()
    {
        float currentDist = Vector3.Distance(Thumb.transform.position, Pointer.transform.position);
        heldObj.transform.localScale = baseScale + (baseScale * scaleRate * (currentDist - baseScaleDist));


        //Get vector pointing from one finger to another
        fingerAxis.x = Thumb.transform.position.x - Pointer.transform.position.x;
        fingerAxis.y = Thumb.transform.position.y - Pointer.transform.position.y;
        fingerAxis.z = Thumb.transform.position.z - Pointer.transform.position.z;
        rotUpdate = Quaternion.LookRotation(fingerAxis, Thumb.transform.up);

        //Get midpoint of the two fingers
        fingerMidPoint.x = Thumb.transform.position.x + (Pointer.transform.position.x - Thumb.transform.position.x) / 2;
        fingerMidPoint.y = Thumb.transform.position.y + (Pointer.transform.position.y - Thumb.transform.position.y) / 2;
        fingerMidPoint.z = Thumb.transform.position.z + (Pointer.transform.position.z - Thumb.transform.position.z) / 2;

        if (centerGrab)
        {
            heldObj.transform.position = fingerMidPoint;
        }
        else
        {
            heldObj.transform.position = fingerMidPoint - rotUpdate * posOffset;
        }
    }

    public bool IsHoldingObj()
    {
        return !emptyState;
    }

    public void SetScaleMode()
    {
        if (!emptyState && !interactState)
        {
            transRotState = false;
            scaleState = true;
            minDistReached = false;
            baseScaleDist = Vector3.Distance(Thumb.transform.position, Pointer.transform.position);
            baseScale = heldObj.transform.localScale;
        }
    }

    public void SetTransRotMode()
    {
        if (!emptyState && !interactState)
        {
            scaleState = false;
            transRotState = true;
            initialGrabDist = Vector3.Distance(Thumb.transform.position, Pointer.transform.position);
            releaseGrabDist = initialGrabDist;
        }
    }
 

    public void SetGrabType()
    {
        if (centerGrab)
        {
            centerGrab = false;
        }
        else
        {
            centerGrab = true;
        }
    }

   
}
