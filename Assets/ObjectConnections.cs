using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectConnections : MonoBehaviour
{
    public GameObject referenceObject = null;
    public bool isGrabbed = false;
    // Start is called before the first frame update
   
    public void SetupParent(GameObject otherObj)
    {
        ObjectConnections otherCon = otherObj.GetComponent<ObjectConnections>();

        if (referenceObject != null && otherCon.referenceObject != null)
        {
            if (referenceObject != otherCon.referenceObject)
            {
                GameObject oldRef = otherCon.referenceObject;
                List<Transform> childrenList = new List<Transform>();
                foreach (Transform child in otherCon.referenceObject.transform)
                {
                    childrenList.Add(child);
                }
                foreach (Transform child in childrenList)
                {
                    child.parent = referenceObject.transform;
                    child.gameObject.GetComponent<ObjectConnections>().referenceObject = referenceObject;
                }

                Destroy(oldRef);
            }

        }
        else if (referenceObject != null && otherCon.referenceObject == null)
        {
            otherCon.referenceObject = referenceObject;
            otherObj.transform.parent = referenceObject.transform;
        }
        else if (referenceObject == null && otherCon.referenceObject != null)
        {
            referenceObject = otherCon.referenceObject;
            gameObject.transform.parent = referenceObject.transform;
        }
        else
        {
            referenceObject = new GameObject();
            otherCon.referenceObject = referenceObject;
            referenceObject.tag = "RefEmpty";
            referenceObject.AddComponent<ObjectConnections>();
            referenceObject.GetComponent<ObjectConnections>().referenceObject = referenceObject; //Empty References should know that they are the reference object of the compound object
            gameObject.transform.parent = referenceObject.transform;
            otherObj.transform.parent = referenceObject.transform;
        }
    }
    public GameObject GetReferenceObject()
    {
        if(referenceObject == null)
        {
            return gameObject;
        }
        else
        {
            return referenceObject;
        }
    }
}
