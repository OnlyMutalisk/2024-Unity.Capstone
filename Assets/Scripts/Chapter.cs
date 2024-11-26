using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter : MonoBehaviour
{
    public GameObject[] chapters;
    public GameObject up;
    public GameObject down;
    private static int activeChapter = 0;

    private void Start()
    {
        On();
    }

    private void On()
    {
        foreach (var item in chapters) item.SetActive(false);
        chapters[activeChapter].SetActive(true);
        Check();
    }

    public void Up()
    {
        if (activeChapter > 0) activeChapter--;
        On();
    }

    public void Down()
    {
        if (activeChapter < chapters.Length - 1) activeChapter++;
        On();
    }

    private void Check()
    {
        if (activeChapter == 0) up.SetActive(false);
        else up.SetActive(true);

        // 다음 Chapter 의 첫 Map 이 On 이면 Down 버튼 활성화
        if (Stages.isOn[5 * (activeChapter + 1)]) down.SetActive(true);
        else down.SetActive(false);
    }
}
