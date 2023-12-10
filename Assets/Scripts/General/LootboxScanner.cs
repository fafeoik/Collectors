using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootboxScanner : MonoBehaviour
{
    [SerializeField] private float _scanCooldown;

    private Vector3 _halfExtents = new Vector3(25, 5, 25);
    private Queue<Lootbox> _detectedLootboxes = new Queue<Lootbox>();

    public bool TryGetLootbox(out Lootbox lootbox)
    {
        Scan();

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

    private void Scan()
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
    }
}

