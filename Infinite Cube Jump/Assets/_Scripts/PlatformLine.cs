using System.Collections.Generic;
using UnityEngine;

public class PlatformLine : MonoBehaviour
{
    [SerializeField] private List<GameObject> _platformVariants;

    private int _platformSpawnCounter;

    private float _platformXGap;
    private float _previousPlatformXValue;

    private Vector3 _lineDirection;
    private Vector3 _spawnPosition;

    private void Start()
    {
        SpawnPlatforms();
    }

    public void Initialize(int platformSpawnCounter, float platformXGap, Vector3 lineDirection)
    {
        _platformSpawnCounter = platformSpawnCounter;
        _platformXGap = platformXGap;
        _lineDirection = lineDirection;   
    }

    private void SpawnPlatforms()
    {
        for (int i = 0; i < _platformSpawnCounter; i++)
        {
            _spawnPosition += transform.forward + _lineDirection * _platformXGap;

            GameObject randomPlatform = _platformVariants[Random.Range(0, _platformVariants.Count)];
            GameObject spawnedPlatform = Instantiate(randomPlatform, _spawnPosition, Quaternion.identity, transform);
        }
    }
}
