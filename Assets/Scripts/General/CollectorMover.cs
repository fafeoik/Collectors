using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    public IEnumerator MoveTo(Transform target)
    {
        bool isStillMoving = true;

        while (isStillMoving)
        {
            Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);

            transform.position = Vector3.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _speed * Time.deltaTime);

            if (transform.position == target.position)
                isStillMoving = false;

            yield return null;
        }
    }
}
