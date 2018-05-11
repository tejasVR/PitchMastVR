﻿using UnityEngine;
using System.Collections.Generic;

/*
class Point {
	public Vector3 p;
	public Point next;
} 
*/

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class MeshLineRenderer : MonoBehaviour
{
    public Vector3 lastV;
    public Material lmat;

    private Mesh ml;

    private Vector3 s;
    private Vector3 sl;
    public float lineSize = 0.1f;

    private bool firstQuad = true;

    void Start()
    {
        ml = GetComponent<MeshFilter>().mesh;
        GetComponent<MeshRenderer>().material = lmat;
        //this.AddComponent<MeshRenderer>().material = lmat;

        //lineSize = .1f;
    }

    public void setWidth(float width)
    {
        lineSize = width;
    }

    public void AddPoint(Vector3 point, Vector3 point1)
    {
        //	lineSize = BrushManager.cursorsize;

        if (s != Vector3.zero)
        {
            AddLine(ml, MakeQuad(s, sl, point, point1, lineSize, firstQuad));
            firstQuad = false;
            //s = point;
        }

        s = point;
        sl = point1;
    }

    Vector3[] MakeQuad(Vector3 s, Vector3 hs, Vector3 er, Vector3 e, float w, bool all)
    {
        w = w / 2;

        Vector3[] q;
        if (all)
        {
            q = new Vector3[4];
        }
        else
        {
            q = new Vector3[2];
        }

        //this method switch the brush strokes to adheare to the local position of the controller allowing for a much larger

        if (all)
        {
            q[0] = s;
            q[1] = hs;
            q[2] = er;
            q[3] = e;
        }
        else
        {
            q[0] = s;
            q[1] = hs;
        }
        return q;


    }

    void AddLine(Mesh m, Vector3[] quad)
    {
        int vl = m.vertices.Length;

        Vector3[] vs = m.vertices;
        vs = resizeVertices(vs, 2 * quad.Length);

        for (int i = 0; i < 2 * quad.Length; i += 2)
        {
            vs[vl + i] = quad[i / 2];
            vs[vl + i + 1] = quad[i / 2];
        }

        Vector2[] uvs = m.uv;
        uvs = resizeUVs(uvs, 2 * quad.Length);

        if (quad.Length == 4)
        {
            uvs[vl] = Vector2.zero;
            uvs[vl + 1] = Vector2.zero;
            uvs[vl + 2] = Vector2.right;
            uvs[vl + 3] = Vector2.right;
            uvs[vl + 4] = Vector2.up;
            uvs[vl + 5] = Vector2.up;
            uvs[vl + 6] = Vector2.one;
            uvs[vl + 7] = Vector2.one;
        }
        else
        {
            if (vl % 8 == 0)
            {
                uvs[vl] = Vector2.zero;
                uvs[vl + 1] = Vector2.zero;
                uvs[vl + 2] = Vector2.right;
                uvs[vl + 3] = Vector2.right;

            }
            else
            {
                uvs[vl] = Vector2.up;
                uvs[vl + 1] = Vector2.up;
                uvs[vl + 2] = Vector2.one;
                uvs[vl + 3] = Vector2.one;
            }
        }

        int tl = m.triangles.Length;

        int[] ts = m.triangles;
        ts = resizeTriangles(ts, 12);

        if (quad.Length == 2)
        {
            vl -= 4;
        }

        // front-facing quad
        ts[tl] = vl;
        ts[tl + 1] = vl + 2;
        ts[tl + 2] = vl + 4;

        ts[tl + 3] = vl + 2;
        ts[tl + 4] = vl + 6;
        ts[tl + 5] = vl + 4;

        // back-facing quad
        ts[tl + 6] = vl + 5;
        ts[tl + 7] = vl + 3;
        ts[tl + 8] = vl + 1;

        ts[tl + 9] = vl + 5;
        ts[tl + 10] = vl + 7;
        ts[tl + 11] = vl + 3;

        m.vertices = vs;
        m.uv = uvs;
        m.triangles = ts;
        m.RecalculateBounds();
        m.RecalculateNormals();
    }

    Vector3[] resizeVertices(Vector3[] ovs, int ns)
    {
        Vector3[] nvs = new Vector3[ovs.Length + ns];
        for (int i = 0; i < ovs.Length; i++)
        {
            nvs[i] = ovs[i];
        }

        return nvs;
    }

    Vector2[] resizeUVs(Vector2[] uvs, int ns)
    {
        Vector2[] nvs = new Vector2[uvs.Length + ns];
        for (int i = 0; i < uvs.Length; i++)
        {
            nvs[i] = uvs[i];
        }

        return nvs;
    }

    int[] resizeTriangles(int[] ovs, int ns)
    {
        int[] nvs = new int[ovs.Length + ns];
        for (int i = 0; i < ovs.Length; i++)
        {
            nvs[i] = ovs[i];
        }

        return nvs;
    }
}