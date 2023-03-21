using DG.Tweening;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _speed;

    private Vector3 _offset;


    void Awake()
    {
        _offset = transform.position - _player.position;
    }
    
    void Update()
    {
        transform.DOMoveX(_player.position.x + _offset.x, Time.deltaTime);
        transform.DOMoveZ(_player.position.z + _offset.z, Time.deltaTime);
    }
}
