using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class VRFileBrowser : MonoBehaviour {

    public SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;
    public Vector2 touchpad;

    public ScrollRect scrollRect;


    string myPath;
    public List<string> fileNames = new List<string>();
    public int fileNameNumber;
    public int numberOfFiles;
    public GameObject content;
    public GameObject buttonPrefab;

    public int filesPerRow;
    public float xSpacing;
    public float ySpacing;

    public Transform fileStart;


	// Use this for initialization
	void Start () {
        // Define beginning path as the overhead project folder
        myPath = Application.dataPath + "/0_Project";

        // Show the directory on start
        ShowFileDirectory();

    }
	
	// Update is called once per frame
	void Update () {

        if (trackedObj.gameObject.activeInHierarchy)
        {
            device = SteamVR_Controller.Input((int)trackedObj.index);

            touchpad = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);

            /*if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && touchpad.y > 0)
            {
                scrollRect.verticalNormalizedPosition = 1f;
            }

            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && touchpad.y < 0)
            {
                scrollRect.verticalNormalizedPosition = 0f;
            }*/
        }
		
	}


    public void ShowFileDirectory()
    {
        // Create a new object called newObj
        GameObject newObj;

        // Print myPath so we know that we are viewing the correct directory
        //print("My Path Dir: " + myPath);

        // Create a DirectoryInfo object linked to myPath
        DirectoryInfo dir = new DirectoryInfo(myPath);

        // Create an array of FileInfo based on all the files in the DirectoryInfo
        //FileInfo[] info = dir.GetDirectories("*");//dir.GetFiles("*");
        DirectoryInfo[] allFiles = dir.GetDirectories("*");

        // Count the number of files/folders in FileInfo
        /*foreach (FileInfo f in info)
        {
            numberOfFiles++;
        }*/

        int row = 0;
        int column = 0;
        //foreach (FileInfo f in info)
        {
            //print("foreach function called");

            //foreach (FileInfo f in info)
            foreach(DirectoryInfo f in allFiles)
            {
                //for (int i = 0; i < numberOfFiles; i++)
                {
                    // Set the transform of where each prefab should be instantiated
                    fileStart.transform.position = new Vector3((column * xSpacing), (row * ySpacing), 0);

                    //print("Y Spacing is now: " + (row * ySpacing));


                    // Instantiate the newObj at the designated transform
                    newObj = Instantiate(buttonPrefab, fileStart.transform.position, Quaternion.identity);
                    newObj.transform.parent = content.transform;
                    //newObj.GetComponent<Image>().color = Random.ColorHSV();

                    // The text of the newObj should be the file name
                    newObj.GetComponentInChildren<TextMeshPro>().text = f.Name;

                    fileNames.Add(f.ToString());
                    print(f.Name);

                    row++;

                    /*if (i == filesPerRow - 1)
                    {
                        row++;
                        column = 0;
                        //i = 0;
                    }*/
                }
            }
            

            
        }
    }
}
