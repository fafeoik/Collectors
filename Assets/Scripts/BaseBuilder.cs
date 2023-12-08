using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuilder : MonoBehaviour
{
    [SerializeField] private Transform _basePrefab;

    public void Build(Collector collector, Flag flag)
    {
        Transform newBase = Instantiate(_basePrefab, flag.transform.position, Quaternion.identity);

        Transform _collectorsContainer = newBase.GetComponentInParent<CollectorsSpawner>().transform;

        collector.transform.SetParent(_collectorsContainer);

        Destroy(flag.gameObject);
    }
}
