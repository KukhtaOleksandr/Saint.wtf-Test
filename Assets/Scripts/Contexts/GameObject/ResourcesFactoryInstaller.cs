using Factory;
using Factory.StateMachine;
using ScriptableObjects.Resources;
using StateMachine.Base;
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

        public override void InstallBindings()
        {
            Container.DeclareSignal<MonoSignalChangedState>();
            Container.DeclareSignal<SignalInStorageIsNotEmpty>();
            Container.BindInstance(_outResource);
            Container.BindInstance(_outStorage);
            Container.BindInstance(_inStorage);
            Container.BindInstance(_spawnPoint).WhenInjectedInto<ProducingWithResourcesState>();
        }
    }
}