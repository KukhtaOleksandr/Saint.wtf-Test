using StateMachine.Base;
using TMPro;
using UnityEngine;
using Zenject;

namespace Factory.StateMachine.ProducingFactory
{
    public class MissingResourcesState : IState
    {
        [Inject] private TextMeshProUGUI _stateText;
        [Inject] SignalBus _signalBus;
        public void Enter()
        {
            _stateText.text = "Missing Resources";
            _stateText.color = Color.red;
            _signalBus.Subscribe<SignalInStorageIsNotEmpty>(Produce);
        }

        public void Exit()
        {
            _signalBus.Unsubscribe<SignalInStorageIsNotEmpty>(Produce);
        }

        public void Produce()
        {
            _signalBus.Fire<MonoSignalChangedState>(new MonoSignalChangedState() { State = new ProducingWithResourcesState() });
        }
    }
}