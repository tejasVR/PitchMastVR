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
         
            // Lets grab all the FOLDERS in the directory and assign a clickable button to that directory path
            foreach (DirectoryInfo f in folders)
            {
                PlaceButton(row, column, f.Name, f.FullName);
               
                row++;

                // If there are too many buttons in a row, start a new column
                if (row == filesPerRow)
                {
                    row = 0;
                    column++;
                }
                
            }

            // Lets grab all the FILES in the directory and assign a clickable button to that directory path
            foreach (FileInfo f in files)
            {
                PlaceButton(row, column, f.Name, f.FullName);

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

    public void HideDirectory()
    {
        foreach (GameObject obj in objectList)
        {
            Destroy(obj.gameObject);
        }

        directoryOpen = false;
    }

    public void PlaceButton(int row, int column, string fileName, string filePath)
    {
        GameObject button;
        // Place the first button according to the row and column count
        fileStart.transform.localPosition = new Vector3((column * xSpacing), -(row * ySpacing), 0);

        // Assign a newObj to the button placement, add it to a list, assign its text as the path of that button, and stick it to a content parent object
        button = Instantiate(buttonPrefab, fileStart.transform.position, Quaternion.identity);
        objectList.Add(button);
        button.GetComponentInChildren<TextMeshPro>().text = fileName;
        button.GetComponent<FileButton>().filePath = filePath;
        button.transform.parent = content.transform;
    }
}
