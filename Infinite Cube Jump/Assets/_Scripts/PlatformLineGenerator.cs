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

    private List<PlatformLine> _displayedPlatformLine = new();

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
        // If received platform isn't previous one
        if (!ReferenceEquals(e.GroundedPlatformLine, _displayedPlatformLine[0]))
        {
            LinePool.Instance.ReturnToPool(_displayedPlatformLine[0]);
            _displayedPlatformLine.Remove(_displayedPlatformLine[0]);
            SpawnSingleLine();
        }
    }

    private void SpawnLinesOnAwake()
    {
        for (int i = 0; i < _awakeSpawnCounter; i++)
        {
            SpawnSingleLine();
        }
    }

    private void SpawnSingleLine()
    {
        _platformCount++;
        _spawnPosition += Vector3.forward * _zLineAxisGap;

        PlatformLine spawnedLine = LinePool.Instance.GetPooledObject();
        Vector3 lineDirection = _platformCount % 2 == 0 ? Vector3.right : Vector3.left;
        
        _displayedPlatformLine.Add(spawnedLine);

        spawnedLine.transform.position = _spawnPosition;
        spawnedLine.Initialize(_platformsInLineSpawnCounter, _xPlatformAxisGap, lineDirection);
    }
}
