using System;
using Gtk;
using System.Collections.Generic;
using BassetuneReaperLauncher;

public partial class MainWindow: Gtk.Window
{    
    List<News> showedNews  =new List<News>();

    public class TextArgs : EventArgs
    {
        private string message;

        public TextArgs(string TextMessage)
        {
            message = TextMessage;
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }
    }

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnClick (object sender, EventArgs e)
	{
		//TODO launch the real game client

		Application.Quit ();
	}


    public void SetNews(List<News> n)
    {
        Gdk.Color col = new Gdk.Color(155, 203, 224);
        label4.ModifyFg(StateType.Normal, col);
        label5.ModifyFg(StateType.Normal, col);
        col = new Gdk.Color(253, 63, 25);
        label6.ModifyFg(StateType.Normal, col);
        showedNews = n;
        NewsTitle1.Label = showedNews[0].Title;
        NewsTitle2.Label = showedNews[1].Title;
        NewsTitle3.Label = showedNews[2].Title;
        NewsTitle4.Label = showedNews[2].Title;
        NewsTitle5.Label = showedNews[2].Title;
        Gdk.Pixbuf pix = new Gdk.Pixbuf("LoginBackground.png");
        Gdk.Pixmap background;
        Gdk.Pixmap mask;
        pix.RenderPixmapAndMask(out background, out mask, 0);
        Style style = new Style();
        style.SetBgPixmap(StateType.Normal, background);
        this.Style = style;
        MainViewer.GetWindow(TextWindowType.Text).Opacity=0.5;
        MainViewer.GetWindow(TextWindowType.Widget).Opacity = 0.3;
        scrolledWindow1.AppPaintable = true;
        scrolledWindow1.GdkWindow.SetBackPixmap(null, true);
        MainViewer.AppPaintable = true;
        MainViewer.GdkWindow.SetBackPixmap(null, true);
    }

    protected void OnClickNews1(object sender, EventArgs e)
    {
        MainViewer.Buffer.Text = showedNews[0].Body;
    }

    protected void OnClickNews2(object sender, EventArgs e)
    {
        MainViewer.Buffer.Text = showedNews[1].Body;
    }

    protected void OnClickNews3(object sender, EventArgs e)
    {
        MainViewer.Buffer.Text = showedNews[2].Body;
    }

    protected void OnClickNews4(object sender, EventArgs e)
    {
        MainViewer.Buffer.Text = showedNews[3].Body;
    }

    protected void OnClickNews5(object sender, EventArgs e)
    {
        MainViewer.Buffer.Text = showedNews[4].Body;
    }

}
