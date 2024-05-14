using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class Mob : MonoBehaviour
{
    public UnityEngine.UI.Slider HP_slider;
    public static List<Mob> Mobs = new List<Mob>();
    public virtual float HP { get; set; }
    public virtual int i { get; set; }
    public virtual int j { get; set; }

    private void Start()
    {
        gameObject.transform.position = new Vector3(0, 0, 0);
        i = (Grid.i / 2) + 1;
        j = (Grid.j / 2) + 1;

        Mobs.Add(this);

        StartCoroutine(CorMove(Player.i, Player.j));
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
}
