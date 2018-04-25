using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImageMagick;
using System.IO;
using UnityEditor;


/*
 * Trying to convert PDF into a folder of PNG/JPEG images using the ImageMagick API
 */

//namespace RootNamespace.Samples.MagickNET
//{
public class PDFConvert : MonoBehaviour
{
        
    // Public get the PDF we are trying to convert
    //public Object pdfToConvert;

    // Set the overhead parents and presentation folder (for all presentations)
    private string parentFolderPath = "Assets/Resources";
    private string presentationImagePath = "Presentations";

    // The path to the pdf that we want to split up and create a presentation of    
    public string pdfPath;

    // Call the presentation manager so we can set the path to where the local images for the presentation are generated
    public PresentationManager presentationManager;

    void Start()
    {
        // Checks if the parent directory where all presentations does exists
        CheckParentDirectory();

        //print(pdfToConvert.name.ToString());

        // Convert the given PDF into images to then be used in the presentation
        //var pdfPath = AssetDatabase.GetAssetPath(pdfToConvert);

        // Let's not call the Convert function on start
        //ConvertPDF(pdfPath);

            

        //CreateWatermark();
    }

    // Converts a PDF from a given file path, splits its slides into images, and puts these images into a designated folder
    public void ConvertPDF(string pdfPath)
    {
        //Get name of the pdf
        var pdfName = Path.GetFileNameWithoutExtension(pdfPath);

        // Create the name of the directory that the images of the PDF will be stored
        var pdfImagesDirectory = parentFolderPath + "/" + presentationImagePath + "/" + pdfName;

        

        print(pdfImagesDirectory);

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
                    AssetDatabase.ImportAsset(imagePath);
                    TextureImporter importer = AssetImporter.GetAtPath(imagePath) as TextureImporter;
                    importer.textureType = TextureImporterType.Sprite;
                    AssetDatabase.WriteImportSettingsIfDirty(imagePath);
                        
                    // Writing to a specific format works the same as for a single image
                    //image.Format = MagickFormat.Ptif;
                    //image.Write("PDF.Page" + page + ".tif");
                    page++;
                }

                //refreshes the database to finanlize the import of the PDF images
                AssetDatabase.Refresh();
            }
        }

        // Sets the presentation manager pull path to the PDF folder derived from the PDF selected
        presentationManager.presentationLocalImagesPath = presentationImagePath + "/" + pdfName;

        //var pdfImagesDirectory = parentFolderPath + "/" + presentationImagePath + "/" + pdfName;
    }

    // Checks directory where all our presentations will be kept
    private void CheckParentDirectory()
    {
        if (!AssetDatabase.IsValidFolder(parentFolderPath + "/" + presentationImagePath))
        {
            //if it doesn't, create it
            AssetDatabase.CreateFolder(parentFolderPath, presentationImagePath);
            print("Checked the parent directory: " + parentFolderPath + presentationImagePath);

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
            print("Checked the presImage directory: " + path);
        }
        return path;
    }

    /*public void CreateWatermark()
    {
        // our image paths
        var sourcePath = Application.dataPath + "/0_Project/Textures/Coming Soon.jpg";
        var watermarkPath = Application.dataPath + "/0_Project/Textures/Coming Soon 1.jpg";

        // Read image that needs a watermark
        using (MagickImage image = new MagickImage(sourcePath))
        {

            // Read the watermark that will be put on top of the image
            using (MagickImage watermark = new MagickImage(watermarkPath))
            {
                // Draw the watermark in the bottom right corner
                image.Composite(watermark, Gravity.Southeast, CompositeOperator.Over);

                // Optionally make the watermark more transparent
                watermark.Evaluate(Channels.Alpha, EvaluateOperator.Divide, 4);

                // Or draw the watermark at a specific location
                image.Composite(watermark, 200, 50, CompositeOperator.Over);
            }

            // Save the result
            image.Write(Application.dataPath + "/0_Project/Textures/" + "Overwrite-watermark.jpg");
        }
    }*/
}
//}