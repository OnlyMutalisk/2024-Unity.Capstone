using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class Effect : MonoBehaviour
{
    public static Effect instance;
    private void Awake() { instance = this; }
    public GameObject[] effects;

    /// <summary>
    /// Effect 를 pos 위치에 scale 크기로 time 번 재생합니다.
    /// </summary>
    public void Play(Effects effect, Vector2 pos, float scale, int time)
    {
        StartCoroutine(CorPlay(effect, pos, scale, time));
    }
    private IEnumerator CorPlay(Effects effect, Vector2 pos, float scale, int time)
    {
        // Enum 과 이펙트 프리팹 바인딩
        foreach (var item in effects)
        {
            if (item.name == effect.ToString())
            {
                // 크기 조정
                Vector2 originScale = item.transform.localScale;
                GameObject prefab = Instantiate(item, pos, Quaternion.identity);
                prefab.transform.localScale = new Vector2(originScale.x * scale, originScale.y * scale);

                // 시간 조정
                Animator anim = prefab.GetComponent<Animator>();
                AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
                AnimationClip clip = clipInfo[0].clip;
                yield return new WaitForSeconds(clip.length * time);

                // 프리팹 제거
                Destroy(prefab);
                break;
            }
        }
    }
}

public enum Effects : byte
{
    // ETC
    airplane1,
    Bat_magic,
    blood,
    Coin,
    Green_line_missile,
    green_Ring1,
    GreenSkull_fire1,
    LaserBeam1,
    Poison_bubble,
    shiny1,
    sword_slash,
    Time_Running2,

    // Explosion
    explosion_2,
    explosion_3,
    explosion_5_poison1,
    explosion_6,
    explosion_7,
    explosion_8,
    explosion_9,
    explosion_10,
    explosion_11,
    PurpleDark_explosion1,
    Rock_explosion,

    // Fire
    Fire_1,
    fire_2,
    Fire_Blast1,
    Fire_nova,
    Fire_Sprit1,

    // Projectile
    projectile_adogen,
    Projectile_Blue1,
    Projectile_Green2,
    Projectile_Green_1,
    Projectile_Green_2,
    Projectile_Purple_circle4,
    Projectile_purple_ellipse1,
    Projectile_shuriken_blue1,

    // Skul
    Skul_jewel_glowing_new1,
    Skull_adogen1,
    skullGhost1,
}