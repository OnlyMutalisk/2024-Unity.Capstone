using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditor.PlayerSettings;

public class Mob : MonoBehaviour
{
    public UnityEngine.UI.Slider HP_slider;
    public static List<Mob> Mobs = new List<Mob>();
    public virtual float HP { get; set; }
    public virtual int i { get; set; }
    public virtual int j { get; set; }
    public virtual int maxAction { get; set; }
    public virtual int action { get; set; }
    public virtual int range { get; set; }
    public virtual int damage { get; set; }
    public virtual int attackCost { get; set; }
    public virtual int moveCost { get; set; }
    private GameObject life;
    private List<UnityEngine.UI.Image> hearts = new List<UnityEngine.UI.Image>();


    private void Start()
    {
        // Life UI 를 가져옵니다.
        life = GameObject.Find("Life");
        int count = life.transform.childCount;
        for (int i = 0; i < count; i++) { hearts.Add(life.transform.GetChild(i).gameObject.GetComponent<UnityEngine.UI.Image>()); }

        // 맵의 중심으로 위치를 정렬합니다.
        gameObject.transform.position = new Vector3(0, 0, 0);
        i = (Grid.i / 2) + 1;
        j = (Grid.j / 2) + 1;
        Mobs.Add(this);
        StartCoroutine(CorMove(Player.i, Player.j));

        // 시작 위치로 이동합니다.
        // StartCoroutine(CorMove(i, j));
    }

    /// <summary>
    /// 행동력을 모두 소모할 때 까지 턴을 진행합니다.
    /// </summary>
    public IEnumerator CorPlay()
    {
        action = maxAction;

        while (action > 0)
        {
            // 공격범위 안이면 공격 후 현재 문 탈출, 체비쇼프 거리
            if (Mathf.Max(Mathf.Abs(i - Player.i), Mathf.Abs(j - Player.j)) <= range)
            {
                action -= attackCost;
                yield return StartCoroutine(CorAttack());
                continue;
            }

            // 체비쇼프 거리 1 내의 랜덤한 타일로 이동
            action -= moveCost;
            System.Random rand = new System.Random();
            int[] options = { -1, 0, 1 };
            int move_i = i + options[rand.Next(options.Length)];
            int move_j = j + options[rand.Next(options.Length)];

            yield return StartCoroutine(CorMove(move_i, move_j));
        }
    }

    public IEnumerator CorAttack()
    {
        int origin_i = i;
        int origin_j = j;

        yield return StartCoroutine(CorMove(Player.i, Player.j));
        Player.Life -= damage;
        DrawLife();
        yield return StartCoroutine(CorMove(origin_i, origin_j));
    }

    public IEnumerator CorMove(int i, int j)
    {
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
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, GameManager.speed_Char);
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
}
