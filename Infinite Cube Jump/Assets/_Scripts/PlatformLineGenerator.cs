using System.Collections.Generic;
using UnityEngine;

public class PlatformLineGenerator : MonoBehaviour
{
    [SerializeField] private Transform _levelTransform;

    [SerializeField] private float _zLineAxisGap;
    [SerializeField] private float _xPlatformAxisGap;

    [SerializeField] private int _awakeSpawnCounter;
    [SerializeField] private int _platformsInLineSpawnCounter;

    private Vector3 _spawnPosition;

    private List<PlatformLine> _displayedPlatformLineList = new();

    private int _platformCount;

    private void Awake()
    {
        SpawnLinesOnAwake();
    }

    private void OnEnable()
    {
        Platform.OnCubeGrounded += Platform_OnCubeGrounded;
    }

    private void OnDisable()
    {
        Platform.OnCubeGrounded -= Platform_OnCubeGrounded;
    }

    private void Platform_OnCubeGrounded(object sender, OnCubeGroundedArgs e)
    {
        PlatformLine firstDisplayedPlatformLine = _displayedPlatformLineList[0];

        // If received platform isn't previous one
        if (!ReferenceEquals(e.GroundedPlatformLine, firstDisplayedPlatformLine))
        {
            firstDisplayedPlatformLine.Reset();
            LinePool.Instance.ReturnToPool(firstDisplayedPlatformLine);
            _displayedPlatformLineList.Remove(firstDisplayedPlatformLine);
            SpawnSingleLine(true);  
        }
    }

    private void SpawnLinesOnAwake()
    {
        for (int i = 0; i < _awakeSpawnCounter; i++)
        {
            SpawnSingleLine();
        }
    }

    private void SpawnSingleLine(bool spawnPlatforms = false)
    {
        _platformCount++;
        _spawnPosition += Vector3.forward * _zLineAxisGap;

        PlatformLine spawnedLine = LinePool.Instance.GetPooledObject();
        Vector3 lineDirection = _platformCount % 2 == 0 ? Vector3.left : Vector3.right;
        
        _displayedPlatformLineList.Add(spawnedLine);

        spawnedLine.transform.position = _spawnPosition;
        spawnedLine.Initialize(_platformsInLineSpawnCounter, _xPlatformAxisGap, lineDirection);

        if (spawnPlatforms)
            spawnedLine.SpawnMultiplePlatforms();
    }
}
