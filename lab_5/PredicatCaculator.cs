
using System;
using System.Collections.Generic;

namespace lab_5
{
	public partial class PredicatCaculator : Gtk.Window
	{
		SourceCodeProccessor predicats;

		public PredicatCaculator() :
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			this.calculate.Clicked += onCalculateBtnCLick;
			this.userInput.Changed += onEditTextChanged;
		}

		public void setPredicats(SourceCodeProccessor predicats)
		{
			this.predicats = predicats;
			var names = predicats.getPredicatsName();
			string singleStringName = "";
			foreach (var s in names)
			{
				singleStringName += s + "\n";
			}
			this.PredicatSignature.Buffer.Text = singleStringName;
		}

		protected void onCalculateBtnCLick(object obj, EventArgs args)
		{
			string[] userInputs = this.userInput.Text.Split(new char[] { ';' });
			List<PredicatProccessor> predicatsExpression = new List<PredicatProccessor>();
			foreach (var s in userInputs)
				if (s.Length > 2)
					predicatsExpression.Add(new PredicatProccessor(s, this.predicats));

			this.resultView.Buffer.Text = "";
			foreach (var i in predicatsExpression)
			{
				resultView.Buffer.Text += i.ToString() +":: "+i.getResult()+ "\n";
				resultView.Buffer.Text += "____________________________________________________________________________________________________________________________________________________" +
					"__________________________________________________________________________________________________________________________________________" +
					"_____________________________________________________________________\n\n";

			}
		}

		protected void onEditTextChanged(object obj, EventArgs args)
		{
			int cursorPos = this.userInput.CursorPosition;
			userInput.Text = userInput.Text.Replace(" ", "").Replace('~', '∀').Replace('#', '∃').Replace('!', '¬');
			userInput.Position = cursorPos;
		}
	}
}
