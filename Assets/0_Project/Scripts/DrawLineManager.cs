using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineManager : MonoBehaviour {

    // Use this for initialization

    public static DrawLineManager Instance;

    // The MeshRenderer Lines created on draw
    public MeshLineRenderer currLine;
    public MeshLineRenderer currLine1;
    public MeshLineRenderer currLine2;
    public MeshLineRenderer currLine3;

    // Creates 
    public int numClicks = 0;
    public Material matter;
    private float width = .1f;

    //public GameObject UndoManager;
    //public Transform meshparent;
    //public bool firstPointTime;
    //public bool secondPointTime;

    // The last position of the controller. Used for smoother drawing
    public Vector3 lastpos;

    // Spheres created to form of the draw mesh
   // public GameObject sphere;
    public Transform sphereHigh;
    public Transform sphereLow;
    //public Transform sphererl;
    //public Transform spherell;
    //public Transform sphererh;
    //public Transform spherelh;

    public GameObject cursor;

    public bool isDrawing;

    private void Awake()
    {
        Instance = this;
    }

    public void DrawInitialize()
    {
        GameObject go = new GameObject();
        go.AddComponent<MeshFilter>();
        go.AddComponent<MeshRenderer>();
        currLine = go.AddComponent<MeshLineRenderer>();

        //go.AddComponent<MeshCollider>().sharedMesh = meshparent.GetComponent<MeshFilter>().mesh;

        currLine.setWidth(width);
        currLine.lmat = new Material(matter);
        if (currLine != null)
        {
            //currLine.lmat.color = ColorManager.Instance.GetCurrentColor();
        }
    }

    public void DrawLine(Vector3 screenPoint)
    {
        isDrawing = true;
        cursor.transform.position = screenPoint;//Vector3.Lerp(cursor.transform.position, screenPoint, Time.deltaTime * 2f);
        currLine.AddPoint(sphereHigh.position, sphereLow.position);
        numClicks++;
    }

    public void DrawStop(Vector3 controllerPosition, GameObject slideToSave)
    {
        isDrawing = false;
        cursor.transform.position = controllerPosition;
        numClicks = 0;
        currLine.transform.SetParent(slideToSave.transform);
        currLine = null;
    }
}
