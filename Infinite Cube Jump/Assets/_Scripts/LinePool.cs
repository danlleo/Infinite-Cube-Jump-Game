using System;
using System.Collections.Generic;
using UnityEngine;

public class LinePool : Singleton<LinePool>
{
    [SerializeField] private Queue<PlatformLine> _platformLinePool = new Queue<PlatformLine>();
    [SerializeField] private PlatformLine _platformLinePrefab;

    protected override void Awake()
        => base.Awake();

    public PlatformLine GetPooledObject()
    {
        if (_platformLinePool.Count == 0)
            AddPlatformLine(1);

        PlatformLine retrievedPlatform = _platformLinePool.Dequeue();
        retrievedPlatform.gameObject.SetActive(true);

        return retrievedPlatform;
    }

    public void ReturnToPool(PlatformLine platformLine)
    {
        platformLine.gameObject.SetActive(false);
        _platformLinePool.Enqueue(platformLine);
    }

    private void AddPlatformLine(int count)
    {
        if (count <= 0)
            throw new ArgumentException("The count value cannot be less than or equal to zero.");

        for (int i = 0; i < count; i++)
        {
            PlatformLine platformLine = Instantiate(_platformLinePrefab);
            platformLine.gameObject.SetActive(true);
            _platformLinePool.Enqueue(platformLine);
        }
    }
}
