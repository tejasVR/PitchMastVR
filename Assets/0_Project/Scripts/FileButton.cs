using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileButton : MonoBehaviour {

    private Renderer rend;
    public string filePath;

    VRFileBrowser fileBrowser;

	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
        fileBrowser = VRFileBrowser.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // If the button is in contact with a controller, turn yellow
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Controller")
        {
            rend.material.color = Color.yellow;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Controller")
        {
            fileBrowser.ShowFullDirectory(filePath);
        }
    }
}
