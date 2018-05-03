using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour {

    #region VARIABLES
    public SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;
    public Vector2 touchpad; // links Vive touchpad to Vector2

    
    DrawLineManager drawLineManager;
    PresentationManager presentationManager;
    VRFileBrowser vrFileBrowser;

    public LaserPointer laserPointer;
    public Vector3 screenHitPoint;
    public bool fileBrowserOpen;

    #endregion

    void Start () {
        // Initialize all utility script instances, so we don't have to refer to actual game objects
        drawLineManager = DrawLineManager.Instance;
        vrFileBrowser = VRFileBrowser.Instance;
        presentationManager = PresentationManager.Instance;
	}
	
	void Update () {

        if (trackedObj.gameObject.activeInHierarchy)
        {
            device = SteamVR_Controller.Input((int)trackedObj.index);

            // If we have touched our touchpad
            if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
            {
                touchpad = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);

                // if we have pressed the left side of our touchpad, go to the previous slide
                if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
                {
                    if (touchpad.x < 0)
                    {
                        presentationManager.SlidePrevious();
                    }
                    // if we have pressed the right side, go to the next slide
                    else
                    {
                        presentationManager.SlideNext();
                    }
                }
            }

            // If we have pressed the application menu, load/reload the presentation
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
            {
                presentationManager.PresentImages();
            }

            // If we have pressed the grip buttons, bring up the VR browser
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
            {
                vrFileBrowser.ShowFullDirectory(vrFileBrowser.defaultPath);
            }


            // If we have pressed the trigger button
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                // turn on the laser pointer
                laserPointer.LaserOn();

                // If the laser is colliding with the slide canvas, initialize the drawing
                if (laserPointer.collidingWithScreen)
                {
                    drawLineManager.DrawInitialize();
                }
            }

            // If we keep pressing on the trigger button,
            if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
            {
                // keep drawing!
                if (laserPointer.collidingWithScreen)
                {
                    drawLineManager.DrawLine(screenHitPoint);
                }
            }

            // If we are not pressing the trigger button anymore, save the drawing
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                laserPointer.LaserOff();

                drawLineManager.DrawStop(transform.position, presentationManager.SaveDraw());
            }
        }
        
    }
}
