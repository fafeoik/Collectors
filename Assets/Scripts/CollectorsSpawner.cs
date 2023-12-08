using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorsSpawner : MonoBehaviour
{
    [SerializeField] private Collector _collectorPrefab;

    public Collector Spawn(Base collectorBase, LootboxStorage storage)
    {
        Collector newCollector = Instantiate(_collectorPrefab, transform);
        newCollector.Init(collectorBase, storage);
        return newCollector;
    }
}
