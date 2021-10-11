using UnityEngine;
using Leopotam.Ecs;

public class InitSystem : IEcsInitSystem
{
    private EcsWorld _world;
    private InitConfig _staticData;
    private SceneData _sceneData;

    public void Init()
    {
        EcsEntity playerPaddle = _world.NewEntity();

        playerPaddle.Get<InputComponent>();
        playerPaddle.Get<SendDataComponent>();

        ref var paddleTransform = ref playerPaddle.Get<TransformComponent>();
        paddleTransform.Transform = _sceneData.PlayerPaddle.transform;

        EcsEntity rivalEntity = _world.NewEntity();

        ref var rivaltransform = ref rivalEntity.Get<TransformComponent>();
        rivaltransform.Transform = _sceneData.RivalPaddle.transform;

        NetworkManager.Instance.RegisterEntity(rivalEntity);

        EcsEntity ball = _world.NewEntity();

        ref var balltransform = ref ball.Get<TransformComponent>();
        balltransform.Transform = _sceneData.Ball.transform;

        if (NetworkManager.Instance.IsHost)
        {
            ball.Get<SendDataComponent>();

            ref var rigidBody = ref ball.Get<RigidbodyComponent>();
            rigidBody.Rigidbody = _sceneData.Ball.GetComponent<Rigidbody>();

            ball.Get<BallResetComponent>();
            ball.Get<CheckBallOutComponent>();
        }
        else
        {
            NetworkManager.Instance.RegisterEntity(ball);

            Object.Destroy(_sceneData.Ball.GetComponent<Rigidbody>());
            
            Collider[] colliders = Object.FindObjectsOfType<Collider>();

            foreach (Collider c in colliders)
                Object.Destroy(c);
        }
    }
}
