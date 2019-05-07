using Newtonsoft.Json;
using SimpleMsgPack;

internal class TextMessage
{
    [JsonProperty ("user_id")]
    public int UserID {
        get; set;
    }

    [JsonProperty ("type")]
    public string Type {
        get; set;
    }

    [JsonProperty ("message")]
    public string Message {
        get; set;
    }

    public override string ToString ()
    {        
        return JsonConvert.SerializeObject (this);
    }

    public byte[] ToByte()
    {
        var msgPack = new MsgPack();
        
        msgPack.ForcePathObject("p.user_id").AsInteger = UserID;
        msgPack.ForcePathObject("p.type").AsString = Type;
        msgPack.ForcePathObject("p.message").AsString = Message;
        
        return msgPack.Encode2Bytes();
    }

    public static string Parse(byte[] data)
    {
        var msgPack = new MsgPack();
        
        msgPack.DecodeFromBytes(data);

        var id = (int) msgPack.ForcePathObject("p.user_id").AsInteger;
        var type = msgPack.ForcePathObject("p.type").AsString;
        var message = msgPack.ForcePathObject("p.message").AsString;

        return new TextMessage
        {
            UserID = id,
            Type = type,
            Message = message
        }.ToString();
    }
}
