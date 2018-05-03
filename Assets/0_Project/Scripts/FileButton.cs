using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileButton : MonoBehaviour {

    #region VARIABLES
    private Renderer rend;
    public string filePath;
    VRFileBrowser fileBrowser;
    #endregion

    // Use this for initialization
    void Start () {
        rend = GetComponent<Renderer>();
        fileBrowser = VRFileBrowser.Instance;
	}

    #region ON TRIGGER FUNCTIONS

    // If the button is in contact with a controller, turn yellow
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Controller")
        {
            rend.material.color = Color.yellow;
        }
    }

    // If the controller on the button is pulled away (i.e., pressed), open that directory/file
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Controller")
        {
            fileBrowser.ShowFullDirectory(filePath);
        }
    }
    #endregion
}
