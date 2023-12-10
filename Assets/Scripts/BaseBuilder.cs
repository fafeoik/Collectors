using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuilder : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;

    public void Build(Collector collector, Flag flag, LootboxScanner scanner)
    {
        Base newBase = Instantiate(_basePrefab, flag.transform.position, Quaternion.identity);
        newBase.GetScanner(scanner);

        Transform _collectorsContainer = newBase.GetComponentInChildren<CollectorsCreator>().transform;

        collector.transform.SetParent(_collectorsContainer);
        newBase.AddCollector(collector);

        Destroy(flag.gameObject);
    }
}
