using System.Collections;
using UnityEngine;

public class Chest : Mob
{
    public override int i { get; set; } = Player.i;
    public override int j { get; set; } = Player.j;
    public override float HP { get; set; } = GameManager.HP_Chest;
    public override float HP_max { get; set; } = GameManager.HP_Chest;
    public override int damage { get; set; } = 0;
    public override int maxAction { get; set; } = 0;
    public override int action { get; set; } = 0;
    public override int range { get; set; } = 0;
    public override int attackCost { get; set; } = 999;
    public override int moveCost { get; set; } = 999;
    public override int visionRange { get; set; } = 0;
    public override RangeType rangeType { get; set; } = RangeType.Chest;
    public GameObject[] items;

    private float height_itemBounce = GameManager.height_ItemBounce;
    private float duration_itemBounce = GameManager.duration_ItemBounce;


    public void DropItem() { StartCoroutine(CorDrop()); }
    private IEnumerator CorDrop()
    {
        Audio.instance.PlaySfx(Audio.Sfx.Enemy_Chest_Pop);
        GameObject obj = Instantiate(items[Random.Range(0, items.Length)], transform.position, Quaternion.identity);
        Item item = obj.GetComponent<Item>();
        Item.Items.Add(item);
        item.sprite = item.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        item.i = i;
        item.j = j;

        Vector3 originalPosition = obj.transform.position; // 원래 위치
        float elapsed = 0f;

        // 위로 튀어오르는 애니메이션 (상승)
        while (elapsed < duration_itemBounce / 2)
        {
            float newY = Mathf.Lerp(originalPosition.y, originalPosition.y + height_itemBounce, elapsed / (duration_itemBounce / 2));
            obj.transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = new Vector3(originalPosition.x, originalPosition.y + height_itemBounce, originalPosition.z);
        elapsed = 0f;

        // 아래로 떨어지는 애니메이션 (하강)
        while (elapsed < duration_itemBounce / 2)
        {
            float newY = Mathf.Lerp(originalPosition.y + height_itemBounce, originalPosition.y, elapsed / (duration_itemBounce / 2));
            obj.transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 원래 위치 복원
        obj.transform.position = originalPosition;
        item.animation_wave.enabled = true;
    }
}
