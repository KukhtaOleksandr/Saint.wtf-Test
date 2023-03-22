using System.Collections;
using System.Threading.Tasks;
using ScriptableObjects.Resources;
using StateMachine.Base;
using UnityEngine;
using Zenject;

namespace Factory.StateMachine
{
    public class ProducingState : IState
    {
        [Inject] protected ResourceBase _resource;
        [Inject] protected OutStorage _outStorage;
        [Inject] protected CoroutineStarter _coroutineStarter;
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
                GameObject.Instantiate(_resource.ResourcePrefab, _outStorage.GetNextCell(), Quaternion.Euler(0, 0, -90));
            }
        }

        public void Exit()
        {
            _isObjectAlive = false;
        }
    }
}