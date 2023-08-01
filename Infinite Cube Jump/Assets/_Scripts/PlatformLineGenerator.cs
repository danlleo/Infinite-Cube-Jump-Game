using System.Collections.Generic;
using UnityEngine;

public class PlatformLineGenerator : MonoBehaviour
{
    [SerializeField] private Transform _levelTransform;
    [SerializeField] private GameObject _linePrefab;

    [SerializeField] private float _zLineAxisGap;
    [SerializeField] private float _xPlatformAxisGap;

    [SerializeField] private int _awakeSpawnCounter;
    [SerializeField] private int _platformsInLineSpawnCounter;

    private Vector3 _spawnPosition;

    private Queue<GameObject> _platformLinesQueue = new();

    private void Awake()
    {
        SpawnLinesOnAwake();
    }

    private void SpawnLinesOnAwake()
    {
        for (int i = 0; i < _awakeSpawnCounter; i++)
        {
            _spawnPosition += Vector3.forward * _zLineAxisGap;
            
            GameObject spawnedLine = Instantiate(_linePrefab, _spawnPosition, Quaternion.identity, _levelTransform);
            Vector3 lineDirection = i % 2 == 0 ? Vector3.right : Vector3.left;

            if (spawnedLine.TryGetComponent(out PlatformLine platformLine))
                platformLine.Initialize(_platformsInLineSpawnCounter, _xPlatformAxisGap, lineDirection);

            _platformLinesQueue.Enqueue(spawnedLine);
        }
    }
}
