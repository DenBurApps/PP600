using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRemover : MonoBehaviour
{
    [SerializeField] private WoodLogSpawner _woodLogSpawner;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out WoodLog woodLog))
        {
            _woodLogSpawner.ReturnObject(woodLog);
        }
    }
}
