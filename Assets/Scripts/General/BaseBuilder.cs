using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseBuilder : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;
    [SerializeField] private Transform _smallBasePrefab;
    [SerializeField] private Transform _trunk;

    private Flag _targetFlag;
    private CollectorMover _mover;
    private LootboxScanner _scanner;
    private Coroutine _buildCoroutine;

    public event UnityAction BuildCompleted;

    private void Start()
    {
        _mover = GetComponent<CollectorMover>();
    }

    private void OnDestroy()
    {
        if (_buildCoroutine != null)
            StopCoroutine(_buildCoroutine);
    }

    public void Init(LootboxScanner scanner)
    {
        _scanner = scanner;
    }

    public void StartBuilding(Flag targetFlag)
    {
        _targetFlag = targetFlag;
        _buildCoroutine = StartCoroutine(BuildBase());
    }

    private IEnumerator BuildBase()
    {
        Transform smallBase = Instantiate(_smallBasePrefab, _trunk);

        yield return StartCoroutine(_mover.MoveTo(_targetFlag.transform));

        Build(_targetFlag);

        Destroy(smallBase.gameObject);
    }

    private void Build(Flag flag)
    {
        Base newBase = Instantiate(_basePrefab, flag.transform.position, Quaternion.identity);
        newBase.GetScanner(_scanner);

        Transform _collectorsContainer = newBase.GetComponentInChildren<CollectorsCreator>().transform;

        Collector collector = GetComponent<Collector>();

        collector.transform.SetParent(_collectorsContainer);
        newBase.AddCollector(collector);

        Destroy(flag.gameObject);

        BuildCompleted?.Invoke();
    }
}
