using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMap : MonoBehaviour {

	// public Transform tilePrefab;
	public Transform blockPrefab;
	public Vector3 mapSize;
	public Vector3 startBlock;
	List<bool[,]> allFloors;
	int[] proportionSize;
	Queue<Coord> shuffledBlockCoords;
	public int seed = 10;
	public int blockCount = 10;

	Coord startPoint;

	void Start () {
		GenerateMap();
	}

	public void GenerateMap() {
		allFloors = new List<bool[,]>();
		startPoint = new Coord((int)startBlock.x, (int)startBlock.y, (int)startBlock.z);

		// int proportionSizeBlock = 3;
		// proportionSize = new int[(int)mapSize.y];
		// proportionSize[proportionSize.GetLength(0)-1] = 1;
		// proportionSize[proportionSize.GetLength(0)-2] = 3;
		// for(int i=proportionSize.GetLength(0)-3; i>=0; i--){
		// 	proportionSize[i] = 1 + proportionSizeBlock;
		// 	proportionSizeBlock += (int)mapSize.x;
		// }


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
		int reductionSize = (y>=(int)mapSize.x)? (int)mapSize.x-1 : y;
		List<Coord> list = new List<Coord> ();
		Queue<Coord> queue;
		int count = 0;

		for (int xSize=reductionSize; xSize<mapSize.x; xSize++){
			for (int zSize=0; zSize<mapSize.z; zSize++){
				if(beforeFloorBlocks[xSize,zSize]){
					for(int x=-1; x<=1; x++){
						for(int z=-1; z<=1; z++){
							int neighbourX = xSize + x;
							int neighbourZ = zSize + z;
							if(Mathf.Abs(x) != Mathf.Abs(z) || (x==0 && z==0)){
								if(neighbourX >= 0 && neighbourX < beforeFloorBlocks.GetLength(0) && neighbourZ >= 0 && neighbourZ < beforeFloorBlocks.GetLength(1)) {
									list.Add(new Coord(neighbourX, 0, neighbourZ));
								}
							}
						}
					}
				}
			}
		}
		queue = new Queue<Coord>(Utility.ShuffleArray(list.ToArray(), seed));
		while(count < blockCount){
			Coord buf = queue.Dequeue();
			canFloorBlocks[buf.x, buf.z] = true;
			count++;
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

	public struct Coord2 {
		public int x;
		public int z;

		public Coord2(int x, int z) {
			this.x = x;
			this.z = z;
		}
	}
}
