using UnityEngine;
using Zenject;

public class UtilsInstaller : MonoInstaller
{
    [SerializeField] private CoroutineStarter _coroutineStarter;
    public override void InstallBindings()
    {
        Container.BindInstance(_coroutineStarter);
    }
}
