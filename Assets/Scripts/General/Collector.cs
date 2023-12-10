using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseBuilder))]
public class Collector : MonoBehaviour
{
    [SerializeField] private Transform _trunk;
    [SerializeField] private float _speed;
    [SerializeField] private Transform _smallBasePrefab;

    private MoneySystem _moneySystem;
    private Base _base;
    private Lootbox _targetLootbox;
    private Flag _targetFlag;
    private BaseBuilder _baseBuilder;

    private Coroutine _gatherCoroutine;
    private Coroutine _buildCoroutine;

    public bool IsFree { get; private set; } = true;

    private void Start()
    {
        _baseBuilder = GetComponent<BaseBuilder>();
    }

    private void OnDestroy()
    {
        if (_gatherCoroutine != null)
            StopCoroutine(_gatherCoroutine);

        if (_buildCoroutine != null)
            StopCoroutine(_buildCoroutine);
    }

    public void StartGathering(Lootbox targetLootbox)
    {
        _targetLootbox = targetLootbox;
        _gatherCoroutine = StartCoroutine(GatherLootbox());
    }

    public void StartBuild(Flag flag, LootboxScanner scanner)
    {
        _targetFlag = flag;
        _buildCoroutine = StartCoroutine(BuildBase(scanner));
    }

    public void Init(Base collectorBase)
    {
        _base = collectorBase;
        _moneySystem = _base.GetComponent<MoneySystem>();
    }

    private void TakeLootbox()
    {
        _targetLootbox.transform.SetParent(_trunk);
        _targetLootbox.transform.localPosition = new Vector3(0, 0, 0);
        _targetLootbox.transform.localRotation = Quaternion.identity;
    }

    private void UnloadLootbox()
    {
        int lootboxAmount = 1;

        Destroy(_targetLootbox.gameObject);
        _moneySystem.ChangeLootboxAmount(lootboxAmount);
    }

    private IEnumerator BuildBase(LootboxScanner scanner)
    {
        IsFree = false;

        Transform smallBase = Instantiate(_smallBasePrefab, _trunk);

        yield return StartCoroutine(Move(_targetFlag.transform));

        _baseBuilder.Build(this,_targetFlag, scanner);

        Destroy(smallBase.gameObject);

        IsFree = true;
    }

    private IEnumerator GatherLootbox()
    {
        IsFree = false;
        yield return StartCoroutine(Move(_targetLootbox.transform));

        TakeLootbox();

        yield return StartCoroutine(Move(_base.transform));

        UnloadLootbox();

        IsFree = true;
    }

    private IEnumerator Move(Transform target)
    {
        bool isStillMoving = true;

        while (isStillMoving)
        {
            Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);

            transform.position = Vector3.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _speed * Time.deltaTime);

            if (transform.position == target.position)
                isStillMoving = false;

            yield return null;
        }
    }
}
