using Leopotam.Ecs;
using Network;

public class SendDataSystem : IEcsRunSystem
{
    private EcsFilter<TransformComponent, SendDataComponent> _filter; 

    public void Run()
    {
        Package package = new Package(PackageType.SceneData);

        foreach (var i in _filter)
        {
            ref var transform = ref _filter.Get1(i);

            package.WriteVector3(transform.Transform.position);
            package.WriteVector3(transform.Transform.localScale);
        }

        NetworkManager.Instance.SendData(package);
    }
}
