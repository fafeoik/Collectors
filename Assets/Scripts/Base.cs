using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LootboxScanner))]
public class Base : MonoBehaviour
{
    [SerializeField] private List<Collector> _collectors;

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
        if (_getNewLootboxCoroutine != null)
            StopCoroutine(_getNewLootboxCoroutine);
    }

    public void AddLootbox()
    {
        _lootboxesAmount++;
    }

    private IEnumerator GetNewLootbox()
    {
        Lootbox newLootbox;

        var waitForOneSecond = new WaitForSeconds(1f);

        while (enabled)
        {
            yield return waitForOneSecond;

            bool isNewLootboxFound = _scanner.TryScan(out newLootbox);

            if (isNewLootboxFound == false)
            {
                continue;
            }

            bool isFreeCollectorFound = false;

            while (isFreeCollectorFound == false)
            {
                yield return waitForOneSecond;

                foreach (Collector collector in _collectors)
                {
                    if (collector.TryMove(newLootbox.transform))
                    {
                        newLootbox.MakeReserved();
                        isFreeCollectorFound = true;
                        break;
                    }
                }
            }
        }
    }
}
