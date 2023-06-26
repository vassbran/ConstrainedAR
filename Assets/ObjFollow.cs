using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjFollow : MonoBehaviour
{
    public GameObject interactionObject = null;
    Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        direction = (this.gameObject.transform.position - interactionObject.transform.position).normalized * 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        direction = (this.gameObject.transform.position - interactionObject.transform.position).normalized * 2.0f;
    }
    private void FixedUpdate()
    {
        interactionObject.GetComponent<Rigidbody>().velocity = direction;
    }
}
