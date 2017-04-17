﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMap : MonoBehaviour {

	// public Transform tilePrefab;
	public Transform blockPrefab;
	public Vector3 mapSize;
	public Vector3 startBlock;
	public Vector3 finishBlock;
	List<bool[,]> allFloors;
	Queue<Coord> shuffledBlockCoords;
	public int seed = 10;
	public int blockCount = 10;

	public Coord startPoint;

	public Coord finishPoint;
	void Start () {
		GenerateMap();
	}

	public void GenerateMap() {
		allFloors = new List<bool[,]>();
		startPoint = new Coord((int)startBlock.x, (int)startBlock.y, (int)startBlock.z);
		finishPoint = new Coord((int)finishBlock.x, (int)finishBlock.y, (int)finishBlock.z);

		//Inheritance
		string holderName = "Generated Map";
		if(transform.FindChild(holderName)) {
			DestroyImmediate(transform.FindChild(holderName).gameObject);
		}
		Transform mapHolder = new GameObject(holderName).transform;
		mapHolder.parent = transform;

		//set block on first floor
		allFloors.Add(baseBlockAccessible());
		for(int i=1; i<mapSize.y; i++)
		allFloors.Add(blockAccessible(allFloors, i));

		for (int y=0; y<mapSize.y; y++){
			for (int x=0; x<mapSize.x; x++){
				for (int z=0; z<mapSize.z; z++){
					if(allFloors[y][x,z]){
						Vector3 blockPosition = CoordToPosition(x, y, z);
						Transform newBlock = Instantiate(blockPrefab, blockPosition + Vector3.up * 0.5f, Quaternion.identity);
						newBlock.parent = mapHolder;
					}
				}
			}
		}
	}

	bool[,] baseBlockAccessible() {
		bool[,] eachFloorBlocks = new bool[(int)mapSize.x, (int)mapSize.z];
		Queue<Coord> queue = new Queue<Coord> ();
		List<Coord> list = new List<Coord> ();
		queue.Enqueue(startPoint);
		eachFloorBlocks[startPoint.x, startPoint.z] = true;

		int check = 1;
		int count = 0;

		while(count < eachFloorBlocks.GetLength(0) * eachFloorBlocks.GetLength(1)){
			Coord block = queue.Dequeue();
			for(int x=-1; x<=1; x++){
				for(int z=-1; z<=1; z++){		
					int neighbourX = block.x + x;
					int neighbourZ = block.z + z;
					if(Mathf.Abs(x) != Mathf.Abs(z)){
						if(neighbourX >= 0 && neighbourX < eachFloorBlocks.GetLength(0) && neighbourZ >= 0 && neighbourZ < eachFloorBlocks.GetLength(1)) {
							if(!eachFloorBlocks[neighbourX, neighbourZ]) {
								list.Add(new Coord(neighbourX, 0 ,neighbourZ));
							}
						}
					}
				}
			}
			list.Add(block);
			int randNum = Utility.randomNumber(list.Count, seed+count);
			Coord buf = list[randNum];
			list.Clear();
			if(!eachFloorBlocks[buf.x, buf.z]) {
				eachFloorBlocks[buf.x, buf.z] = true;
				check ++;
			}
			queue.Enqueue(buf);
			count++;
			if(check >= blockCount)
				break;
		}
		
		return eachFloorBlocks;
	}

	bool[,] blockAccessible(List<bool[,]> allFloors, int y) {
		bool[,] beforeFloorBlocks = allFloors[y-1];
		bool[,] canFloorBlocks = new bool[(int)mapSize.x, (int)mapSize.z];
		List<Coord> list = new List<Coord> ();
		int seedCount = 0;

		for (int xSize=0; xSize<mapSize.x; xSize++){
			for (int zSize=0; zSize<mapSize.z; zSize++){
				if(beforeFloorBlocks[xSize,zSize]){
					for(int x=-1; x<=1; x++){
						for(int z=-1; z<=1; z++){
							int neighbourX = xSize + x;
							int neighbourZ = zSize + z;
							if(Mathf.Abs(x) != Mathf.Abs(z) || (x==0 && z==0)){
								if(neighbourX >= 0 && neighbourX < beforeFloorBlocks.GetLength(0) && neighbourZ >= 0 && neighbourZ < beforeFloorBlocks.GetLength(1)) {
									if(Utility.TrueOrFalse(3,seedCount*seed))
									canFloorBlocks[neighbourX, neighbourZ] = true;
								}
							}
							seedCount++;
						}
					}
				}
			}
		}	
		return canFloorBlocks;
	}

	Vector3 CoordToPosition(int x, int y, int z) {
		return new Vector3(-mapSize.x/2 + 0.5f + x, y, -mapSize.z/2 + 0.5f + z);
	}

	public Coord GetRandomCoord() {
		Coord randomCoord = shuffledBlockCoords.Dequeue ();
		shuffledBlockCoords.Enqueue(randomCoord);
		return randomCoord;
	}
	public struct Coord {
		public int x;
		public int y;
		public int z;

		public Coord(int x, int y, int z) {
			this.x = x;
			this.y = y;
			this.z = z;
		}
	}
}
