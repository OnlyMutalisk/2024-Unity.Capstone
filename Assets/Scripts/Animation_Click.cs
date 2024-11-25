using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Click : MonoBehaviour
{
    public float scaleFactor = 1.2f;
    public float animationDuration = 0.2f;

    private Coroutine corLast;
    private Vector3 originalScale;
    void Start() { originalScale = transform.localScale; }

    public void OnClick()
    {
        if (corLast != null) StopCoroutine(corLast);
        transform.localScale = originalScale;
        corLast = StartCoroutine(CorClick());
    }

    private IEnumerator CorClick()
    {
        // 크기 증가
        float elapsed = 0f;
        while (elapsed < animationDuration / 2)
        {
            transform.localScale = Vector3.Lerp(originalScale, originalScale * scaleFactor, elapsed / (animationDuration / 2));
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = originalScale * scaleFactor;

        // 크기 복구
        elapsed = 0f;
        while (elapsed < animationDuration / 2)
        {
            transform.localScale = Vector3.Lerp(originalScale * scaleFactor, originalScale, elapsed / (animationDuration / 2));
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = originalScale;
    }
}