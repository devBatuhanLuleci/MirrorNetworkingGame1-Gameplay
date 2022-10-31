using ACGAuthentication;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadBalancer : Singleton<LoadBalancer>
{
    [Header("Listener Setup")]
    [SerializeField] private string host = "localhost";
    [SerializeField] private bool startClientOnStart = true;
    [Space]
    [SerializeField] private Transport transport;


    private bool isServer = false;
    private bool isClient = false;
    // store all users of connected to lobby
    private Dictionary<byte, EventManagerBase> eventHandlers = new Dictionary<byte, EventManagerBase>();


    #region Managers
    public ACGAuthenticationManager AuthenticationManager { get; private set; }
    public SpawnServer SpawnServer { get; private set; }
    public LobbyManager LobbyManager { get; private set; }
    #endregion



    #region MonoBehavior Callcks

    private void Start()
    {
        Application.runInBackground = true;
        if (transport == null)
        {
            Debug.LogError("Transport not found!");
            return;
        }
        // setup client and server listeners callbacks
        SetupLinstener();
        if (startClientOnStart)
        {
            isClient = true;
            transport.ClientConnect(host);
            Debug.Log("loadbalancer connected on " + host + ":" + transport.ServerUri().Port);
        }

    }

    private void SetupManagers()
    {
        AuthenticationManager = new ACGAuthenticationManager(this);
        SpawnServer = new SpawnServer(this);
        LobbyManager = new LobbyManager(this);
    }

    private void Update()
    {
        if (isServer)
        {
            transport.ServerEarlyUpdate();
        }
        if (isClient)
        {
            transport.ClientEarlyUpdate();
        }
    }
    private void LateUpdate()
    {
        if (isServer)
        {
            transport.ServerLateUpdate();
        }
        if (isClient)
        {
            //Application.runInBackground = true;
            transport.ClientLateUpdate();
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
            transport.ServerStop();
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
        transport.OnClientConnected -= OnClientConnected;
        transport.OnClientDataReceived -= OnClientDataReceived;
        transport.OnClientDataSent -= OnClientDataSent;
        transport.OnClientDisconnected -= OnClientDisconnected;
        transport.OnClientError -= OnClientError;
    }
    #endregion
    private void SetupLinstener()
    {
        AddClientListeners();
    }

    private void AddClientListeners()
    {
        transport.OnClientConnected += OnClientConnected;
        transport.OnClientDataReceived += OnClientDataReceived;
        transport.OnClientDataSent += OnClientDataSent;
        transport.OnClientDisconnected += OnClientDisconnected;
        transport.OnClientError += OnClientError;
    }


    private void OnClientError(Exception ex)
    {
        Debug.LogError("OnClientError Message: " + ex.Message);
    }

    private void OnClientDisconnected()
    {
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
            throw new Exception($"Event handler not found! type: {type}");
        }
    }

    private void OnClientConnected()
    {
        SetupManagers();
    }
    #endregion

    #region Public Methods
    public void ClientSend(ArraySegment<byte> segment, int channelId = Channels.Reliable)
    {
        transport.ClientSend(segment, channelId);
    }

    public void ServerDisconnect(int connectionId)
    {
        transport.ServerDisconnect(connectionId);
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
    #endregion
}