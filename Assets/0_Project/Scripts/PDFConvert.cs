using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImageMagick;
using System.IO;
using UnityEditor;


/*
 * Trying to convert PDF into a folder of PNG/JPEG images using the ImageMagick API
 */

public class PDFConvert : MonoBehaviour
{
    #region VARIABLES
    // Set the overhead parents and presentation folder (for all presentations)
    private static string parentFolderPath = "Assets/Resources";
    private static string presentationImagePath = "Presentations";

    // The path to the pdf that we want to split up and create a presentation of    
    public string pdfPath;
    #endregion

    void Start()
    {
        // Checks if the parent directory where all presentations does exists
        CheckParentDirectory();
    }

    #region FUNCTIONS

    // Converts a PDF from a given file path, splits its slides into images, and puts these images into a designated folder
    public static void ConvertPDF(string pdfPath)
    {
        //Get name of the pdf we are converting
        var pdfName = Path.GetFileNameWithoutExtension(pdfPath);

        // Create the name of the directory that the images of the PDF will be stored
        var pdfImagesDirectory = parentFolderPath + "/" + presentationImagePath + "/" + pdfName;

        // If the folder needed to house the images for this specific presentation doesn't yet exists (meaning, we haven't yet crated the PDF), then convert the PDF
        if (!AssetDatabase.IsValidFolder(pdfImagesDirectory))
        {
            //Creates a folder to house image assets
            AssetDatabase.CreateFolder(parentFolderPath + "/" + presentationImagePath, pdfName);                
                
            // Settings the density to 300 dpi will create an image with a better quality
            MagickReadSettings settings = new MagickReadSettings();
            settings.Density = new Density(350, 350);
           
            // Create an image collection when splitting up the PDF
            using (MagickImageCollection images = new MagickImageCollection())
            {
                // Reads the PDF, and creates images for eah of the pages
                images.Read(pdfPath, settings);

                int page = 1;
                foreach (MagickImage image in images)
                {
                    // Write page to file that contains the page number
                    var imagePath = pdfImagesDirectory + "/Slide_" + page.ToString("000") + ".jpg";
                    image.Write(imagePath);

                    // Import the newly created image and convert it to type Texture2D
                    #region KEEPING FOR FUTURE REFERENCE
                    // keeping this if we need it in the future
                    AssetDatabase.ImportAsset(imagePath);
                    TextureImporter importer = AssetImporter.GetAtPath(imagePath) as TextureImporter;
                    importer.textureType = TextureImporterType.Sprite;
                    AssetDatabase.WriteImportSettingsIfDirty(imagePath);

                    // Writing to a specific format works the same as for a single image
                    //image.Format = MagickFormat.Ptif;
                    //image.Write("PDF.Page" + page + ".tif");
                    #endregion

                    page++;
                }
                //refreshes the database to finanlize the import of the PDF images
                AssetDatabase.Refresh();
            }
        }

        // Sets the presentation manager pull path to the PDF folder derived from the PDF selected
        PresentationManager.GetPresentionPath(presentationImagePath + "/" + pdfName);
    }

    // Checks directory where all our presentations will be kept
    private void CheckParentDirectory()
    {
        if (!AssetDatabase.IsValidFolder(parentFolderPath + "/" + presentationImagePath))
        {
            //if it doesn't, create it
            AssetDatabase.CreateFolder(parentFolderPath, presentationImagePath);

        }
    }

    // Checks directory for presentation we want to convert
    private string CheckPresentationDirectory(string presentationName)
    {
        string path = parentFolderPath + "/" + presentationImagePath + "/" + presentationName;
        if (!AssetDatabase.IsValidFolder(path))
        {
            //if it doesn't, create it
            AssetDatabase.CreateFolder(parentFolderPath + "/" + presentationImagePath, presentationName);
        }
        return path;
    }

    #endregion
}
