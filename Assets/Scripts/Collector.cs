using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CollectorMover), typeof(BaseBuilder))]
public class Collector : MonoBehaviour
{
    [SerializeField] private Transform _trunk;

    private Transform _base;
    private LootboxStorage _storage;
    private CollectorMover _collectorMover;
    private Transform _detectedLootbox;
    private BaseBuilder _baseBuilder;

    public State State { get; private set; } = State.Free;

    private void Start()
    {
        _collectorMover = GetComponent<CollectorMover>();
        _baseBuilder = GetComponent<BaseBuilder>();
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
                UnloadLootbox();
            }
        }
        else if(State == State.Building)
        {
            if(collider.TryGetComponent<Flag>(out Flag flag))
            {
                _baseBuilder.Build(this, flag);

                
            }
        }
    }

    public void Init(Base collectorBase, LootboxStorage storage)
    {
        _base = collectorBase.transform;
        _storage = storage;
    }

    public void SetBuildingState()
    {
        State = State.Building;
    }

    public void SetGatherState()
    {
        State = State.Gather;
    }

    public void Move(Transform lootboxPosition)
    {
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

    private void UnloadLootbox()
    {
        _collectorMover.StopMoving();

        Destroy(_detectedLootbox.gameObject);
        _storage.ChangeLootboxAmount(1);

        State = State.Free;
    }
}

public enum State
{
    Free,
    Gather,
    Return,
    Building
}
