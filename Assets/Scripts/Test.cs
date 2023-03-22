using System.Collections;
using System.Collections.Generic;
using Factory;
using ScriptableObjects.Resources;
using UnityEngine;
using Zenject;

public class Test : MonoBehaviour
{
    [SerializeField] protected ResourceBase _resource;
    [SerializeField] protected InStorage _inStorage;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Spawn();
        }
    }
    public void Spawn()
    {
        GameObject.Instantiate(_resource.ResourcePrefab, _inStorage.GetNextCell(), Quaternion.Euler(0, 0, 0));
    }
}
