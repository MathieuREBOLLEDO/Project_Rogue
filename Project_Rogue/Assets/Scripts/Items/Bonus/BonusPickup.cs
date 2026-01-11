using UnityEngine;

public class BonusPickup : MonoBehaviour
{
    [SerializeField] private BonusItemSO bonus;

    private void OnDestroy()
    {
        Debug.Log(bonus + "Is destroy");
    }

    public void OnMouseDown()
    {
        ItemManager.Instance.AddBonus(bonus);
        Destroy(gameObject);
    }
}
