using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour {

    public SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;

    public LaserPointer laserPointer;

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
                GameObject go = new GameObject("line");
                go.AddComponent<MeshFilter>();
                go.AddComponent<MeshRenderer>();
                currLine = go.AddComponent<MeshLineRenderer>();

                currLine.lmat = new Material(lmat);
                currLine.setWidth(width);
            }
        }

        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            currLine.AddPoint(screenHitPoint);
            numClicks++;
        }
        else
        {
        }

        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            device.TriggerHapticPulse(500);
            laserPointer.LaserOff();

            numClicks = 0;
            currLine = null;
        }
    }
}
