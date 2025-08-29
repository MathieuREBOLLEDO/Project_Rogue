using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootHandler : MonoBehaviour
{
    public static ShootHandler Instance;

    [SerializeField] private int initShootNumber = 10000;
    private int currentShootNumber;

    public IntEvent OnChangeNumberOfShoot;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);
    }

    private void Start()
    {
        currentShootNumber = initShootNumber;
        OnChangeNumberOfShoot.Invoke(initShootNumber);
    }
    public void RemoveShootNumber(int value)
    {
        currentShootNumber -= value;
        currentShootNumber = Mathf.Max(currentShootNumber, 0);
        OnChangeNumberOfShoot.Invoke(currentShootNumber);
    }
    public void AddShootNumber(int value)
    {
        currentShootNumber += value;
        OnChangeNumberOfShoot.Invoke(currentShootNumber);
    }
    public static bool CheckNumberIsLessThan(int number)
    {
        if (Instance.currentShootNumber <= number)
            return true;
        else return false;
    }
}
