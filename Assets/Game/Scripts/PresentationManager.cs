using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PresentationManager : MonoBehaviour {

    // Basic tracking for Vive wands
    public SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;
    public Vector2 touchpad; // links Vive touchpad to Vector2

    //public int totalNumberOfSlides;

    public RawImage currentSlide;
    public int slideNumber = 0;
    public Texture2D[] slides;

	void Start () {
        CollectImages();
        //slides = new Texture[totalNumberOfSlides];

        // Set the starting slide as the first slides in the slides array
        currentSlide.GetComponent<RawImage>().texture = slides[0];
	}
	
	void Update () {

        // Update the Steam VR controllers every frame
        device = SteamVR_Controller.Input((int)trackedObj.index);

        // if we are touching the touchpad, get the coordinates of the Vector2
        if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
        {
            touchpad = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);

            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
            {
                if (touchpad.x < 0)
                {
                    SlidePrevious();
                }
                else
                {
                    SlideNext();
                }
            }
        }
    }

    public void CollectImages()
    {
        // Scans the folder for Texture2D files, and then imports all files into the slides array to be used in a persentation format
        slides = Resources.LoadAll<Texture2D>("Presentation_01");       
    }

    // Function for going to the previous slide
    public void SlidePrevious()
    {
        //decrease the slide number count
        slideNumber--;

        // if the slides number is less than zero, go to the last slide available
        if (slideNumber < 0)
        {
            slideNumber = slides.Length - 1;
        }

        // change the current slide image to reflect new slide
        currentSlide.GetComponent<RawImage>().texture = slides[slideNumber];
    }

    public void SlideNext()
    {
        // increase the slide number count
        slideNumber++;

        // if the slides number is greater than the total number of slides, go to the first slide available
        if (slideNumber > slides.Length - 1)
        {
            slideNumber = 0;
        }

        // change the current slide image to reflect new slide
        currentSlide.GetComponent<RawImage>().texture = slides[slideNumber];
    }
}
