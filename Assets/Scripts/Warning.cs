using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Timeline;

public class Warning : MonoBehaviour
{
    public List<Flag> flags = new List<Flag>();
    public GameObject prefab_flag;
    public Camera cam;
    /// <summary>
    /// Mob 마다 담당 Flag 생성
    /// </summary>
    private void Start()
    {
        StartCoroutine(CorStart());
    }
    public IEnumerator CorStart()
    {
        yield return new WaitForSeconds(GameManager.delay_loading);

        foreach (Mob mob in Mob.Mobs)
        {
            Flag flag = Instantiate(prefab_flag, this.transform).GetComponent<Flag>();
            flag.targetMob = mob;
            flags.Add(flag);
        }
    }

    /// <summary>
    /// Flag 위치 조정 및 관리 데몬
    /// </summary>
    private void FixedUpdate()
    {
        // 대응하는 몬스터가 죽었을 경우, 플래그도 삭제
        for (int i = flags.Count - 1; i >= 0; i--)
        {
            if (flags[i].targetMob == null)
            {
                Destroy(flags[i].gameObject);
                flags.RemoveAt(i);
            }
        }

        // Flag 위치 조정
        foreach (Flag flag in flags)
        {
            // 몬스터의 월드 좌표를 뷰포트 좌표로 변환
            Vector3 viewPos_mob = cam.WorldToViewportPoint(flag.targetMob.gameObject.transform.position);

            // Mob 이 카메라 밖에 있는 경우 Flag 활성화
            if (viewPos_mob.x >= 0 && viewPos_mob.x <= 1 && viewPos_mob.y >= 0 && viewPos_mob.y <= 1)
            {
                flag.gameObject.SetActive(false);
            }
            // Mob 이 카메라 안에 있는 경우 Flag 비활성화
            else
            {
                flag.gameObject.SetActive(true);
            }

            // 뷰포트 좌표를 [0,1] 범위로 클램프
            viewPos_mob.x = Mathf.Clamp(viewPos_mob.x, 0.01f, 0.99f);
            viewPos_mob.y = Mathf.Clamp(viewPos_mob.y, 0.01f, 0.99f);

            // 클램프된 뷰포트 좌표를 월드 좌표로 변환
            Vector3 worldPos_mob = cam.ViewportToWorldPoint(viewPos_mob);

            // Flag 위치 업데이트
            flag.gameObject.transform.position = worldPos_mob;
        }
    }
}
