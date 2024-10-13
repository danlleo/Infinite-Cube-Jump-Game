using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlatformLine : MonoBehaviour
{
    [SerializeField] private float _movingSpeed = 2f;
    [SerializeField] private float _spawnDistanceFromCenter = 15f;
    [SerializeField] private Color[] _platformColors;

    private int _platformSpawnCounter;

    private float _platformXGap;

    private Vector3 _lineDirection;
    private Vector3 _spawnPosition;

    private readonly List<Platform> _displayedPlatformList = new();

    private void Start()
    {
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

    public void Initialize(int platformSpawnCounter, float platformXGap, Vector3 lineDirection)
    {
        _platformSpawnCounter = platformSpawnCounter;
        _platformXGap = platformXGap;
        _lineDirection = lineDirection;
    }

    public void Reset()
    {
        foreach (Platform platform in _displayedPlatformList)
        {
            platform.transform.SetParent(null);
            PlatformPool.Instance.ReturnToPool(platform);
        }

        _displayedPlatformList.Clear();
    }

    public void SpawnMultiplePlatforms()
    {
        _spawnPosition = transform.position;

        for (int i = 0; i < _platformSpawnCounter; i++)
        {
            Color randomPlatformColor = _platformColors[Random.Range(0, _platformColors.Length - 1)];

            Platform platform = PlatformPool.Instance.GetPooledObject();
            platform.transform.SetParent(transform);
            platform.transform.position = _spawnPosition;
            platform.transform.right = _lineDirection;
            platform.Initialize(this, _lineDirection, randomPlatformColor, i == 0);

            AddDisplayedPlatform(platform);
            _spawnPosition = platform.GetGapAnchorPosition() - _lineDirection * _platformXGap;
        }
    }
    
    private void Platform_OnPlatformDisappear(object sender, OnPlatformDisappearedArgs e)
    {
        PlatformLine senderPlatformLine = sender as PlatformLine;

        if (ReferenceEquals(senderPlatformLine, this))
        {
            RemoveDisplayedPlatform(e.RecievedPlatform);
            _displayedPlatformList[0].SetLeading();
            SpawnSinglePlatform();
        }
    }

    private void SpawnSinglePlatform()
    {
        Color randomPlatformColor = _platformColors[Random.Range(0, _platformColors.Length - 1)];

        _spawnPosition = GetLastDisplayedPlatform()
            .GetGapAnchorPosition() - _lineDirection * _platformXGap;
        
        Platform platform = PlatformPool.Instance.GetPooledObject();
        platform.transform.SetParent(transform);
        platform.transform.position = _spawnPosition;
        platform.transform.right = _lineDirection;
        platform.Initialize(this, _lineDirection, randomPlatformColor, false);

        AddDisplayedPlatform(platform);
    }

    private void AddDisplayedPlatform(Platform platform)
        => _displayedPlatformList.Add(platform);

    private void RemoveDisplayedPlatform(Platform platform)
        => _displayedPlatformList.Remove(platform);

    private Platform GetLastDisplayedPlatform()
        => _displayedPlatformList[^1];
}
