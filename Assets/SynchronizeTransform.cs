using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynchronizeTransform : MonoBehaviour
{
    private WebSocketMessage.Transform _transform;

    private void Awake()
    {
        _transform = new WebSocketMessage.Transform();
    }

    private void Update()
    {
        if (WebSocketClient.Instance != null)
        {
            _transform.Position = transform.position;
            _transform.Rotation = transform.rotation;
            _transform.Scale = transform.localScale;
            
            WebSocketClient.Instance.Send("transform", _transform);
        }
    }
}
