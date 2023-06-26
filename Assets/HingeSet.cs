using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeSet : PSet
{

    //Make the arguement that hinge should not be considered as part of the whole

    public enum rotAxis
    {
        xAxis,
        yAxis,
        zAxis
    };

    //Hinge Variables
    public rotAxis axisOfRotation;
    public bool limitRotation = false;
    public float maxRotation = 0;
    public float minRotation = 0;

   

    private void OnTriggerEnter(Collider other)
    {
        connectedObj = other.gameObject;
        if (connectedObj.GetComponent<HingeReceive>() != null && pairValue == connectedObj.GetComponent<HingeReceive>().pairValue && IsAligned(connectedObj))
        {
             transform.rotation = connectedObj.transform.rotation;
                thisParent.transform.rotation = transform.rotation * rotOffset;
                thisParent.transform.RotateAround(connectedObj.transform.position, connectedObj.transform.right, xRotation);
                thisParent.transform.RotateAround(connectedObj.transform.position, connectedObj.transform.up, yRotation);
                thisParent.transform.RotateAround(connectedObj.transform.position, connectedObj.transform.forward, zRotation);

                offset = transform.position - thisParent.transform.position;
                thisParent.transform.position = connectedObj.transform.position - offset - thisParent.transform.rotation * (new Vector3(xOffset, yOffset, zOffset));

            HingeJoint parentHingeJoint = thisParent.AddComponent<HingeJoint>();
            parentHingeJoint.connectedBody = connectedObj.transform.parent.gameObject.GetComponent<Rigidbody>();
            parentHingeJoint.anchor = transform.localPosition;
            if (axisOfRotation == rotAxis.xAxis)
            {
                parentHingeJoint.axis = new Vector3(1, 0, 0);
            }
            else if (axisOfRotation == rotAxis.yAxis)
            {
                parentHingeJoint.axis = new Vector3(0, 1, 0);
            }
            else
            {
                parentHingeJoint.axis = new Vector3(0, 0, 1);
            }

            if (limitRotation)
            {
                parentHingeJoint.useLimits = true;
                JointLimits limits = parentHingeJoint.limits;
                limits.min = minRotation;
                limits.max = maxRotation;
                parentHingeJoint.limits = limits;
            }


            if (updateParent)
            {
                thisParent.transform.parent = connectedObj.transform.parent.transform.parent;
            }
            GameObject.Find("Fingers").GetComponent<FingerState>().Release();
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
