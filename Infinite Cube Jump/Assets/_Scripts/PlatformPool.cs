using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPool : Singleton<PlatformPool>
{
    [SerializeField] private Queue<Platform> _platformPool = new Queue<Platform>();
    [SerializeField] private List<Platform> _platformPrefabList;

    protected override void Awake()
        => base.Awake();

    public Platform GetPooledObject()
    {
        if (_platformPool.Count == 0)
            AddPlatform(1);

        Platform retrievedPlatform = _platformPool.Dequeue();
        retrievedPlatform.gameObject.SetActive(true);

        return retrievedPlatform;
    }

    public void ReturnToPool(Platform platform)
    {
        platform.gameObject.SetActive(false);
        _platformPool.Enqueue(platform);
    }

    private void AddPlatform(int count)
    {
        if (count <= 0)
            throw new ArgumentException("The count value cannot be less than or equal to zero.");

        for (int i = 0; i < count; i++)
        {
            Platform randomPlatform = _platformPrefabList[UnityEngine.Random.Range(0, _platformPrefabList.Count)];
            Platform spawnedPlatform = Instantiate(randomPlatform);
            spawnedPlatform.gameObject.SetActive(false);
            _platformPool.Enqueue(spawnedPlatform);
        }
    }
}
