using StateMachine.Base;

namespace Factory.StateMachine.SelfProducingFactory
{
    public class SelfProducingStateMachine : MonoStateMachineBase
    {
        protected override void Initialize()
        {
            ChangeState<ProducingState>();
        }
    }
}