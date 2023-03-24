using System.Collections;
using System.Collections.Generic;
using Extensions;
using Factory.StateMachine;
using UnityEngine;
using Zenject;

namespace Factory
{
    public class InStorage : MonoBehaviour
    {
        [Inject] SignalBus _signalBus;
        [SerializeField] private CoroutineStarter _coroutineStarter;
        [SerializeField] private InputCraft _inputCraft;
        [SerializeField] private int _capacity;
        [SerializeField] private Transform _leftBound;
        [SerializeField] private Transform _rightBound;
        [SerializeField] private Transform _forwardBound;
        [SerializeField] private Transform _lowBound;

        [SerializeField] private Vector3 _cellSize = new Vector3(1, 1, 2);

        public bool IsFull
        {
            get
            {
                bool isFull = true;
                foreach (var r in _resources)
                {
                    if (r.Value.Count != _capacity)
                        isFull = false;
                }
                return isFull;
            }

        }

        public bool CanProduce
        {
            get
            {
                bool canProduce = true;
                foreach (var r in _resources)
                {
                    if (r.Value.Count == 0)
                        canProduce = false;
                }
                return canProduce;
            }
        }

        public InputCraft InputCraft { get => _inputCraft; }

        private Dictionary<ResourceType, List<Resource>> _resources;
        private List<Vector3> _cells;
        private List<Vector3> _freeCells;
        private List<bool> _isMoveAnimationsFinished;
        private Vector3 _last;

        private bool isFirstInRow = true;

        void Awake()
        {
            Init();

            for (int i = 0; i < _capacity * InputCraft.ResourcesForProduction.Count; i++)
            {
                _cells.Add(GetNextCellPosition());
                _freeCells.Add(_cells[i]);
            }
        }

        public bool IsFullOfType(ResourceType type)
        {
            if(_resources[type].Count==_capacity)
                return true;
            return false;
        }

        public void PreAdd()
        {
            _isMoveAnimationsFinished = new List<bool>();
        }

        public void Add(Resource resource)
        {
            _resources[resource.ResourceType].Add(resource);
            Vector3 newPosition = GetNextFreeCell();
            _resources[resource.ResourceType].GetLast().Cell = newPosition;

            resource.transform.parent = null;

            _isMoveAnimationsFinished.Add(false);
            _coroutineStarter.StartCoroutine(DoMove(resource, newPosition, _isMoveAnimationsFinished.GetLastIndex()));

            resource.transform.rotation = Quaternion.identity;
        }

        public void RemoveLastOfType(ResourceType resourceType)
        {
            if (_resources.Count > 0)
            {
                Resource resource = _resources[resourceType].GetLast();

                _resources[resourceType].Remove(resource);
                int index = _cells.IndexOf(resource.Cell);
                _freeCells.Insert(0, _cells[index]);
            }
        }

        public Resource GetLastOfType(ResourceType resourceType)
        {
            if (_resources.Count > 0)
                return _resources[resourceType].GetLast();
            return null;
        }

        public Vector3 GetNextFreeCell()
        {
            Vector3 result = _freeCells[0];
            _freeCells.Remove(_freeCells[0]);
            return result;
        }

        private Vector3 GetNextCellPosition()
        {
            Vector3 result;
            if (isFirstInRow)
            {
                result = GetFirstInRowPosition();
                isFirstInRow = false;
            }
            else
            {
                float zResult = _last.z + _cellSize.z;
                if (zResult < _rightBound.position.z)
                    result = GetFirstInNextRowPosition();
                else
                {
                    float xResult = _last.x + _cellSize.x;
                    if (xResult > _lowBound.position.x)
                        result = GetFirstInNewVerticalRowPosition();
                    else
                        result = new Vector3(xResult, _last.y, _leftBound.position.z + _cellSize.z / 2);
                }
            }
            
            _last = result;
            return result;
        }

        private Vector3 GetFirstInNewVerticalRowPosition()
        {
            return new Vector3(_forwardBound.position.x + _cellSize.x / 2, _last.y + _cellSize.y, _leftBound.position.z + _cellSize.z / 2);
        }

        private Vector3 GetFirstInNextRowPosition()
        {
            return new Vector3(_last.x, _last.y, _last.z + _cellSize.z);
        }

        private Vector3 GetFirstInRowPosition()
        {
            return new Vector3(_last.x, _last.y, _last.z + _cellSize.z / 2);
        }

        IEnumerator DoMove(Resource resource, Vector3 to, int index)
        {
            while (Vector3.Distance(resource.transform.position, to) > 0.001f)
            {
                resource.transform.position = Vector3.Lerp(resource.transform.position, to, Time.deltaTime * 15);
                yield return null;
            }

            _isMoveAnimationsFinished[index] = true;
            bool isFinished = true;

            foreach (var i in _isMoveAnimationsFinished)
            {
                if (i == false)
                {
                    isFinished = false;
                    break;
                }
            }

            if (isFinished)
            {
                bool isReadyForProduction = true;
                foreach (var r in InputCraft.ResourcesForProduction)
                {
                    if (_resources[r].Count == 0)
                        isReadyForProduction = false;
                }
                if (isReadyForProduction)
                    _signalBus.Fire<SignalInStorageIsNotEmpty>();
            }
        }

        private void Init()
        {
            _isMoveAnimationsFinished = new List<bool>();
            _resources = new Dictionary<ResourceType, List<Resource>>();

            foreach (var r in InputCraft.ResourcesForProduction)
            {
                _resources.Add(r, new List<Resource>());
            }

            _cells = new List<Vector3>();
            _freeCells = new List<Vector3>();
            _last = new Vector3(_forwardBound.position.x + _cellSize.x / 2, _leftBound.position.y + _cellSize.y / 2, _leftBound.position.z);
        }
    }
}
