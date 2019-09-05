using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
	private int x;
	private int y;
	private bool visited;
	private bool current;

	public Cell(int x, int y){
		this.x = x;
		this.y = y;
		visited = false;
		current = false;
	}


	public int getX(){
		return x;
	}

	public int getY(){
		return y;
	}

	public virtual bool getCurrent(){
		return current;
	}

	public virtual void setCurrent(){
		current = true;
	}

	public virtual void setNonCurrent(){
		current = false;
	}

	public bool getVisited(){
		return visited;
	}

	public void isVisited(){
		visited = true;
	}

	public virtual int getCount(){
		return 0;
	}

	public virtual void decCount(){
		
	}

	public virtual int getRoad(int i){
		return -1;
	}

	public virtual bool isBorderWall(){
		return false;
	}

	public virtual bool isRoad(){
		return false;
	}

	public virtual bool isWall(){
		return false;
	}

	public virtual bool isStart(){
		return false;
	}

	public virtual bool isEnd(){
		return false;
	}

	public virtual int translate(int st){
		return 50;
	}

    public virtual bool isVerticalWall()
    {
        return false;
    }

    public virtual bool isHorizontalWall()
    {
        return false;
    }

    public virtual bool isTower()
    {
        return false;
    }
}
