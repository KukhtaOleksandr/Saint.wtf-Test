using System.Collections.Generic;
using UnityEngine;

namespace Factory
{
    public class InStorage : MonoBehaviour
    {
        [SerializeField] private ResourceType _inputResource;
        [SerializeField] private int Capacity;
        [SerializeField] private Transform _leftBound;
        [SerializeField] private Transform _rightBound;
        [SerializeField] private Transform _forwardBound;
        [SerializeField] private Transform _lowBound;

        [SerializeField] private Vector3 _cellSize = new Vector3(1, 1, 2);

        public bool IsFull { get => _resources.Count == Capacity; }
        public bool IsEmpty { get => _resources.Count == 0; }
        public ResourceType InputResource { get => _inputResource; }

        private List<Resource> _resources;
        private Vector3 _last;

        private bool isFirstInRow = true;

        void Awake()
        {
            _resources = new List<Resource>();
            _last = new Vector3(_forwardBound.position.x + _cellSize.x / 2, _leftBound.position.y + _cellSize.y / 2, _leftBound.position.z);
        }

        public void Add(Resource resource)
        {
            _resources.Add(resource);
        }

        public void RemoveLast()
        {
            if (_resources.Count > 0)
            {
                int index = _resources.Count - 1;
                Transform result = _resources[index].transform;
                _resources.RemoveAt(index);

                if (_resources.Count == 0)
                {
                    _last = new Vector3(_forwardBound.position.x + _cellSize.x / 2, _leftBound.position.y + _cellSize.y / 2, _leftBound.position.z - _cellSize.z / 2);
                }
                else
                    _last = result.position;
            }
        }

        public Resource GetLast()
        {
            if(_resources.Count>0)
                return _resources[_resources.Count-1];
            return null;
        }


        public Vector3 GetNextCell()
        {
            Vector3 result;
            if (isFirstInRow)
            {
                result = new Vector3(_last.x, _last.y, _last.z + _cellSize.z / 2);
                isFirstInRow = false;
            }
            else
            {
                float zResult = _last.z + _cellSize.z;
                if (zResult < _rightBound.position.z)
                    result = new Vector3(_last.x, _last.y, _last.z + _cellSize.z);
                else
                {
                    float xResult = _last.x + _cellSize.x;
                    if (xResult > _lowBound.position.x)
                        result = new Vector3(_forwardBound.position.x + _cellSize.x / 2, _last.y + _cellSize.y, _leftBound.position.z + _cellSize.z / 2);
                    else
                        result = new Vector3(xResult, _last.y, _leftBound.position.z + _cellSize.z / 2);
                }
            }
            _last = result;
            return result;

        }

    }
}
