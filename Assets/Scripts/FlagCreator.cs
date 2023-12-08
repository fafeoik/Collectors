using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagCreator : MonoBehaviour
{
    [SerializeField] private Transform _flagPrefab;

    private Transform _createdFlag;

    public bool IsFlagCreated { get; private set; } = false;

    public Transform Create(Vector3 raycastPosition)
    {
        if (IsFlagCreated)
        {
            Destroy(_createdFlag.gameObject);
        }

        _createdFlag = Instantiate(_flagPrefab, raycastPosition, Quaternion.identity);
        IsFlagCreated = true;

        return _createdFlag;
    }
}
