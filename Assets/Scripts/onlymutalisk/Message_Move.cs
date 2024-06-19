using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TMPro;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Message_Move : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private GameObject UI;
    private GameObject msg_move;
    private TextMeshProUGUI tmp;
    private RectTransform UIRect;
    private RectTransform msgRect;
    private GameObject visionTemp;
    private GameObject posTemp;

    private void Start()
    {
        UI = GameObject.Find("UI");
        msg_move = FindChildObject(UI, "Message_Move");
        tmp = msg_move.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        UIRect = UI.GetComponent<RectTransform>();
        msgRect = msg_move.GetComponent<RectTransform>();
    }

    /// <summary>
    /// 버튼을 누를 때 호출되는 함수
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        Tile tile = GetComponent<Tile>();

        // 타일
        if (tile != null)
        {
            if (tile.gameObject.GetComponent<Image>().sprite.name.Contains("Select") == false)
            {
                StringBuilder strB = new StringBuilder();
                bool isMob = false;

                // 몬스터가 존재하면, 몬스터 정보도 출력
                foreach (Mob mob in Mob.Mobs)
                {
                    if (mob.i == tile.i && mob.j == tile.j)
                    {
                        isMob = true;
                        string active;
                        if (mob.isSleep == true) { active = "수면중"; }
                        else { active = "깨어남"; }

                        strB.Append("\n\n\n");
                        strB.Append("[ 몬스터 정보 ]" + "\n\n");
                        strB.Append($"이동 타입 : {mob.rangeType}" + "\n");
                        strB.Append($"상태 : {active}" + "\n");
                        strB.Append($"체력 : {mob.HP} / {mob.HP_max}" + "\n");
                        strB.Append($"공격력 : {mob.damage}" + "\n");
                        strB.Append($"행동력 : {mob.action} / {mob.maxAction}" + "\n");
                        strB.Append($"공격 사거리 : {mob.range}" + "\n");
                        strB.Append($"시야 : {mob.visionRange}" + "\n");
                        strB.Append("ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ" + "\n");

                        visionTemp = FindChildObject(mob.gameObject, "Vision");
                        visionTemp.SetActive(true);
                    }
                }

                if (isMob == false) { strB.Append("\n\n\n"); }
                strB.Append("[ 타일 정보 ]" + "\n\n");
                strB.Append($"좌표 : [{tile.i}],[{tile.j}]" + "\n");
                strB.Append($"타일 속성 : {GameManager.tileProperty[tile.gameObject.GetComponent<Image>().sprite.name]}" + "\n");
                strB.Append("\n\n\n");

                tmp.text = strB.ToString();
                msgRect.anchoredPosition = new Vector2(-500, 0);
                msg_move.SetActive(true);
            }
        }
        // UI
        else
        {
            // text 변경
            string fieldName = "msg_" + gameObject.name;
            Type type = typeof(GameManager);
            FieldInfo fieldInfo = type.GetField(fieldName, BindingFlags.Public | BindingFlags.Static);
            string str = (string)fieldInfo.GetValue(null);

            tmp.text = str;
            SetMsgPosition(gameObject, 0, 30);
            msg_move.SetActive(true);
        }

        Debug.Log("Button Pressed");
    }

    /// <summary>
    /// 버튼을 뗄 때 호출되는 함수
    /// </summary>
    public void OnPointerUp(PointerEventData eventData)
    {
        if (visionTemp != null) { visionTemp.SetActive(false); }
        msg_move.SetActive(false);
        Debug.Log("Button Released");
    }

    /// <summary>
    /// 이름을 통해 자식 오브젝트를 탐색합니다.
    /// </summary>
    private GameObject FindChildObject(GameObject gameObject, string name)
    {
        Transform parent = gameObject.transform;

        foreach (Transform child in parent)
        {
            if (child.name == name)
            {
                return child.gameObject;
            }
        }

        return null;
    }

    /// <summary>
    /// <br>Msg 의 위치를 오브젝트에 맞게 조정합니다.</br>
    /// <br>gameObject 위치로 이동시킨 후, x와 y 만큼 평행이동합니다.</br>
    /// </summary>
    private void SetMsgPosition(GameObject gameObject, float x, float y)
    {
        Vector3 pos = gameObject.transform.position;
        pos.x += x;
        pos.y += y;
        msg_move.transform.position = pos;

        float x_msg = msgRect.anchoredPosition.x;
        float y_msg = msgRect.anchoredPosition.y;
        float width_msg = msgRect.rect.width;
        float height_msg = msgRect.rect.height;
        float width_canv = UIRect.rect.width;
        float height_canv = UIRect.rect.height;

        if (Math.Abs(x_msg) > Math.Abs((width_canv / 2) - (width_msg / 2)))
        {
            if (x_msg < 0)
            {
                x_msg = -Math.Abs((width_canv / 2) - (width_msg / 2));
            }
            else
            {
                x_msg = Math.Abs((width_canv / 2) - (width_msg / 2));
            }
        }

        if (Math.Abs(y_msg) > Math.Abs((height_canv / 2) - (height_msg / 2)))
        {
            if (y_msg < 0)
            {
                y_msg = -Math.Abs((height_canv / 2) - (height_msg / 2));
            }
            else
            {
                y_msg = Math.Abs((height_canv / 2) - (height_msg / 2));
            }
        }

        Vector2 newPos = new Vector2(x_msg, y_msg);
        msgRect.anchoredPosition = newPos;
    }
}
