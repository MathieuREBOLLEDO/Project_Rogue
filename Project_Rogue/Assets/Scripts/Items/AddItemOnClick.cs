using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItemOnClick : MonoBehaviour
{
    [SerializeField] private ItemSO [] listItems= new ItemSO[3];
    private int nb = 0;
    public void AddItem()
    {
        if (nb < 3)
        {
            ItemManager.Instance.AddItem(listItems[nb]);
            nb++;
        }
    }
}
