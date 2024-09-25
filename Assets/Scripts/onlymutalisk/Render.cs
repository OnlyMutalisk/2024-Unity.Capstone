using System.Collections.Generic;
using UnityEngine;

public class Render : MonoBehaviour
{
    public Camera cam_main;
    public Camera cam_minimap;
    public Transform grid;
    private List<CanvasRenderer> fogRenderers = new List<CanvasRenderer>();

    void Start()
    {
        foreach (Transform tile in grid)
        {
            fogRenderers.Add(tile.GetChild(0).GetComponent<CanvasRenderer>());
        }
    }

    void OnPreCull()
    {
        //if (Camera.current == cam_main)
        //{
        //    foreach (var renderer in fogRenderers)
        //    {
        //        renderer.SetAlpha(0);
        //    }
        //}
    }

    void OnPostRender()
    {
        //if (Camera.current == cam_main)
        //{
        //    foreach (var renderer in fogRenderers)
        //    {
        //        renderer.SetAlpha(1);
        //    }
        //}
    }
}
