using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LootboxStorage : MonoBehaviour
{
    public int LootboxAmount { get; private set; } = 0;
    public event UnityAction AmountChanged;

    public void ChangeLootboxAmount(int amount)
    {
        LootboxAmount += amount;
        AmountChanged?.Invoke();
    }
}
