public static class SocketIOEvents
{

    // General Input/Output
    public static string talk = "c";
    public static string leave = "e";
    public static string move = "m";

    // Output
    public static class Output
    {
        // Knight
        public static class KnightIO
        {
            public static string USE_WEAPON = "w";
            public static string USE_ABILITY = "a";
            public static string USE_ITEM = "i";
        }

        // Boss
        public static class BossIO
        {
            public static string TRIGGER_TRAP = "t";
            public static string PUT_TRAP = "a";
            public static string SPAWN_CREATURE = "s";
        }
    }

    // Input
    public static class Input
    {
        // General Input
        public static string PLAYER_LEAVES = "e";
    }

}
