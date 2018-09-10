using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

static class Constants
{
    //Player Status
    public const int NOT_READY_STATUS_ID = 0;
    public const int READY_STATUS_ID = 1;
    public const int PAIRING = 2;
    public const int NO_ASSOCIATED_PAIR = 3;
    public const int PAIRED = 4;

    //Device types
    public const int ANDROID_TABLET = 0;
    public const int OCCULUS_GO_HEADSET = 1;

    //Device names
    public const string SAMSUNG_TABLET_SMT380 = "samsung SM-T380";
    public const string OCCULUS_GO_PACIFIC = "Oculus Pacific";

    //Server ip adress (temp)
    public const string SERVER_IP_ADRESS = "192.168.0.102";// should use local network discovery
}
