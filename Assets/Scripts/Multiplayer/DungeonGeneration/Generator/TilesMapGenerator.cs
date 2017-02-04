using DungeonGeneration.Generator.Domain;
using DungeonGeneration.Generator.Pickers;
using DungeonGeneration.Logging;
using DungeonGeneration.Generator.Plotters;

namespace DungeonGeneration.Generator {

    public class TilesMapGenerator {
        private int _corridorSizeMin;
        private int _corridorSizeMax;
        private int _roomSizeMin;
        private int _roomSizeMax;
        private int _roomsNumberMin;
        private int _roomsNumberMax;
        private int _seed;
        private int _mapRows;
        private int _mapColumns;
        private IXLogger _logger;
        private IPlotter _plotter;
        private Board _board;

        public TilesMapGenerator() {
            _logger = new NullLogger();
            clearBoard();
        }

        private void clearBoard() {
            _board = null;
        }

        private bool isBoardCleared() {
            return _board == null;
        }

        public void setCorridorSizeRange(int v1, int v2) {
            _corridorSizeMin = v1;
            _corridorSizeMax = v2;
            clearBoard();
        }

        public void setRoomSizeRange(int v1, int v2) {
            _roomSizeMin = v1;
            _roomSizeMax = v2;
            clearBoard();
        }

        public void setRoomsNumberRange(int v1, int v2) {
            _roomsNumberMin = v1;
            _roomsNumberMax = v2;
            clearBoard();
        }

        public void setSeed(int v) {
            _seed = v;
            clearBoard();
        }

        public void setMapSize(int rows, int columns) {
            _mapRows = rows;
            _mapColumns = columns;
            clearBoard();
        }

        public void setLogger(IXLogger logger) {
            _logger = logger;
            clearBoard();
        }

        public Board asBoard() {
            if (!isBoardCleared()) return _board;
            _board = new Board(_mapRows, _mapColumns);

            //IPickerStrategy seedStrategy = new RandomSeededPickerStrategy(_seed);
            CustomSeededPickerStrategy seedStrategy = new CustomSeededPickerStrategy(_seed);
            seedStrategy.setLogger(_logger);

            IntInRangePicker roomNumberPicker = new IntInRangePicker(_roomsNumberMin, _roomsNumberMax, seedStrategy);
            IntInRangePicker roomSizePicker = new IntInRangePicker(_roomSizeMin, _roomSizeMax, seedStrategy);
            IntInRangePicker corrSizePicker = new IntInRangePicker(_corridorSizeMin, _corridorSizeMax, seedStrategy);
            CardinalPointPicker cardPointPicker = new CardinalPointPicker(seedStrategy);
            CellInRangePicker cellRangePicker = new CellInRangePicker(seedStrategy);

            int roomNumber = roomNumberPicker.draw();
            if (roomNumber < 1) {
                _logger.warning("Room number should be at least 1. Instead is: " + roomNumber);
                return _board;
            }

            Grid grid = new Grid(roomSizePicker.draw(), roomSizePicker.draw());
            Cell topLeftVertexMin = new Cell(0, 0);
            Cell topLeftVertexMax = new Cell(_mapRows - 1, _mapColumns - 1).minusSize(grid.rows(), grid.columns());
            Cell topLeftCell = cellRangePicker.drawBetween(topLeftVertexMin, topLeftVertexMax);
            Room lastRoom = new Room(topLeftCell, grid);
            if (!_board.fitsIn(lastRoom)) {
                _logger.error("First room not fit in. This should never happen");
                return _board;
            }
            _logger.info("OK: " + lastRoom);
            _board.addRoom(lastRoom);

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
                Corridor lastCorridor = null;
                do {
                    lastCorridor = generateCorridor(lastDirection, lastRoom, corrSizePicker, cellRangePicker);
                    if (!_board.fitsIn(lastCorridor)) {
                        _logger.info("NO FITS: " + lastCorridor + " " + lastDirection);
                        lastCorridor = null;
                        cardinalPointAttempt++;
                        lastDirection = cardPointPicker.nextClockwise(lastDirection);
                    }
                } while (cardinalPointAttempt <= 4 && lastCorridor == null);

                //If no corridor has been created, then terminate the algorithm
                if (lastCorridor == null) {
                    _logger.warning("PROCEDURAL GENERATION INTERRUPTED: No more chance for a Corridor to fit in");
                    break;
                }
                _logger.info("OK: " + lastCorridor + " " + lastDirection);
                _board.addCorridor(lastCorridor);

                //Try to create a room. If there is enough space retry (until 4 times) restarting from a new corridor
                Room newRoom = generateRoom(lastDirection, lastCorridor, roomSizePicker, cellRangePicker);
                if (!_board.fitsIn(newRoom)) {
                    _board.removeLast(); //remove last item added to the board (a corridor in this case)
                    if (roomCreationAttempt <= 4) {
                        _logger.info("NO FITS: " + newRoom + " Retry: " + roomCreationAttempt);
                        roomCreationAttempt++;
                        i--;
                        continue;
                    } else {
                        _logger.warning("PROCEDURAL GENERATION INTERRUPTED: No more chance for a Room to fit in");
                        break;
                    }
                } else {
                    _logger.info("OK: " + newRoom + " Retry: " + roomCreationAttempt);
                    lastRoom = newRoom;
                    roomCreationAttempt = 1;
                    _board.addRoom(lastRoom);
                }
            }
            return _board;
        }

        public int[,] asMatrix() {
            return asBoard().asTilesMatrix(_plotter);
        }

        public void setPlotter(IPlotter plotter) {
            _plotter = plotter;
        }

        private Room generateRoom(CardinalPoint lastCorridorDirection, Corridor lastCorr, IntInRangePicker roomSizePicker, CellInRangePicker cellInRangePicker) {
            int roomRows = roomSizePicker.draw();
            int roomCols = roomSizePicker.draw();
            Grid grid = new Grid(roomRows, roomCols);
            Cell topLeftCell = null;
            if (lastCorridorDirection == CardinalPoint.NORD) {
                Cell topLeftVertexMax = lastCorr.topLeftVertex().minusSize(roomRows, 0);
                Cell topLeftVertexMin = topLeftVertexMax.minusCell(0, roomCols - lastCorr.width());
                //Excluding cells to avoid Inward and Outward Corner Walls Overlapping
                Cell excludeOne = topLeftVertexMin.plusCell(0, 1);
                Cell excludeTwo = topLeftVertexMax.minusCell(0, 1);
                topLeftCell = cellInRangePicker.drawBetweenWithExclusion(topLeftVertexMin, topLeftVertexMax, excludeOne, excludeTwo);
                _logger.info("Min: " + topLeftVertexMin + " Max: " + topLeftVertexMax + " Selected: " + topLeftCell + " Exclusions: " + excludeOne + " - " + excludeTwo);
            } else if (lastCorridorDirection == CardinalPoint.EST) {
                Cell topLeftVertexMax = lastCorr.topRightVertex();
                Cell topLeftVertexMin = topLeftVertexMax.minusCell(roomRows-lastCorr.height(), 0);
                //Excluding cells to avoid Inward and Outward Corner Walls Overlapping
                Cell excludeOne = topLeftVertexMin.plusCell(1, 0);
                Cell excludeTwo = topLeftVertexMax.minusCell(1, 0);
                topLeftCell = cellInRangePicker.drawBetweenWithExclusion(topLeftVertexMin, topLeftVertexMax, excludeOne, excludeTwo);
                _logger.info("Min: " + topLeftVertexMin + " Max: " + topLeftVertexMax + " Selected: " + topLeftCell + " Exclusions: " + excludeOne + " - " + excludeTwo);
            } else if (lastCorridorDirection == CardinalPoint.SUD) {
                Cell topLeftVertexMax = lastCorr.bottomLeftVertex();
                Cell topLeftVertexMin = topLeftVertexMax.minusCell(0, roomCols - lastCorr.width());
                //Excluding cells to avoid Inward and Outward Corner Walls Overlapping
                Cell excludeOne = topLeftVertexMin.plusCell(0, 1);
                Cell excludeTwo = topLeftVertexMax.minusCell(0, 1);
                topLeftCell = cellInRangePicker.drawBetweenWithExclusion(topLeftVertexMin, topLeftVertexMax, excludeOne, excludeTwo);
                _logger.info("Min: " + topLeftVertexMin + " Max: " + topLeftVertexMax + " Selected: " + topLeftCell + " Exclusions: " + excludeOne + " - " + excludeTwo);
            } else if (lastCorridorDirection == CardinalPoint.WEST) {
                Cell topLeftVertexMax = lastCorr.topLeftVertex().minusSize(0, roomCols);
                Cell topLeftVertexMin = topLeftVertexMax.minusCell(roomRows - lastCorr.height(), 0);
                //Excluding cells to avoid Inward and Outward Corner Walls Overlapping
                Cell excludeOne = topLeftVertexMin.plusCell(1, 0);
                Cell excludeTwo = topLeftVertexMax.minusCell(1, 0);
                topLeftCell = cellInRangePicker.drawBetweenWithExclusion(topLeftVertexMin, topLeftVertexMax, excludeOne, excludeTwo);
                _logger.info("Min: " + topLeftVertexMin + " Max: " + topLeftVertexMax + " Selected: " + topLeftCell + " Exclusions: " + excludeOne + " - " + excludeTwo);
            }
            return new Room(topLeftCell, grid);
        }

        private Corridor generateCorridor(CardinalPoint mapDirection, Room lastRoom, IntInRangePicker corrSizePicker, CellInRangePicker cellRangePicker) {
            int corridorLenght = corrSizePicker.draw();
            int corridorSection = 3;

            Corridor.Orientation corrOrient = 0;
            Grid grid = null;
            Cell topLeftCell = lastRoom.topLeftVertex();
            if (mapDirection == CardinalPoint.NORD) {
                grid = new Grid(corridorLenght, corridorSection);
                corrOrient = Corridor.Orientation.vertical;
                Cell topLeftVertexMin = lastRoom.topLeftVertex().minusSize(corridorLenght, 0);
                Cell topLeftVertexMax = topLeftVertexMin.plusCell(0, lastRoom.width() - corridorSection);
                //Excluding cells to avoid Inward and Outward Corner Walls Overlapping
                Cell excludeOne = topLeftVertexMin.plusCell(0, 1);
                Cell excludeTwo = topLeftVertexMax.minusCell(0, 1);
                topLeftCell = cellRangePicker.drawBetweenWithExclusion(topLeftVertexMin, topLeftVertexMax, excludeOne, excludeTwo);
                _logger.info("Min: " + topLeftVertexMin + " Max: " + topLeftVertexMax + " Selected: " + topLeftCell + " Exclusions: " + excludeOne + " - " + excludeTwo);
            } else if (mapDirection == CardinalPoint.EST) {
                grid = new Grid(corridorSection, corridorLenght);
                corrOrient = Corridor.Orientation.horizontal;
                Cell topLeftVertexMin = lastRoom.topRightVertex();
                Cell topLeftVertexMax = topLeftVertexMin.plusCell(lastRoom.height() - corridorSection, 0);
                //Excluding cells to avoid Inward and Outward Corner Walls Overlapping
                Cell excludeOne = topLeftVertexMin.plusCell(1, 0);
                Cell excludeTwo = topLeftVertexMax.minusCell(1, 0);
                topLeftCell = cellRangePicker.drawBetweenWithExclusion(topLeftVertexMin, topLeftVertexMax, excludeOne, excludeTwo);
                _logger.info("Min: " + topLeftVertexMin + " Max: " + topLeftVertexMax + " Selected: " + topLeftCell + " Exclusions: " + excludeOne + " - " + excludeTwo);
            } else if (mapDirection == CardinalPoint.SUD) {
                grid = new Grid(corridorLenght, corridorSection);
                corrOrient = Corridor.Orientation.vertical;
                Cell topLeftVertexMin = lastRoom.bottomLeftVertex();
                Cell topLeftVertexMax = topLeftVertexMin.plusCell(0, lastRoom.width() - corridorSection);
                //Excluding cells to avoid Inward and Outward Corner Walls Overlapping
                Cell excludeOne = topLeftVertexMin.plusCell(0, 1);
                Cell excludeTwo = topLeftVertexMax.minusCell(0, 1);
                topLeftCell = cellRangePicker.drawBetweenWithExclusion(topLeftVertexMin, topLeftVertexMax, excludeOne, excludeTwo);
                _logger.info("Min: " + topLeftVertexMin + " Max: " + topLeftVertexMax + " Selected: " + topLeftCell + " Exclusions: " + excludeOne + " - " + excludeTwo);
            } else if (mapDirection == CardinalPoint.WEST) {
                grid = new Grid(corridorSection, corridorLenght);
                corrOrient = Corridor.Orientation.horizontal;
                Cell topLeftVertexMin = lastRoom.topLeftVertex().minusSize(0, corridorLenght);
                Cell topLeftVertexMax = topLeftVertexMin.plusCell(lastRoom.height() - corridorSection, 0);
                //Excluding cells to avoid Inward and Outward Corner Walls Overlapping
                Cell excludeOne = topLeftVertexMin.plusCell(1, 0);
                Cell excludeTwo = topLeftVertexMax.minusCell(1, 0);
                topLeftCell = cellRangePicker.drawBetweenWithExclusion(topLeftVertexMin, topLeftVertexMax, excludeOne, excludeTwo);
                _logger.info("Min: " + topLeftVertexMin + " Max: " + topLeftVertexMax + " Selected: " + topLeftCell + " Exclusions: " + excludeOne + " - " + excludeTwo);
            }
            return new Corridor(topLeftCell, grid, corrOrient);
        }
    }
}
