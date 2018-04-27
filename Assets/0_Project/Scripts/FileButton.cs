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

    private void OnTriggerEnter(Collider other)
    {
        print("On TRigger Enter!");
        rend.material.color = Color.yellow;
    }

    private void OnTriggerExit(Collider other)
    {
        print("On TRigger Exit!");

        fileBrowser.ShowFullDirectory(filePath);
    }
}
