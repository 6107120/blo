﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMap : MonoBehaviour {

	// public Transform tilePrefab;
	public Transform blockPrefab;
	public Vector3 mapSize;
	public Vector2 startPoint;

	public Vector3 finishPoint;
	List<Coord> allBlockCoords;
	Queue<Coord> shuffledBlockCoords;
	public int seed = 10;
	public int blockCount = 10;

	// Coord mapCentre;
	void Start () {
		GenerateMap();
	}

	public void GenerateMap() {
		float blockHalfSize = (float)blockPrefab.transform.localScale.x/2;
		//blocks can position from Coords
		allBlockCoords = new List<Coord> ();
		for (int y=0; y<mapSize.y; y++){
			for (int x=0; x<mapSize.x; x++){
				for (int z=0; z<mapSize.z; z++){
					allBlockCoords.Add(new Coord(x,y,z));
				}
			}
		}
		shuffledBlockCoords = new Queue<Coord>(Utility.ShuffleArray(allBlockCoords.ToArray(), seed));
		//Inheritance
		string holderName = "Generated Map";
		if(transform.FindChild(holderName)) {
			DestroyImmediate(transform.FindChild(holderName).gameObject);
		}
		Transform mapHolder = new GameObject(holderName).transform;
		mapHolder.parent = transform;

		
		for(int i=0; i<blockCount; i++) {
					Coord randomCoord = GetRandomCoord();
					Vector3 blockPosition = CoordToPosition(randomCoord.x, randomCoord.y, randomCoord.z);
					Transform newBlock = Instantiate(blockPrefab, blockPosition + Vector3.up * blockHalfSize, Quaternion.identity);
					newBlock.parent = mapHolder;
		}
		
		//first floor search
		// for(int i=0; i<mapSize.x * mapSize.z; i++) {
		// 			Coord randomCoord = GetRandomCoord();
		// 			Vector3 blockPosition = CoordToPosition(randomCoord.x, randomCoord.y, randomCoord.z);
		// 			Transform newBlock = Instantiate(blockPrefab, blockPosition + Vector3.up * blockHalfSize, Quaternion.identity);
		// 			newBlock.parent = mapHolder;
		// }
	}


// 		allTileCoords = new List<Coord> ();
// 		for (int x=0; x<mapSize.x; x++){
// 			for (int y=0; y<mapSize.y; y++){
// 				allTileCoords.Add(new Coord(x,y));
// 			}
// 		}
// 		shuffledTileCoords = new Queue<Coord>(Utility.ShuffleArray(allTileCoords.ToArray(), seed));
// 		mapCentre = new Coord ((int)mapSize.x/2, (int)mapSize.y/2);

// 		string holderName = "Generated Map";
// 		if(transform.FindChild(holderName)) {
// 			DestroyImmediate(transform.FindChild(holderName).gameObject);
// 		}
// 		Transform mapHolder = new GameObject(holderName).transform;
// 		mapHolder.parent = transform;

// 		for (int x=0; x<mapSize.x; x++){
// 			for (int y=0; y<mapSize.y; y++){
// 				Vector3 tilePosition = new Vector3(-mapSize.x/2 + 0.5f + x, 0, -mapSize.y/2 + 0.5f + y);
// 				Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right*90)) as Transform;
// 				newTile.localScale = Vector3.one * (1-outlinePercent);
// 				newTile.parent = mapHolder;
// 			}
// 		}

// 		bool[,] obstacleMap = new bool[(int)mapSize.x, (int)mapSize.y];

// 		int obstacleCount = (int)(mapSize.x * mapSize.y * obstaclePercent);
// 		int currentObstacleCount = 0;
// 		for(int i=0; i<obstacleCount; i++){
// 			Coord randomCoord = GetRandomCoord();
// 			obstacleMap[randomCoord.x, randomCoord.y] = true;
// 			currentObstacleCount ++;
// 			if(randomCoord != mapCentre && MapIsFullyAccessible(obstacleMap, currentObstacleCount)){
// 			Vector3 obstaclePosition = CoordToPosition(randomCoord.x, randomCoord.y);

// 			Transform newObstacle = Instantiate(obstaclePrefab, obstaclePosition + Vector3.up*0.5f, Quaternion.identity);
// 			newObstacle.parent = mapHolder;
// 			}
// 			else {
// 				obstacleMap[randomCoord.x, randomCoord.y] = false;
// 				currentObstacleCount --;
// 			}
// 		}
// 	}

// 	bool MapIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount) {
// 		bool[,] mapFlags = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
// 		Queue<Coord> queue = new Queue<Coord> ();
// 		queue.Enqueue(mapCentre);
// 		mapFlags[mapCentre.x, mapCentre.y] = true;

// 		int accessibleTileCount = 1;

// 		while(queue.Count > 0 ){
// 			Coord tile = queue.Dequeue();
			
// 			for(int x=-1; x<=1; x++){
// 				for(int y=-1; y<=1; y++){
// 					int neighbourX = tile.x + x;
// 					int neighbourY = tile.y + y;
// 					if(x == 0 || y == 0){
// 						if(neighbourX >= 0 && neighbourX < obstacleMap.GetLength(0) && neighbourY >= 0 && neighbourY < obstacleMap.GetLength(1)) {
// 							if(!mapFlags[neighbourX, neighbourY] && !obstacleMap[neighbourX, neighbourY]) {
// 								mapFlags[neighbourX, neighbourY] = true;
// 								queue.Enqueue(new Coord(neighbourX, neighbourY));
// 								accessibleTileCount ++;
// 							}
// 						}
// 					}
// 				}
// 			}
// 		}
// 		int targetAccessibleTileCount = (int)(mapSize.x * mapSize.y - currentObstacleCount);
// 		return targetAccessibleTileCount == accessibleTileCount;
// 	}

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

		public static bool operator ==(Coord c1, Coord c2) {
			return c1.x == c2.x && c1.y == c2.y;
		}
		public static bool operator !=(Coord c1, Coord c2) {
			return !(c1 == c2);
		}
	}
}
