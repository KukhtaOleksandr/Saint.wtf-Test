using StateMachine.Base;

namespace Factory.StateMachine
{
    public class ProducingStateMachine : MonoStateMachineBase
    {
        protected override void Initialize()
        {
            ChangeState<ProducingWithResourcesState>();
        }
    }
}