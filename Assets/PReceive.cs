using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PReceive : MonoBehaviour
{
    public int pairValue = 0;
    protected GameObject thisParentRef;
    // Start is called before the first frame update
    void Start()
    {
        SetColor();
       // thisParentRef = transform.parent.gameObject.GetComponent<ObjectConnections>().GetReferenceObject();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    //public GameObject UpdateParentRef()
    //{
    //    thisParentRef = transform.parent.gameObject.GetComponent<ObjectConnections>().GetReferenceObject();
    //    return thisParentRef;
    //}
}
