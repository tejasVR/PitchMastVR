using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour {

    public SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;

    public LaserPointer laserPointer;

	// Use this for initialization
	void Start () {
        laserPointer.LaserOff();
	}
	
	// Update is called once per frame
	void Update () {

        device = SteamVR_Controller.Input((int)trackedObj.index);

        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            laserPointer.LaserOn();
        } else
        {
            laserPointer.LaserOff();
        }
		
	}
}
