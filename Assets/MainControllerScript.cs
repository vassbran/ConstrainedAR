using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainControllerScript : MonoBehaviour
{
    private bool imageMode = true;
    private bool groundMode = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SwapMode()
    {
        if(imageMode) //Swapping to Ground Target Mode
        {
            foreach(Transform child in this.transform)
            {
                if(child.CompareTag("ImageTargets"))
                {
                    child.gameObject.SetActive(false);
                }
                else if(child.CompareTag("GroundTarget"))
                {
                    child.gameObject.SetActive(true);
                }
            }
            imageMode = false;
            groundMode = true;
        }
        else if (groundMode) //Swapping to Image Target Mode
        {
            foreach (Transform child in this.transform)
            {
                if (child.CompareTag("ImageTargets"))
                {
                    child.gameObject.SetActive(true);
                }
                else if (child.CompareTag("GroundTarget"))
                {
                    child.gameObject.SetActive(false);
                }
            }
            groundMode = false;
            imageMode = true;
        }
    }

    public bool InGroundTargetMode()
    {
        return groundMode;
    }


    
}
