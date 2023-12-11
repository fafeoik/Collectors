using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootboxScanner : MonoBehaviour
{
    private Vector3 _halfExtents = new Vector3(25, 5, 25);
    private Queue<Lootbox> _detectedLootboxes = new Queue<Lootbox>();
    private List<Lootbox> _reservedLootboxes = new List<Lootbox>();

    public bool TryGetLootbox(out Lootbox lootbox)
    {
        Scan();

        if (_detectedLootboxes.Count > 0)
        {
            lootbox = _detectedLootboxes.Dequeue();
            _reservedLootboxes.Add(lootbox);
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
                 && _detectedLootboxes.Contains(foundLootbox) == false
                 && _reservedLootboxes.Contains(foundLootbox) == false)
            {
                _detectedLootboxes.Enqueue(foundLootbox);
            }

            CheckReservedRelevance();
        }
    }

    private void CheckReservedRelevance()
    {
        for (int i = _reservedLootboxes.Count - 1; i >= 0; i--)
        {
            if (_reservedLootboxes[i] == null)
            {
                _reservedLootboxes.Remove(_reservedLootboxes[i]);
            }
        }
    }
}
