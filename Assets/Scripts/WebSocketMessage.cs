using SimpleMsgPack;
using UnityEngine;

public class WebSocketMessage
{
    public int UserID {
        get; set;
    }

    public string Type {
        get; set;
    }

    public Transform Message {
        get; set;
    }

    public override string ToString ()
    {        
        return JsonUtility.ToJson(this);
    }

    public byte[] ToByte()
    {
        var msgPack = new MsgPack();
        
        msgPack.ForcePathObject("p.user_id").AsInteger = UserID;
        msgPack.ForcePathObject("p.type").AsString = Type;
        msgPack.ForcePathObject("p.message").AsString = JsonUtility.ToJson(Message);
        
        return msgPack.Encode2Bytes();
    }

    public static WebSocketMessage Parse(byte[] data)
    {
        var msgPack = new MsgPack();
        
        msgPack.DecodeFromBytes(data);

        var id = (int) msgPack.ForcePathObject("p.user_id").AsInteger;
        var type = msgPack.ForcePathObject("p.type").AsString;
        var message = JsonUtility.FromJson<Transform>(msgPack.ForcePathObject("p.message").AsString);

        return new WebSocketMessage
        {
            UserID = id,
            Type = type,
            Message = message
        };
    }
    
    public struct Transform 
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;
    }
}
