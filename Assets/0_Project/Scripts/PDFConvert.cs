using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImageMagick;


/*
 * Trying to convert PDF into a folder of PNG/JPEG images using the GhostScript API
 */
namespace RootNamespace.Samples.MagickNET
{
    public class PDFConvert : MonoBehaviour
    {

        public Object pdfToConvert;

        void Start()
        {
            //ConvertPDF();

            CreateWatermark();
        }


        /*public void ConvertPDF()
        {
            MagickReadSettings settings = new MagickReadSettings();
            // Settings the density to 300 dpi will create an image with a better quality
            settings.Density = new Density(300, 300);

            using (MagickImageCollection images = new MagickImageCollection())
            {
                // Add all the pages of the pdf file to the collection
                images.Read(pdfToConvert.ToString(), settings);

                int page = 1;
                foreach (MagickImage image in images)
                {
                    // Write page to file that contains the page number
                    image.Write("PDF.Page" + page + ".png");
                    // Writing to a specific format works the same as for a single image
                    //image.Format = MagickFormat.Ptif;
                    //image.Write("PDF.Page" + page + ".tif");
                    page++;
                }
            }
        }*/

        public void CreateWatermark()
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
                image.Write(Application.dataPath + "/Images/" + "FujiFilmFinePixS1Pro-watermark.jpg");
            }
        }
}


}