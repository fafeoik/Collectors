using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CollectorMover))]
public class Collector : MonoBehaviour
{
    [SerializeField] private Transform _base;
    [SerializeField] private Transform _trunk;

    private CollectorMover _collectorMover;
    private Transform _detectedLootbox;

    public State State { get; private set; } = State.Free;

    private void Start()
    {
        _collectorMover = GetComponent<CollectorMover>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (State == State.Gather)
        {
            if (collider.TryGetComponent<Lootbox>(out Lootbox lootbox))
            {
                if (_detectedLootbox == lootbox.transform)
                {
                    TakeLootbox();
                }
            }
        }
        else if (State == State.Return)
        {
            if (collider.TryGetComponent<Base>(out Base collectorBase))
            {
                UnloadLootbox(collectorBase);
            }
        }
    }

    public void Move(Transform lootboxPosition)
    {
        State = State.Gather;

        _detectedLootbox = lootboxPosition;
        _collectorMover.StartMoving(lootboxPosition);
    }

    private void TakeLootbox()
    {
        State = State.Return;

        _detectedLootbox.SetParent(_trunk);
        _detectedLootbox.localPosition = new Vector3(0, 0, 0);
        _detectedLootbox.localRotation = Quaternion.identity;

        _collectorMover.StartMoving(_base);
    }

    private void UnloadLootbox(Base collectorBase)
    {
        _collectorMover.StopMoving();

        Destroy(_detectedLootbox.gameObject);
        collectorBase.AddLootbox();

        State = State.Free;
    }
}

public enum State
{
    Free,
    Gather,
    Return
}
