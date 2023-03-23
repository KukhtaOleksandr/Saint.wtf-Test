using StateMachine.Base;
using Zenject;

namespace Factory.StateMachine
{
    public class MissingResourcesState : IState
    {
        [Inject] SignalBus _signalBus;
        public void Enter()
        {
            _signalBus.Subscribe<SignalInStorageIsNotEmpty>(Produce);
        }

        public void Exit()
        {
            _signalBus.Unsubscribe<SignalInStorageIsNotEmpty>(Produce);
        }

        public void Produce()
        {
            _signalBus.Fire<MonoSignalChangedState>(new MonoSignalChangedState() { State = new ProducingWithResourcesState ()});
        }
    }
}