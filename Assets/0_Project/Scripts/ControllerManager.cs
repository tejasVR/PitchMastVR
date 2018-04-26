using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour {

    public SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;
    public Vector2 touchpad; // links Vive touchpad to Vector2


    public LaserPointer laserPointer;
    public DrawLineManager dLine;
    public PresentationManager presentationManager;

    public bool fileBrowserOpen;

    // For Drawing on screen
    public Vector3 screenHitPoint;


    // Use this for initialization
    void Start () {
        //laserPointer.LaserOff();
	}
	
	// Update is called once per frame
	void Update () {

        if (trackedObj.gameObject.activeInHierarchy)
        {
            device = SteamVR_Controller.Input((int)trackedObj.index);

            // For Presentating
            if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
            {
                touchpad = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);

                if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
                {
                    if (touchpad.x < 0)
                    {
                        presentationManager.SlidePrevious();
                    }
                    else
                    {
                        presentationManager.SlideNext();
                    }
                }
            }

            if (device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
            {
                presentationManager.PresentImages();
                
            }

            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
            {
                presentationManager.OpenExplorer();
            }


            // For Drawing
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
                if (laserPointer.collidingWithScreen)
                {
                    dLine.DrawLine(screenHitPoint);
                }
            }

            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                laserPointer.LaserOff();

                dLine.DrawStop(transform.position);
            }
        }
        
    }
}
