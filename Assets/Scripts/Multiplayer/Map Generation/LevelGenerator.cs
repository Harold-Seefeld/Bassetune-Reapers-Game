using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    // The type of tile that will be laid in a specific position.
    public enum TileType
    {
        Empty,
        Floor,
        BottomLeftCorner,
        TopLeftCorner,
        BottomRightCorner,
        TopRightCorner,
        LeftWall,
        RightWall,
        TopWall,
        BottomWall
    }

    [SerializeField] private int columns = 100;                                 // The number of columns on the board (how wide it will be).
    [SerializeField] private int rows = 100;                                    // The number of rows on the board (how tall it will be).
    [SerializeField] private float spacing = 8;                                     // The spacing between tiles
    [SerializeField] private IntRange numRooms = new IntRange(15, 20);          // The range of the number of rooms there can be.
    [SerializeField] private IntRange roomWidth = new IntRange(3, 10);          // The range of widths rooms can have.
    [SerializeField] private IntRange roomHeight = new IntRange(3, 10);         // The range of heights rooms can have.
    [SerializeField] private IntRange corridorLength = new IntRange(6, 10);     // The range of lengths corridors between rooms can have.
    [SerializeField] private GameObject[] floorTiles;                           // An array of floor tile prefabs.
    [SerializeField] private GameObject[] wallTiles;                            // An array of wall tile prefabs.
    [SerializeField] private GameObject player;

    private TileType[][] tiles;                               // A jagged array of tile types representing the board, like a grid.
    private Room[] rooms;                                     // All the rooms that are created for this board.
    private Corridor[] corridors;                             // All the corridors that connect the rooms.
    private GameObject boardHolder;                           // GameObject that acts as a container for all other tiles.

    private void Start()
    {
        // Create the board holder.
        boardHolder = new GameObject("Level 1");

        SetupTilesArray();

        CreateRoomsAndCorridors();

        SetTilesValuesForRooms();
        SetTilesValuesForCorridors();

        InstantiateTiles();
    }

    void SetupTilesArray()
    {
        // Set the tiles jagged array to the correct width.
        tiles = new TileType[columns][];

        // Go through all the tile arrays...
        for (int i = 0; i < tiles.Length; i++)
        {
            // ... and set each tile array is the correct height.
            tiles[i] = new TileType[rows];
        }
    }

    void CreateRoomsAndCorridors()
    {
        // Create the rooms array with a random size.
        rooms = new Room[numRooms.Random];

        // There should be one less corridor than there is rooms.
        corridors = new Corridor[rooms.Length - 1];

        // Create the first room and corridor.
        rooms[0] = new Room();
        corridors[0] = new Corridor();

        // Setup the first room, there is no previous corridor so we do not use one.
        rooms[0].SetupRoom(roomWidth, roomHeight, columns, rows);

        // Setup the first corridor using the first room.
        corridors[0].SetupCorridor(rooms[0], corridorLength, roomWidth, roomHeight, columns, rows, true);

        for (int i = 1; i < rooms.Length; i++)
        {
            // Create a room.
            rooms[i] = new Room();

            // Setup the room based on the previous corridor.
            rooms[i].SetupRoom(roomWidth, roomHeight, columns, rows, corridors[i - 1]);

            // If we haven't reached the end of the corridors array...
            if (i < corridors.Length)
            {
                // ... create a corridor.
                corridors[i] = new Corridor();

                // Setup the corridor based on the room that was just created.
                corridors[i].SetupCorridor(rooms[i], corridorLength, roomWidth, roomHeight, columns, rows, false);
            }

            if (i == rooms.Length * .5f)
            {
                Vector3 playerPos = new Vector3(rooms[i].x, rooms[i].y, 0);
                if (player != null) Instantiate(player, playerPos, Quaternion.identity);
            }
        }

    }

    void SetTilesValuesForRooms()
    {
        // Go through all the rooms...
        for (int i = 0; i < rooms.Length; i++)
        {
            Room currentRoom = rooms[i];

            // ... and for each room go through it's width.
            for (int j = 0; j < currentRoom.width; j++)
            {
                int xCoord = currentRoom.x + j;

                // For each horizontal tile, go up vertically through the room's height.
                for (int k = 0; k < currentRoom.height; k++)
                {
                    int yCoord = currentRoom.y + k;

                    // The coordinates in the jagged array are based on the room's position and it's width and height.
                    tiles[xCoord][yCoord] = TileType.Floor;
                }
            }
            SetupRoomWalls(currentRoom);
        }
    }

    void SetTilesValuesForCorridors()
    {
        // Go through every corridor...
        for (int i = 0; i < corridors.Length; i++)
        {
            Corridor currentCorridor = corridors[i];

            // and go through it's length.
            for (int j = 0; j < currentCorridor.length; j++)
            {
                // Start the coordinates at the start of the corridor.
                int xCoord = currentCorridor.startX;
                int yCoord = currentCorridor.startY;

                // Depending on the direction, add or subtract from the appropriate
                // coordinate based on how far through the length the loop is.
                switch (currentCorridor.direction)
                {
                    case Direction.North:
                        yCoord += j;
                        break;
                    case Direction.East:
                        xCoord += j;
                        break;
                    case Direction.South:
                        yCoord -= j;
                        break;
                    case Direction.West:
                        xCoord -= j;
                        break;
                }

                // Set the tile at these coordinates to Floor.
                tiles[xCoord][yCoord] = TileType.Floor;
            }
            SetupCorridorWalls(currentCorridor);
        }
    }

    private void SetupRoomWalls(Room currentRoom)
    {
        SetupRoomWalls(currentRoom.x, currentRoom.y, currentRoom.width, currentRoom.height);
    }

    private void SetupRoomWalls(int x, int y, int width, int height)
    {
        //To encapsulate the room width and height have to increase by 2
        width += 1;
        height += 2;

        //Origin of room is bottom left, to encapsulate decrement axis 
        x--;
        y--;

        try
        {
            SetCorners(x, y, width, height);
        }
        catch
        {
            print("Corners out of bounds");
        }

        try
        {

            //Set walls between corners
            for (int i = 1; i < height - 1; i++)
            {
                if (tiles[x][y + i] == TileType.Empty)
                    tiles[x][y + i] = TileType.LeftWall;
                if (tiles[x + width][y + i] == TileType.Empty)
                    tiles[x + width][y + i] = TileType.RightWall;
            }
            for (int i = 1; i < width; i++)
            {
                if (tiles[x + i][y] == TileType.Empty)
                    tiles[x + i][y] = TileType.BottomWall;
                if (tiles[x + i][y + height - 1] == TileType.Empty)
                    tiles[x + i][y + height - 1] = TileType.TopWall;
            }
        }
        catch
        {
            print("Wall out of bounds");
        }
    }

    private void SetupCorridorWalls(Corridor currentCorridor)
    {
        SetupCorridorWalls(currentCorridor.startX, currentCorridor.startY, currentCorridor.length - 1,
            currentCorridor.direction);
    }

    private void SetupCorridorWalls(int x, int y, int length, Direction direction)
    {
        //For loop after switch to save performance
        switch (direction)
        {
            case Direction.North:
                for (int i = 0; i < length; i++)
                {
                    if (tiles[x - 1][y + i] == TileType.Empty)
                        tiles[x - 1][y + i] = TileType.LeftWall;
                    if (tiles[x + 1][y + i] == TileType.Empty)
                        tiles[x + 1][y + i] = TileType.RightWall;
                }
                break;
            case Direction.East:
                for (int i = 0; i < length; i++)
                {
                    if (tiles[x + i][y - 1] == TileType.Empty)
                        tiles[x + i][y - 1] = TileType.BottomWall;
                    if (tiles[x + i][y + 1] == TileType.Empty)
                        tiles[x + i][y + 1] = TileType.TopWall;
                }
                break;
            case Direction.South:
                for (int i = 0; i < length; i++)
                {
                    if (tiles[x - 1][y - i] == TileType.Empty)
                        tiles[x - 1][y - i] = TileType.LeftWall;
                    if (tiles[x + 1][y - i] == TileType.Empty)
                        tiles[x + 1][y - i] = TileType.RightWall;
                }
                break;
            case Direction.West:
                for (int i = 0; i < length; i++)
                {
                    if (tiles[x - i][y - 1] == TileType.Empty)
                        tiles[x - i][y - 1] = TileType.BottomWall;
                    if (tiles[x - i][y + 1] == TileType.Empty)
                        tiles[x - i][y + 1] = TileType.TopWall;
                }
                break;
        }
    }

    private void SetCorners(int x, int y, int width, int height)
    {
        //Manually set corners for performance
        if (tiles[x][y] == TileType.Empty)
            tiles[x][y] = TileType.BottomLeftCorner;
        if (tiles[x + width][y] == TileType.Empty)
            tiles[x + width][y] = TileType.BottomRightCorner;
        if (tiles[x][y + height] == TileType.Empty)
            tiles[x][y + height - 1] = TileType.TopLeftCorner;
        if (tiles[x + width][y + height] == TileType.Empty)
            tiles[x + width][y + height - 1] = TileType.TopRightCorner;
    }

    void InstantiateTiles()
    {
        // Go through all the tiles in the jagged array...
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < tiles[i].Length; j++)
            {
                TileType currentTile = tiles[i][j];
                switch (currentTile)
                {
                    case TileType.Empty:
                        break;
                    case TileType.Floor:
                        InstantiateFromArray(floorTiles, i, j);
                        break;
                    case TileType.BottomLeftCorner:
                        //InstantiateFromArray(wallTiles, i, j);
                        break;
                    case TileType.TopLeftCorner:
                        //InstantiateFromArray(wallTiles, i, j);
                        break;
                    case TileType.BottomRightCorner:
                        //InstantiateFromArray(wallTiles, i, j);
                        break;
                    case TileType.TopRightCorner:
                        //InstantiateFromArray(wallTiles, i, j);
                        break;
                    case TileType.LeftWall:
                        //InstantiateFromArray(wallTiles, i, j);
                        break;
                    case TileType.RightWall:
                        InstantiateFromArray(wallTiles, i-1, j+1,270);
                        break;
                    case TileType.TopWall:
                        InstantiateFromArray(wallTiles, i-1, j,180); //-1 = wall's offset
                        break;
                    case TileType.BottomWall:
                        InstantiateFromArray(wallTiles, i, j+1); //1 = wall's offset
                        break;
                }
            }
        }
    }

    void InstantiateFromArray(GameObject[] prefabs, float x, float y, float rotation = 0,string name ="")
    {
        // Create a random index for the array.
        int randomIndex = Random.Range(0, prefabs.Length);

        // The position to be instantiated at is based on the coordinates.
        Vector3 position = new Vector3(x*spacing, 0, y*spacing);

        GameObject tileInstance = (GameObject) Instantiate(prefabs[randomIndex], position, Quaternion.identity);
        if (Math.Abs(rotation) > 0)
            tileInstance.transform.Rotate(0,rotation,0);

        tileInstance.name = name;
        // Set the tile's parent to the board holder.
        tileInstance.transform.parent = boardHolder.transform;
    }
}