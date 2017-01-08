using System;

public class XDungeonSampleCases {
    public static void case_RoomWithRightSideCorridorHorizontal_plot(int[,] map) {
        XRoom room = new XRoom(new XCell(0, 0), new XGrid(5, 5));
        XCorridor corr = new XCorridor(new XCell(0, 4), new XGrid(5, 3), XCorridor.Orientation.horizontal);
        room.setCorridorOutcoming(corr);
        corr.setSourceRoom(room);

        room.plotOn(map);
    }

    public static void case_RoomWithBottomRightCorridorVertical_plot(int[,] map) {
        XRoom room = new XRoom(new XCell(0, 0), new XGrid(5, 8));
        XCorridor corr = new XCorridor(new XCell(4, 5), new XGrid(4, 3), XCorridor.Orientation.vertical);
        room.setCorridorOutcoming(corr);
        corr.setSourceRoom(room);

        room.plotOn(map);
    }

    public static void case_RoomWithBottomRightCorridorHorizontal_plot(int[,] map) {
        XRoom room = new XRoom(new XCell(0, 0), new XGrid(5, 8));
        XCorridor corr = new XCorridor(new XCell(2, 7), new XGrid(3, 4), XCorridor.Orientation.horizontal);
        room.setCorridorOutcoming(corr);
        corr.setSourceRoom(room);

        room.plotOn(map);
    }

    public static void case_RoomWithTopRightCorridorHorizontal_plot(int[,] map) {
        XRoom room = new XRoom(new XCell(0, 0), new XGrid(5, 8));
        XCorridor corr = new XCorridor(new XCell(0, 7), new XGrid(3, 4), XCorridor.Orientation.horizontal);
        room.setCorridorOutcoming(corr);
        corr.setSourceRoom(room);

        room.plotOn(map);
    }

    public static void case_RoomWithTopRightCorridorVertical_plot(int[,] map) {
        XRoom room = new XRoom(new XCell(5, 0), new XGrid(5, 8));
        XCorridor corr = new XCorridor(new XCell(2, 5), new XGrid(4, 3), XCorridor.Orientation.vertical);
        room.setCorridorOutcoming(corr);
        corr.setSourceRoom(room);

        room.plotOn(map);
    }

    public static void case_RoomWithBottomLeftCorridorVertical_plot(int[,] map) {
        XRoom room0 = new XRoom(new XCell(0, 0), new XGrid(5, 8));
        XCorridor corr0_down = new XCorridor(new XCell(4, 0), new XGrid(4, 3), XCorridor.Orientation.vertical);
        room0.setCorridorOutcoming(corr0_down);
        corr0_down.setSourceRoom(room0);

        room0.plotOn(map);
    }

    public static void case_RoomWithBottomLeftCorridorHorizontal_plot(int[,] map) {
        XRoom room = new XRoom(new XCell(0, 8), new XGrid(5, 8));
        XCorridor corr = new XCorridor(new XCell(2, 5), new XGrid(3, 4), XCorridor.Orientation.horizontal);
        room.setCorridorOutcoming(corr);
        corr.setSourceRoom(room);

        room.plotOn(map);
    }

    public static void case_RoomWithTopLeftCorridorHorizontal_plot(int[,] map) {
        XRoom room = new XRoom(new XCell(0, 8), new XGrid(5, 8));
        XCorridor corr = new XCorridor(new XCell(0, 5), new XGrid(3, 4), XCorridor.Orientation.horizontal);
        room.setCorridorOutcoming(corr);
        corr.setSourceRoom(room);

        room.plotOn(map);
    }

    public static void case_RoomWithTopLeftCorridorVertical_plot(int[,] map) {
        XRoom room = new XRoom(new XCell(3, 0), new XGrid(5, 8));
        XCorridor corr = new XCorridor(new XCell(0, 0), new XGrid(4, 3), XCorridor.Orientation.vertical);
        room.setCorridorOutcoming(corr);
        corr.setSourceRoom(room);

        room.plotOn(map);
    }
}

