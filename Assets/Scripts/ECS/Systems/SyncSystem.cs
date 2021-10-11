using UnityEngine;
using Leopotam.Ecs;

public class SyncSystem : IEcsRunSystem
{
    private EcsFilter<SyncComponent, TransformComponent> _filter;

    public void Run()
    {
        foreach (var i in _filter)
        {
            ref var sync = ref _filter.Get1(i);
            ref var transform = ref _filter.Get2(i);

            Vector3 pos = sync.Package.ReadVector3();
            pos.z = -pos.z;

            transform.Transform.position = pos;
            transform.Transform.localScale = sync.Package.ReadVector3();
        }
    }
}
