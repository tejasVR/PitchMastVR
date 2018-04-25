using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImageMagick;


/*
 * Trying to convert PDF into a folder of PNG/JPEG images using the GhostScript API
 */

    public class PDFConvert : MonoBehaviour
    {

    void Start()
    {
        CreateWatermark();
    }


    public void CreateWatermark()
    {
        // our image paths
        var sourcePath = Application.dataPath + "/Images/FujiFilmFinePixS1Pro.jpg";
        var watermarkPath = Application.dataPath + "/Images/Snakeware.png";

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