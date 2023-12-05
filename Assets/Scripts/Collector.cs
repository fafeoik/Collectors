using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CollectorMover))]
public class Collector : MonoBehaviour
{
    [SerializeField] private Transform _base;
    [SerializeField] private Transform _trunk;

    private CollectorMover _collectorMover;
    private Lootbox _carriedLootbox;
    private Transform _detectedLootbox;

    private bool _isFree = true;
    private bool _isComingToLootbox = false;
    private bool _isComingHome = false;

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
                if(_detectedLootbox == lootbox.transform)
                {
                    _carriedLootbox = lootbox;
                    _carriedLootbox.transform.SetParent(_trunk);
                    _carriedLootbox.transform.localPosition = new Vector3(0, 0, 0);
                    _carriedLootbox.transform.localRotation = Quaternion.identity;

                    _isComingHome = true;
                    _isComingToLootbox = false;

                    _collectorMover.StartMoving(_base);
                }
            }
        }
        else if (_isComingHome)
        {
            if(collider.TryGetComponent<Base>(out Base collectorBase))
            {
                _isComingHome = false;
                _isFree = true;

                _collectorMover.StopMoving();

                Destroy(_carriedLootbox.gameObject);
                collectorBase.AddLootbox();
            }
        }
    }

    public bool TryMove(Transform lootboxPosition)
    {
        if (!_isFree)
        {
            return false;
        }
        else
        {
            _detectedLootbox = lootboxPosition;
            _isFree = false;
            _isComingToLootbox = true;

            _collectorMover.StartMoving(lootboxPosition);
            return true;
        }
    }
}
