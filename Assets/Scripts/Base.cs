using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LootboxScanner))]
public class Base : MonoBehaviour
{
    [SerializeField] private List<Collector> _collectors;
    [SerializeField] private Transform _detectedLootboxesContainer;

    private LootboxScanner _scanner;

    private Coroutine _getNewLootboxCoroutine;

    private int _lootboxesAmount = 0;

    private void Start()
    {
        _scanner = GetComponent<LootboxScanner>();

        _getNewLootboxCoroutine = StartCoroutine(GetNewLootbox());
    }

    private void OnDestroy()
    {
        StopCoroutine(_getNewLootboxCoroutine);
    }

    public void AddLootbox()
    {
        _lootboxesAmount++;

        print($"Лутбоксов собрано: {_lootboxesAmount}");
    }

    private IEnumerator GetNewLootbox()
    {
        Transform newLootbox;

        var waitForOneSecond = new WaitForSeconds(1f);

        while (enabled)
        {
            yield return waitForOneSecond;

            bool isNewLootboxFound = _scanner.TryScan(out newLootbox);

            if (!isNewLootboxFound)
            {
                continue;
            }

            bool isFreeCollectorFound = false;

            while (!isFreeCollectorFound)
            {
                yield return waitForOneSecond;

                foreach (Collector collector in _collectors)
                {
                    if (collector.TryMove(newLootbox))
                    {
                        newLootbox.SetParent(_detectedLootboxesContainer);
                        isFreeCollectorFound = true;
                        break;
                    }
                }
            }
        }
    }
}
