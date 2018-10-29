using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public struct Tile
{
    public int x;
    public int y;
    public TileColor color;

    public Tile(int xPosition, int yPosition, TileColor tileColor)
    {
        x = xPosition;
        y = yPosition;
        color = tileColor;
    }
}

