using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynchronizeTransform : MonoBehaviour
{
    private void Update()
    {
        if (WebSocketClient.Instance != null)
        {
            WebSocketClient.Instance.Send("position", transform.position.ToString());
            WebSocketClient.Instance.Send("rotation", transform.rotation.ToString());
            WebSocketClient.Instance.Send("scale", transform.localScale.ToString());
        }
    }
}
