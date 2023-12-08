using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastHandler : MonoBehaviour
{
    private Camera _mainCamera;
    private int _raycastDistance = 100;
    private Base _collectorBase;

    private void Start()
    {
        _mainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Handle();
        }
    }

    private void Handle()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _raycastDistance))
        {
            if (hit.transform.TryGetComponent<Base>(out Base collectorBase))
            {
                _collectorBase = collectorBase;
            }
            else if (hit.transform.TryGetComponent<Ground>(out Ground ground))
            {
                if (_collectorBase != null)
                {
                    _collectorBase.CreateFlag(hit.point);
                }
            }
        }
    }
}
