using UnityEngine.Networking;

public class CustomMsgType
{
    public const short Action = 100;
    public const short Directive = 101;

}

public class ActionMessage : MessageBase
{
    public int actionId;
}

public class DirectiveMessage : MessageBase
{
    public int directiveId;
}