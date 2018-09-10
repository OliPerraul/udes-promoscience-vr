using UnityEngine.Networking;

public class CustomMsgType
{
    public const short Action = 100;
    public const short Directive = 101;
    public const short PairingRequest = 102;
    public const short PairingResult = 103;
}

public class ActionMessage : MessageBase
{
    public int actionId;
}

public class DirectiveMessage : MessageBase
{
    public int directiveId;
}

public class PairingRequestMessage : MessageBase
{
    public int deviceType;
    public string deviceId;
}

public class PairingResultMessage : MessageBase
{
    public bool isPairingSucess;
    public string message;
}