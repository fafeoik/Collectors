using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorsCreator : MonoBehaviour
{
    [SerializeField] private Collector _collectorPrefab;

    public Collector Spawn()
    {
        Collector newCollector = Instantiate(_collectorPrefab, transform);
        return newCollector;
    }
}
