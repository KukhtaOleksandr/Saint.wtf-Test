using Zenject;

namespace Contexts.Project
{
    public class GlobalSignalsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
        }
    }
}