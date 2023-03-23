using Factory;
using Factory.StateMachine.SelfProducingFactory;
using ScriptableObjects.Resources;
using StateMachine.Base;
using TMPro;
using UnityEngine;
using Zenject;

public class FactoryInstaller : MonoInstaller
{
    [SerializeField] protected ResourceBase _resource;
    [SerializeField] protected OutStorage _outStorage;
    [SerializeField] protected Transform _spawnPoint;
    [SerializeField] private TextMeshProUGUI _stateText;
    public override void InstallBindings()
    {
        Container.DeclareSignal<MonoSignalChangedState>();
        Container.DeclareSignal<SignalStorageIsNotFull>();
        Container.BindInstance(_resource);
        Container.BindInstance(_stateText);
        Container.BindInstance(_outStorage);
        Container.BindInstance(_spawnPoint).WhenInjectedInto<ProducingState>();
    }
}
