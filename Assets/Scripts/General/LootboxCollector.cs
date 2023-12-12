using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BotMover))]
public class LootboxCollector : MonoBehaviour
{
    [SerializeField] private Transform _trunk;

    private LootboxStorage _storage;
    private BotMover _mover;
    private Lootbox _targetLootbox;
    private Base _base;

    private Coroutine _collectCoroutine;

    public event UnityAction LootboxCollected;

    private void Start()
    {
        _mover = GetComponent<BotMover>();
    }

    private void OnDestroy()
    {
        if (_collectCoroutine != null)
            StopCoroutine(_collectCoroutine);
    }

    public void Init(Base collectorBase, LootboxStorage storage)
    {
        _base = collectorBase;
        _storage = storage;
    }

    public void StartCollecting(Lootbox targetLootbox)
    {
        _targetLootbox = targetLootbox;
        _collectCoroutine = StartCoroutine(CollectLootbox());
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
        _storage.ChangeLootboxAmount(lootboxAmount);
    }

    private IEnumerator CollectLootbox()
    {
        yield return StartCoroutine(_mover.MoveTo(_targetLootbox.transform));

        TakeLootbox();

        yield return StartCoroutine(_mover.MoveTo(_base.transform));

        UnloadLootbox();

        LootboxCollected?.Invoke();
    }
}
