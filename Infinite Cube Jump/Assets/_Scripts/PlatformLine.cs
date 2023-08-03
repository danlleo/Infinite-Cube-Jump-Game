using System.Collections.Generic;
using UnityEngine;

public class PlatformLine : MonoBehaviour
{
    [SerializeField] private float _movingSpeed = 2f;
    [SerializeField] private float _spawnDistanceFromCenter = 20f;

    private int _platformSpawnCounter;

    private float _platformXGap;

    private Vector3 _lineDirection;
    private Vector3 _spawnPosition;

    private List<Platform> _displayedPlatformList = new();

    private void Start()
    {
        _spawnPosition = transform.position;
        SpawnMultiplePlatforms();
    }

    private void OnEnable()
    {
        Platform.OnPlatformDisappeared += Platform_OnPlatformDisappear;
    }

    private void OnDisable()
    {
        Platform.OnPlatformDisappeared -= Platform_OnPlatformDisappear;
    }

    private void Platform_OnPlatformDisappear(object sender, OnPlatformDisappearedArgs e)
    {
        PlatformLine senderPlatformLine = sender as PlatformLine;

        if (ReferenceEquals(senderPlatformLine, this))
        {
            SpawnSinglePlatform();
            RemoveDispalyedPlatform(e.RecievedPlatform);
        }
    }

    public void Initialize(int platformSpawnCounter, float platformXGap, Vector3 lineDirection)
    {
        _platformSpawnCounter = platformSpawnCounter;
        _platformXGap = platformXGap;
        _lineDirection = lineDirection;
    }

    private void SpawnMultiplePlatforms()
    {
        for (int i = 0; i < _platformSpawnCounter; i++)
        {
            Platform platform = PlatformPool.Instance.GetPooledObject();
            platform.transform.SetParent(transform);
            platform.transform.position = _spawnPosition;
            platform.transform.right = _lineDirection;
            platform.Initalize(this, _lineDirection);

            AddDisplayedPlatform(platform);
            _spawnPosition = platform.GetGapAnchorPosition() + -_lineDirection * _platformXGap;
        }
    }

    private void SpawnSinglePlatform()
    {
        _spawnPosition = GetLastDisplayedPlatform()
            .GetGapAnchorPosition() + -_lineDirection * _platformXGap;
        
        Platform platform = PlatformPool.Instance.GetPooledObject();
        platform.transform.SetParent(transform);
        platform.transform.position = _spawnPosition;
        platform.transform.right = _lineDirection;
        platform.Initalize(this, _lineDirection);

        AddDisplayedPlatform(platform);
    }

    private void AddDisplayedPlatform(Platform platform)
        => _displayedPlatformList.Add(platform);

    private void RemoveDispalyedPlatform(Platform platform)
        => _displayedPlatformList.Remove(platform);

    private Platform GetLastDisplayedPlatform()
        => _displayedPlatformList[^1];
}
