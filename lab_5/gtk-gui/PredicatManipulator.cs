
// This file has been generated by the GUI designer. Do not modify.

public partial class PredicatManipulator
{
	private global::Gtk.Fixed fixed2;

	private global::Gtk.ScrolledWindow GtkScrolledWindow;

	private global::Gtk.TextView PredicatSignature;

	private global::Gtk.Entry expression;

	private global::Gtk.Label label3;

	private global::Gtk.Button Calculate;

	private global::Gtk.ScrolledWindow GtkScrolledWindow1;

	private global::Gtk.TextView result;

	protected virtual void Build()
	{
		global::Stetic.Gui.Initialize(this);
		// Widget PredicatManipulator
		this.WidthRequest = 900;
		this.HeightRequest = 700;
		this.Name = "PredicatManipulator";
		this.Title = global::Mono.Unix.Catalog.GetString("PredicatManipulator");
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		// Container child PredicatManipulator.Gtk.Container+ContainerChild
		this.fixed2 = new global::Gtk.Fixed();
		this.fixed2.WidthRequest = 900;
		this.fixed2.HeightRequest = 700;
		this.fixed2.Name = "fixed2";
		this.fixed2.HasWindow = false;
		// Container child fixed2.Gtk.Fixed+FixedChild
		this.GtkScrolledWindow = new global::Gtk.ScrolledWindow();
		this.GtkScrolledWindow.WidthRequest = 880;
		this.GtkScrolledWindow.HeightRequest = 200;
		this.GtkScrolledWindow.Name = "GtkScrolledWindow";
		this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
		this.PredicatSignature = new global::Gtk.TextView();
		this.PredicatSignature.CanFocus = true;
		this.PredicatSignature.Name = "PredicatSignature";
		this.PredicatSignature.Editable = false;
		this.GtkScrolledWindow.Add(this.PredicatSignature);
		this.fixed2.Add(this.GtkScrolledWindow);
		global::Gtk.Fixed.FixedChild w2 = ((global::Gtk.Fixed.FixedChild)(this.fixed2[this.GtkScrolledWindow]));
		w2.X = 10;
		w2.Y = 10;
		// Container child fixed2.Gtk.Fixed+FixedChild
		this.expression = new global::Gtk.Entry();
		this.expression.WidthRequest = 795;
		this.expression.CanFocus = true;
		this.expression.Name = "expression";
		this.expression.IsEditable = true;
		this.expression.InvisibleChar = '●';
		this.fixed2.Add(this.expression);
		global::Gtk.Fixed.FixedChild w3 = ((global::Gtk.Fixed.FixedChild)(this.fixed2[this.expression]));
		w3.X = 10;
		w3.Y = 290;
		// Container child fixed2.Gtk.Fixed+FixedChild
		this.label3 = new global::Gtk.Label();
		this.label3.WidthRequest = 880;
		this.label3.Name = "label3";
		this.label3.LabelProp = global::Mono.Unix.Catalog.GetString("Інструкція: \n\nДля того аби примінити квантор загальності потрібно ввести символ \"~\" на клавітурі.  Для квантору існування слід ввестисимвол \"#\".  Для того аби додати заперечення використовується символ \"!\"");
		this.label3.Wrap = true;
		this.fixed2.Add(this.label3);
		global::Gtk.Fixed.FixedChild w4 = ((global::Gtk.Fixed.FixedChild)(this.fixed2[this.label3]));
		w4.X = 10;
		w4.Y = 218;
		// Container child fixed2.Gtk.Fixed+FixedChild
		this.Calculate = new global::Gtk.Button();
		this.Calculate.CanFocus = true;
		this.Calculate.Name = "Calculate";
		this.Calculate.UseUnderline = true;
		this.Calculate.Label = global::Mono.Unix.Catalog.GetString("Обрахувати");
		this.fixed2.Add(this.Calculate);
		global::Gtk.Fixed.FixedChild w5 = ((global::Gtk.Fixed.FixedChild)(this.fixed2[this.Calculate]));
		w5.X = 810;
		w5.Y = 287;
		// Container child fixed2.Gtk.Fixed+FixedChild
		this.GtkScrolledWindow1 = new global::Gtk.ScrolledWindow();
		this.GtkScrolledWindow1.WidthRequest = 880;
		this.GtkScrolledWindow1.HeightRequest = 370;
		this.GtkScrolledWindow1.Name = "GtkScrolledWindow1";
		this.GtkScrolledWindow1.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow1.Gtk.Container+ContainerChild
		this.result = new global::Gtk.TextView();
		this.result.CanFocus = true;
		this.result.Name = "result";
		this.result.Editable = false;
		this.GtkScrolledWindow1.Add(this.result);
		this.fixed2.Add(this.GtkScrolledWindow1);
		global::Gtk.Fixed.FixedChild w7 = ((global::Gtk.Fixed.FixedChild)(this.fixed2[this.GtkScrolledWindow1]));
		w7.X = 10;
		w7.Y = 325;
		this.Add(this.fixed2);
		if ((this.Child != null))
		{
			this.Child.ShowAll();
		}
		this.DefaultWidth = 900;
		this.DefaultHeight = 708;
		this.Show();
	}
}
