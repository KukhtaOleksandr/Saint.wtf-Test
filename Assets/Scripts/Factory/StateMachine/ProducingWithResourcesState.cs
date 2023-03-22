using System.Collections;
using DG.Tweening;
using ScriptableObjects.Resources;
using StateMachine.Base;
using UnityEngine;
using Zenject;

namespace Factory.StateMachine
{
    public class ProducingWithResourcesState : IState
    {
        [Inject] private Transform _spawnPoint;
        [Inject] private ResourceBase _outResource;
        [Inject] protected OutStorage _outStorage;
        [Inject] protected InStorage _inStorage;
        [Inject] protected CoroutineStarter _coroutineStarter;
        [Inject] protected SignalBus _signalBus;
        private bool _isObjectAlive;

        public void Enter()
        {
            _isObjectAlive = true;
            _coroutineStarter.StartCoroutine(ProduceResource());
        }

        public IEnumerator ProduceResource()
        {
            while (_isObjectAlive)
            {
                yield return new WaitForSeconds(1);
                if (_inStorage.IsEmpty == false)
                {
                    if (_outStorage.IsFull == false)
                    {
                        Resource inResource = _inStorage.GetLast();
                        inResource.transform.DOMove(_spawnPoint.position, 0.5f).OnComplete(() =>
                        {
                            _inStorage.RemoveLast();
                            Resource resource = GameObject.Instantiate(_outResource.ResourcePrefab, _spawnPoint.position, Quaternion.Euler(0, 90, 0));
                            resource.transform.DOMove(_outStorage.GetNextCell(), 0.3f);
                            _outStorage.Add(resource);
                        });

                    }
                    else
                    {
                        _signalBus.Fire<MonoSignalChangedState>(new MonoSignalChangedState() { State = new OutStorageIsFull() });
                    }
                }
            }
        }

        public void Exit()
        {
            _isObjectAlive = false;
        }
    }
}