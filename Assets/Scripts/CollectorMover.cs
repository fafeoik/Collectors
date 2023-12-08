using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Transform _currentTarget;
    private Coroutine _moveCoroutine;

    private void OnDestroy()
    {
        StopMoving();
    }

    public void StopMoving()
    {
        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);
    }

    public void StartMoving(Transform target)
    {
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
        }

        _currentTarget = target;

        _moveCoroutine = StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (enabled)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_currentTarget.transform.position - transform.position);

            transform.position = Vector3.MoveTowards(transform.position, _currentTarget.position, _speed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _speed * Time.deltaTime);

            yield return null;
        }
    }
}
