using System.Collections.Generic;
using Extensions;
using ScriptableObjects.Resources;
using UnityEngine;
using Zenject;

namespace Factory
{
    public class OutStorage : MonoBehaviour
    {
        [Inject] SignalBus _signalBus;
        [SerializeField] private int _capacity;
        [SerializeField] private Transform _leftBound;
        [SerializeField] private Transform _rightBound;
        [SerializeField] private Transform _forwardBound;
        [SerializeField] private Transform _lowBound;
        [SerializeField] private Transform _cellPrefab;

        [SerializeField] private Vector3 _cellSize = new Vector3(2f, 1, 1);

        public bool IsFull { get => _resources.Count == _capacity; }
        public bool IsEmpty { get => _resources.Count == 0; }
        public List<Resource> Resources { get => _resources; }

        private List<Resource> _resources;
        private List<Vector3> _cells;
        private List<Vector3> _freeCells;
        private Vector3 _last;

        private bool isFirstInRow = true;

        void Awake()
        {
            _resources = new List<Resource>();
            _cells = new List<Vector3>();
            _freeCells = new List<Vector3>();
            _last = new Vector3(_leftBound.position.x, _leftBound.position.y + _cellSize.y / 2, _forwardBound.position.z - _cellSize.z / 2);

            for (int i = 0; i < _capacity; i++)
            {
                _cells.Add(GetNextCellPosition());
                _freeCells.Add(_cells[i]);
            }
        }

        public void Add(Resource resource)
        {
            _resources.Add(resource);
        }

        public Resource GetAndRemoveLast()
        {
            if (_resources.Count > 0)
            {
                
                int index = _resources.GetLastIndex();
                Resource result = _resources[index];
                _resources.RemoveAt(index);
                
                _freeCells.Insert(0,_cells[index]);
                _signalBus.Fire<SignalStorageIsNotFull>();

                return result;
            }
            return null;
        }

        public Vector3 GetNextFreeCell()
        {
            Vector3 result =  _freeCells[0];
            _freeCells.Remove(_freeCells[0]);
            return result;
        }

        private Vector3 GetNextCellPosition()
        {
            Vector3 result;
            if (isFirstInRow)
            {
                result = new Vector3(_last.x + _cellSize.x / 2, _last.y, _last.z);
                isFirstInRow = false;
            }
            else
            {
                float xResult = _last.x + _cellSize.x;
                if (xResult < _rightBound.position.x)
                    result = new Vector3(_last.x + _cellSize.x, _last.y, _last.z);
                else
                {
                    float zResult = _last.z - _cellSize.z;
                    if (zResult < _lowBound.position.z)
                        result = new Vector3(_leftBound.position.x + _cellSize.x / 2, _last.y + _cellSize.y, _forwardBound.position.z - _cellSize.z / 2);
                    else
                        result = new Vector3(_leftBound.position.x + _cellSize.x / 2, _last.y, zResult);
                }
            }
            _last = result;
            return result;
        }
    }
}
