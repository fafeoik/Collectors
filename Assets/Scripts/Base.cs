using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LootboxScanner), typeof(LootboxStorage), typeof(FlagCreator))]
public class Base : MonoBehaviour
{
    [SerializeField] private Transform _collectorsParent;
    [SerializeField] private float _scanCooldown;
    [SerializeField] private float _takeNewLootboxCooldown;
    [SerializeField] private Collector _collectorPrefab;
    [SerializeField] private CollectorsSpawner _collectorsSpawner;

    private List<Collector> _collectors;
    private Queue<Lootbox> _detectedLootboxes;
    private LootboxScanner _scanner;
    private LootboxStorage _storage;
    private FlagCreator _flagCreator;
    private Transform _flag;
    private Coroutine _scanCoroutine;
    private Coroutine _takeLootboxCoroutine;
    private Coroutine _findCollectorCoroutine;
    private int _newCollectorPrice = 3;
    private int _newBasePrice = 5;
    private int _maxCollectorsAmount = 3;

    private void Start()
    {
        _scanner = GetComponent<LootboxScanner>();
        _storage = GetComponent<LootboxStorage>();
        _flagCreator = GetComponent<FlagCreator>();
        _detectedLootboxes = new Queue<Lootbox>();

        _collectors = new List<Collector>();

        for (int i = 0; i < _collectorsParent.childCount; i++)
        {
            Collector collector = _collectorsParent.GetChild(i).GetComponent<Collector>();
            collector.Init(this, _storage);
            _collectors.Add(collector);
        }

        _scanCoroutine = StartCoroutine(Scan());
        _takeLootboxCoroutine = StartCoroutine(GatherLootbox());

        _storage.AmountChanged += BuyCollector;
    }

    private void OnDestroy()
    {
        if (_scanCoroutine != null)
            StopCoroutine(_scanCoroutine);

        if (_takeLootboxCoroutine != null)
            StopCoroutine(_takeLootboxCoroutine);

        if(_findCollectorCoroutine != null)
            StopCoroutine(_findCollectorCoroutine);

        _storage.AmountChanged -= BuyCollector;
    }

    public void CreateFlag(Vector3 position)
    {
        _flag = _flagCreator.Create(position);
        StartBuilding();
    }

    private void BuyCollector()
    {
        if (_storage.LootboxAmount >= _newCollectorPrice && _collectors.Count < _maxCollectorsAmount)
        {
            _collectors.Add(_collectorsSpawner.Spawn(this, _storage));
            _storage.ChangeLootboxAmount(-_newCollectorPrice);
        }
    }

    private void StartBuilding()
    {
        _storage.AmountChanged -= BuyCollector;
        _storage.AmountChanged += BuildBase;
    }

    private void BuildBase()
    {
        if (_storage.LootboxAmount >= _newBasePrice)
        {
            _findCollectorCoroutine = StartCoroutine(FindFreeCollector());
        }
    }

    private IEnumerator FindFreeCollector()
    {
        bool isWorking = true;
        float collectorSearchCooldown = 1f;
        var waitForCooldown = new WaitForSeconds(collectorSearchCooldown);

        while (isWorking)
        {
            yield return waitForCooldown;

            foreach (Collector collector in _collectors)
            {
                if (collector.State == State.Free)
                {
                    collector.SetBuildingState();
                    collector.Move(_flag);
                    isWorking = false;
                    break;
                }
            }
        }
    }

    private IEnumerator Scan()
    {
        var waitForCooldown = new WaitForSeconds(_scanCooldown);

        while (enabled)
        {
            yield return waitForCooldown;

            _scanner.Scan(_detectedLootboxes);
        }
    }

    private IEnumerator GatherLootbox()
    {
        var waitForOneSecond = new WaitForSeconds(_takeNewLootboxCooldown);

        while (enabled)
        {
            yield return waitForOneSecond;

            foreach (Collector collector in _collectors)
            {
                if (_detectedLootboxes.Count == 0)
                {
                    continue;
                }

                Lootbox newLootbox = _detectedLootboxes.Peek();

                if (collector.State == State.Free)
                {
                    _detectedLootboxes.Dequeue();
                    collector.SetGatherState();
                    collector.Move(newLootbox.transform);
                    newLootbox.MakeReserved();
                }
            }
        }
    }
}
