using UnityEngine;
using Leopotam.Ecs;

public class PaddleMoveSystem : IEcsRunSystem
{
    private EcsFilter<TransformComponent, InputComponent> _filter;
    private SceneData _sceneData;

    public void Run()
    {
        foreach (var i in _filter)
        {
            ref var transform = ref _filter.Get1(i);
            ref var input = ref _filter.Get2(i);

            Vector3 target = transform.Transform.position;
            target.x += input.DeltaX;

            Vector3 newPos = Vector3.Lerp(transform.Transform.position, target, Time.fixedDeltaTime);

            newPos.x = Mathf.Clamp(newPos.x, -_sceneData.PaddleLimitX, _sceneData.PaddleLimitX);

            transform.Transform.position = newPos;
        }
    }
}
