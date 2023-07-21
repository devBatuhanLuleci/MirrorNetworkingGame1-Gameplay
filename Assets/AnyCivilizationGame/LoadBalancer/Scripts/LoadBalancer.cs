using ACGAuthentication;
using log4net;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadBalancer : Singleton<LoadBalancer>
{

    [Header("Listener Setup")]
    [SerializeField] private Host host = Host.LocalHost;
    [SerializeField] private bool startClientOnStart = true;
    private Transport _transport;
    public Transport Transport { get 
        {
            if (_transport ==null)
            {
                _transport = FindObjectOfType<Transport>();
            }
            return _transport;
        }
    }



    private bool isServer = false;
    private bool isClient = false;
    // store all users of connected to lobby
    private Dictionary<byte, EventManagerBase> eventHandlers = new Dictionary<byte, EventManagerBase>();
    private static readonly ILog log = LogManager.GetLogger(typeof(LoadBalancer));

    public Host Host => host;

    #region Managers
    public ACGAuthenticationManager ACGAuthenticationManager { get; private set; }
    public SpawnServer SpawnServer { get; private set; }
    public LobbyManager LobbyManager { get; private set; }
    #endregion



    #region MonoBehavior Callcks
    Coroutine closingAppCoroutine;
    private void Start()
    {
        log.Debug($"Loadbalancer Started");

        Application.runInBackground = true;
        SetupManagers();
        if (Transport == null)
        {
            Debug.LogError("Transport not found!");
            return;
        }
        // setup client and server listeners callbacks
        SetupLinstener();
        //if (startClientOnStart)
        //{
        //    StartClient();
        //}

    }


    public void StartClient(string hostAddress)
    {
        // disconnect if already connected.
        if (isClient)
        {
            Transport.ClientDisconnect();
        }
        isClient = true;
        Transport.ClientConnect(hostAddress);
        Debug.Log("Host address:" + hostAddress + " Port: " + Transport.ServerUri().Port);
    }
    public void StartClient(Host host)
    {
        StartClient(host.GetStringValue());
    }
    public void StartClient()
    {
        StartClient(host);
    }

    private void SetupManagers()
    {
        ACGAuthenticationManager = new ACGAuthenticationManager(this);
        SpawnServer = new SpawnServer(this);
        LobbyManager = new LobbyManager(this);
    }

    private void Update()
    {
        if (isServer)
        {
            Transport.ServerEarlyUpdate();
        }
        if (isClient)
        {
            Transport.ClientEarlyUpdate();
        }
    }
    private void LateUpdate()
    {
        if (isServer)
        {
            Transport.ServerLateUpdate();
        }
        if (isClient)
        {
            //Application.runInBackground = true;
            Transport.ClientLateUpdate();
        }
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }

    // called when quitting the application by closing the window / pressing
    // stop in the editor. virtual so that inheriting classes'
    // OnApplicationQuit() can call base.OnApplicationQuit() too
    public virtual void OnApplicationQuit()
    {
        // stop client first
        // (we want to send the quit packet to the server instead of waiting
        //  for a timeout)
        if (isServer)
        {
            Transport.ServerStop();
        }
    }

    #endregion

    #region SetupListeners

    #region Remove
    private void RemoveListeners()
    {
        RemoveClientListeners();
    }

    private void RemoveClientListeners()
    {
        Transport.OnClientConnected -= OnClientConnected;
        Transport.OnClientDataReceived -= OnClientDataReceived;
        Transport.OnClientDataSent -= OnClientDataSent;
        Transport.OnClientDisconnected -= OnClientDisconnected;
        Transport.OnClientError -= OnClientError;
    }
    #endregion
    private void SetupLinstener()
    {
        AddClientListeners();
    }

    private void AddClientListeners()
    {
        Transport.OnClientConnected += OnClientConnected;
        Transport.OnClientDataReceived += OnClientDataReceived;
        Transport.OnClientDataSent += OnClientDataSent;
        Transport.OnClientDisconnected += OnClientDisconnected;
        Transport.OnClientError += OnClientError;
    }


    private void OnClientError(Exception ex)
    {
        Debug.LogError("OnClientError Message: " + ex.Message);
    }

    private void OnClientDisconnected()
    {
        Debug.LogError("loadbalancer disconnect to " + Transport.ServerUri().Host + ":" + Transport.ServerUri().Port);
    }

    private void OnClientDataSent(ArraySegment<byte> data, int arg2)
    {

    }

    private void OnClientDataReceived(ArraySegment<byte> data, int arg2)
    {
        var reader = new NetworkReader(data);
        var type = reader.ReadByte(); // read message type sequens
        if (eventHandlers.TryGetValue(type, out var handler))
        {
            handler.HandleServerEvents(reader);
        }
        else
        {
            Debug.LogError($"Event handler not found! type: {type}");
        }
    }

    private void OnClientConnected()
    {
        Debug.Log("loadbalancer connected to " + host.GetStringValue() + ":" + Transport.ServerUri().Port);

        if (AuthenticationManager.Instance != null)
        {
            var ev = new LoginEvent(AuthenticationManager.Instance.User.accessToken);
            ACGAuthenticationManager.SendClientRequestToServer(ev);
        }
    }
    #endregion

    #region Public Methods
    public void ClientSend(ArraySegment<byte> segment, int channelId = Channels.Reliable)
    {
        Transport.ClientSend(segment, channelId);
    }

    public void ServerDisconnect(int connectionId)
    {
        Transport.ServerDisconnect(connectionId);
    }
    #endregion

    #region EventManagerHandlers
    public void AddEventHandler(LoadBalancerEvent eventKey, EventManagerBase eventValue)
    {
        if (eventHandlers.ContainsKey((byte)eventKey))
        {
            eventHandlers[(byte)eventKey] = eventValue;
        }
        else
        {
            eventHandlers.Add((byte)eventKey, eventValue);
        }
    }
    public void RemoveEventHandler(LoadBalancerEvent eventKey, EventManagerBase eventValue)
    {
        if (eventHandlers.ContainsKey((byte)eventKey))
        {
            eventHandlers.Remove((byte)eventKey);
        }
    }
    public  void CloseAPP(float delay = 0)
    {
        if (closingAppCoroutine != null)
        {
            StopCoroutine(closingAppCoroutine);
        }
        closingAppCoroutine = StartCoroutine(Closing());
        IEnumerator Closing()
        {
            Debug.Log($"closing app after {delay} s");
            yield return new WaitForSecondsRealtime(delay);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
    #endregion


}
