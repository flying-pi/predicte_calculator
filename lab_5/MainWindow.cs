using System;
using System.Collections.Generic;
using Gtk;
using lab_5;


public partial class MainWindow : Gtk.Window
{
	private SourceCodeProccessor code = null;

	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{
		Build();
		this.CompileBtn.Clicked += this.onCompileBtnClicked;
		this.loadFromFileBtn.Clicked += this.onLoadBtnClicked;
		this.processPredicats.Clicked += this.onProccessBtnClicked;
		this.sourceCodeField.MoveCursor += this.onCursorMoved;
	}

	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{
		Application.Quit();
		a.RetVal = true;
	}

	protected void onCompileBtnClicked(object obj, EventArgs args)
	{
		compile();
	}

	private bool compile()
	{
		code = new SourceCodeProccessor(this.sourceCodeField.Buffer.Text);
		List<string> errors = new List<string>();
		if (!code.compile(ref errors))
		{
			string errorStr = "";
			foreach (var i in errors)
				errorStr += (i + "\n");
			this.errorList.Buffer.Text = errorStr;
			return false;
		}
		return true;
	}

	protected void onLoadBtnClicked(object obj, EventArgs args)
	{
		Gtk.FileChooserDialog filechooser =
		new Gtk.FileChooserDialog("Виберіть файл з кодом на мові C#",
			this,
			FileChooserAction.Open,
			"Відміна", ResponseType.Cancel,
			"Відкрити", ResponseType.Accept);
		filechooser.Filter = new FileFilter();
		filechooser.Filter.AddPattern("*.cs");
		if (filechooser.Run() == (int)ResponseType.Accept)
		{
			try
			{
				System.IO.StreamReader file = new System.IO.StreamReader(filechooser.Filename);
				string fileCode = file.ReadToEnd();
				this.sourceCodeField.Buffer.Text = fileCode;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
		}
		filechooser.Destroy();
	}

	protected void onProccessBtnClicked(object obj, EventArgs args)
	{
		if (!compile()) return;
		PredicatCaculator predicatManipulator = new PredicatCaculator();
		predicatManipulator.Show();
		predicatManipulator.setPredicats(this.code);
	}

	protected void onCursorMoved(object obj, EventArgs args)
	{
	}

}
