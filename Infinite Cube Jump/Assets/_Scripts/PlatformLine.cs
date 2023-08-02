using UnityEngine;

public class PlatformLine : MonoBehaviour
{
    [SerializeField] private float _movingSpeed = 2f;
    [SerializeField] private float _spawnDistanceFromCenter = 20f;

    private int _platformSpawnCounter;

    private float _platformXGap;

    private Vector3 _lineDirection;
    private Vector3 _spawnPosition;

    private void Start()
    {
        _spawnPosition = transform.position;
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
            Platform platform = PlatformPool.Instance.GetPooledObject();
            platform.transform.SetParent(transform);
            platform.transform.position = _spawnPosition;

            _spawnPosition = platform.GetGapAnchorPosition() + -_lineDirection * _platformXGap;
        }
    }
}
