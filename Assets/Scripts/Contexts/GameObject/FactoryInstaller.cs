using Factory;
using Factory.StateMachine;
using ScriptableObjects.Resources;
using StateMachine.Base;
using UnityEngine;
using Zenject;

public class FactoryInstaller : MonoInstaller
{
    [SerializeField] protected ResourceBase _resource;
    [SerializeField] protected OutStorage _outStorage;
    [SerializeField] protected Transform _spawnPoint;
    public override void InstallBindings()
    {
        Container.DeclareSignal<MonoSignalChangedState>();
        Container.BindInstance(_resource);
        Container.BindInstance(_outStorage);
        Container.BindInstance(_spawnPoint).WhenInjectedInto<ProducingState>();
    }
}
