using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class VRFileBrowser : MonoBehaviour {

    public SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;
    public Vector2 touchpad;

    public ScrollRect scrollRect;


    string myPath;
    public List<string> fileNames = new List<string>();
    public GameObject contentUI;
    public GameObject buttonPrefab;


	// Use this for initialization
	void Start () {
        myPath = Application.dataPath + "/0_Project/Textures";
        FindFile();

    }
	
	// Update is called once per frame
	void Update () {

        if (trackedObj.gameObject.activeInHierarchy)
        {
            device = SteamVR_Controller.Input((int)trackedObj.index);

            touchpad = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);

            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && touchpad.y > 0)
            {
                scrollRect.verticalNormalizedPosition = 1f;
            }

            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && touchpad.y < 0)
            {
                scrollRect.verticalNormalizedPosition = 0f;
            }
        }
		
	}


    public void FindFile()
    {
        GameObject newObj;

        print("My Path Dir: " + myPath);
        DirectoryInfo dir = new DirectoryInfo(myPath);
        FileInfo[] info = dir.GetFiles("*");
        foreach (FileInfo f in info)
        {
            newObj = (GameObject)Instantiate(buttonPrefab, contentUI.transform);
            //newObj.GetComponent<Image>().color = Random.ColorHSV();
            newObj.GetComponentInChildren<Text>().text = f.Name;

            fileNames.Add(f.ToString());
            print(f.Name);
        }
    }
}
