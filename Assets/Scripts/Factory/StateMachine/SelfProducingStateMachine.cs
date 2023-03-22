using StateMachine.Base;

namespace Factory.StateMachine
{
    public class SelfProducingStateMachine : MonoStateMachineBase
    {
        protected override void Initialize()
        {
            ChangeState<ProducingState>();
        }
    }
}