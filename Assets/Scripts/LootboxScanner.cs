using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootboxScanner : MonoBehaviour
{
    [SerializeField] private float _scanCooldown;

    private Vector3 _halfExtents = new Vector3(25, 5, 25);
    private Queue<Lootbox> _detectedLootboxes = new Queue<Lootbox>();

    private Coroutine _scanCoroutine;

    private void Start()
    {
        _scanCoroutine = StartCoroutine(Scan());
    }

    private void OnDestroy()
    {
        if (_scanCoroutine != null)
            StopCoroutine(_scanCoroutine);
    }

    public bool TryGetLootboxes(out Lootbox lootbox)
    {
        if (_detectedLootboxes.Count > 0)
        {
            lootbox = _detectedLootboxes.Dequeue();
            return true;
        }
        else
        {
            lootbox = null;
            return false;
        }
    }

    private IEnumerator Scan()
    {
        var waitForCooldown = new WaitForSeconds(_scanCooldown);

        while (enabled)
        {
            Collider[] foundColliders = Physics.OverlapBox(transform.position, _halfExtents);

            foreach (Collider collider in foundColliders)
            {
                if (collider.TryGetComponent<Lootbox>(out Lootbox foundLootbox)
                     && foundLootbox.IsDetected == false
                     && _detectedLootboxes.Contains(foundLootbox) == false)
                {
                    _detectedLootboxes.Enqueue(foundLootbox);
                    foundLootbox.MakeDetected();
                }
            }

            yield return waitForCooldown;
        }
    }
}
