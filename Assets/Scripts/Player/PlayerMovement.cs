using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _movementSpeed = 3;
        [SerializeField] private float _rotationSpeed = 3;
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private FloatingJoystick _joystick;

        private Vector3 _moveVector;
        private Vector3 _direction;

        void Update()
        {
            _moveVector = Vector3.zero;

            if (_joystick.Horizontal != 0 && _joystick.Vertical != 0)
            {
                _moveVector.x = _joystick.Horizontal * _movementSpeed * Time.deltaTime;
                _moveVector.z = _joystick.Vertical * _movementSpeed * Time.deltaTime;
                _direction = Vector3.RotateTowards(transform.forward, _moveVector, _rotationSpeed * Time.deltaTime, 0);
                transform.rotation = Quaternion.LookRotation(_direction);
            }

            _rigidBody.MovePosition(_rigidBody.position + _moveVector);
        }

    }
}
