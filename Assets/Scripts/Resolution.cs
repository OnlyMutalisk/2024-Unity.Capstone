using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resolution : MonoBehaviour
{
    void Start()
    {
        // 고정할 화면 비율 (16:9)
        float targetAspect = 16.0f / 9.0f;

        // 현재 화면 비율 계산
        float windowAspect = (float)Screen.width / Screen.height;

        // 비율 비교
        float scaleHeight = windowAspect / targetAspect;

        Camera mainCamera = Camera.main;

        if (scaleHeight < 1.0f) // 현재 화면이 더 좁은 경우
        {
            Rect rect = mainCamera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f; // 세로 여백 추가 (Letterboxing)

            mainCamera.rect = rect;
        }
        else // 현재 화면이 더 넓은 경우
        {
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = mainCamera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f; // 가로 여백 추가 (Pillarboxing)
            rect.y = 0;

            mainCamera.rect = rect;
        }
    }
}
