using DungeonGeneration.Generator.Domain;
using DungeonGeneration.Generator;
using DungeonGeneration.Generator.Pickers;
using System.Collections.Generic;
using System;

public class CaveGridGenerator : TilesMapGenerator {
    private int _cellularFillChance;
    private int _cellularSmoothingStep;

    public CaveGridGenerator() {
    }

    public void setCellularFillChance(int percentage) {
        _cellularFillChance = percentage;
    }

    public void setCellularSmoothingSteps(int steps) {
        _cellularSmoothingStep = steps;
    }

    public new CaveBoard asBoard() {
        Board board = base.asBoard();

        board = board.crop(4);
        ShapeCellularAutomaton roomAlgo = new ShapeCellularAutomaton(_seed, _cellularFillChance, _cellularSmoothingStep);
        CustomSeededPickerStrategy shapePicker = new CustomSeededPickerStrategy(_seed);

        CaveBoard result = new CaveBoard(board.rows(), board.cols());
        List<IXShape> onlyRooms = new List<IXShape>();

        foreach (Room each in board.rooms()) {
            Cell leftVert = each.topLeftVertex();
            int rows = each.height();
            int cols = each.width();
            APolyShape currentRoom; //Select a shape for the Room
            if (shapePicker.drawBetween(0, 100) < 50) {
                currentRoom = new RectShape(leftVert, new OIGrid(rows, cols));
            } else {
                currentRoom = new ElliShape(leftVert, new OIGrid(rows, cols));
            }
            roomAlgo.applyOn(currentRoom);


            if (!currentRoom.hasRegions()) continue;
                        
            //UnityEngine.Debug.Log(shape.topLeftVertex());
            //shape.grid().printOnUnityConsole();
            currentRoom.deleteRegionsButTheBiggest();

            result.addRoom(currentRoom);
            //_shapes.Add(room);
            onlyRooms.Add(currentRoom);
            if (onlyRooms.Count > 1) {
                IXShape previousRoom = onlyRooms[onlyRooms.Count - 2];
                int corrIndex = onlyRooms.Count - 2;
                Corridor corr = board.corridors()[corrIndex];
                int corridorSection = corr.isVertical()?corr.width(): corr.height();             
                result.addCorridor(createCorrShape(previousRoom, currentRoom, corridorSection));
            }

        }
        return result;
    }

    private List<Cell> GetLine(Cell from, Cell to) {
        List<Cell> line = new List<Cell>();

        int x = from.row();
        int y = from.col();

        int dx = to.row() - from.row();
        int dy = to.col() - from.col();

        bool inverted = false;
        int step = Math.Sign(dx);
        int gradientStep = Math.Sign(dy);

        int longest = Math.Abs(dx);
        int shortest = Math.Abs(dy);

        if (longest < shortest) {
            inverted = true;
            longest = Math.Abs(dy);
            shortest = Math.Abs(dx);

            step = Math.Sign(dy);
            gradientStep = Math.Sign(dx);
        }

        int gradientAccumulation = longest / 2;
        for (int i = 0; i < longest; i++) {
            line.Add(new Cell(x, y));

            if (inverted) {
                y += step;
            } else {
                x += step;
            }

            gradientAccumulation += shortest;
            if (gradientAccumulation >= longest) {
                if (inverted) {
                    x += gradientStep;
                } else {
                    y += gradientStep;
                }
                gradientAccumulation -= longest;
            }
        }

        return line;
    }

    private List<Cell> DrawCircle(Cell cell, int sectionSize) {
        List<Cell> result = new List<Cell>();
        for (int row = -sectionSize; row <= sectionSize; row++) {
            for (int col = -sectionSize; col <= sectionSize; col++) {
                if (row * row + col * col <= sectionSize * sectionSize) {
                    int drawX = cell.row() + row;
                    int drawY = cell.col() + col;
                    /*
                    if (IsInMapRange(drawX, drawY)) {
                        map[drawX, drawY] = 0;
                    }
                    */
                    result.Add(new Cell(drawX, drawY));
                }
            }
        }
        return result;
    }

    public OIGrid asOIGrid() {
        CaveBoard board =  asBoard();
        OIGrid grid = new OIGrid(board.rows(), board.cols());
        board.accept(new OIGridFiller(grid));
        return grid;
    }

    public new int[,] asMatrix() {
        return asOIGrid().toIntMatrix();
    }

    private FreeShape createCorrShape(IXShape roomA, IXShape roomB, int corrWidth) {
        CellPair pair = roomA.shortestCellPair(roomB);
        List<Cell> line = GetLine(pair.cell1, pair.cell2);
        FreeShape corrAtoB = new FreeShape();
        foreach (Cell each in line) {
            List<Cell> vCells = DrawCircle(each, corrWidth);
            corrAtoB.add(vCells);
        }
        return corrAtoB;
    }


}

