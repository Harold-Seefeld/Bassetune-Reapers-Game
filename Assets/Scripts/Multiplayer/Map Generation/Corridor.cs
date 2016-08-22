using UnityEngine;

// Enum to specify the direction is heading.
public enum Direction
{
    North, East, South, West,
}

public class Corridor
{
    public int startX;         // The x coordinate for the start of the corridor.
    public int startY;         // The y coordinate for the start of the corridor.
    public int length;            // How many units long the corridor is.
    public Direction direction;   // Which direction the corridor is heading from it's room.

    // Get the end position of the corridor based on it's start position and which direction it's heading.
    public int EndPositionX
    {
        get
        {
            switch (direction)
            {
                case Direction.North:
                    return startX;
                case Direction.East:
                    return startX + length - 1;
                case Direction.South:
                    return startX;
                case Direction.West:
                    return startX - length + 1;
            }
            return 0;
        }
    }

    public int EndPositionY
    {
        get
        {
            switch (direction)
            {
                case Direction.North:
                    return startY + length - 1;
                case Direction.East:
                    return startY;
                case Direction.South:
                    return startY - length + 1;
                case Direction.West:
                    return startY;
            }
            return 0;
        }
    }

    public void SetupCorridor(Room room, IntRange length, IntRange roomWidth, IntRange roomHeight, int columns, int rows, bool firstCorridor)
    {
        // Set a random direction (a random index from 0 to 3, cast to Direction).
        direction = (Direction)Random.Range(0, 4);

        // Find the direction opposite to the one entering the room this corridor is leaving from.
        Direction oppositeDirection = RotateClockwise(180, room.enteringCorridor);

        // If this is noth the first corridor and the randomly selected direction is opposite to the previous corridor's direction...
        if (!firstCorridor && direction == oppositeDirection)
        {
            // Rotate the direction 90 degrees clockwise (North becomes East, East becomes South, etc).
            direction = RotateClockwise(90, direction);

        }

        // Set a random length.
        this.length = length.Random;

        // Create a cap for how long the length can be (this will be changed based on the direction and position).
        int maxLength = length.max;

        switch (direction)
        {
            // If the choosen direction is North (up)...
            case Direction.North:
                // ... the starting position in the x axis can be random but within the width of the room.
                startX = Random.Range(room.x, room.x + room.width - 1);

                // The starting position in the y axis must be the top of the room.
                startY = room.y + room.height;

                // The maximum length the corridor can be is the height of the board (rows) but from the top of the room (y pos + height).
                maxLength = rows - startY - roomHeight.min - 1;
                break;
            case Direction.East:
                startX = room.x + room.width;
                startY = Random.Range(room.y, room.y + room.height - 1);
                maxLength = columns - startX - roomWidth.min - 1;
                break;
            case Direction.South:
                startX = Random.Range(room.x, room.x + room.width);
                startY = room.y;
                maxLength = startY - roomHeight.min - 1;
                break;
            case Direction.West:
                startX = room.x;
                startY = Random.Range(room.y, room.y + room.height);
                maxLength = startX - roomWidth.min - 1;
                break;
        }

        // We clamp the length of the corridor to make sure it doesn't go off the board.
        this.length = Mathf.Clamp(this.length, 1, maxLength);
    }

    private Direction RotateClockwise(int degree, Direction direction)
    {
        int rotation = degree / 90;
        int directionInt = (int)direction;
        directionInt += rotation;
        directionInt %= 4;
        return (Direction)directionInt;
    }
}