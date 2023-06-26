using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public GameObject mainController = null;
    public TextMeshProUGUI currentMode = null;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updateModeText();
    }

    private void updateModeText()
    {
        if(mainController.GetComponent<MainControllerScript>().InGroundTargetMode())
        {
            currentMode.text = "Ground Target Mode";
        }
        else
        {
            currentMode.text = "Image Target Mode";
        }
    }
}
