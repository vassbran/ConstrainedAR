using System.Collections;
using System.Collections.Generic;
using Vuforia;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    public void TurnOnGravity()
    {
        foreach(Transform child in transform)
        {
            if(child.gameObject.GetComponent<Rigidbody>() != null)
            {
                child.GetComponent<Rigidbody>().useGravity = true;
            }
        }
        
    }
    public void TurnOffGravity()
    {
        var rigidBody = GetComponentInChildren<Rigidbody>(true);

        rigidBody.useGravity = false;
    }
}