using Factory;
using Factory.StateMachine;
using Factory.StateMachine.ProducingFactory;
using ScriptableObjects.Resources;
using StateMachine.Base;
using TMPro;
using UnityEngine;
using Zenject;

namespace Contexts.GameObject
{
    public class ResourcesFactoryInstaller : MonoInstaller
    {
        [SerializeField] private ResourceBase _outResource;
        [SerializeField] protected OutStorage _outStorage;
        [SerializeField] protected InStorage _inStorage;
        [SerializeField] protected Transform _spawnPoint;
        [SerializeField] private TextMeshProUGUI _stateText;

        public override void InstallBindings()
        {
            Container.DeclareSignal<MonoSignalChangedState>();
            Container.DeclareSignal<SignalInStorageIsNotEmpty>();
            Container.DeclareSignal<SignalStorageIsNotFull>();
            Container.BindInstance(_outResource);
            Container.BindInstance(_stateText);
            Container.BindInstance(_outStorage);
            Container.BindInstance(_inStorage);
            Container.BindInstance(_spawnPoint).WhenInjectedInto<ProducingWithResourcesState>();
        }
    }
}