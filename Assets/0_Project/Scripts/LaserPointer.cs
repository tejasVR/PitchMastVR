using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : MonoBehaviour {

    private LineRenderer lr;
    public bool collidingWithScreen;

    public ControllerManager controllerMananger;

	// Use this for initialization
	void Awake () {

        lr = GetComponent<LineRenderer>();

        
	}
	
	// Update is called once per frame
	void Update () {
        lr.SetPosition(0, transform.position);
        //lr.SetPosition(1, controllerMananger.transform.forward * 5);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider.gameObject.tag == "Screen")
            {
                lr.SetPosition(1, hit.point);
                //lr.SetPosition(1, new Vector3(0, 0, hit.distance));
                controllerMananger.screenHitPoint = lr.GetPosition(1);
                collidingWithScreen = true;
            }
        } else
        {
            //lr.SetPosition(0, transform.position);

            //lr.SetPosition(1, new Vector3(0, 0, 10));
            lr.SetPosition(1, transform.position);
            collidingWithScreen = false;
        }
		
	}

    public void LaserOff()
    {
        //lr.enabled = false;
    }

    public void LaserOn()
    {
        //lr.enabled = true;
    }

}
