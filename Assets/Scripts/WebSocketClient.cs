using System;
using UnityEngine;
using WebSocketSharp;

public class WebSocketClient : MonoBehaviour
{
    [SerializeField] private string url = "ws://localhost:8080";
    [SerializeField] private bool showLog = false;

    private int _id;
    private WebSocket _webSocket;

    public static WebSocketClient Instance;
    
    public delegate void MessageAction(WebSocketMessage webSocketMessage);
    public static event MessageAction OnMessageReceived;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
        
        _id = gameObject.GetInstanceID();
        
        _webSocket = new WebSocket(url);
        ConnectClient();
    }

    private void ConnectClient()
    {
        _webSocket.OnOpen += OnOpen;
        _webSocket.OnClose += OnClose;
        _webSocket.OnError += OnError;
        _webSocket.OnMessage += OnMessage;
        
        _webSocket.Connect ();
    }

    /*
     * Events
     */

    private void OnOpen(object sender, EventArgs e)
    {
        if (showLog) Debug.Log("WebSocketClient - OnOpen");
    }
    
    private void OnClose(object sender, CloseEventArgs e)
    {
        if (showLog) Debug.Log("WebSocketClient - OnClose");
        
        _webSocket.OnOpen -= OnOpen;
        _webSocket.OnClose -= OnClose;
        _webSocket.OnError -= OnError;
        _webSocket.OnMessage -= OnMessage;
    }
    
    private void OnError(object sender, ErrorEventArgs e)
    {
        if (showLog) Debug.Log("WebSocketClient - OnError : " + e.Message);
    }

    private void OnMessage(object sender, MessageEventArgs e)
    {        
        if (showLog) Debug.Log("WebSocketClient - OnMessage : " + e.Data);

        if (e.IsBinary) 
        {
            var webSocketMessage = ProcessMessage (e.RawData);
            
            //if (webSocketMessage.UserID == _id) return;
            
            Debug.Log("WebSocketClient - OnMessage - ID : " + webSocketMessage.UserID);
            Debug.Log("WebSocketClient - OnMessage - Type : " + webSocketMessage.Type);
            Debug.Log("WebSocketClient - OnMessage - Position : " + webSocketMessage.Message.Position);
            Debug.Log("WebSocketClient - OnMessage - Rotation : " + webSocketMessage.Message.Rotation);
            Debug.Log("WebSocketClient - OnMessage - Scale : " + webSocketMessage.Message.Scale);
            
            OnMessageReceived?.Invoke(webSocketMessage);
        }
    }
    
    /*
     * Methods
     */

    public void Send(string type, WebSocketMessage.Transform message)
    {
        if (!_webSocket.IsAlive)
        {
            ConnectClient();
            return;
        }
                    
        _webSocket.Send (CreateMessage(type, message));
    }
    
    private byte[] CreateMessage (string type, WebSocketMessage.Transform message)
    {
        return new WebSocketMessage {
                UserID = _id,
                Type = type,
                Message = message
            }
            .ToByte();
    }

    private WebSocketMessage ProcessMessage(byte[] data)
    {
        return WebSocketMessage.Parse(data);        
    }
}
