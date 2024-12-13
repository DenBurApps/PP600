using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WoodLogSpawner : ObjectPool<WoodLog>
{
    [SerializeField] private WoodLog _prefab;
    [SerializeField] private int _spawnInterval = 3;
    [SerializeField] private Vector2 _spawnPosition;

    private IEnumerator _spawnCoroutine;
    private List<WoodLog> _spawnedObjects = new List<WoodLog>();

    private void Awake()
    {
        Initalize(_prefab);
    }

    public void EnableSpawn()
    {
        if (_spawnCoroutine == null)
            _spawnCoroutine = StartSpawning();

        StartCoroutine(_spawnCoroutine);
    }

    public void StopSpawning()
    {
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
            ReturnAllObjects();
        }
    }

    public void ReturnAllObjects()
    {
        if(_spawnedObjects.Count <= 0)
            return;

        List<WoodLog> objectsToRetuns = new List<WoodLog>(_spawnedObjects);
        
        foreach (var log in objectsToRetuns)
        {
            ReturnObject(log);
        }
    }

    public void StopAllLogs()
    {
        foreach (var log in _spawnedObjects)
        {
            log.StopMovement();
        }
    }

    public void EnableAllLogs()
    {
        foreach (var log in _spawnedObjects)
        {
            log.EnableMovement();
        }
    }

    public void ReturnObject(WoodLog woodLog)
    {
        if (woodLog == null)
            throw new ArgumentNullException(nameof(woodLog));

        PutObject(woodLog);

        if (_spawnedObjects.Contains(woodLog))
        {
            _spawnedObjects.Remove(woodLog);
        }
    }

    private IEnumerator StartSpawning()
    {
        WaitForSeconds interval = new WaitForSeconds(_spawnInterval);

        while (enabled)
        {
            Spawn();

            yield return interval;
        }
    }

    private void Spawn()
    {
        WoodLog woodLog = null;

        if (TryGetObject(out woodLog, _prefab))
        {
            woodLog.gameObject.SetActive(true);
            woodLog.transform.position = _spawnPosition;
            woodLog.EnableMovement();
            
            _spawnedObjects.Add(woodLog);
        }
    }
}