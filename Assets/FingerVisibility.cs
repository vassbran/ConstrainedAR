using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerVisibility : MonoBehaviour
{
    // Start is called before the first frame update
    public void makeInvisible()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
    }

    public void makeVisible()
    {
        gameObject.GetComponent<Renderer>().enabled = true;
    }
}
