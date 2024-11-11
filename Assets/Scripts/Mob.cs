using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class Mob : MonoBehaviour
{
    public UnityEngine.UI.Slider HP_slider;
    public static List<Mob> Mobs = new List<Mob>();
    public static bool mobCounting = false;
    public virtual float HP { get; set; }
    public virtual float HP_max { get; set; }
    public virtual int i { get; set; }
    public virtual int j { get; set; }
    public virtual int maxAction { get; set; }
    public virtual int action { get; set; }
    public virtual int range { get; set; }
    public virtual int damage { get; set; }
    public virtual int attackCost { get; set; }
    public virtual int moveCost { get; set; }
    public virtual int visionRange { get; set; }
    public virtual RangeType rangeType { get; set; }
    public bool isSleep = true;
    public GameObject vision;
    private GameObject life;
    private GameObject shield;
    public Image Action_Image;
    public TextMeshProUGUI Action_Text;
    public GameObject Zzz;
    public GameObject WakeUp;
    public Animator anim;
    private List<UnityEngine.UI.Image> hearts = new List<UnityEngine.UI.Image>();
    private List<UnityEngine.UI.Image> shields = new List<UnityEngine.UI.Image>();
    private static List<UnityEngine.UI.Image> s_hearts = new List<UnityEngine.UI.Image>();
    private static List<UnityEngine.UI.Image> s_shields = new List<UnityEngine.UI.Image>();

    private void Awake()
    {
        Mobs = new List<Mob>();
        mobCounting = false;
        s_hearts = hearts;
        s_shields = shields;
    }

    private void Start()
    {
        // Life UI 를 가져옵니다.
        life = GameObject.Find("Life");
        int heartCount = life.transform.childCount;
        for (int i = 0; i < heartCount; i++) { hearts.Add(life.transform.GetChild(i).gameObject.GetComponent<UnityEngine.UI.Image>()); }

        // Shield UI 를 가져옵니다.
        shield = GameObject.Find("Shield");
        int shieldCount = shield.transform.childCount;
        for (int i = 0; i < shieldCount; i++) { shields.Add(shield.transform.GetChild(i).gameObject.GetComponent<UnityEngine.UI.Image>()); }

        // Vision 사이즈를 조절한 후 SetActive(false) 합니다.
        float offset = Grid.cellSize * (visionRange * 2 + 1) + Grid.spacing * (visionRange * 2);
        vision.GetComponent<RectTransform>().sizeDelta = new Vector2(offset, offset);
        vision.SetActive(false);

        // Animator 를 바인딩 합니다.
        Transform sprite = transform.Find("Sprite");
        anim = sprite.GetComponent<Animator>();

        Mobs.Add(this);
        mobCounting = true;
    }

    private void Update()
    {
        // 체력 바를 갱신합니다.
        if (HP_slider.value > HP / HP_max) HP_slider.value -= (HP_max * 0.07f) * Time.deltaTime;

        // 몬스터가 깨어나면, Zzz 애니메이션을 비활성화 하고 WakeUp 애니메이션을 활성화합니다.
        if (isSleep == false) { Zzz.SetActive(false); WakeUp.SetActive(true); }

        UpdateAction();
    }

    /// <summary>
    /// 행동력을 모두 소모할 때 까지 턴을 진행합니다.
    /// </summary>
    public IEnumerator CorPlay()
    {
        if (anim != null) anim.SetBool("isMove", true);

        // 시야범위 이내, 혹은 최대 체력이 아니면 활동 시작
        if (Mathf.Max(Mathf.Abs(i - Player.i), Mathf.Abs(j - Player.j)) <= visionRange | HP != HP_max) { isSleep = false; }
        //if (anim != null) anim.SetBool("isMove", true);

        List<Tile> path = A_Star.PathFind(Grid.GetTile(i, j).GetComponent<Tile>(), rangeType);

        while (action >= Mathf.Min(moveCost, attackCost) && isSleep == false)
        {
            // 공격범위 안이면 공격 후 현재 문 탈출, 체비쇼프 거리
            if (Mathf.Max(Mathf.Abs(i - Player.i), Mathf.Abs(j - Player.j)) <= range)
            {
                if (anim != null) anim.SetBool("isAttack", true);
                action -= attackCost;
                yield return StartCoroutine(CorAttack());
                if (anim != null) anim.SetBool("isAttack", false);
                continue;
            }

            // 타겟으로의 경로가 존재하면 A* 알고리즘에 따라 이동
            if (path.Count != 0)
            {
                action -= moveCost;
                int i = path[0].i;
                int j = path[0].j;
                path.Remove(path[0]);
                yield return StartCoroutine(CorMove(i, j));
                yield return new WaitForSeconds(GameManager.delay_mobMove);
            }
            else { break; }
        }

        if (anim != null) anim.SetBool("isMove", false);
        action = maxAction;
    }

    public IEnumerator CorAttack()
    {

        int origin_i = i;
        int origin_j = j;

        int calcDamage = (int)Tile.CalcDamage(damage, Grid.GetTile(i, j), Grid.GetTile(Player.i, Player.j));

        // 실드를 우선 감소시키고 체력을 감소시킵니다.
        Player.anim.SetBool("isHurt", true);
        if (Player.shield >= calcDamage)
        {
            Player.shield -= calcDamage;
        }
        else
        {
            calcDamage -= Player.shield;
            Player.shield = 0;
            Player.life -= calcDamage;
        }

        yield return StartCoroutine(CorMove(Player.i, Player.j));
        DrawLife();
        yield return StartCoroutine(CorMove(origin_i, origin_j));
    }

    public IEnumerator CorMove(int i, int j)
    {
        A_Star.SwitchWall(this.i, this.j);
        A_Star.SwitchWall(i, j);

        Vector3 target = gameObject.transform.position;
        float diameter = Grid.cellSize + Grid.spacing;

        int xCount = j - this.j;
        int yCount = i - this.i;

        this.j += xCount;
        target.x += xCount * diameter;
        this.i += yCount;
        target.y += -yCount * diameter;

        while (gameObject.transform.position != target)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, GameManager.speed_Mob);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public static void DrawLife()
    {
        // 체력을 그립니다.
        int lifeCopy = Player.life;

        foreach (var heart in s_hearts) { heart.sprite = Resources.Load<Sprite>("Images/Heart_Empty"); }

        foreach (var heart in s_hearts)
        {
            if (lifeCopy == 0) { break; }

            if (heart.sprite.name == "Heart_Empty")
            {
                heart.sprite = Resources.Load<Sprite>("Images/Heart_Half");
                lifeCopy--;
                if (lifeCopy == 0) { break; }
            }

            if (heart.sprite.name == "Heart_Half")
            {
                heart.sprite = Resources.Load<Sprite>("Images/Heart");
                lifeCopy--;
                if (lifeCopy == 0) { break; }
            }
        }

        // 실드를 그립니다.
        int shieldCopy = Player.shield;

        foreach (var shield in s_shields) { shield.sprite = Resources.Load<Sprite>("Images/Shield_Empty"); }

        foreach (var shield in s_shields)
        {
            if (shieldCopy == 0) { break; }

            if (shield.sprite.name == "Shield_Empty")
            {
                shield.sprite = Resources.Load<Sprite>("Images/Shield_Half");
                shieldCopy--;
                if (shieldCopy == 0) { break; }
            }

            if (shield.sprite.name == "Shield_Half")
            {
                shield.sprite = Resources.Load<Sprite>("Images/Shield");
                shieldCopy--;
                if (shieldCopy == 0) { break; }
            }
        }
    }

    /// <summary>
    /// <br>행동력에 따라 스프라이트를 변경합니다.</br>
    /// </summary>
    private void UpdateAction()
    {
        Action_Text.text = action.ToString();

        if (action <= 0) { Action_Image.sprite = Resources.Load<Sprite>("Images/Action_0"); }
        else if (action < maxAction * 0.25) { Action_Image.sprite = Resources.Load<Sprite>("Images/Action_1"); }
        else if (action < maxAction * 0.5) { Action_Image.sprite = Resources.Load<Sprite>("Images/Action_2"); }
        else if (action < maxAction * 0.75) { Action_Image.sprite = Resources.Load<Sprite>("Images/Action_3"); }
        else if (action < maxAction * 1) { Action_Image.sprite = Resources.Load<Sprite>("Images/Action_4"); }
        else { Action_Image.sprite = Resources.Load<Sprite>("Images/Action_5"); }
    }
}
