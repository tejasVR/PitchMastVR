using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class VRFileBrowser : MonoBehaviour {

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
