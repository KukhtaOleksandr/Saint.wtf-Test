using ScriptableObjects.Resources;
using UnityEngine;

namespace Factory
{
    public class OutStorage : MonoBehaviour
    {
        [SerializeField] private int Capacity;
        [SerializeField] private Transform _leftBound;
        [SerializeField] private Transform _rightBound;
        [SerializeField] private Transform _forwardBound;
        [SerializeField] private Transform _lowBound;

        [SerializeField] private Vector3 _cellSize = new Vector3(2f, 1, 1);
        private Vector3 _last;

        private bool isFirstInRow = true;

        void Awake()
        {
            _last = new Vector3(_leftBound.position.x, _leftBound.position.y + _cellSize.y / 2, _forwardBound.position.z - _cellSize.z / 2);
        }

        public Vector3 GetNextCell()
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
