using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LootboxScanner))]
public class Base : MonoBehaviour
{
    [SerializeField] private List<Collector> _collectors;

    private Queue<Lootbox> _detectedLootboxes;
    private LootboxScanner _scanner;
    private Coroutine _scanCoroutine;
    private Coroutine _TakeLootboxCoroutine;
    private int _lootboxesAmount = 0;
    private float _scanCooldown = 1f;
    private float _takeNewLootboxCooldown = 1f;

    private void Start()
    {
        _scanner = GetComponent<LootboxScanner>();
        _detectedLootboxes = new Queue<Lootbox>();
        _scanCoroutine = StartCoroutine(Scan());
        _TakeLootboxCoroutine = StartCoroutine(TakeNewLootbox());
    }

    private void OnDestroy()
    {
        if (_scanCoroutine != null)
            StopCoroutine(_scanCoroutine);

        if (_TakeLootboxCoroutine != null) 
            StopCoroutine(_TakeLootboxCoroutine);
    }

    public void AddLootbox()
    {
        _lootboxesAmount++;
    }

    private IEnumerator Scan()
    {
        var waitForCooldown = new WaitForSeconds(_scanCooldown);

        while (enabled)
        {
            yield return waitForCooldown;

            _scanner.Scan(ref _detectedLootboxes);
        }
    }

    private IEnumerator TakeNewLootbox()
    {
        var waitForOneSecond = new WaitForSeconds(_takeNewLootboxCooldown);

        while (enabled)
        {
            yield return waitForOneSecond;

            if(_detectedLootboxes.Count == 0)
            {
                continue;
            }

            foreach (Collector collector in _collectors)
            {
                Lootbox newLootbox = _detectedLootboxes.Peek();

                if (collector.TryMove(newLootbox.transform))
                {
                    newLootbox.MakeReserved();
                    _detectedLootboxes.Dequeue();
                    break;
                }
            }
        }
    }
}
