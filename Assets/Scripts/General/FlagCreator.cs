using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagCreator : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;

    private Flag _createdFlag;

    public bool IsFlagCreated { get; private set; } = false;

    public void ChangeBool(bool isFlagCreated)
    {
        IsFlagCreated = isFlagCreated;
    }

    public Flag Create(Vector3 raycastPosition)
    {
        if (IsFlagCreated)
        {
            Destroy(_createdFlag.gameObject);
        }

        _createdFlag = Instantiate(_flagPrefab, raycastPosition, Quaternion.identity);
        ChangeBool(true);

        return _createdFlag;
    }
}
