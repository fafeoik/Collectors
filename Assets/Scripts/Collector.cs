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
    private bool _isComingToLootbox = false;
    private bool _isComingHome = false;

    public bool IsFree { get; private set; } = true;


    private void Start()
    {
        _collectorMover = GetComponent<CollectorMover>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (_isComingToLootbox)
        {
            if (collider.TryGetComponent<Lootbox>(out Lootbox lootbox))
            {
                if (_detectedLootbox == lootbox.transform)
                {
                    TakeLootbox();
                }
            }
        }
        else if (_isComingHome)
        {
            if (collider.TryGetComponent<Base>(out Base collectorBase))
            {
                UnloadLootbox(collectorBase);
            }
        }
    }

    public void Move(Transform lootboxPosition)
    {
        IsFree = false;
        _isComingToLootbox = true;

        _detectedLootbox = lootboxPosition;
        _collectorMover.StartMoving(lootboxPosition);
    }

    private void TakeLootbox()
    {
        _isComingHome = true;
        _isComingToLootbox = false;

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

        _isComingHome = false;
        IsFree = true;
    }
}
