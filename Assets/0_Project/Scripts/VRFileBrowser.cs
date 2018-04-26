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
    public int fileNameNumber;
    public int numberOfFiles;
    public GameObject contentUI;
    public GameObject buttonPrefab;

    public int filesPerRow;
    public float xSpacing;
    public float ySpacing;

    public Transform transformPoint;


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
            numberOfFiles++;
        }


        //foreach (FileInfo f in info)
        {
            print("foreach function called");
            int row = 0;
            for(int i = 0; i < numberOfFiles; i++)
            {
                transformPoint.transform.position = new Vector3(transformPoint.transform.position.x + i * xSpacing, transformPoint.transform.position.y + row * ySpacing);

                newObj = Instantiate(buttonPrefab, transformPoint.transform);
                //newObj.GetComponent<Image>().color = Random.ColorHSV();
                newObj.GetComponentInChildren<Text>().text = f.Name;

                fileNames.Add(f.ToString());
                print(f.Name);

                if (i == filesPerRow - 1)
                {
                    row++;
                    //i = 0;
                }
            }

            
        }
    }
}
