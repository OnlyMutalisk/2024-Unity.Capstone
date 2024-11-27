using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Vision : MonoBehaviour
{
    public Sprite vision_3x3_in;
    public Sprite vision_5x5_in;
    public Sprite vision_7x7_in;
    public Sprite vision_9x9_in;

    public Sprite vision_3x3_out;
    public Sprite vision_5x5_out;
    public Sprite vision_7x7_out;
    public Sprite vision_9x9_out;

    public Image vision_in;
    public Image vision_out;
    private int visionRange;
    private Color originColor_vision_in;
    private Color originColor_vision_out;
    private Color warningColor_vision_in;
    private Color warningColor_vision_out;
    private Mob mob;

    /// <summary>
    /// 몬스터의 시야에 따라 Vision Sprite 를 변경합니다.
    /// </summary>
    private void Start()
    {
        mob = transform.parent.GetComponent<Mob>();
        visionRange = mob.visionRange;

        switch (visionRange)
        {
            case 1:
                vision_in.sprite = vision_3x3_in;
                vision_out.sprite = vision_3x3_out;
                break;
            case 2:
                vision_in.sprite = vision_5x5_in;
                vision_out.sprite = vision_5x5_out;
                break;
            case 3:
                vision_in.sprite = vision_7x7_in;
                vision_out.sprite = vision_7x7_out;
                break;
            case 4:
                vision_in.sprite = vision_9x9_in;
                vision_out.sprite = vision_9x9_out;
                break;
        }

        originColor_vision_in = vision_in.color;
        originColor_vision_out = vision_out.color;
        warningColor_vision_in = new Color(255f, 0f, 0f, vision_in.color.a);
        warningColor_vision_out = new Color(255f, 0f, 0f, vision_out.color.a);
    }

    private void OnEnable()
    {
        StartCoroutine(CorCheck());
    }

    /// <summary>
    /// 몬스터의 시야 범위 이내인지 감시하여 색상을 조절합니다.
    /// </summary>
    private IEnumerator CorCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            // 플레이어가 몬스터의 시야 이내이면 붉은 Color 적용
            if (Mathf.Max(Mathf.Abs(mob.i - Player.i), Mathf.Abs(mob.j - Player.j)) <= mob.visionRange)
            {
                vision_in.color = warningColor_vision_in;
                vision_out.color = warningColor_vision_out;
            }
            // 아니면 원래 Color 적용
            else
            {
                vision_in.color = originColor_vision_in;
                vision_out.color = originColor_vision_out;
            }
        }
    }
}
