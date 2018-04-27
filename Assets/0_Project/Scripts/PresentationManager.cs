using SimpleFileBrowser;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;



public class PresentationManager : MonoBehaviour {

    public static PresentationManager Instance;

    //public static PDFConvert pdfConvert;
    // Basic tracking for Vive wands
    public SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;
    public Vector2 touchpad; // links Vive touchpad to Vector2

    DrawLineManager drawLineManager;

    //public int totalNumberOfSlides;

    public RawImage currentSlide;
    public int slideNumber = 0;
    public Texture2D[] slides;
    //public List<GameObject> slidesToDraw = new List<GameObject>();
    public GameObject[] slidesToDraw;

    public Transform slideDrawParent;

    public string explorerPDFPath;


    // The local path within the Unity project where a presentations' images are stored
    public static string presentationLocalImagesPath;

    private void Awake()
    {
        Instance = this;
    }

    void Start () {
        //CollectImages();
        //slides = new Texture[totalNumberOfSlides];
        drawLineManager = DrawLineManager.Instance;
        // Set the starting slide as the first slides in the slides array
        slidesToDraw = new GameObject[slides.Length];
        //print("slideToDraw is now " + slidesToDraw.Count);

    }

    void Update () {

        


        // Update the Steam VR controllers every frame
        /*if (trackedObj.gameObject.activeInHierarchy)
        {
            device = SteamVR_Controller.Input((int)trackedObj.index);

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

            if (device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
            {
                PresentImages();
            }

            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
            {
                //OpenExplorer();
            }
        }*/

        // if we are touching the touchpad, get the coordinates of the Vector2
        

    }

    public GameObject SaveDraw()
    {
        GameObject slideDrawSpace;

        if (slidesToDraw[slideNumber] != null)
        {
            return slidesToDraw[slideNumber].gameObject;

        }else
        {
            slideDrawSpace = new GameObject(slideNumber + "_draw");
            slideDrawSpace.transform.parent = slideDrawParent;
            slidesToDraw[slideNumber] = slideDrawSpace;
            return slidesToDraw[slideNumber].gameObject;

        }
        //if(slidesToDraw[slideNumber])

    }

    public void PresentImages()
    {
        print("Presentation Local Images Path: " + presentationLocalImagesPath);
        print("Presenting images to canvas");

        // Scans the folder for Texture2D files, and then imports all files into the slides array to be used in a persentation format
        slides = Resources.LoadAll<Texture2D>(presentationLocalImagesPath);
        foreach (GameObject s in slidesToDraw)
        {
            if (s != null)
            {
                Destroy(s);
            }
        }
        slidesToDraw = new GameObject[slides.Length];
        slideNumber = 0;
        currentSlide.GetComponent<RawImage>().texture = slides[0];


        //print(slides[0]);
        //print("Collecting Images");

    }

    public void OpenExplorer()
    {
        // Sets filters for file browser
        FileBrowser.SetFilters(true, new FileBrowser.Filter("PDF", ".pdf"));
        FileBrowser.SetDefaultFilter(".pdf");

        //Adds quick like to file browser
        FileBrowser.AddQuickLink("Users", "C:\\Users", null);

        // Show a save file dialog
        // onSuccess event: not registered (which means this dialog is pretty useless)
        // onCancel event: not registered
        // Save file/folder: file, Initial path: "C:\", Title: "Save As", submit button text: "Save"
        // FileBrowser.ShowSaveDialog( null, null, false, "C:\\", "Save As", "Save" );

        // Show a select folder dialog 
        // onSuccess event: print the selected folder's path
        // onCancel event: print "Canceled"
        // Load file/folder: folder, Initial path: default (Documents), Title: "Select Folder", submit button text: "Select"
        // FileBrowser.ShowLoadDialog( (path) => { Debug.Log( "Selected: " + path ); }, 
        //                                () => { Debug.Log( "Canceled" ); }, 
        //                                true, null, "Select Folder", "Select" );



        /*
        explorerPDFPath = EditorUtility.OpenFilePanel("Overwrite PDF", "", "pdf");
        GetNewPDF();
        */

        StartCoroutine(ShowLoadDialog());
    }

    IEnumerator ShowLoadDialog()
    {
        yield return FileBrowser.WaitForLoadDialog(false, null, "Load PDF", "Load");
        print("showing load dialog");
        GetNewPDF(FileBrowser.Result);
    }

    public void GetNewPDF(string getPDFPath)
    {
        print("successfuly got the new PDF file");
        if (getPDFPath != null)
        {
            //WWW www = new WWW("file://" + explorerPDFPath);
            PDFConvert.ConvertPDF(getPDFPath);
            PresentImages();
            //print(www.ToString());
           // print(explorerPDFPath);
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

    public static void GetPresentionPath(string presentationPath)
    {
        presentationLocalImagesPath = presentationPath;
        //print("Presentation Local Images Path: " + presentationLocalImagesPath);
    }

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
}
