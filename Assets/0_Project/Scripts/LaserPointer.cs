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

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider.gameObject.tag == "Screen")
            {
                lr.SetPosition(1, hit.point);
                controllerMananger.screenHitPoint = hit.point;
                collidingWithScreen = true;
            }
        } else
        {
            lr.SetPosition(1, transform.forward * 5);
            collidingWithScreen = false;
        }
		
	}

    public void LaserOff()
    {
        lr.enabled = false;
    }

    public void LaserOn()
    {
        lr.enabled = true;
    }

}
