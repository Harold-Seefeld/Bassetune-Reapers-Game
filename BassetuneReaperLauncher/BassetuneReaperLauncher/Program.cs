using System;
using Gtk;

namespace BassetuneReaperLauncher
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			Launcher launcher = new Launcher ();
			launcher.StartCheck ();
			Application.Run ();
		}



	}
}
