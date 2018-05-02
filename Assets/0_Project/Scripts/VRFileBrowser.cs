using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

/*
 * This class creates a 3D file browser for the user to search through through PC directory, and select a file they want to open
 * 
 */
public class VRFileBrowser : MonoBehaviour {

    #region Singelon
    public static VRFileBrowser Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    #region Class Variables
    PresentationManager presentationManager;

    // Creates a list of button objects to track when the VR browser is open
    public List<GameObject> objectList = new List<GameObject>();


    public string defaultPath;
    public int fileNameNumber;
    public int numberOfFiles;
    public GameObject content;
    public GameObject buttonPrefab;

    // For spacing of how each button in the VR browser is laid out
    public int filesPerRow;
    public float xSpacing;
    public float ySpacing;
    public Transform fileStart;

    public bool directoryOpen;
    #endregion

    void Start () {
        // Define beginning path as the overhead project folder
        if (defaultPath == null)
        {
            defaultPath = "C:/Users/";
        }

        presentationManager = PresentationManager.Instance;
    }
	
    public void ShowFullDirectory(string path)
    {
        //Sets up new objects and default path to grab files/folders
        GameObject newObj;
        HideDirectory();

        // Create a DirectoryInfo variable linked to the path of an external directory
        DirectoryInfo dir = new DirectoryInfo(path);

        // If the path is already a ".pdf" file, skip creating the 3D browser, and lets convert the file into a presentation
        if (dir.Extension == ".pdf")
        {
            presentationManager.GetNewPDF(path);
            HideDirectory();
        }
        else
        {
            directoryOpen = true;

            // Create an array of FileInfo based on all the files in the DirectoryInfo
            FileInfo[] files = dir.GetFiles("*.pdf");
            DirectoryInfo[] folders = dir.GetDirectories("*");

            // Initialize the row and column count for the VR browser          
            int row = 0;
            int column = 0;
         
            // Lets grab all the folders in the directory and assigna clickable button to that directory path
            foreach (DirectoryInfo f in folders)
            {
                // Place the first button according to the row and column count
                fileStart.transform.localPosition = new Vector3((column * xSpacing), -(row * ySpacing), 0);

                // Assign a newObj to the button placement, add it to a list, assign its text as the path of that button, and stick it to a content parent object
                newObj = Instantiate(buttonPrefab, fileStart.transform.position, Quaternion.identity);
                objectList.Add(newObj);
                newObj.GetComponentInChildren<TextMeshPro>().text = f.Name;
                newObj.GetComponent<FileButton>().filePath = f.FullName;
                newObj.transform.parent = content.transform;
               
                row++;

                if (row == filesPerRow)
                {
                    row = 0;
                    column++;
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
    }

    public void HideDirectory()
    {
        foreach (GameObject obj in objectList)
        {
            Destroy(obj.gameObject);
        }

        directoryOpen = false;
    }
}
