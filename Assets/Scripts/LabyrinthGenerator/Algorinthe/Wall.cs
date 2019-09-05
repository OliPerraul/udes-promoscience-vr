using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Cell
{
	public Wall (int x, int y) : base( x, y){

	}

	public override bool isWall(){
		return true;
	}
}
