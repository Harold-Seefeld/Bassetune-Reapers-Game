﻿using UnityEngine;

public class Room
{
    public int x;                      // The x coordinate of the lower left tile of the room.
    public int y;                      // The y coordinate of the lower left tile of the room.
    public int width;                     // How many tiles wide the room is.
    public int height;                    // How many tiles high the room is.
    public Direction enteringCorridor;    // The direction of the corridor that is entering this room.


    // This is used for the first room.  It does not have a Corridor parameter since there are no corridors yet.
    public void SetupRoom(IntRange widthRange, IntRange heightRange, int columns, int rows)
    {
        // Set a random width and height.
        width = widthRange.Random;
        height = heightRange.Random;

        // Set the x and y coordinates so the room is roughly in the middle of the board.
        x = Mathf.RoundToInt(columns / 2f - width / 2f);
        y = Mathf.RoundToInt(rows / 2f - height / 2f);
    }


    // This is an overload of the SetupRoom function and has a corridor parameter that represents the corridor entering the room.
    public void SetupRoom(IntRange widthRange, IntRange heightRange, int columns, int rows, Corridor corridor)
    {
        // Set the entering corridor direction.
        enteringCorridor = corridor.direction;

        // Set random values for width and height.
        width = widthRange.Random;
        height = heightRange.Random;

        switch (corridor.direction)
        {
            // If the corridor entering this room is going north...
            case Direction.North:
                // ... the height of the room mustn't go beyond the board so it must be clamped based
                // on the height of the board (rows) and the end of corridor that leads to the room.
                height = Mathf.Clamp(height, 1, rows - corridor.EndPositionY);

                // The y coordinate of the room must be at the end of the corridor (since the corridor leads to the bottom of the room).
                y = corridor.EndPositionY;

                // The x coordinate can be random but the left-most possibility is no further than the width
                // and the right-most possibility is that the end of the corridor is at the position of the room.
                x = Random.Range(corridor.EndPositionX - width + 1, corridor.EndPositionX);

                // This must be clamped to ensure that the room doesn't go off the board.
                x = Mathf.Clamp(x, 0, columns - width);
                break;
            case Direction.East:
                width = Mathf.Clamp(width, 1, columns - corridor.EndPositionX);
                x = corridor.EndPositionX;

                y = Random.Range(corridor.EndPositionY - height + 1, corridor.EndPositionY);
                y = Mathf.Clamp(y, 0, rows - height - 1);
                break;
            case Direction.South:
                height = Mathf.Clamp(height, 1, corridor.EndPositionY);
                y = corridor.EndPositionY - height + 1;

                x = Random.Range(corridor.EndPositionX - width + 1, corridor.EndPositionX);
                x = Mathf.Clamp(x, 0, columns - width - 1);
                break;
            case Direction.West:
                width = Mathf.Clamp(width, 1, corridor.EndPositionX);
                x = corridor.EndPositionX - width + 1;

                y = Random.Range(corridor.EndPositionY - height + 1, corridor.EndPositionY);
                y = Mathf.Clamp(y, 0, rows - height - 1);
                break;
        }
    }
}