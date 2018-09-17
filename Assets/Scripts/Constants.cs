using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

static class Constants
{
    //Game states and player status
    public const int NOT_READY = 0;
    public const int READY = 1;
    public const int PAIRING = 2;
    public const int NO_ASSOCIATED_PAIR = 3;
    public const int PAIRED = 4;
    public const int PLAYING_TUTORIAL = 5;
    public const int PLAYING = 6;
    public const int WAITING_FOR_NEXT_ROUND = 7;

    //Device types
    public const int ANDROID_TABLET = 0;
    public const int OCCULUS_GO_HEADSET = 1;

    //Device names
    public const string SAMSUNG_TABLET_SMT380 = "samsung SM-T380";
    public const string OCCULUS_GO_PACIFIC = "Oculus Pacific";

    //Algorith identifier
    public const int TUTORIAL_ALGORITH = 0;

    //Algorith identifier
    public const int TUTORIAL_ALGORITH = 0;

    //Algorith identifier
    public const int TUTORIAL_ALGORITH = 0;

    //Labyrinth tiles
    public const float tileSize = 5.0f;

    //0-49 Floors
    public const int TILE_FLOOR_START_ID = 0;
    public const int TILE_FLOOR_1_ID = 1;

    //50-99 Walls
    public const int TILE_WALL_1_ID = 50;

    //100-149 Ends
    public const int TILE_END_1_ID = 100;

    //Directives
    public const int DIRECTIVE_TURN_RIGHT = 0;
    public const int DIRECTIVE_TURN_LEFT = 1;
    public const int DIRECTIVE_MOVE_FOWARD = 2;
    public const int DIRECTIVE_UTURN = 3;
    public const int DIRECTIVE_STOP = 4;
}
