using StateMachine.Base;

namespace Factory.StateMachine.ProducingFactory
{
    public class ProducingStateMachine : MonoStateMachineBase
    {
        protected override void Initialize()
        {
            ChangeState<MissingResourcesState>();
        }
    }
}