using UnityEngine;
using System.Collections;
using System;

public class XGeneratorBehaviour : MonoBehaviour {
    public int _mapHeight = 100;
    public int _mapWidth = 100;
    public int _roomsNumberMin = 5;
    public int _roomsNumberMax = 15;
    public int _roomSizeMin = 5;
    public int _roomSizeMax = 20;
    public int _corridorLengthMin = 2;
    public int _corridorLengthMax = 7;
    public int _seed = 123456;
    public bool _randomSeed = false;

    public GameObject _floorPrefab;
    public GameObject _wallPrefab;
    public GameObject _wallSeparatorPrefab;
    public GameObject _cornerInnPrefab;
    public GameObject _cornerOutPrefab;

    public enum TileType {
        Empty, 
        Floor,
        Wall_N,
        Wall_E,
        Wall_S,
        Wall_W,
        Corner_INN_NW,
        Corner_INN_NE,
        Corner_INN_SE,
        Corner_INN_SW,
        Corner_OUT_NE,
        Corner_OUT_NW,
        Corner_OUT_SW,
        Corner_OUT_SE
    }

    void Start() {
        //XDungeonSampleCases.case_RoomWithBottomLeftCorridorHorizontal_plot(map);
        //XDungeonSampleCases.case_RoomWithRightSideCorridorHorizontal_plot(map);
        
        BRTilesMapGenerator gen = new BRTilesMapGenerator();
        gen.setMapSize(_mapHeight, _mapWidth);
        gen.setRoomsNumberRange(_roomsNumberMin, _roomsNumberMax);
        gen.setRoomSizeRange(_roomSizeMin, _roomSizeMax);
        gen.setCorridorSizeRange(_corridorLengthMin, _corridorLengthMax);

        if (_randomSeed) {
            _seed = Time.time.ToString().GetHashCode();
        }
        gen.setSeed(_seed);


        int[,] map = gen.result();
        convertToMeshes(map);
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Destroy(GameObject.Find("BoardHolder"));
            Start();
        }
    }

    private void convertToMeshes(int[,] map) {
        GameObject boardHolder = new GameObject("BoardHolder");
        boardHolder.transform.parent = this.transform;

        addMainMashes(boardHolder, map);
        overlapWallSerators(boardHolder, map);

    }

    private void addMainMashes(GameObject boardHolder, int[,] map) {
        float floorSpan = 8;
        float halfFloorSpan = floorSpan * 0.5f;
        float oneQuarterFloorSpan = floorSpan * 0.25f;
        float threeQuartersFloorSpan = floorSpan * 0.75f;
        /*
        Vector3 size = _floorPrefab.GetComponentInChildren<MeshRenderer>().bounds.size;
        float xSpacing = size.x;
        float zSpacing = size.z;
        */
        for (int row = 0; row < map.GetLength(0); row++) {
            for (int col = 0; col < map.GetLength(1); col++) {
                int value = map[row, col];
                TileType type = (TileType)value;

                if (type == TileType.Floor) {
                    GameObject prefab = _floorPrefab;
                    float xPos = col * floorSpan;
                    float zPos = -row * floorSpan;
                    float yRot = 0f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, row, col, type.ToString());
                } else if (type == TileType.Wall_N) {
                    GameObject prefab = _wallPrefab;
                    float xPos = col * floorSpan - floorSpan;
                    float zPos = -row * floorSpan;
                    float yRot = 180f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, row, col, type.ToString());
                } else if (type == TileType.Wall_E) {
                    GameObject prefab = _wallPrefab;
                    float xPos = col * floorSpan - floorSpan;
                    float zPos = -row * floorSpan + floorSpan;
                    float yRot = 270f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, row, col, type.ToString());
                } else if (type == TileType.Wall_S) {
                    GameObject prefab = _wallPrefab;
                    float xPos = col * floorSpan;
                    float zPos = -row * floorSpan + floorSpan;
                    float yRot = 0f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, row, col, type.ToString());
                } else if (type == TileType.Wall_W) {
                    GameObject prefab = _wallPrefab;
                    float xPos = col * floorSpan;
                    float zPos = -row * floorSpan;
                    float yRot = 90f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, row, col, type.ToString());
                } else if (type == TileType.Corner_INN_NW) {
                    GameObject prefab = _cornerInnPrefab;
                    float xPos = col * floorSpan;
                    float zPos = -row * floorSpan;
                    float yRot = 180f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, row, col, type.ToString());
                } else if (type == TileType.Corner_INN_NE) {
                    GameObject prefab = _cornerInnPrefab;
                    float xPos = col * floorSpan - floorSpan;
                    float zPos = -row * floorSpan;
                    float yRot = 270f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, row, col, type.ToString());
                } else if (type == TileType.Corner_INN_SE) {
                    GameObject prefab = _cornerInnPrefab;
                    float xPos = col * floorSpan - floorSpan;
                    float zPos = -row * floorSpan + floorSpan;
                    float yRot = 0f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, row, col, type.ToString());
                } else if (type == TileType.Corner_INN_SW) {
                    GameObject prefab = _cornerInnPrefab;
                    float xPos = col * floorSpan;
                    float zPos = -row * floorSpan + floorSpan;
                    float yRot = 90f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, row, col, type.ToString());
                } else if (type == TileType.Corner_OUT_NW) {
                    GameObject prefab = _cornerOutPrefab;
                    float xPos = col * floorSpan - halfFloorSpan;
                    float zPos = -row * floorSpan + halfFloorSpan;
                    float yRot = 180f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, row, col, type.ToString());
                } else if (type == TileType.Corner_OUT_NE) {
                    GameObject prefab = _cornerOutPrefab;
                    float xPos = col * floorSpan - halfFloorSpan;
                    float zPos = -row * floorSpan + halfFloorSpan;
                    float yRot = 90f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, row, col, type.ToString());
                } else if (type == TileType.Corner_OUT_SW) {
                    GameObject prefab = _cornerOutPrefab;
                    float xPos = col * floorSpan - halfFloorSpan;
                    float zPos = -row * floorSpan + halfFloorSpan;
                    float yRot = 270f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, row, col, type.ToString());
                } else if (type == TileType.Corner_OUT_SE) {
                    GameObject prefab = _cornerOutPrefab;
                    float xPos = col * floorSpan - halfFloorSpan;
                    float zPos = -row * floorSpan + halfFloorSpan;
                    float yRot = 0f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, row, col, type.ToString());
                }
            }
        }
    }

    private void overlapWallSerators(GameObject boardHolder, int[,] map) {
        GameObject prefab = _wallSeparatorPrefab;
        String objectName = "Wall_Separator";

        float floorSpan = 8f;
        float halfFloorSpan = floorSpan * 0.5f;
        float oneQuarterFloorSpan = floorSpan * 0.25f;
        float threeQuartersFloorSpan = floorSpan * 0.75f;

        for (int row = 0; row < map.GetLength(0); row++) {
            for (int col = 0; col < map.GetLength(1); col++) {
                int value = map[row, col];
                TileType type = (TileType)value;

                if (type == TileType.Wall_N) {
                    float xPos = col * floorSpan;
                    float zPos = -row * floorSpan + oneQuarterFloorSpan;
                    float yRot = 180f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, row, col, objectName);
                } else if (type == TileType.Wall_E) {
                    float xPos = col * floorSpan - threeQuartersFloorSpan;
                    float zPos = -row * floorSpan;
                    float yRot = 270f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, row, col, objectName);
                } else if (type == TileType.Wall_S) {
                    float xPos = col * floorSpan - floorSpan;
                    float zPos = -row * floorSpan + threeQuartersFloorSpan;
                    float yRot = 0f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, row, col, objectName);
                } else if (type == TileType.Wall_W) {
                    float xPos = col * floorSpan - oneQuarterFloorSpan;
                    float zPos = -row * floorSpan + floorSpan;
                    float yRot = 90f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, row, col, objectName);
                } else if (type == TileType.Corner_INN_NW) {
                    float xPos = col * floorSpan + floorSpan;
                    float zPos = -row * floorSpan + oneQuarterFloorSpan;
                    float yRot = 180f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, row, col, objectName);
                } else if (type == TileType.Corner_INN_NE) {
                    float xPos = col * floorSpan - threeQuartersFloorSpan;
                    float zPos = -row * floorSpan - floorSpan;
                    float yRot = 270f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, row, col, objectName);
                } else if (type == TileType.Corner_INN_SE) {
                    float xPos = col * floorSpan - 2 * floorSpan;
                    float zPos = -row * floorSpan + threeQuartersFloorSpan;
                    float yRot = 0f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, row, col, objectName);
                } else if (type == TileType.Corner_INN_SW) {
                    float xPos = col * floorSpan - oneQuarterFloorSpan;
                    float zPos = -row * floorSpan + 2*floorSpan;
                    float yRot = 90f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, row, col, objectName);
                } else if (type == TileType.Corner_OUT_NW) {
                    float xPos = col * floorSpan - floorSpan;
                    float zPos = -row * floorSpan + threeQuartersFloorSpan;
                    float yRot = 0f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, row, col, objectName);
                } else if (type == TileType.Corner_OUT_NE) {
                    float xPos = col * floorSpan - threeQuartersFloorSpan;
                    float zPos = -row * floorSpan;
                    float yRot = 270f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, row, col, objectName);
                } else if (type == TileType.Corner_OUT_SW) {
                    float xPos = col * floorSpan - oneQuarterFloorSpan;
                    float zPos = -row * floorSpan + floorSpan;
                    float yRot = 90f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, row, col, objectName);
                } else if (type == TileType.Corner_OUT_SE) {
                    float xPos = col * floorSpan;
                    float zPos = -row * floorSpan + oneQuarterFloorSpan;
                    float yRot = 180f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, row, col, objectName);
                }
            }
        }

    }

    private void istantiate(GameObject prefab, float xPos, float zPos, float yRot, GameObject parent, int mapX, int mapZ, String name) {
        Vector3 position = new Vector3(xPos, 0, zPos);
        GameObject instance = (GameObject)Instantiate(prefab, position, Quaternion.identity);
        instance.transform.Rotate(0, yRot, 0);
        instance.name = "(" + mapX + "," + mapZ + ") " + name;
        instance.transform.parent = parent.transform;
    }

    private void print(int[,] expected) {
        String result = "";
        for (int i = 0; i < expected.GetLength(0); i++) {
            for (int j = 0; j < expected.GetLength(1); j++) {
                result += expected[i, j] + ",";
            }
            result += "\r\n";
        }
        Debug.Log(result);
    }
}
