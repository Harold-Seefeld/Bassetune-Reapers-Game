using System;
using System.Collections.Generic;

namespace BassetuneReaperLauncher
{
	public class Launcher
	{
		MainWindow mainWindow;

		public Launcher ()
		{
            mainWindow = new MainWindow();
		}

		public void StartCheck ()
		{				
			mainWindow.Show();
            GetNewsFromServer();
			string actualVersion = GetActualVersion();
			string serverVersion = GetServerVersion();
			if (actualVersion == serverVersion)
			{
				//TODO disable update button, enable launch button
			}
			else
			{
				//the other way around
			}
		}


		string GetActualVersion ()
		{
			//TODO get the actual version from our client
			return "0.000";
		}

		string GetServerVersion ()
		{
			//TODO get the version from server
			return "0.000";
		}                   
       
        void GetNewsFromServer()
        {            
            //TODO retrieve the list from server
            List<News> myNews=new List<News>();
            News n= new News();
            n.Title=("Patch note 0.000");
            n.Body = ("-EQWEWQEeqewqeq\r-eweqewqeqwe\r-okokokokko\r-qieoieqopewp\r" +
            "-lklkqeqw\r-dasdasasdqw");
            myNews.Add(n);
            n.Title=("Relase date announced!");
            n.Body = ("dwqdwqdqwkdowkdoqwodqwkdokqok");
            myNews.Add(n);
            n.Title=("another news");
            n.Body=("blablabla");
            myNews.Add(n);
            myNews.Add(n);
            myNews.Add(n);
            mainWindow.SetNews(myNews);
        }

	}
}

