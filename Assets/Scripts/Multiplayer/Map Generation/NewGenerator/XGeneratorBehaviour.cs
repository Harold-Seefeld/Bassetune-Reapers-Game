using UnityEngine;
using System.Collections;
using System;

public class XGeneratorBehaviour : MonoBehaviour {
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
        Corner_OUT_NW,
        Corner_OUT_NE,
        Corner_OUT_SE,
        Corner_OUT_SW
    }


    void Start() {
        int[,] map = new int[100, 100];

        XRoom room1 = new XRoom(new XCoord(0, 0), new XSize(20, 20));
        XRoom room2 = new XRoom(new XCoord(25, 3), new XSize(7, 7));
        XCorridor corr1_2 = new XCorridor(new XCoord(19, 5), new XSize(7, 3), XCorridor.Orientation.vertical);
        XCorridor corr2_N = new XCorridor(new XCoord(27, 9), new XSize(3, 4), XCorridor.Orientation.horizontal);


        room1.plotOn(map);
        room2.plotOn(map);
        corr1_2.plotOn(map);
        corr2_N.plotOn(map);

        /* T Map
        int[,] map = new int[9, 7] { { 6, 0, 2, 2, 2, 0, 7 },
                                     { 0, 1, 1, 1, 1, 1, 0 },
                                     { 5, 1, 1, 1, 1, 1, 3 },
                                     { 5, 1, 1, 1, 1, 1, 3 },
                                     { 0, 1, 1, 1, 1, 1, 0 },
                                     { 9, 0, 11, 1, 10, 0, 8 },
                                     { 0, 0,  5, 1, 3, 0, 0 },
                                     { 0, 0,  5, 1, 3, 0, 0 },
                                     { 0, 0,  5, 1, 3, 0, 0 }
        };
        */

        /* L Map  */
        int[,] map1 = new int[9, 7] { { 6, 0, 2, 2, 2, 0, 7 },
                                     { 0, 1, 1, 1, 1, 1, 0 },
                                     { 5, 1, 1, 1, 1, 1, 3 },
                                     { 5, 1, 1, 1, 1, 1, 3 },
                                     { 5, 1, 1, 1, 1, 1, 0 },
                                     { 5, 1, 10, 4, 4, 0, 8 },
                                     { 5, 1,  3, 0, 0, 0, 0 },
                                     { 5, 1,  3, 0, 0, 0, 0 },
                                     { 5, 1,  3, 0, 0, 0, 0 }
        };
        


        //XRoom room = new XRoom(new XCoord(1, 1), new XSize(2, 2));
        //room.plotOn(map);

        attachMeshesTo(map);

        


        /*
        int mapWidth = 50;
        int mapHeight = 100;
        
        int roomNumber = 1;
        int corridorNumber = 0;

        int roomWidth = 10;
        int roomHeight = 20;

        int corridorLenght = 5;


        int direction = 2; //destra


        XCoord room1Coord = new XCoord(0, 0);
        XSize room1Size = new XSize(roomWidth, roomHeight);
        XRoom room1 = new XRoom(room1Coord,  room1Size);

        if (direction == 2) {
            XCoord room1RightCoord = room1Coord.plus(room1Size.justWidth);
        }

        if (room1RightCoord.plus(corridorLenght))

        
        XCorridor corridor1_2 = new XCorridor()
        */



    }

    private void attachMeshesTo(int[,] map) {
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
        for (int x = 0; x < map.GetLength(0); x++) {
            for (int z = 0; z < map.GetLength(1); z++) {
                int value = map[x, z];
                TileType type = (TileType)value;

                if (type == TileType.Floor) {
                    GameObject prefab = _floorPrefab;
                    float xPos = x * floorSpan;
                    float zPos = z * floorSpan;
                    float yRot = 0f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, x, z, type.ToString());
                } else if (type == TileType.Wall_N) {
                    GameObject prefab = _wallPrefab;
                    float xPos = x * floorSpan;
                    float zPos = z * floorSpan;
                    float yRot = 90f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, x, z, type.ToString());
                } else if (type == TileType.Wall_E) {
                    GameObject prefab = _wallPrefab;
                    float xPos = x * floorSpan - floorSpan;
                    float zPos = z * floorSpan;
                    float yRot = 180f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, x, z, type.ToString());
                } else if (type == TileType.Wall_S) {
                    GameObject prefab = _wallPrefab;
                    float xPos = x * floorSpan - floorSpan;
                    float zPos = z * floorSpan + floorSpan;
                    float yRot = 270f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, x, z, type.ToString());
                } else if (type == TileType.Wall_W) {
                    GameObject prefab = _wallPrefab;
                    float xPos = x * floorSpan;
                    float zPos = z * floorSpan + floorSpan;
                    float yRot = 0f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, x, z, type.ToString());
                } else if (type == TileType.Corner_INN_NW) {
                    GameObject prefab = _cornerInnPrefab;
                    float xPos = x * floorSpan;
                    float zPos = z * floorSpan + floorSpan;
                    float yRot = 90f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, x, z, type.ToString());
                } else if (type == TileType.Corner_INN_NE) {
                    GameObject prefab = _cornerInnPrefab;
                    float xPos = x * floorSpan;
                    float zPos = z * floorSpan;
                    float yRot = 180f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, x, z, type.ToString());
                } else if (type == TileType.Corner_INN_SE) {
                    GameObject prefab = _cornerInnPrefab;
                    float xPos = x * floorSpan - floorSpan;
                    float zPos = z * floorSpan;
                    float yRot = 270f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, x, z, type.ToString());
                } else if (type == TileType.Corner_INN_SW) {
                    GameObject prefab = _cornerInnPrefab;
                    float xPos = x * floorSpan - floorSpan;
                    float zPos = z * floorSpan + floorSpan;
                    float yRot = 0f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, x, z, type.ToString());
                } else if (type == TileType.Corner_OUT_NE) {
                    GameObject prefab = _cornerOutPrefab;
                    float xPos = x * floorSpan - halfFloorSpan;
                    float zPos = z * floorSpan + halfFloorSpan;
                    float yRot = 90f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, x, z, type.ToString());
                } else if (type == TileType.Corner_OUT_NW) {
                    GameObject prefab = _cornerOutPrefab;
                    float xPos = x * floorSpan - halfFloorSpan;
                    float zPos = z * floorSpan + halfFloorSpan;
                    float yRot = 0f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, x, z, type.ToString());
                } else if (type == TileType.Corner_OUT_SE) {
                    GameObject prefab = _cornerOutPrefab;
                    float xPos = x * floorSpan - halfFloorSpan;
                    float zPos = z * floorSpan + halfFloorSpan;
                    float yRot = 180f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, x, z, type.ToString());
                } else if (type == TileType.Corner_OUT_SW) {
                    GameObject prefab = _cornerOutPrefab;
                    float xPos = x * floorSpan - halfFloorSpan;
                    float zPos = z * floorSpan + halfFloorSpan;
                    float yRot = 270f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, x, z, type.ToString());
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

        for (int x = 0; x < map.GetLength(0); x++) {
            for (int z = 0; z < map.GetLength(1); z++) {
                int value = map[x, z];
                TileType type = (TileType)value;

                if (type == TileType.Wall_N) {
                    float xPos = x * floorSpan - oneQuarterFloorSpan;
                    float zPos = z * floorSpan;
                    float yRot = 90;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, x, z, objectName);
                } else if (type == TileType.Wall_E) {
                    float xPos = (x - 1) * floorSpan;
                    float zPos = z * floorSpan + oneQuarterFloorSpan;
                    float yRot = 180;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, x, z, objectName);
                } else if (type == TileType.Wall_S) {
                    float xPos = x * floorSpan - threeQuartersFloorSpan;
                    float zPos = z * floorSpan + floorSpan;
                    float yRot = 270;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, x, z, objectName);
                } else if (type == TileType.Wall_W) {
                    float xPos = x * floorSpan;
                    float zPos = z * floorSpan + threeQuartersFloorSpan;
                    float yRot = 0;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, x, z, objectName);
                } else if (type == TileType.Corner_INN_NW) {
                    float xPos = x * floorSpan + floorSpan;
                    float zPos = (z + 1) * floorSpan - oneQuarterFloorSpan;
                    float yRot = 0f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, x, z, objectName);
                } else if (type == TileType.Corner_INN_NE) {
                    float xPos = x * floorSpan - oneQuarterFloorSpan;
                    float zPos = (z-1) * floorSpan;
                    float yRot = 90f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, x, z, objectName);
                } else if (type == TileType.Corner_INN_SE) {
                    float xPos = (x - 2) * floorSpan;
                    float zPos = z * floorSpan + oneQuarterFloorSpan;
                    float yRot = 180f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, x, z, objectName);
                } else if (type == TileType.Corner_INN_SW) {
                    float xPos = (x - 1) * floorSpan + oneQuarterFloorSpan;
                    float zPos = (z + 2) * floorSpan;
                    float yRot = 270f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, x, z, objectName);
                } else if (type == TileType.Corner_OUT_NE) {
                    float xPos = x * floorSpan;
                    float zPos = z * floorSpan + threeQuartersFloorSpan;
                    float yRot = 0;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, x, z, objectName);
                } else if (type == TileType.Corner_OUT_NW) {
                    float xPos = x * floorSpan - threeQuartersFloorSpan;
                    float zPos = (z+1) * floorSpan;
                    float yRot = 270;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, x, z, objectName);
                } else if (type == TileType.Corner_OUT_SE) {
                    float xPos = x * floorSpan - oneQuarterFloorSpan;
                    float zPos = z * floorSpan;
                    float yRot = 90f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, x, z, objectName);
                } else if (type == TileType.Corner_OUT_SW) {
                    float xPos = x * floorSpan - floorSpan;
                    float zPos = z * floorSpan + oneQuarterFloorSpan;
                    float yRot = 180f;
                    istantiate(prefab, xPos, zPos, yRot, boardHolder, x, z, objectName);
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
