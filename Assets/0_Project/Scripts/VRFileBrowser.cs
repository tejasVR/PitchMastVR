using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class VRFileBrowser : MonoBehaviour {

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singelon
    public static VRFileBrowser Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;
    public Vector2 touchpad;

    public ScrollRect scrollRect;
    PresentationManager presentationManager;
   

    public Dictionary<string, Queue<GameObject>> poolDictionary;

    public string defaultPath;
    public List<Pool> fileNamesList = new List<Pool>();

    public List<GameObject> objectList = new List<GameObject>();
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
        //poolDictionary = new Dictionary<string, Queue<GameObject>>();

        /*foreach(Pool file in fileNamesList)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for(int  i = 0; i < file.size; i++)
            {
                GameObject obj = Instantiate(file.prefab, content.transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(file.tag, objectPool);
        }*/


        // Define beginning path as the overhead project folder
        defaultPath = "C:/Users/";
        presentationManager = PresentationManager.Instance;
        // Show the directory on start
        //ShowFullDirectory(defaultPath);

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


    public void ShowFullDirectory(string path)
    {
        // Create a new object called newObj
        GameObject newObj;
        //print("destroying current objects");
        HideDirectory();

        

        //objectToSpawn.SetActive(true);
        //objectToSpawn
        // Print myPath so we know that we are viewing the correct directory
        //print("My Path Dir: " + myPath);

        // Create a DirectoryInfo object linked to myPath
        //print("getting new path");
        DirectoryInfo dir = new DirectoryInfo(path);

        if (dir.Extension == ".pdf")
        {
            presentationManager.GetNewPDF(path);
            HideDirectory();
        }


        // Create an array of FileInfo based on all the files in the DirectoryInfo
        FileInfo[] files = dir.GetFiles("*.pdf");
        DirectoryInfo[] folders = dir.GetDirectories("*");

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
            foreach(DirectoryInfo f in folders)
            {
                //for (int i = 0; i < numberOfFiles; i++)
                {
                    //GameObject objectToSpawn = poolDictionary["file"].Dequeue();


                    // Set the transform of where each prefab should be instantiated
                    fileStart.transform.localPosition = new Vector3((column * xSpacing), -(row * ySpacing), 0);
                    newObj = Instantiate(buttonPrefab, fileStart.transform.position, Quaternion.identity);
                    objectList.Add(newObj);

                    newObj.GetComponentInChildren<TextMeshPro>().text = f.Name;

                    //newObj.transform.position = fileStart.transform.position;
                    //print("Y Spacing is now: " + (row * ySpacing));


                    // Instantiate the newObj at the designated transform
                    newObj.transform.parent = content.transform;
                    //newObj.GetComponent<Image>().color = Random.ColorHSV();

                    // The text of the newObj should be the file name

                    //fileNames.Add(f.ToString());
                    //print(f.Name);

                    newObj.GetComponent<FileButton>().filePath = f.FullName;
                    row++;

                    if (row == filesPerRow)
                    {
                        row = 0;
                        column++;
                        //i = 0;
                    }
                }
            }

            foreach (FileInfo f in files)
            {
                //for (int i = 0; i < numberOfFiles; i++)
                {
                    //GameObject objectToSpawn = poolDictionary["file"].Dequeue();

                    // Set the transform of where each prefab should be instantiated
                    fileStart.transform.localPosition = new Vector3((column * xSpacing), -(row * ySpacing), 0);
                    newObj = Instantiate(buttonPrefab, fileStart.transform.position, Quaternion.identity);
                    objectList.Add(newObj);

                    newObj.GetComponentInChildren<TextMeshPro>().text = f.Name;

                    //newObj.transform.position = fileStart.transform.position;

                    //print("Y Spacing is now: " + (row * ySpacing));

                    // Instantiate the newObj at the designated transform
                    newObj.transform.parent = content.transform;
                    //newObj.GetComponent<Image>().color = Random.ColorHSV();

                    // The text of the newObj should be the file name
                    //objectToSpawn.GetComponentInChildren<TextMeshPro>().text = f.Name;

                    //fileNames.Add(f.ToString());

                    newObj.GetComponent<FileButton>().filePath = f.FullName;


                    // FileInfo.FullName == the full directory path to that file
                    //print(f.FullName);

                    row++;

                    if (row == filesPerRow)
                    {
                        row = 0;
                        column++;
                        //i = 0;
                    }
                }
            }





        }
    }

    public void HideDirectory()
    {
        foreach (GameObject obj in objectList)
        {
            Destroy(obj.gameObject);
        }
    }
}
