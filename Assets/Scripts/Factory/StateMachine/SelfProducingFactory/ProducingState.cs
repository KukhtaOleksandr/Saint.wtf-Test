using System.Collections;
using DG.Tweening;
using ScriptableObjects.Resources;
using StateMachine.Base;
using TMPro;
using UnityEngine;
using Zenject;

namespace Factory.StateMachine.SelfProducingFactory
{
    public class ProducingState : IState
    {
        [Inject] private Transform _spawnPoint;
        [Inject] private TextMeshProUGUI _stateText;
        [Inject] private ResourceBase _resource;
        [Inject] private OutStorage _outStorage;
        [Inject] private CoroutineStarter _coroutineStarter;
        [Inject] private SignalBus _signalBus;
        private bool _isObjectAlive;

        public void Enter()
        {
            _isObjectAlive = true;
            _stateText.text = "PRODUCING";
            _stateText.color  = Color.green;
            _coroutineStarter.StartCoroutine(ProduceResource());
        }

        public IEnumerator ProduceResource()
        {
            while (_isObjectAlive)
            {
                yield return new WaitForSeconds(1);
                if (_outStorage.IsFull == false)
                {
                    Resource resource = GameObject.Instantiate(_resource.ResourcePrefab, _spawnPoint.position, Quaternion.Euler(0, 90, 0));
                    resource.transform.DOMove(_outStorage.GetNextFreeCell(), 0.3f);
                    _outStorage.Add(resource);
                }
                else
                {
                    _signalBus.Fire<MonoSignalChangedState>(new MonoSignalChangedState() { State = new OutStorageIsFullState() });
                }
            }
        }

        public void Exit()
        {
            _isObjectAlive = false;
        }
    }
}