using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Factory;
using UnityEngine;
using Zenject;

public class PlayerInventory : MonoBehaviour
{
    [Inject] private CoroutineStarter _coroutineStarter;
    [SerializeField] private int _capacity = 20;

    private const int RotationOffset = 55;

    private List<Resource> _resources;

    private OutStorage _outStorage;
    private InStorage _inStorage;
    private Vector3 _lastPosition;
    private Transform _lastTransform;
    private bool _isFirst = true;

    void Awake()
    {
        _resources = new List<Resource>();
        _lastPosition = transform.position;
        _lastTransform = transform;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<InStorage>(out _inStorage))
        {
            if (_inStorage.IsFull == false && _resources.Count > 0)
            {
                List<Resource> inputResources = _resources.Where(x => x.ResourceType == _inStorage.InputResource).ToList();
                if (inputResources.Count > 0)
                {
                    foreach (var r in inputResources)
                    {
                        if (_inStorage.IsFull == false)
                            Remove(_inStorage, _resources[_resources.Count - 1]);
                        else
                            return;
                    }
                }
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<OutStorage>(out _outStorage))
        {
            if (_outStorage.IsEmpty == false)
            {
                Add(_outStorage.GetAndRemoveLast());
            }
        }
    }

    private void Remove(InStorage inStorage, Resource resource)
    {
        Vector3 newPosition = inStorage.GetNextCell();

        resource.transform.parent = null;
        _coroutineStarter.StartCoroutine(DoMove(resource, newPosition, inStorage));

        resource.transform.rotation = Quaternion.identity;

        _resources.RemoveAt(_resources.Count - 1);

        if (_resources.Count > 0)
        {
            _lastPosition.y = _resources[_resources.Count - 1].transform.position.y;
            _lastTransform = _resources[_resources.Count - 1].transform;
        }
        else
        {
            _isFirst = true;
            _lastPosition.y = transform.position.y;
            _lastTransform = transform;
        }
    }

    void Add(Resource resource)
    {
        Transform _resourceTransform = resource.transform;
        _resources.Add(resource);

        Vector3 newPosition = new Vector3(transform.position.x, _lastPosition.y + 1, transform.position.z);

        _coroutineStarter.StartCoroutine(DoMove(_resourceTransform, transform, _lastPosition.y));

        _resourceTransform.parent = this.transform;
        if (_isFirst == false)
        {
            _resourceTransform.rotation = _lastTransform.rotation;
        }
        else
        {
            _isFirst = false;
            _resourceTransform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - RotationOffset, 0);
        }

        _lastPosition = newPosition;

        _lastTransform = _resourceTransform;
    }

    IEnumerator DoMove(Transform transform, Transform to, float yPosition)
    {
        while (Vector3.Distance(transform.position, new Vector3(to.position.x, yPosition + 1, to.position.z)) > 0.001f)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(to.position.x, yPosition + 1, to.position.z), Time.deltaTime * 15);
            yield return null;
        }
    }
    IEnumerator DoMove(Resource resource, Vector3 to, InStorage inStorage)
    {
        while (Vector3.Distance(resource.transform.position, to) > 0.001f)
        {
            resource.transform.position = Vector3.Lerp(resource.transform.position, to, Time.deltaTime * 15);
            yield return null;
        }
        inStorage.Add(resource);
    }


}
