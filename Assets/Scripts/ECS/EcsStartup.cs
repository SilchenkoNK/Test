using UnityEngine;
using Leopotam.Ecs;

public class EcsStartup : MonoBehaviour
{
    public static EcsWorld World;

    private EcsSystems _updateSystems;
    private EcsSystems _fixedUpdateSystems;
    private EcsSystems _netSystems;

    [SerializeField]
    private InitConfig StaticData;

    [SerializeField]
    private SceneData SceneData;

    private void Start()
    {
        World = new EcsWorld();

        _updateSystems = new EcsSystems(World);
        _fixedUpdateSystems = new EcsSystems(World);
        _netSystems = new EcsSystems(World);

        RuntimeData runtimeData = new RuntimeData();

        _updateSystems
            .Add(new InitSystem())
            .Add(new InputSystem())
            .Add(new CheckBallOutSystem())
            .Add(new BallResetSystem())
            .Inject(StaticData)
            .Inject(SceneData)
            .Inject(runtimeData)
            .OneFrame<BallResetComponent>();

        _fixedUpdateSystems
            .Add(new PaddleMoveSystem())
            .Inject(SceneData);

        _netSystems
            .Add(new SendDataSystem())
            .Add(new SyncSystem())
            .OneFrame<SyncComponent>();

        _updateSystems.Init();
        _fixedUpdateSystems.Init();
        _netSystems.Init();
    }

    private void Update()
    {
        _updateSystems?.Run();
        _netSystems?.Run();
    }

    private void FixedUpdate()
    {
        _fixedUpdateSystems?.Run();
    }

    private void OnDestroy()
    {
        _updateSystems?.Destroy();
        _fixedUpdateSystems?.Destroy();
        _netSystems?.Destroy();

        World?.Destroy();

        World = null;

        _updateSystems = null;
        _fixedUpdateSystems = null;
        _netSystems = null;
    }
}
