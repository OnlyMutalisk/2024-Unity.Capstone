using System.Collections;
using UnityEngine;

public class Animation_CameraShake : MonoBehaviour
{
    public static Animation_CameraShake instance;
    public void Awake() { instance = this; }

    public float duration = 0.2f;
    public float magnitude = 0.01f;

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    public void StartShake()
    {
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = (Mathf.PerlinNoise(Time.time * 10f, 0f) - 0.5f) * magnitude;
            float y = (Mathf.PerlinNoise(0f, Time.time * 10f) - 0.5f) * magnitude;

            transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
