using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class Animation_Wave : MonoBehaviour
{
    // Wave 속도 / 진폭
    public float speed = 3f;
    public float size = 3f;
    private Vector3 startPos;

    private void FixedUpdate()
    {
        if (startPos == new Vector3()) startPos = transform.position;

        float newY = Mathf.Sin(Time.time * speed) * size;
        transform.position = startPos + new Vector3(0, newY, 0);
    }
}
