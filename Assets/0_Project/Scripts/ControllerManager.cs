using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour {

    public SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;

    public LaserPointer laserPointer;

    public DrawLineManager dLine;

    // For Drawing on screen
    private MeshLineRenderer currLine;
    public Material lmat;
    private int numClicks = 0;
    private float width = .05f;
    public Vector3 screenHitPoint;


    // Use this for initialization
    void Start () {
        laserPointer.LaserOff();
	}
	
	// Update is called once per frame
	void Update () {

        device = SteamVR_Controller.Input((int)trackedObj.index);

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            laserPointer.LaserOn();

            if (laserPointer.collidingWithScreen)
            {
                dLine.DrawInitialize();
            }
        }

        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            dLine.DrawLine();
        }
        
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            laserPointer.LaserOff();

            dLine.DrawStop();
        }
    }
}
