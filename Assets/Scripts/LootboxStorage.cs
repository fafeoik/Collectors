using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LootboxStorage : MonoBehaviour
{
    [SerializeField] private int _lootboxAmount;

    public event UnityAction AmountChanged;

    public int LootboxAmount => _lootboxAmount;

    public void ChangeLootboxAmount(int amount)
    {
        _lootboxAmount += amount;
        AmountChanged?.Invoke();
    }
}
