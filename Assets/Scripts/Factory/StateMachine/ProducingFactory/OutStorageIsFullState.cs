using UnityEngine;
using StateMachine.Base;
using TMPro;
using Zenject;

namespace Factory.StateMachine.ProducingFactory
{
    public class OutStorageIsFullState : IState
    {
        [Inject] private TextMeshProUGUI _stateText;
        [Inject] private SignalBus _signalBus;
        [Inject] private InStorage _inStorage;
        public void Enter()
        {
            _stateText.text = "Storage is full";
            _stateText.color  = Color.red;
            _signalBus.Subscribe<SignalStorageIsNotFull>(ChangeState);
        }

        public void Exit()
        {
            _signalBus.Unsubscribe<SignalStorageIsNotFull>(ChangeState);
        }

        private void ChangeState()
        {
            if(_inStorage.CanProduce)
                _signalBus.Fire<MonoSignalChangedState>(new MonoSignalChangedState() { State = new ProducingWithResourcesState() });
            else
                _signalBus.Fire<MonoSignalChangedState>(new MonoSignalChangedState() { State = new MissingResourcesState() });
        }
    }
}