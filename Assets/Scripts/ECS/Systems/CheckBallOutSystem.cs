using UnityEngine;
using Leopotam.Ecs;
using Network;

public class CheckBallOutSystem : IEcsRunSystem
{
    private SceneData _sceneData;

    private EcsFilter<CheckBallOutComponent, TransformComponent> _filter;

    public void Run()
    {
        foreach (var i in _filter)
        {
            ref var transform = ref _filter.Get2(i);

            if (Mathf.Abs(transform.Transform.position.z) > _sceneData.BallLimitZ)
            {
                EcsEntity entity = _filter.GetEntity(i);
                entity.Get<BallResetComponent>();

                bool playerScore = false;
                if (transform.Transform.position.z > 0)
                    playerScore = true;

                GameScene.Instance.AddScore(playerScore);

                Package package = new Package(PackageType.AddScore);
                package.WriteBool(!playerScore);
                NetworkManager.Instance.SendData(package);
            }
        }
    }
}
