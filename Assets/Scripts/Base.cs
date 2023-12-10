using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LootboxStorage), typeof(FlagCreator), typeof(Shop))]
public class Base : MonoBehaviour
{
    [SerializeField]private LootboxScanner _scanner;

    private List<Collector> _collectors = new List<Collector>();
    private LootboxStorage _storage;
    private FlagCreator _flagCreator;
    private Flag _flag;
    private Shop _shop;

    private Coroutine _takeLootboxCoroutine;
    private Coroutine _findCollectorCoroutine;

    private int _maxCollectorsAmount = 3;
    private bool _isCollectorNeededToBuild = false;

    private void Start()
    {
        GetRequiredComponents();

        _takeLootboxCoroutine = StartCoroutine(MakeCollectorsGather());

        _storage.AmountChanged += OnCollectorMoneyEnough;
        OnCollectorMoneyEnough();
    }

    private void OnDestroy()
    {
        if (_takeLootboxCoroutine != null)
            StopCoroutine(_takeLootboxCoroutine);

        if (_findCollectorCoroutine != null)
            StopCoroutine(_findCollectorCoroutine);

        _storage.AmountChanged -= OnCollectorMoneyEnough;
    }

    public void GetScanner(LootboxScanner scanner)
    {
        _scanner = scanner;
    }

    private void GetRequiredComponents()
    {
        _storage = GetComponent<LootboxStorage>();
        _flagCreator = GetComponent<FlagCreator>();
        _shop = GetComponent<Shop>();
    }

    private void OnCollectorMoneyEnough()
    {
        if (_collectors.Count < _maxCollectorsAmount && _shop.TryBuyCollector(_storage.LootboxAmount, out Collector collector))
        {
            AddCollector(collector);
            _storage.ChangeLootboxAmount(-_shop.CollectorPrice);
        }
    }

    public void AddCollector(Collector collector)
    {
        collector.Init(this);
        _collectors.Add(collector);
    }

    public void StartBuilding(Vector3 position)
    {
        int collectorsRequiredToBuild = 2;

        if(_collectors.Count >= collectorsRequiredToBuild)
        {
            if (_flagCreator.IsFlagCreated == false)
            {
                _storage.AmountChanged -= OnCollectorMoneyEnough;
                _storage.AmountChanged += OnBaseMoneyEnough;
            }

            _flag = _flagCreator.Create(position);
        }
    }

    private void OnBaseMoneyEnough()
    {
        if (_shop.TryBuyBase(_storage.LootboxAmount))
        {
            _findCollectorCoroutine = StartCoroutine(MakeCollectorBuild());
            _isCollectorNeededToBuild = true;
        }
    }

    private Queue<Collector> FindFreeCollectors()
    {
        IEnumerable<Collector> freeCollectors = _collectors.Where(collector => collector.IsFree);
        Queue<Collector> queueCollectors = new Queue<Collector>();

        foreach (Collector collector in freeCollectors)
        {
            queueCollectors.Enqueue(collector);
        }

        return queueCollectors;
    }

    private IEnumerator MakeCollectorBuild()
    {
        bool isWorking = true;
        float collectorSearchCooldown = 0.5f;
        var waitForCooldown = new WaitForSeconds(collectorSearchCooldown);

        while (isWorking)
        { 
            Queue<Collector> freeCollectors = FindFreeCollectors();

            if(freeCollectors.Count > 0)
            {
                Collector collector = freeCollectors.Dequeue();
                collector.StartBuild(_flag, _scanner);
                _collectors.Remove(collector);
                _storage.ChangeLootboxAmount(-_shop.BasePrice);

                _flagCreator.ChangeBool(false);

                _isCollectorNeededToBuild = false;

                _storage.AmountChanged -= OnBaseMoneyEnough;
                _storage.AmountChanged += OnCollectorMoneyEnough;

                isWorking = false;
            }

            yield return waitForCooldown;
        }
    }

    private IEnumerator MakeCollectorsGather()
    {
        float _takeNewLootboxCooldown = 0.5f;
        var waitForCooldown = new WaitForSeconds(_takeNewLootboxCooldown);

        while (enabled)
        {
            yield return waitForCooldown;

            if (_isCollectorNeededToBuild)
            {
                continue;
            }

            Queue<Collector> freeCollectors = FindFreeCollectors();

            for (int i = freeCollectors.Count - 1; i >= 0; i--)
            {
                if (_scanner.TryGetLootboxes(out Lootbox lootbox))
                {
                    Collector freeCollector = freeCollectors.Dequeue();
                    freeCollector.StartGathering(lootbox);
                }
            }
        }
    }
}
