using UnityEngine;
using System.Collections;
using ConvertPDF;
using UnityEditor;

public class PDFInput : MonoBehaviour {

    public Object pdfToConvert;

    void Start()
    {
        PDFConvert converter = new PDFConvert();

        converter.Convert(AssetDatabase.GetAssetPath(pdfToConvert),
                         @"C:/tempPDF/Textures/texting.jpg",
                         1,
                         10,
                         "jpeg",
                         800,
                         600);


    }
}
