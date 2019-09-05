using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRoad : Road
{
  public StartRoad (int x, int y): base(x,y){

  }

  public override bool isStart(){
  	return true;
  }

  public override int translate(int st){
  	switch(st){
  		case 1:
  			return Constants.TILE_ROME_START_ID;
  		case 2:
  			return Constants.TILE_PTOL_START_ID;
  		case 3:
  			return Constants.TILE_BRIT_START_ID;
  		default:
  			return Constants.TILE_KART_START_ID;
  	}
  }
}
