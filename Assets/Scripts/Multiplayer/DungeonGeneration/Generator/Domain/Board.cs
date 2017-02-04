using System.Collections.Generic;
using DungeonGeneration.Generator.Plotters;

namespace DungeonGeneration.Generator.Domain {

    public class Board {
        private Grid _grid;
        private List<IShape> _roomsAndCorridors;

        public Board(int rows, int columns)
            : this(new Grid(rows, columns)) {
        }

        public Board(Grid mapGrid) {
            _grid = mapGrid;
            _roomsAndCorridors = new List<IShape>();
        }

        public bool fitsIn(IShape aSquare) {
            if (!aSquare.isWithin(_grid)) return false;
            //Evito di controllare la collisione con l'ultimo, poiche' e' necessaria
            for (int i = 0; i < _roomsAndCorridors.Count - 1; i++) {
                IShape each = _roomsAndCorridors[i];
                if (aSquare.collidesWith(each)) return false;
            }
            return true;
        }

        public void addRoom(Room aRoom) {
            if (_roomsAndCorridors.Count != 0) {
                Corridor corr = (Corridor)_roomsAndCorridors[_roomsAndCorridors.Count - 1];
                corr.setDestinationRoom(aRoom);
                aRoom.setCorridorIncoming(corr);
            }
            _roomsAndCorridors.Add(aRoom);
        }

        public int[,] asTilesMatrix(IPlotter plotter) {
            int[,] result = _grid.toIntMatrix();

            if (_roomsAndCorridors.Count > 0) {
                _roomsAndCorridors[0].plotOn(result, plotter);
            }
            return result;
        }

        public void addCorridor(Corridor corr) {
            if (_roomsAndCorridors.Count != 0) {
                Room room = (Room)_roomsAndCorridors[_roomsAndCorridors.Count - 1];
                room.setCorridorOutcoming(corr);
                corr.setSourceRoom(room);
            }
            _roomsAndCorridors.Add(corr);
        }

        public void removeLast() {
            if (_roomsAndCorridors.Count == 0) return;

            IShape last = _roomsAndCorridors[_roomsAndCorridors.Count - 1];
            IShape beforeLast = null;
            if (_roomsAndCorridors.Count >= 2) {
                beforeLast = _roomsAndCorridors[_roomsAndCorridors.Count - 2];
            }

            if (last is Room) {
                Room room = (Room)last;
                room.setCorridorIncoming(null);
                if (beforeLast != null) {
                    Corridor corr = (Corridor)beforeLast;
                    corr.setDestinationRoom(null);
                }
            } else {
                Corridor corr = (Corridor)last;
                corr.setSourceRoom(null);
                if (beforeLast != null) {
                    Room room = (Room)_roomsAndCorridors[_roomsAndCorridors.Count - 2];
                    room.setCorridorOutcoming(null);
                }
            }
            _roomsAndCorridors.RemoveAt(_roomsAndCorridors.Count - 1);
        }

        public int numberOfRoomsAndCorridors() {
            return _roomsAndCorridors.Count;
        }

        //Added for Javascript
        public Room[] rooms() {
            List<Room> result = new List<Room>();
            foreach(IShape each in _roomsAndCorridors) {
                if (each is Room) result.Add((Room)each);
            }
            return result.ToArray();
        }

        //Added for Javascript
        public Corridor[] corridors() {
            List<Corridor> result = new List<Corridor>();
            foreach (IShape each in _roomsAndCorridors) {
                if (each is Corridor) result.Add((Corridor)each);
            }
            return result.ToArray();
        }
    }
}