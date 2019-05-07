using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using UnityEngine;
using WebSocketSharp;

public class WebSocketClient : MonoBehaviour
{
    [SerializeField] private string url = "ws://localhost:8080";
    [SerializeField] private bool showLog = false;

    private int _id;
    private WebSocket _webSocket;

    public static WebSocketClient Instance;
    
    private void Awake()
    {
        _id = gameObject.GetInstanceID();

        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        _webSocket = new WebSocket(url);
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
            var data = ProcessTextMessage (e.RawData);
            
            var id = (uint) data["user_id"];
            var type = (string) data["type"];
            var message = (string) data["message"];

            if (id == _id) return;
            
            //Send Event Action
            
            Debug.Log("id : " + id);
            Debug.Log("type : " + type);
            Debug.Log("message : " + message);
        }
    }
    
    /*
     * Methods
     */

    public void Send(string type, string text)
    {
        
        _webSocket.Send (CreateTextMessage(type, text));
    }
    
    private byte[] CreateTextMessage (string type, string message)
    {
        return new TextMessage {
                UserID = _id,
                Type = type,
                Message = message
            }
            .ToByte();
    }

    private JObject ProcessTextMessage(byte[] data)
    {
        return JObject.Parse(TextMessage.Parse(data));        
    }
}
