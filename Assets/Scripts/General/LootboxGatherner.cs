using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LootboxGatherner : MonoBehaviour
{
    [SerializeField] private Transform _trunk;

    private MoneySystem _moneySystem;
    private CollectorMover _mover;
    private Lootbox _targetLootbox;
    private Base _base;

    private Coroutine _gatherCoroutine;

    public event UnityAction LootboxGathered;

    private void Start()
    {
        _mover = GetComponent<CollectorMover>();
    }

    private void OnDestroy()
    {
        if (_gatherCoroutine != null)
            StopCoroutine(_gatherCoroutine);
    }

    public void Init(Base collectorBase)
    {
        _base = collectorBase;
        _moneySystem = _base.GetComponent<MoneySystem>();
    }

    public void StartGathering(Lootbox targetLootbox)
    {
        _targetLootbox = targetLootbox;
        _gatherCoroutine = StartCoroutine(GatherLootbox());
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

    private IEnumerator GatherLootbox()
    {
        yield return StartCoroutine(_mover.MoveTo(_targetLootbox.transform));

        TakeLootbox();

        yield return StartCoroutine(_mover.MoveTo(_base.transform));

        UnloadLootbox();

        LootboxGathered?.Invoke();
    }
}
