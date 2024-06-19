using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditor.PlayerSettings;
using Image = UnityEngine.UI.Image;

public class Mob : MonoBehaviour
{
    public UnityEngine.UI.Slider HP_slider;
    public static List<Mob> Mobs = new List<Mob>();
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
    public Image Action_Image;
    public TextMeshProUGUI Action_Text;
    public GameObject Zzz;
    private List<UnityEngine.UI.Image> hearts = new List<UnityEngine.UI.Image>();

    private void Start()
    {
        // Life UI 를 가져옵니다.
        life = GameObject.Find("Life");
        int count = life.transform.childCount;
        for (int i = 0; i < count; i++) { hearts.Add(life.transform.GetChild(i).gameObject.GetComponent<UnityEngine.UI.Image>()); }

        // Vision 사이즈를 조절한 후 SetActive(false) 합니다.
        float offset = Grid.cellSize * (visionRange * 2 + 1) + Grid.spacing * (visionRange * 2);
        vision.GetComponent<RectTransform>().sizeDelta = new Vector2(offset, offset);
        vision.SetActive(false);

        Mobs.Add(this);
    }

    private void Update()
    {
        // 체력 바를 갱신합니다.
        HP_slider.value = HP / HP_max;

        // 몬스터가 깨어나면, Zzz 애니메이션을 비활성화 합니다.
        if (isSleep == false) { Zzz.SetActive(false); }

        UpdateAction();
    }

    /// <summary>
    /// 행동력을 모두 소모할 때 까지 턴을 진행합니다.
    /// </summary>
    public IEnumerator CorPlay()
    {
        // 시야범위 이내, 혹은 최대 체력이 아니면 활동 시작
        if (Mathf.Max(Mathf.Abs(i - Player.i), Mathf.Abs(j - Player.j)) <= visionRange | HP != HP_max) { isSleep = false; }

        List<Tile> path = A_Star.PathFind(Grid.GetTile(i, j).GetComponent<Tile>(), rangeType);

        while (action >= Mathf.Min(moveCost, attackCost) && isSleep == false)
        {
            // 공격범위 안이면 공격 후 현재 문 탈출, 체비쇼프 거리
            if (Mathf.Max(Mathf.Abs(i - Player.i), Mathf.Abs(j - Player.j)) <= range)
            {
                action -= attackCost;
                yield return StartCoroutine(CorAttack());
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

        action = maxAction;
    }

    public IEnumerator CorAttack()
    {
        int origin_i = i;
        int origin_j = j;

        Player.Life -= (int)Tile.CalcDamage(damage, Grid.GetTile(i, j), Grid.GetTile(Player.i, Player.j));
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

    private void DrawLife()
    {
        int lifeCopy = Player.Life;

        foreach (var heart in hearts) { heart.sprite = Resources.Load<Sprite>("Images/Heart_Empty"); }

        foreach (var heart in hearts)
        {
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
