using UnityEngine;
using StateMachine.Base;
using TMPro;
using Zenject;

namespace Factory.StateMachine.SelfProducingFactory
{
    public class OutStorageIsFullState : IState
    {
        [Inject] private TextMeshProUGUI _stateText;
        [Inject] private SignalBus _signalBus;
        public void Enter()
        {
            _stateText.text = "Storage is full";
            _stateText.color  = Color.red;
            _signalBus.Subscribe<SignalStorageIsNotFull>(Produce);
        }

        public void Exit()
        {
            _signalBus.Unsubscribe<SignalStorageIsNotFull>(Produce);
        }

        private void Produce()
        {
            _signalBus.Fire<MonoSignalChangedState>(new MonoSignalChangedState() { State = new ProducingState() });
        }
    }
}