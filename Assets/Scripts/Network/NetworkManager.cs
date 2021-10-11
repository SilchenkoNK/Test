using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Network;
using Leopotam.Ecs;

public class NetworkManager : MonoBehaviour
{
    private const long SIO_UDP_CONNRESET = -1744830452;
    private const int _port = 3080;

    static public NetworkManager Instance;

    [HideInInspector]
    public bool IsHost;

    private UdpClient _udpClient;
    private IPEndPoint _remoteIP;
    private Thread _receiveThread;
    private List<Package> _packages = new List<Package>();
    private bool _gameStarted = false;

    private delegate void PackageDelegate(Package package);
    private Dictionary<PackageType, PackageDelegate> RegisteredPackages = new Dictionary<PackageType, PackageDelegate>();
    private List<EcsEntity> _entities = new List<EcsEntity>();

    // Public functions
    public void HostGame()
    {
        IsHost = true;
        _gameStarted = false;

        _udpClient = new UdpClient(_port);
        SetupSocket();

        StartReceiveThread();
    }

    public void JoinGame()
    {
        IsHost = false;

        _udpClient = new UdpClient();
        SetupSocket();

        StartReceiveThread();
    }

    public void RegisterEntity(EcsEntity entity)
    {
        _entities.Add(entity);
    }

    public void SendData(Package package)
    {
        SendData(package, _remoteIP);
    }

    // Private functions
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        RegisterPackages();
    }

    private void RegisterPackages()
    {
        RegisteredPackages.Add(PackageType.Join, ReceivedJoin);
        RegisteredPackages.Add(PackageType.StartGame, ReceivedStartGame);
        RegisteredPackages.Add(PackageType.SceneData, ReceivedSceneData);
        RegisteredPackages.Add(PackageType.AddScore, ReceivedAddScore);
    }

    private void Update()
    {
        lock (_packages)
        {
            foreach (Package p in _packages)
            {
                PackageType type = (PackageType)p.Type;
                RegisteredPackages[type]?.Invoke(p);
            }

            _packages.Clear();
        }
    }

    private void SetupSocket()
    {
        _udpClient.EnableBroadcast = true;
        _udpClient.Client.IOControl((IOControlCode)SIO_UDP_CONNRESET, new byte[] { 0, 0, 0, 0 }, null);
    }

    private void StartReceiveThread()
    {
        _entities.Clear();

        _receiveThread = new Thread(ReceiveThread);
        _receiveThread.Start();
    }

    private void ReceiveThread()
    {
        if (!IsHost)
        {
            _udpClient.Client.ReceiveTimeout = 1000;

            Package package = new Package(PackageType.Join);
            IPEndPoint remoteIP = new IPEndPoint(IPAddress.Broadcast, _port);

            while (true)
            {
                SendData(package, remoteIP);

                try
                {
                    Receive();
                    break;
                }
                catch (SocketException)
                {
                }
            }

            _udpClient.Client.ReceiveTimeout = 0;
        }

        try
        {
            while (true)
            {
                Receive();
            }
        }
        catch (SocketException ex)
        {
            Debug.Log(ex.Message);
        }
    }

    private void Receive()
    {
        byte[] bytes = _udpClient.Receive(ref _remoteIP);

        Package package = new Package(bytes);

        lock (_packages)
        {
            _packages.Add(package);
        }
    }

    private void SendData(Package package, IPEndPoint remoteIP)
    {
        byte[] bytes = package.GetBytes();

        _udpClient.Send(bytes, bytes.Length, remoteIP);
    }

    private void ReceivedJoin(Package package)
    {
        if (_gameStarted)
            return;

        Package p = new Package(PackageType.StartGame);
        SendData(p);

        _gameStarted = true;

        StartGame();
    }

    private void ReceivedStartGame(Package package)
    {
        StartGame();
    }

    private void ReceivedSceneData(Package package)
    {
        foreach (EcsEntity e in _entities)
        {
            ref var sync = ref e.Get<SyncComponent>();
            sync.Package = package;
        }
    }

    private void ReceivedAddScore(Package package)
    {
        GameScene.Instance.AddScore(package.ReadBool());
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Scenes/Game");
    }
}
