using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagCreator : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;

    private Flag _createdFlag;

    private bool _isFlagCreated => _createdFlag != null;

    public bool IsFlagCreated => _isFlagCreated;

    public Flag Create(Vector3 raycastPosition)
    {
        if (IsFlagCreated)
        {
            Destroy(_createdFlag.gameObject);
        }

        _createdFlag = Instantiate(_flagPrefab, raycastPosition, Quaternion.identity);

        return _createdFlag;
    }
}
