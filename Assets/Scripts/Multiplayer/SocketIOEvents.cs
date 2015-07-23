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
			public static string USE_ITEM = "i";
			public static string CHANGE_EQUIPPED = "ce";
		}
		
		// Boss
		public static class Boss 
		{
			public static string ABILITY_START = "bs";
			public static string ABILITY_END = "be";
			public static string PUT_TRAP = "p";
		}
	}

}
