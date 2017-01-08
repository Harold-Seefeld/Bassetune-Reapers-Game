using System;
using UnityEngine;

public class BRTilesMapGenerator {
    private int _corridorSizeMin;
    private int _corridorSizeMax;
    private int _roomSizeMin;
    private int _roomSizeMax;
    private int _roomsNumberMin;
    private int _roomsNumberMax;
    private int _seed;
    private int _mapRows;
    private int _mapColumns;

    public BRTilesMapGenerator() {}

    public int[,] result() {
        SeededPickerStrategy seedStrategy = new SeededPickerStrategy(_seed);
        IntInRangePicker roomNumberPicker = new IntInRangePicker(_roomsNumberMin, _roomsNumberMax, seedStrategy);
        IntInRangePicker roomSizePicker = new IntInRangePicker(_roomSizeMin, _roomSizeMax, seedStrategy);
        IntInRangePicker corrSizePicker = new IntInRangePicker(_corridorSizeMin, _corridorSizeMax, seedStrategy);
        CardinalPointPicker cardPointPicker = new CardinalPointPicker(seedStrategy);
        CellInRangePicker cellRangePicker = new CellInRangePicker(seedStrategy);


        int roomNumber = roomNumberPicker.draw();
        

        BRDungeonBoard board = new BRDungeonBoard(_mapRows, _mapColumns);

        
        XGrid grid = new XGrid(roomSizePicker.draw(), roomSizePicker.draw());

        XCell topLeftVertexMin = new XCell(0, 0);
        XCell topLeftVertexMax = new XCell(_mapRows - 1, _mapColumns - 1).minus(grid.rows(), grid.columns());
        XCell topLeftCell = cellRangePicker.draw(topLeftVertexMin, topLeftVertexMax);
        XRoom lastRoom = new XRoom(topLeftCell, grid);
        if (!board.fitsIn(lastRoom)) {
            Debug.LogError("First room not fit in. This should never happen");
            return board.asTilesMatrix();
        }
        Debug.Log("OK: " + lastRoom);
        board.add(lastRoom);

        CardinalPoint lastDirection = 0;
        int roomCreationAttempt = 1;
        for (int i = 1; i < roomNumber; i++) {
            //If room and corridor has not been dropped, pick random direction
            if (roomCreationAttempt == 1) {
                lastDirection = cardPointPicker.draw();
            } else { //else force to next direction
                lastDirection = cardPointPicker.nextClockwise(lastDirection);
            }
            
            //Try to create a Corridor on a direction if there is enough space
            int cardinalPointAttempt = 1;
            XCorridor lastCorridor = null;
            do {
                lastCorridor = generateCorridor(lastDirection, lastRoom, corrSizePicker, cellRangePicker);
                if (!board.fitsIn(lastCorridor)) {
                    Debug.Log("NO FITS: " + lastCorridor + " " + lastDirection);
                    lastCorridor = null;
                    cardinalPointAttempt++;
                    lastDirection = cardPointPicker.nextClockwise(lastDirection);
                }
            } while (cardinalPointAttempt <= 4 && lastCorridor == null);

            //If no corridor has been created, then terminate the algorithm
            if (lastCorridor == null) {
                Debug.LogWarning("PROCEDURAL GENERATION INTERRUPTED: No more chance for a Corridor to fit in");
                break;
            }
            Debug.Log("OK: " + lastCorridor + " " + lastDirection);
            board.add(lastCorridor);

            //Try to create a room. If there is enough space retry (until 4 times) restarting from a new corridor
            XRoom newRoom = generateRoom(lastDirection, lastCorridor, roomSizePicker, cellRangePicker);
            if (!board.fitsIn(newRoom)) {
                board.removeLast(); //remove last item added to the board (a corridor in this case)
                if (roomCreationAttempt <= 4) {
                    Debug.Log("NO FITS: " + newRoom + " Retry: " + roomCreationAttempt);
                    roomCreationAttempt++;
                    i--;
                    continue;
                } else {
                    Debug.LogWarning("PROCEDURAL GENERATION INTERRUPTED: No more chance for a Room to fit in");
                    break;
                }
            } else {
                Debug.Log("OK: " + newRoom + " Retry: " + roomCreationAttempt);
                lastRoom = newRoom;
                roomCreationAttempt = 1;
                board.add(lastRoom);
            }
        }
        return board.asTilesMatrix();
    }

    private XRoom generateRoom(CardinalPoint lastCorridorDirection, XCorridor lastCorr, IntInRangePicker roomSizePicker, CellInRangePicker cellInRangePicker) {
        int roomRows = roomSizePicker.draw();
        int roomCols = roomSizePicker.draw();
        XGrid grid = new XGrid(roomRows, roomCols);
        XCell topLeftCell = null;//lastCorr.topLeftVertex();
        if (lastCorridorDirection == CardinalPoint.NORD) {
            //topLeftCell = topLeftCell.minus(roomRows, roomCols / 2 - 1);
            XCell roomTopLeftVertexMax = lastCorr.topLeftVertex().minus(roomRows, 0);
            XCell roomTopLeftVertexMin = roomTopLeftVertexMax.minus(0, roomCols - lastCorr.width());
            topLeftCell = cellInRangePicker.draw(roomTopLeftVertexMin, roomTopLeftVertexMax);
            Debug.Log("Min: " + roomTopLeftVertexMin + " Max: " + roomTopLeftVertexMax + " Selected: " + topLeftCell);
        } else if (lastCorridorDirection == CardinalPoint.EST) {
            //topLeftCell = topLeftCell.plus(0, lastCorr.width()).minus(roomRows / 2 - 1, 0);
            XCell roomTopLeftVertexMax = lastCorr.topRightVertex();
            int rowsDiff = roomRows - lastCorr.height();
            if (rowsDiff < 0) rowsDiff = 0;
            XCell roomTopLeftVertexMin = roomTopLeftVertexMax.minus(rowsDiff, 0);
            topLeftCell = cellInRangePicker.draw(roomTopLeftVertexMin, roomTopLeftVertexMax);
            Debug.Log("Min: " + roomTopLeftVertexMin + " Max: " + roomTopLeftVertexMax + " Selected: " + topLeftCell);
        } else if (lastCorridorDirection == CardinalPoint.SUD) {
            //topLeftCell = topLeftCell.plus(lastCorr.height(), 0).minus(0, roomCols / 2 - 1);
            XCell roomTopLeftVertexMax = lastCorr.bottomLeftVertex();
            XCell roomTopLeftVertexMin = roomTopLeftVertexMax.minus(0, roomCols - lastCorr.width());
            topLeftCell = cellInRangePicker.draw(roomTopLeftVertexMin, roomTopLeftVertexMax);
            Debug.Log("Min: " + roomTopLeftVertexMin + " Max: " + roomTopLeftVertexMax + " Selected: " + topLeftCell);
        } else if (lastCorridorDirection == CardinalPoint.WEST) {
            //topLeftCell = topLeftCell.minus(roomRows / 2 - 1, roomCols);
            XCell roomTopLeftVertexMax = lastCorr.topLeftVertex().minus(0, roomCols);
            XCell roomTopLeftVertexMin = roomTopLeftVertexMax.minus(roomRows - lastCorr.height(), 0);
            topLeftCell = cellInRangePicker.draw(roomTopLeftVertexMin, roomTopLeftVertexMax);
            Debug.Log("Min: " + roomTopLeftVertexMin + " Max: " + roomTopLeftVertexMax + " Selected: " + topLeftCell);
        } 
        return new XRoom(topLeftCell, grid);
    }

    private XCorridor generateCorridor(CardinalPoint mapDirection, XRoom lastRoom, IntInRangePicker corrSizePicker, CellInRangePicker cellRangePicker) {
        int corridorLenght = corrSizePicker.draw();
        int corridorSection = 3;


        XCorridor.Orientation corrOrient = 0;
        XGrid grid = null;
        XCell topLeftCell = lastRoom.topLeftVertex();
        if (mapDirection == CardinalPoint.NORD) {
            //topLeftCell = topLeftCell.minus(corridorLenght, 0).plus(0, lastRoom.width() / 2 - 1);
            grid = new XGrid(corridorLenght, corridorSection);
            corrOrient = XCorridor.Orientation.vertical;

            XCell topLeftVertexMin = lastRoom.topLeftVertex().minus(corridorLenght, 0);
            XCell topLeftVertexMax = topLeftVertexMin.plus(0, lastRoom.width() - corridorSection);
            topLeftCell = cellRangePicker.draw(topLeftVertexMin, topLeftVertexMax);
            Debug.Log("Min: " + topLeftVertexMin + " Max: " + topLeftVertexMax + " Selected: " + topLeftCell);
        } else if (mapDirection == CardinalPoint.EST) {
            //topLeftCell = topLeftCell.plus(lastRoom.height() / 2 - 1, lastRoom.width());
            grid = new XGrid(corridorSection, corridorLenght);
            corrOrient = XCorridor.Orientation.horizontal;
            
            XCell topLeftVertexMin = lastRoom.topRightVertex();
            XCell topLeftVertexMax = topLeftVertexMin.plus(lastRoom.height()-corridorSection, 0);
            topLeftCell = cellRangePicker.draw(topLeftVertexMin, topLeftVertexMax);
            Debug.Log("Min: " + topLeftVertexMin + " Max: " + topLeftVertexMax + " Selected: " + topLeftCell);
        } else if (mapDirection == CardinalPoint.SUD) {
            //topLeftCell = topLeftCell.plus(lastRoom.height(), lastRoom.width() / 2 - 1);
            grid = new XGrid(corridorLenght, corridorSection);
            corrOrient = XCorridor.Orientation.vertical;
            XCell topLeftVertexMin = lastRoom.bottomLeftVertex();
            XCell topLeftVertexMax = topLeftVertexMin.plus(0, lastRoom.width() - corridorSection);
            topLeftCell = cellRangePicker.draw(topLeftVertexMin, topLeftVertexMax);
            Debug.Log("Min: " + topLeftVertexMin + " Max: " + topLeftVertexMax + " Selected: " + topLeftCell);
        } else if (mapDirection == CardinalPoint.WEST) {
            //            topLeftCell = topLeftCell.minus(0, corridorLenght).plus(lastRoom.height() / 2 - 1, 0);
            grid = new XGrid(corridorSection, corridorLenght);
            corrOrient = XCorridor.Orientation.horizontal;

            XCell topLeftVertexMin = lastRoom.topLeftVertex().minus(0, corridorLenght);
            XCell topLeftVertexMax = topLeftVertexMin.plus(lastRoom.height() - corridorSection, 0);
            topLeftCell = cellRangePicker.draw(topLeftVertexMin, topLeftVertexMax);
            Debug.Log("Min: " + topLeftVertexMin + " Max: " + topLeftVertexMax + " Selected: " + topLeftCell);
        }
        return new XCorridor(topLeftCell, grid, corrOrient);
    }

    public void setCorridorSizeRange(int v1, int v2) {
        _corridorSizeMin = v1;
        _corridorSizeMax = v2;
    }

    public void setRoomSizeRange(int v1, int v2) {
        _roomSizeMin = v1;
        _roomSizeMax = v2;
    }

    public void setRoomsNumberRange(int v1, int v2) {
        _roomsNumberMin = v1;
        _roomsNumberMax = v2;
    }

    public void setSeed(int v) {
        _seed = v;
    }

    public void setMapSize(int rows, int columns) {
        _mapRows = rows;
        _mapColumns = columns;
    }
}
