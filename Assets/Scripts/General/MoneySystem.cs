using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoneySystem : MonoBehaviour
{
    [SerializeField] private CollectorsCreator _collectorsCreator;
    [SerializeField] private int _collectorPrice;
    [SerializeField] private int _basePrice;
    [SerializeField] private int _lootboxAmount;

    public event UnityAction AmountChanged;

    public int CollectorPrice => _collectorPrice;
    public int BasePrice => _basePrice;

    public int LootboxAmount => _lootboxAmount;

    public void ChangeLootboxAmount(int amount)
    {
        _lootboxAmount += amount;
        AmountChanged?.Invoke();
    }

    public bool TryBuyCollector(int money, out Collector collector)
    {
        if (money >= _collectorPrice)
        {
            collector = _collectorsCreator.Spawn();
            return true;
        }

        collector = null;
        return false;
    }

    public bool TryBuyBase(int money) => money >= _basePrice;
}
