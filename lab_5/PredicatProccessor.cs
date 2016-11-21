using System;
using System.Collections.Generic;

namespace lab_5
{
	
	public class PredicatProccessor
	{
		private enum operations
		{
			eUniversal,eExistential
		}

		string expression;
		SourceCodeProccessor predicatSource;
		bool isNegation = false;
		string predicatName = "";
		string []argsName = new string[1];

		List<KeyValuePair<operations, string>> cvantorsList = new List<KeyValuePair<operations, string>>();

		public PredicatProccessor(string expression,SourceCodeProccessor predicatSource)
		{
			this.expression = expression;
			this.predicatSource = predicatSource;
			pareseExpression();
		}

		void pareseExpression()
		{
			string argName;
			string[] puts = expression.Split(new string[] { ")(" }, StringSplitOptions.None);
			foreach (var s in puts)
			{
				if (s.Contains(Consts.existentialSymbol))
				{
					argName = s.Replace(")", "").Replace(")", "").Replace(Consts.existentialSymbol, "").Trim();
					cvantorsList.Add(new KeyValuePair<operations, string>(operations.eExistential, argName));
					continue;
				}
				if (s.Contains(Consts.universalSymbol))
				{
					argName = s.Replace(")", "").Replace(")", "").Replace(Consts.universalSymbol, "").Trim();
					cvantorsList.Add(new KeyValuePair<operations, string>(operations.eUniversal, argName));
					continue;
				}
				string buf = s;
				if (s.Contains(Consts.negationSymbol))
				{
					this.isNegation = true;
					buf = buf.Replace(Consts.negationSymbol, "");
				}
				for (int i = 0; i < buf.Length; i++)
				{
					if (buf[i] == '(')
						break;
					predicatName += buf[i];
				}
				buf = buf.Replace(predicatName, "").Replace("(","").Replace(")","");
				argsName[0] = buf;
				if (buf.Contains(","))
					argsName = buf.Split(',');
			}
		}

		public string getResult()
		{
			List<KeyValuePair<List<object>, bool>> truthArgs = this.predicatSource.getAllDataForTruth(getSignature());
			if (truthArgs.Count == 0)
			{
				return "::\t\tunderfind";
			}
			bool boolResult = true;
			if (argsName.Length == 1)
			{
				boolResult = cvantorsList[0].Key == operations.eUniversal;
			}
			string result = "\n\t{";
			foreach (var m in truthArgs)
			{
				if(cvantorsList[0].Key == operations.eUniversal)
				boolResult = boolResult && m.Value;
				else
					boolResult = boolResult || m.Value;
				result += "("+m.Value+":: {";
				foreach (var v in m.Key)
					result += v.ToString()+", ";
				result = result.Substring(0, result.Length - 2);
				result += "}), ";

			}
			result = result.Substring(0, result.Length - 2);
			result += "}";
			if (isNegation) boolResult = !boolResult;
			if (argsName.Length == 1)
			{
				result = "Значення виразу :: " + boolResult.ToString() +result;
			}
			return result;
		}

		public string getSignature()
		{
			string result = predicatName + "(" + argsName[0];
			for (int i = 1; i < argsName.Length; i++)
				result += "," + argsName[i];
			result += ")";
			return result;
		}


		public override string ToString()
		{
			return expression;
		}
}
}
