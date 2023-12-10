using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootboxSpawner : MonoBehaviour
{
    [SerializeField] private Vector2 _negativeSpawnerEdge;
    [SerializeField] private Vector2 _positiveSpawnerEdge;
    [SerializeField] private Lootbox _lootboxPrefab;
    [SerializeField] private float _spawnRate;

    private Coroutine _spawnCoroutine;

    private void Start()
    {
        _spawnCoroutine = StartCoroutine(Spawn());
    }

    private void OnDestroy()
    {
        if (_spawnCoroutine != null)
            StopCoroutine(_spawnCoroutine);
    }

    private IEnumerator Spawn()
    {
        float abscissaSpawnPosition;
        float applicateSpawnPosition;
        WaitForSeconds waitForNextSpawn = new WaitForSeconds(_spawnRate);

        while (enabled)
        {
            yield return waitForNextSpawn;

            abscissaSpawnPosition = Random.Range(_negativeSpawnerEdge.x, _positiveSpawnerEdge.x);
            applicateSpawnPosition = Random.Range(_negativeSpawnerEdge.y, _positiveSpawnerEdge.y);

            Vector3 spawnPosition = new Vector3(abscissaSpawnPosition, 0, applicateSpawnPosition);

            Instantiate(_lootboxPrefab, spawnPosition, Quaternion.identity, transform);           
        }
    }
}
