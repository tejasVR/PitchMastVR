using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;



public class PresentationManager : MonoBehaviour {

    #region SINGLETON
    public static PresentationManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    #region VARIABLES
    DrawLineManager drawLineManager;

    // Track what slide Image and number we are cuttently on
    public RawImage currentSlide;
    public int slideNumber = 0;

    // Create an array of textures for each slide, as well as a game object if we need to save drawings for each slide
    public Texture2D[] slides;
    public GameObject[] slidesToDraw;

    // The transform parent where each slide drawing is attached to
    public Transform slideDrawParent;

    // The local path within the Unity project where a presentations' images are stored
    public static string presentationLocalImagesPath;

    #endregion

    void Start () {
        drawLineManager = DrawLineManager.Instance;

        // Set the starting slide as the first slides in the slides array
        slidesToDraw = new GameObject[slides.Length];
    }

    #region FUNCTIONS

    // Return a game object for each drawing to be attached to
    public GameObject SaveDraw()
    {
        // Create an empty gameObject
        GameObject slideDrawSpace;

        // If there is already a alideDraw object for this slide
        if (slidesToDraw[slideNumber] != null)
        {
            // reutn that existing object
            return slidesToDraw[slideNumber].gameObject;
        
        }
        // or create a new object
        else
        {
            // name the object, parent it, and return that gameObject
            slideDrawSpace = new GameObject(slideNumber + "_draw");
            slideDrawSpace.transform.parent = slideDrawParent;
            slidesToDraw[slideNumber] = slideDrawSpace;
            return slidesToDraw[slideNumber].gameObject;
        }
    }

    // Take the folder of images into something that can be featured on a canvas
    public void PresentImages()
    {
        // Scans the folder for Texture2D files, and then imports all files into the slides array to be used in a persentation format
        slides = Resources.LoadAll<Texture2D>(presentationLocalImagesPath);

        // If there are any existing objects contains drawings, destroy those objects
        foreach (GameObject s in slidesToDraw)
        {
            if (s != null)
            {
                Destroy(s);
            }
        }

        // Create a new array to keep the users' drawed objects
        slidesToDraw = new GameObject[slides.Length];

        // Sets the current slide counter to 0, and gets the first image of the presentation onto the canvas
        slideNumber = 0;
        currentSlide.GetComponent<RawImage>().texture = slides[0];
    }

    // Gets the new PDF from the path, converts it via PDFConverter class, and automatically presents those images onto the canvas
    public void GetNewPDF(string getPDFPath)
    {
        if (getPDFPath != null)
        {
            PDFConvert.ConvertPDF(getPDFPath);
            PresentImages();
        }
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

        CheckForSlideDrawing();
    }

    // Function for going to the next slide
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

        CheckForSlideDrawing();
    }

    // Make a determined string the folder path that holds all the presentation images
    public static void GetPresentionPath(string presentationPath)
    {
        presentationLocalImagesPath = presentationPath;
    }

    // check if there is a drawing attached to the current slide. If so, enable the game object
    public void CheckForSlideDrawing()
    {
        foreach (GameObject s in slidesToDraw)
        {
            if (s != null)
            {
                s.SetActive(false);
            }
        }

        if (slidesToDraw[slideNumber] != null)
        {
            slidesToDraw[slideNumber].SetActive(true);
        }
    }

    #endregion
}
