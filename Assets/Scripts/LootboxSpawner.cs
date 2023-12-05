using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootboxSpawner : MonoBehaviour
{
    [SerializeField] private Vector2 _negativeSpawnerEdge;
    [SerializeField] private Vector2 _positiveSpawnerEdge;
    [SerializeField] private Lootbox _lootboxPrefab;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(_lootboxPrefab, new Vector3(Random.Range(_negativeSpawnerEdge.x, _positiveSpawnerEdge.x), 0, Random.Range(_negativeSpawnerEdge.y, _positiveSpawnerEdge.y)), Quaternion.identity,transform);
        }
    }
}
