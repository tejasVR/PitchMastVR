using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : MonoBehaviour {

    private LineRenderer lr;
    public bool collidingWithScreen;
    public SteamVR_TrackedObject trackedObj;

    public ControllerManager controllerMananger;

	void Awake () {

        lr = GetComponent<LineRenderer>();

        
	}
	
	void Update () {
        lr.SetPosition(0, transform.position);
        Debug.DrawRay(transform.position, trackedObj.transform.forward, Color.green);

        RaycastHit hit;
        int layerMask = 1 << 9;
        //layerMask = ~layerMask;
        if (Physics.Raycast(transform.position, trackedObj.transform.forward, out hit, Mathf.Infinity, layerMask))
        {
            //if (hit.collider.gameObject.tag == "Screen")
            {
                lr.SetPosition(1, hit.point);
                //lr.SetPosition(1, new Vector3(0, 0, hit.distance));
                controllerMananger.screenHitPoint = lr.GetPosition(1);
                collidingWithScreen = true;
                //print("calling raycast");
            }
            
        }else
            {
                //lr.SetPosition(0, transform.position);

                //lr.SetPosition(1, new Vector3(0, 0, 10));
                //lr.SetPosition(1, transform.position);
                //print("calling else");
                collidingWithScreen = false;
                lr.SetPosition(1, transform.position);

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
