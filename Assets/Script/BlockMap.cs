using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMap : MonoBehaviour {

	public Transform blockPrefab;
	public Vector3 mapSize;
	public Vector3 startBlock;
	List<bool[,]> allFloors;
	int[] proportionSize;
	Queue<Coord2> shuffledBlockCoords;
	public int seed = 10;
	[RangeAttribute(0,50)]
	public int blockCount = 10;
	[RangeAttribute(0,1)]
	public float blockPercent = 1;
	

	Coord2 startPoint;

	void Start () {
		GenerateMap();
	}

	public void GenerateMap() {
		allFloors = new List<bool[,]>();
		startPoint = new Coord2((int)startBlock.x, (int)startBlock.z);

		// int proportionSizeBlock = 3;
		// proportionSize = new int[(int)mapSize.y];
		// proportionSize[proportionSize.GetLength(0)-1] = 1;
		// proportionSize[proportionSize.GetLength(0)-2] = 3;
		// for(int i=proportionSize.GetLength(0)-3; i>=0; i--){
		// 	proportionSize[i] = 1 + proportionSizeBlock;
		// 	proportionSizeBlock += (int)mapSize.x;
		// }

		int[,] availableBlock = new int[(int)mapSize.y,3];
		int[] availableArea = new int[(int)mapSize.y];

		for(int i=0; i<availableArea.GetLength(0); i++){
			int result = (int)mapSize.z / (i+1);
			if(result == 0)
				result = 1;
			availableArea[availableArea.GetLength(0)-1 - i] = result;
		}
		for(int i=0; i<availableBlock.GetLength(0); i++){
			int minResult = (int)Mathf.Sqrt(mapSize.x * availableArea[i]);
			int maxResult = (int)(minResult * minResult * blockPercent);
			if(maxResult < minResult + 1)
				maxResult = minResult + 1;
			if(maxResult > (int)mapSize.x * availableArea[i])
				maxResult = (int)mapSize.x * availableArea[i];
			availableBlock[i,0] = minResult;
			availableBlock[i,1] = maxResult;
		}
		for(int i=0; i<availableBlock.GetLength(0); i++){
			availableBlock[i,2] = Utility.randomNumber(availableBlock[i,0], availableBlock[i,1]);
			print(availableBlock[i,0]);
			print(availableBlock[i,1]);
		}


		//Inheritance
		string holderName = "Generated Map";
		if(transform.FindChild(holderName)) {
			DestroyImmediate(transform.FindChild(holderName).gameObject);
		}
		Transform mapHolder = new GameObject(holderName).transform;
		mapHolder.parent = transform;

		//set block on first floor
		allFloors.Add(baseBlockAccessible(availableBlock));
		for(int i=1; i<mapSize.y; i++)
		allFloors.Add(blockAccessible(allFloors, i, availableBlock, availableArea));

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

	bool[,] baseBlockAccessible(int[,] availableBlock) {
		bool[,] eachFloorBlocks = new bool[(int)mapSize.x, (int)mapSize.z];
		List<Coord2> list = new List<Coord2> ();

		eachFloorBlocks[startPoint.x, startPoint.z] = true;
		Coord2 block = startPoint;

		int check = 1;
		int count = 0;

		for(int i=0; i<eachFloorBlocks.GetLength(0) * eachFloorBlocks.GetLength(1); i++){
			for(int x=-1; x<=1; x++){
				for(int z=-1; z<=1; z++){		
					int neighbourX = block.x + x;
					int neighbourZ = block.z + z;
					if(Mathf.Abs(x) != Mathf.Abs(z)){
						if(neighbourX >= 0 && neighbourX < eachFloorBlocks.GetLength(0) && neighbourZ >= 0 && neighbourZ < eachFloorBlocks.GetLength(1)) {
							if(!eachFloorBlocks[neighbourX, neighbourZ]) {
								list.Add(new Coord2(neighbourX, neighbourZ));
							}
						}
					}
				}
			}
			list.Add(block);
			int randNum = Utility.randomNumber(list.Count, seed+count);
			Coord2 buf = list[randNum];
			list.Clear();
			if(!eachFloorBlocks[buf.x, buf.z]) {
				eachFloorBlocks[buf.x, buf.z] = true;
				check ++;
			}
			block = buf;
			count++;
			if(check >= availableBlock[0,2])
				break;
		}
		
		return eachFloorBlocks;
	}

	bool[,] blockAccessible(List<bool[,]> allFloors, int y, int[,] availableBlock, int[] availableArea) {
		bool[,] beforeFloorBlocks = allFloors[y-1];
		bool[,] canFloorBlocks = new bool[(int)mapSize.x, (int)mapSize.z];
		int reductionSize = (y>=(int)mapSize.x)? (int)mapSize.x-1 : y;
		List<Coord2> list = new List<Coord2> ();
		Queue<Coord2> queue;
		int count = 0;

		for (int xSize=0; xSize<mapSize.x; xSize++){
			for (int zSize=reductionSize; zSize<mapSize.z; zSize++){
				if(beforeFloorBlocks[xSize,zSize]){
					for(int x=-1; x<=1; x++){
						for(int z=-1; z<=1; z++){
							int neighbourX = xSize + x;
							int neighbourZ = zSize + z;
							if(Mathf.Abs(x) != Mathf.Abs(z) || (x==0 && z==0)){
								if(neighbourX >= 0 && neighbourX < beforeFloorBlocks.GetLength(0) && neighbourZ >= 0 && neighbourZ < beforeFloorBlocks.GetLength(1)) {
									list.Add(new Coord2(neighbourX, neighbourZ));
								}
							}
						}
					}
				}
			}
		}
		if(list.Count > 0){
			queue = new Queue<Coord2>(Utility.ShuffleArray(list.ToArray(), seed));
			for(int i=0; i<availableBlock[y,2]; i++){
				if(queue.Count == 0)
					break;
				Coord2 buf = queue.Dequeue();
				canFloorBlocks[buf.x, buf.z] = true;
				count++;
			}
		}
		return canFloorBlocks;
	}

	Vector3 CoordToPosition(int x, int y, int z) {
		return new Vector3(-mapSize.x/2 + 0.5f + x, y, -mapSize.z/2 + 0.5f + z);
	}

	public Coord2 GetRandomCoord() {
		Coord2 randomCoord = shuffledBlockCoords.Dequeue ();
		shuffledBlockCoords.Enqueue(randomCoord);
		return randomCoord;
	}
	public struct Coord3 {
		public int x;
		public int y;
		public int z;

		public Coord3(int x, int y, int z) {
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
