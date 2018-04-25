using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImageMagick;
using System.IO;
using UnityEditor;


/*
 * Trying to convert PDF into a folder of PNG/JPEG images using the ImageMagick API
 */
namespace RootNamespace.Samples.MagickNET
{
    public class PDFConvert : MonoBehaviour
    {
        // Public get the PDF we are trying to convert
        public Object pdfToConvert;

        // Set the overhead parents and presentation folder (for all presentations)
        private string parentFolderPath = "Assets";
        private string presentationImagePath = "PresentationImages";

        void Start()
        {
            CheckParentDirectory();

            //print(pdfToConvert.name.ToString());
            
            // Convert the given PDF into images to then be used in the presentation
            ConvertPDF();

            

            //CreateWatermark();
        }


        public void ConvertPDF()
        {
            //var pdfPath = Application.dataPath + "/0_Project/Textures/Neon Bullet_ MassDigi 18_5.pdf";

            // pdfPath = where the given PDF is located in the project
            var pdfPath = AssetDatabase.GetAssetPath(pdfToConvert);
            var pdfName = pdfToConvert.name;

            //var pdfImagesDirectory = parentFolderPath + "/" + presentationImagePath + "/" + pdfName;

            var pdfImagesDirectory = CheckPresentationDirectory(pdfName);

             MagickReadSettings settings = new MagickReadSettings();
            // Settings the density to 300 dpi will create an image with a better quality
            settings.Density = new Density(300, 300);

            using (MagickImageCollection images = new MagickImageCollection())
            {
                // Add all the pages of the pdf file to the collection
                images.Read(pdfPath);//h, settings);

                int page = 1;
                foreach (MagickImage image in images)
                {
                    // Create Directory for all presentation images it it doesn't exist


                    // Write page to file that contains the page number
                    var imagePath = pdfImagesDirectory + "/PDF.Page" + page + ".png";
                    image.Write(imagePath);
                    //AssetDatabase.Refresh();

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

                AssetDatabase.Refresh();

            }

            //refreshes the database to actually import images
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


}