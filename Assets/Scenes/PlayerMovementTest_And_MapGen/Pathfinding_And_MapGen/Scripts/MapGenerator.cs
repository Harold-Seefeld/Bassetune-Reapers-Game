using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SocketIO;

public class MapGenerator : MonoBehaviour {

	public int width;
	public int height;
	public string seed;
	public bool useRandomSeed;

	//adding the socketIO gameobject
	private GameObject socketObject;
	public SocketIOComponent socket;

	[Range(0,100)]
	public int randomFillPercent;
	
	int[,] map;

	void Start()
	{
		//initialize the socket emitter.
		socketObject = GameObject.Find ("SocketIO");
		socket = socketObject.GetComponent<SocketIOComponent>();
		socket.On("listening", OpenSocket);

		GenerateMap();

	}

	private void OpenSocket(SocketIOEvent ev)
	{
		Debug.Log("listening for emitter");
	}

	void GenerateMap()
	{
		map = new int[width,height];
		RandomFillMap();

		for(int i = 0; i < 5; i++)
		{
			SmoothMap();
		
		}
	
		ProcessMap();

		int borderSize = 10;
		int[,] borderedMap = new int[width + borderSize * 2, height + borderSize * 2];
		for(int x = 0; x < borderedMap.GetLength(0); x++)
		{
			for(int y = 0; y < borderedMap.GetLength(1); y++)
			{
				if(x >= borderSize && x < width + borderSize && y >= borderSize && y< height + borderSize)
				{
					borderedMap[x,y] = map[x-borderSize, y-borderSize];
				}
				else
				{
					borderedMap[x,y] = 1;
				}
			
			}
		}

		MeshGenerator meshGen = GetComponent<MeshGenerator>();
		meshGen.GenerateMesh(borderedMap, 1);
	}

	List<List<Coord>> GetRegions(int tileType)
	{
		List<List<Coord>> regions = new List<List<Coord>> ();
		int[,] mapFlags = new int[width,height];

		//create a json object to add a json object array
		JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);

		for(int x = 0; x < width; x++)
		{
			for(int y = 0; y < height; y++)
			{
				if(mapFlags[x,y] == 0 && map[x,y] == tileType)
				{
					List<Coord> newRegion = GetRegionTiles(x,y);
					regions.Add (newRegion);

					foreach(Coord tile in newRegion)
					{
						mapFlags[tile.tileX, tile.tileY] = 1;
                    }
                }
			}
		}

        foreach (var region in regions)
        {
            foreach (var tile in region)
            {
                JSONObject regionsData = new JSONObject(JSONObject.Type.ARRAY);

                regionsData.Add(tile.tileX);
                regionsData.Add(tile.tileY);

                jsonObject.Add(regionsData);
            }
        }

        //emit the array of regions to the server
        socket.Emit("making regions data " + tileType.ToString(), jsonObject);

		return regions;
	}

	void ProcessMap()
	{
		List<List<Coord>> wallRegions = GetRegions(1);

		int wallThresholdSize = 50;

		foreach(List<Coord> wallRegion in wallRegions)
		{
			if(wallRegion.Count < wallThresholdSize)
			{
				foreach(Coord tile in wallRegion)
				{
					map[tile.tileX, tile.tileY] = 0;
				}
			}
		}
	
		List<List<Coord>> roomRegions = GetRegions(0);
		
		int roomThresholdSize = 50;
		List<Room> survivingRooms = new List<Room> ();
		
		foreach(List<Coord> roomRegion in roomRegions)
		{
			if(roomRegion.Count < roomThresholdSize)
			{
				foreach(Coord tile in roomRegion)
				{
					map[tile.tileX, tile.tileY] = 1;
				}
			}
			else
			{
				survivingRooms.Add(new Room(roomRegion, map));
			}
		}

		survivingRooms.Sort();
		foreach(Room r in survivingRooms)
		{
			print (r.roomSize);
		}
		ConnectClosestRooms(survivingRooms);
	}

	void ConnectClosestRooms(List<Room> allRooms)
	{
		int bestDistance = 0;
		Coord bestTileA = new Coord();
		Coord bestTileB = new Coord();
		Room bestRoomA = new Room();
		Room bestRoomB = new Room();
		bool possibleConnectionFound = false;

		foreach(Room roomA in allRooms)
		{
			possibleConnectionFound = false;

			foreach(Room roomB in allRooms)
			{
				if(roomA == roomB)
				{
					continue;
				}

				if(roomA.IsConnected(roomB))
				{
					possibleConnectionFound = false;
					break;
				}

				for(int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA++)
				{
					for(int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB++)
					{
						Coord tileA = roomA.edgeTiles[tileIndexA];
						Coord tileB = roomB.edgeTiles[tileIndexB];
						int distanceBetweenRooms = (int)(Mathf.Pow(tileA.tileX-tileB.tileX,2) + Mathf.Pow(tileA.tileY-tileB.tileY,2));
					
						if(distanceBetweenRooms < bestDistance || !possibleConnectionFound)
						{
							bestDistance = distanceBetweenRooms;
							possibleConnectionFound = true;
							bestTileA = tileA;
							bestTileB = tileB;
							bestRoomA = roomA;
							bestRoomB = roomB;
						}
					}
				}
			}
		
			if(possibleConnectionFound)
			{
				CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
			}
		}
	}

	void CreatePassage(Room roomA, Room roomB, Coord tileA, Coord tileB)
	{
		Room.ConnectRooms(roomA, roomB);
		Debug.DrawLine(CoordToWorldPoint(tileA), CoordToWorldPoint(tileB), Color.green, 100);
	}

	Vector3 CoordToWorldPoint(Coord tile)
	{
		return new Vector3(-width/2+.5f + tile.tileX, 2, -height/2+.5f + tile.tileY);
	}

	List<Coord> GetRegionTiles(int startX, int startY)
	{
		List<Coord> tiles = new List<Coord> ();
		int[,] mapFlags = new int[width,height];
		int tileType = map[startX, startY];

		Queue<Coord> queue = new Queue<Coord>();
		queue.Enqueue(new Coord(startX, startY));
		mapFlags[startX, startY] = 1;

		while(queue.Count > 0)
		{
			Coord tile = queue.Dequeue();
			tiles.Add(tile);

			for(int x = tile.tileX -1; x <= tile.tileX + 1; x++)
			{
				for(int y = tile.tileY -1; y <= tile.tileY + 1; y++)
				{
					if(IsInMapRange(x, y) && (y == tile.tileY || x == tile.tileX))
					{
						if(mapFlags[x,y] == 0 && map[x,y] == tileType)
						{
							mapFlags[x,y] =1;
							queue.Enqueue(new Coord(x,y));
						}
					}
				}
			}
		}

		return tiles;
	}

	bool IsInMapRange(int x, int y)
	{
		return x >= 0 && x < width && y >= 0 && y < height;
	}

	void RandomFillMap()
	{
		//create a json object to add a json object array
		JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);
		
		//create a json object type array  
		JSONObject mapData = new JSONObject(JSONObject.Type.ARRAY);

		if(useRandomSeed)
		{
            seed = UnityEngine.Random.Range(1, 10000).ToString();

        }

		System.Random pRandom = new System.Random(seed.GetHashCode());

		for(int x = 0; x < width; x++)
		{
			mapData.Add(x);

			for(int y = 0; y < height; y++)
			{
				if(x == 0 || x == width - 1 || y == 0 || y == height -1)
				{
					map[x,y] = 1;
				}
				else
				{
					map[x,y] = (pRandom.Next(0, 100) < randomFillPercent)? 1: 0;
				}
			
				mapData.Add(y);

			}
		}
	
		jsonObject.AddField("map data", mapData);
		
		//emit the array of map to the server
		socket.Emit("making map data", jsonObject);
	}

	void SmoothMap()
	{
		for(int x = 0; x < width; x++)
		{
			for(int y = 0; y < height; y++)
			{
				int neighbourWallTiles = GetSurroundingWallCount(x, y);
				if(neighbourWallTiles > 4)
				{
					map[x,y] = 1;
				}
				else if(neighbourWallTiles < 4)
				{
					map[x,y] = 0;
				}
				
			}
		}
	}

	int GetSurroundingWallCount(int gridX, int gridY)
	{
		int wallCount = 0;

		for(int neighbourX = gridX -1; neighbourX <= gridX +1; neighbourX++)
		{
			for(int neighbourY = gridY -1; neighbourY <= gridY +1; neighbourY++)
			{
				if(IsInMapRange(neighbourX, neighbourY))
				{
					if(neighbourX != gridX || neighbourY != gridY)
					{
						wallCount += map[neighbourX, neighbourY];
					}
				}
				else
				{
					wallCount ++;
				}
			}

		}
	
		return wallCount;
	}

	struct Coord
	{
		public int tileX;
		public int tileY;

		public Coord(int x, int y)
		{
			tileX = x;
			tileY = y;
		}
	}

	class Room :IComparable<Room>
	{
		public List<Coord> tiles;
		public List<Coord> edgeTiles;
		public List<Room> connectedRooms;
		public int roomSize;

		//adding the socketIO gameobject
		private GameObject socketObject;
		private SocketIOComponent socket;

		public Room()
		{
		}

		public Room(List<Coord> roomTiles, int[,] map)
		{
			tiles = roomTiles;
			roomSize = tiles.Count;
			connectedRooms = new List<Room>();

			edgeTiles = new List<Coord>();

			foreach(Coord tile in tiles)
			{
				for(int x = tile.tileX-1; x <= tile.tileX+1; x++)
				{
					for(int y = tile.tileY-1; y <= tile.tileY+1; y++)
					{
						if(x == tile.tileX || y == tile.tileY)
						{
							if(map[x,y] == 1)
							{
								edgeTiles.Add(tile);
							}
						}
					}

				}
			}
		}

		public static void ConnectRooms(Room roomA, Room roomB)
		{
			roomA.connectedRooms.Add(roomB);
			roomB.connectedRooms.Add(roomA);
		}

		public bool IsConnected(Room otherRoom)
		{
			return connectedRooms.Contains(otherRoom);


		}

		public int CompareTo(Room otherRoom)
		{
			return otherRoom.roomSize.CompareTo(roomSize);
		} 
	}
}
