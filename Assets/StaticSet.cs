using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticSet : PSet
{
   

    public bool hasSimultaneous = false;
    public StaticSet[] simultaneousConnections;
    private bool isReady = false; //Simul


    private void OnTriggerEnter(Collider other)
    {
        connectedObj = other.gameObject;
        if (connectedObj.GetComponent<StaticReceive>() != null && pairValue == connectedObj.GetComponent<StaticReceive>().pairValue && IsAligned(connectedObj))
        {
            //GameObject connectedObjRef = connectedObj.GetComponent<StaticReceive>().UpdateParentRef();
            //thisParentRef = thisParent.GetComponent<ObjectConnections>().GetReferenceObject();
            if (hasSimultaneous)
            {
                isReady = true;
                gameObject.GetComponent<MeshRenderer>().material = Resources.Load("OverlapMaterial", typeof(Material)) as Material;
                if (SimulReady())
                {
                    transform.rotation = connectedObj.transform.rotation;
                    thisParent.transform.rotation = transform.rotation * rotOffset;
                    offset = transform.position - thisParent.transform.position;
                    thisParent.transform.position = connectedObj.transform.position - offset - thisParent.transform.rotation * (new Vector3(xOffset, yOffset, zOffset));
                    FixedJoint parentFixedJoint = thisParent.AddComponent<FixedJoint>();
                    parentFixedJoint.connectedBody = connectedObj.transform.parent.gameObject.GetComponent<Rigidbody>();

                    List<StaticSet> simulList = new List<StaticSet>();
                    foreach (StaticSet ele in simultaneousConnections)
                    {
                        simulList.Add(ele);
                    }
                    foreach (StaticSet ele in simulList)
                    {
                        ele.SimulUpdate();
                    }

                    Destroy(connectedObj);
                    Destroy(gameObject);
                }
            }
            else
            {
                    
                transform.rotation = connectedObj.transform.rotation;
                thisParent.transform.rotation = transform.rotation * rotOffset;
                thisParent.transform.RotateAround(connectedObj.transform.position, connectedObj.transform.right, xRotation);
                thisParent.transform.RotateAround(connectedObj.transform.position, connectedObj.transform.up, yRotation);
                thisParent.transform.RotateAround(connectedObj.transform.position, connectedObj.transform.forward, zRotation);

                offset = transform.position - thisParent.transform.position;
                thisParent.transform.position = connectedObj.transform.position - offset - thisParent.transform.rotation * (new Vector3(xOffset, yOffset, zOffset));
                FixedJoint parentFixedJoint = thisParent.AddComponent<FixedJoint>();
                parentFixedJoint.connectedBody = connectedObj.transform.parent.gameObject.GetComponent<Rigidbody>();
                if (updateParent)
                {
                    thisParent.transform.parent = connectedObj.transform.parent.transform.parent;
                }
                GameObject.Find("Fingers").GetComponent<FingerState>().Release();
                Destroy(connectedObj);
                Destroy(gameObject);
            }

            //thisParent.GetComponent<ObjectConnections>().SetupParent(connectedObj.transform.parent.gameObject);
            //if (thisParentRef.GetComponent<ObjectConnections>().isGrabbed)
            //{
            //    GameObject.Find("Fingers").GetComponent<FingerState>().SetHeldObjCombine(thisParentRef.GetComponent<ObjectConnections>().GetReferenceObject());
            //}
            //else if (connectedObjRef.GetComponent<ObjectConnections>().isGrabbed)
            //{
            //    GameObject.Find("Fingers").GetComponent<FingerState>().SetHeldObjCombine(connectedObjRef.GetComponent<ObjectConnections>().GetReferenceObject());
            //}

        }
    }
    private void OnTriggerExit(Collider other)
    {
        connectedObj = null;
        if(hasSimultaneous)
        {
            gameObject.GetComponent<MeshRenderer>().material = Resources.Load("JointMaterial", typeof(Material)) as Material;
            Random.InitState(pairValue);
            Material myMaterial = gameObject.GetComponent<MeshRenderer>().material;
            myMaterial.color = Color.HSVToRGB(Random.Range(0f, 1f), 1, 1);
            Color changeA = myMaterial.color;
            changeA.a = 0.5f;
            myMaterial.color = changeA;
            isReady = false;
        }
    }


    private bool SimulReady()
    {
        foreach(StaticSet ele in simultaneousConnections)
        {
            if(!ele.isReady)
            {
                return false;
            }
        }
        return true;
    }

    private void SimulUpdate()
    {

        transform.rotation = connectedObj.transform.rotation;
        thisParent.transform.rotation = transform.rotation * rotOffset;
        offset = transform.position - thisParent.transform.position;
        thisParent.transform.position = connectedObj.transform.position - offset - thisParent.transform.rotation * (new Vector3(xOffset, yOffset, zOffset));
        FixedJoint parentFixedJoint = thisParent.AddComponent<FixedJoint>();
        parentFixedJoint.connectedBody = connectedObj.transform.parent.gameObject.GetComponent<Rigidbody>();
        Destroy(connectedObj);
        Destroy(gameObject);
    }
}
