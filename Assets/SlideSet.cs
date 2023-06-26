using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideSet : PSet
{
    //Going to make the arguement that I don't want the sliders to be a part of the whole, and instead treat it independent


    public enum slideAxis
    {
        xAxis,
        yAxis,
        zAxis
    };

    public slideAxis slidingAxis;
    public bool limit = false;
    public float slideLimit = 0;
   

    private void OnTriggerEnter(Collider other)
    {
        connectedObj = other.gameObject;
        if(connectedObj.GetComponent<SlideReceive>() != null && pairValue == connectedObj.GetComponent<SlideReceive>().pairValue && IsAligned(connectedObj))
        {



            transform.rotation = connectedObj.transform.rotation;
            thisParent.transform.rotation = transform.rotation * rotOffset;
            thisParent.transform.RotateAround(connectedObj.transform.position, connectedObj.transform.right, xRotation);
            thisParent.transform.RotateAround(connectedObj.transform.position, connectedObj.transform.up, yRotation);
            thisParent.transform.RotateAround(connectedObj.transform.position, connectedObj.transform.forward, zRotation);

            offset = transform.position - thisParent.transform.position;
            thisParent.transform.position = connectedObj.transform.position - offset - thisParent.transform.rotation * (new Vector3(xOffset, yOffset, zOffset));
            ConfigurableJoint parentConfigurableJoint = thisParent.AddComponent<ConfigurableJoint>();
            parentConfigurableJoint.connectedBody = connectedObj.transform.parent.gameObject.GetComponent<Rigidbody>();
            parentConfigurableJoint.anchor = transform.localPosition;

            parentConfigurableJoint.angularXMotion = ConfigurableJointMotion.Locked;
            parentConfigurableJoint.angularYMotion = ConfigurableJointMotion.Locked;
            parentConfigurableJoint.angularZMotion = ConfigurableJointMotion.Locked;

            if(slidingAxis == slideAxis.xAxis)
            {
                if(limit)
                {
                    parentConfigurableJoint.xMotion = ConfigurableJointMotion.Limited;
                }
                parentConfigurableJoint.yMotion = ConfigurableJointMotion.Locked;
                parentConfigurableJoint.zMotion = ConfigurableJointMotion.Locked;
            }
            else if (slidingAxis ==slideAxis.yAxis)
            {
                if (limit)
                {
                    parentConfigurableJoint.yMotion = ConfigurableJointMotion.Limited;
                }
                parentConfigurableJoint.xMotion = ConfigurableJointMotion.Locked;
                parentConfigurableJoint.zMotion = ConfigurableJointMotion.Locked;
            }
            else
            {
                if (limit)
                {
                    parentConfigurableJoint.zMotion = ConfigurableJointMotion.Limited;
                }
                parentConfigurableJoint.xMotion = ConfigurableJointMotion.Locked;
                parentConfigurableJoint.yMotion = ConfigurableJointMotion.Locked;
            }
            if (limit)
            {
                SoftJointLimit sjl = new SoftJointLimit();
                sjl.limit = slideLimit;
                parentConfigurableJoint.linearLimit = sjl;
            }

            if(updateParent)
            {
                thisParent.transform.parent = connectedObj.transform.parent.transform.parent;
            }
            GameObject.Find("Fingers").GetComponent<FingerState>().Release();
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
   
}
