using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BotMover))]
public class BaseBuilder : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;
    [SerializeField] private Transform _smallBasePrefab;
    [SerializeField] private Transform _trunk;

    private Flag _targetFlag;
    private BotMover _mover;
    private LootboxScanner _scanner;
    private Coroutine _buildCoroutine;

    public event UnityAction<Base> BuildCompleted;

    private void Start()
    {
        _mover = GetComponent<BotMover>();
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

        Transform _botsContainer = newBase.GetComponentInChildren<BotsCreator>().transform;

        transform.SetParent(_botsContainer);

        Destroy(flag.gameObject);

        BuildCompleted?.Invoke(newBase);
    }
}
