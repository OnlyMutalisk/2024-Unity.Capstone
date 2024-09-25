using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Camera minimapCam;

    /// <summary>
    /// 150 ~ 500 사이즈 범위에서 미니맵을 확장합니다.
    /// </summary>
    public void ZoomIn()
    {
        if (minimapCam.orthographicSize > 200)
        {
            minimapCam.orthographicSize -= 100;
        }
    }

    public void ZoomOut()
    {
        if (minimapCam.orthographicSize < 500)
        {
            minimapCam.orthographicSize += 100;
        }
    }
}
