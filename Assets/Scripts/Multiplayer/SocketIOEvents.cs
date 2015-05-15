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
		public static class KnightIO 
		{
			public static string USE_ABILITY = "a";
			public static string USE_ITEM = "i";
			public static string CHANGE_EQUIPPED = "ce";
		}
		
		// Boss
		public static class BossIO 
		{
			public static string TRIGGER_TRAP = "u";
			public static string PUT_TRAP = "p";
			public static string SPAWN_CREATURE = "s";
		}
	}
	
	// Input
	public static class Input
	{
		// General Input
		public static string PLAYER_LEAVES = "l";
		public static string HP = "h";
		public static string TRAP_TRIGGERED = "t";
	}

}
