using System.Collections;

public static class SocketIOEvents {

	// General Input/Output
	public static string talk = "c";
	public static string leave = "l";
	public static string move = "m";
	
	// Output
	public static class Output 
	{
		// Knight
		public static class Knight 
		{
			public static string ABILITY_START = "as";
			public static string USE_ITEM = "i";
			public static string CHANGE_EQUIPPED = "ce";
		}
		
		// Boss
		public static class Boss 
		{
			public static string ABILITY_START = "bs";
			public static string TRIGGER_TRAP = "u";
			public static string PUT_TRAP = "p";
		}
	}
	
	// Input
	public static class Input
	{
		// General Input
		public static string PLAYER_LEAVES = "l";
        public static string PLAYER = "p";
        public static string HP = "h";
		public static string TRAP_TRIGGERED = "t";
		public static string CHAR_CREATED = "nc";
		public static string EFFECT = "e";
		public static string MAP = "m";

		// Knight
		public static class Knight 
		{
			public static string ABILITY_START = "as";
			public static string ABILITY_END = "ae";
			public static string USE_ITEM_START = "i";
            public static string USE_ITEM_END = "ie";
            public static string USE_ITEM_INTERRUPTED = "ii";
			public static string CHANGE_EQUIPPED = "ce";

            public static string ITEM_INVENTORY = "inv";
            public static string ABILITY_INVENTORY = "anv";
		}
		
		// Boss
		public static class Boss 
		{
			public static string ABILITY_START = "bs";
			public static string ABILITY_END = "be";
		}
	}

    // Matchmaking Events
    public static class Matchmaker
    {
        // Input
        public static string SEARCHING = "s";
        public static string INTERRUPTED = "i";
        public static string MATCH_CREATION = "m";
        public static string FOUND = "f";
        public static string ACCEPTED = "a";

        // Output
        public static string START_SEARCH = "s";
        public static string DECLINE = "m";
        public static string CANCEL = "c";
        public static string ACCEPT = "a";
        public static string FIND = "f";
    }
}
