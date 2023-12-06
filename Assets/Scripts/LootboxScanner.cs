using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootboxScanner : MonoBehaviour
{
    [SerializeField] private Transform _overlapCubeCenter;

    private Vector3 _halfExtents = new Vector3(25,10,25);

    public void Scan(ref Queue<Lootbox> _detectedLootbox)
    {
        Collider[] foundColliders = Physics.OverlapBox(_overlapCubeCenter.position, _halfExtents);

        foreach(Collider collider in foundColliders)
        {
            if(collider.TryGetComponent<Lootbox>(out Lootbox foundLootbox))
            {
                if (foundLootbox.IsReserved == false)
                {
                    _detectedLootbox.Enqueue(foundLootbox);
                }
            }
        }
    }
}
