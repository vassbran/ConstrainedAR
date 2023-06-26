using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSet : MonoBehaviour
{

    public int pairValue = 0;
    protected GameObject thisParent;
    protected GameObject thisParentRef;
    protected GameObject connectedObj;

    protected Vector3 offset;
    protected Quaternion rotOffset;

    public float initialOrientationRestriction = 15;

    //User set positional offset
    public float xOffset = 0;
    public float yOffset = 0;
    public float zOffset = 0;

    //User set rotation
    public float xRotation = 0;
    public float yRotation = 0;
    public float zRotation = 0;

    public bool updateParent = false;

    void Start()
    {
        SetColor();
        thisParent = transform.parent.gameObject;
        rotOffset = Quaternion.Inverse(transform.rotation) * thisParent.transform.rotation;
    }

    protected bool IsAligned(GameObject connectedObj)
    {
        return (Vector3.Angle(transform.forward, connectedObj.transform.forward) < initialOrientationRestriction && Vector3.Angle(transform.up, connectedObj.transform.up) < initialOrientationRestriction);
    }

    protected void SetColor()
    {
        Random.InitState(pairValue);
        Material myMaterial = gameObject.GetComponent<MeshRenderer>().material;
        myMaterial.color = Color.HSVToRGB(Random.Range(0f, 1f), 1, 1);
        Color changeA = myMaterial.color;
        changeA.a = 0.5f;
        myMaterial.color = changeA;

    }
}
