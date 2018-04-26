using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PopulateGridTest : MonoBehaviour {

    public GameObject cubePrefab;

    public int numberToCreate;

	// Use this for initialization
	void Start () {

        Populate();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Populate()
    {
        GameObject newObj;

        for (int i = 0; i < numberToCreate; i++)
        {
            newObj = (GameObject)Instantiate(cubePrefab, transform);
            newObj.GetComponent<Image>().color = Random.ColorHSV();


        }
    }
}
