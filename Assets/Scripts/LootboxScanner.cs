using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootboxScanner : MonoBehaviour
{
    [SerializeField] private Transform _lootboxContainer;

    public bool TryScan(out Transform newLootbox)
    {
        if (_lootboxContainer.childCount > 0)
        {
            int firstElementIndex = 0;

            newLootbox = _lootboxContainer.GetChild(firstElementIndex);

            return true;
        }

        newLootbox = null;
        return false;
    }
}
