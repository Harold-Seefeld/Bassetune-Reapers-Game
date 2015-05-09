using DungeonGenerator.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DungeonGenerator
{
    public static class Helper
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        /// <summary>
        /// Shuffles a list
        /// </summary>
        /// <typeparam name="T">Object which the list contains</typeparam>
        /// <param name="list">which should be shuffled</param>
        /// <returns>new list which is shuffled</returns>
        public static List<T> Shuffle<T>(List<T> list)
        {
            var result = new List<T>(list);
            result.Shuffle();
            return result;
        }

        public static Dictionary<Directions, int> di = new Dictionary<Directions, int>()
    {
    {Directions.North,-1},
    {Directions.South,1},
    {Directions.West,0},
    {Directions.East,0},
    };
        public static Dictionary<Directions, int> dj = new Dictionary<Directions, int>()
    {
    {Directions.North,0},
    {Directions.South,0},
    {Directions.West,-1},
    {Directions.East,1},
    };

    }
}
