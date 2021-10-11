using UnityEngine;
using Leopotam.Ecs;
using Network;

public class BallResetSystem : IEcsRunSystem
{
    private InitConfig _sceneData;

    private EcsFilter<TransformComponent, RigidbodyComponent, BallResetComponent> _filter;

    public void Run()
    {
        foreach (var i in _filter)
        {
            ref var transform = ref _filter.Get1(i);
            ref var rigidbody = ref _filter.Get2(i);

            transform.Transform.position = Vector3.up * transform.Transform.position.y;
            rigidbody.Rigidbody.velocity = Vector3.zero;

            float x = Random.Range(0.25f, 1f);
            float y = Random.Range(0.25f, 1f);
            if (Random.Range(0, 2) == 0)
                x = -x;
            if (Random.Range(0, 2) == 0)
                y = -y;

            Vector3 startForce = new Vector3(x, 0f, y);
            float speed = Random.Range(_sceneData.BallSpeedMin, _sceneData.BallSpeedMax);

            rigidbody.Rigidbody.AddForce(startForce.normalized * speed, ForceMode.Impulse);

            float size = Random.Range(_sceneData.BallScaleMin, _sceneData.BallScaleMax);
            transform.Transform.localScale = Vector3.one * size;
        }
    }
}
