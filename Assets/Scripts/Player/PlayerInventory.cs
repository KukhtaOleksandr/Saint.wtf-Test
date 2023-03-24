using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Factory;
using UnityEngine;
using Zenject;
using Extensions;

namespace Player
{
    public class PlayerInventory : MonoBehaviour
    {
        [Inject] private CoroutineStarter _coroutineStarter;
        [SerializeField] private int _capacity = 20;

        private const int RotationOffset = 55;

        private List<Resource> _resources;
        private List<Vector3> _cells;
        private List<Vector3> _freeCells;

        private OutStorage _outStorage;
        private InStorage _inStorage;
        private Vector3 _lastPosition;
        private Transform _lastTransform;
        private bool _isFirst = true;
        private int _firstMovedResourceIndex;

        void Awake()
        {
            _cells = new List<Vector3>();
            _freeCells = new List<Vector3>();
            _resources = new List<Resource>();

            _lastPosition = transform.position;
            _lastTransform = transform;

            for (int i = 0; i < _capacity; i++)
            {
                _cells.Add(GetNextCellPosition());
                _freeCells.Add(_cells[i]);
            }
        }


        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<InStorage>(out _inStorage))
            {
                if (_inStorage.IsFull == false && _resources.Count > 0)
                {
                    List<Resource> inputResources = new List<Resource>();
                    foreach (var i in _inStorage.InputCraft.ResourcesForProduction)
                    {
                        inputResources.AddRange(_resources.Where(x => x.ResourceType == i).ToList());
                    }
                    if (inputResources.Count > 0)
                    {
                        _firstMovedResourceIndex = _resources.IndexOf(inputResources[0]);
                        _inStorage.PreAdd();
                        Remove(_inStorage, inputResources);
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
                    if (_resources.Count <= _capacity)
                        Add(_outStorage.GetAndRemoveLast());
                }
            }
        }

        private Vector3 GetNextCellPosition()
        {
            return new Vector3(0, _lastPosition.y + 1, 0);
        }

        private void Remove(InStorage inStorage, List<Resource> resources)
        {
            foreach (var r in resources)
            {
                if (_inStorage.IsFullOfType(r.ResourceType) == false)
                {
                    _freeCells.Insert(0, _cells[_resources.IndexOf(r)]);
                    _resources.Remove(r);
                    inStorage.Add(r);
                }
            }

            if (_resources.Count > 0)
            {
                if (AreResourcesOnTop())
                {
                    MoveResourceToFreeCells();
                }
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

            Vector3 cell = GetNextCellPosition();
            Vector3 newPosition = new Vector3(transform.position.x, cell.y, transform.position.z + cell.z);

            _coroutineStarter.StartCoroutine(DoMove(_resourceTransform, transform, _lastPosition.y));

            _resourceTransform.parent = this.transform;
            SetRotation(_resourceTransform);

            _lastPosition = newPosition;
            _lastTransform = _resourceTransform;
        }

        private void MoveResourceToFreeCells()
        {
            List<Resource> resourcesToMove = _resources.Where(x => x.transform.position.y > _freeCells[0].y).ToList();
            float yPosition = _freeCells[0].y;
            foreach (var res in resourcesToMove)
            {
                res.transform.position = new Vector3(transform.position.x, yPosition + 1, transform.position.z);
                yPosition++;
            }
        }

        private bool AreResourcesOnTop()
        {
            return _freeCells[0].y <= _resources.GetLast().transform.position.y;
        }


        private void SetRotation(Transform _resourceTransform)
        {
            if (_isFirst == false)
            {
                _resourceTransform.rotation = _lastTransform.rotation;
            }
            else
            {
                _isFirst = false;
                _resourceTransform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - RotationOffset, 0);
            }
        }

        IEnumerator DoMove(Transform transform, Transform to, float yPosition)
        {
            while (Vector3.Distance(transform.position, new Vector3(to.position.x, yPosition + 1, to.position.z)) > 0.001f)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(to.position.x, yPosition + 1, to.position.z), Time.deltaTime * 15);
                yield return null;
            }
        }
    }
}
