using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private CollectorsCreator _collectorsCreator;
    [SerializeField] private int _collectorPrice;
    [SerializeField] private int _basePrice;

    public int CollectorPrice => _collectorPrice;
    public int BasePrice => _basePrice;

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
