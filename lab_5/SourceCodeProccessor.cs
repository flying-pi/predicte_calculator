using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.CSharp;
using System.Collections.Immutable;


public class SourceCodeProccessor
{
	private string sourceCode;

	private List<PrdicatMethodInfo> prediactsList = new List<PrdicatMethodInfo>();

	CompilerResults results;

	public SourceCodeProccessor(string sourceCode)
	{
		this.sourceCode = sourceCode;
	}

	public bool compile(ref List<string> errors)
	{
		prediactsList.Clear();
		Dictionary<string, string> providerOptions = new Dictionary<string, string>
				{
					{"CompilerVersion", "v5.0"}
				};
		CSharpCodeProvider provider = new CSharpCodeProvider(providerOptions);

		CompilerParameters compilerParams = new CompilerParameters
		{
			GenerateInMemory = true,
			GenerateExecutable = false
		};

		CompilerResults results = provider.CompileAssemblyFromSource(compilerParams, sourceCode);

		if (results.Errors.Count != 0)
		{
			bool isError = false;
			foreach (CompilerError e in results.Errors)
			{
				isError = isError | (!e.IsWarning);
				if (e.IsWarning)
					errors.Add(e.ErrorText + "\tat " + e.Line.ToString() + ":" + e.Column.ToString());
			}
			if (isError)
				return false;
		}
		Assembly assembly = results.CompiledAssembly;
		Type predicateSource = null;
		if ((predicateSource = validateClass(ref assembly, ref errors)) == null)
			return false;
		if (!parsePrdicats(ref predicateSource, ref errors))
			return false;
		this.results = results;
		return true;
	}

	private bool parsePrdicats(ref Type predicateSource, ref List<string> errors)
	{
		MethodInfo[] methods = predicateSource.GetMethods();
		MethodInfo m;
		for (int i = 0; i < methods.Length; i++)
		{
			m = methods[i];
			if (!m.Name.Contains(Consts.predicateMethodPrefix))
				continue;
			if (m.ReturnType.FullName != "System.Boolean")
			{
				errors.Add("method \"" + m.Name + "\" must have bool as return type");
				return false;
			}
			if (!parsePredicatItem(ref predicateSource, ref m, ref errors))
				return false;
		}
		return true;
	}

	private bool parsePredicatItem(ref Type predicateSource, ref MethodInfo method, ref List<string> errors)
	{
		PrdicatMethodInfo predicat = new PrdicatMethodInfo();
		predicat.name = method.Name.Replace(Consts.predicateMethodPrefix, "");
		predicat.method = method;
		ParameterInfo[] parameters = method.GetParameters();
		foreach (ParameterInfo p in parameters)
		{
			predicat.argsNames.Add(p.Name);
			predicat.arsTypes.Add(p.ParameterType);
		}
		MethodInfo predicatDataSource;
		string predicatDataSourceName;
		for (int i = 0; i < predicat.argsNames.Count; i++)
		{
			var name = predicat.argsNames[i];
			predicatDataSourceName = Consts.argumentMethidPrefix
										   + name
										   + "Of"
										   + predicat.name;
			if ((predicatDataSource = predicateSource.GetMethod(predicatDataSourceName)) == null)
			{
				errors.Add("Can not found method with name " + predicatDataSourceName + "that will be used as " +
						   "data source for predicat witn name " + predicat.name + " to argument with name " +
						   name);
				return false;
			}
			Type t = predicat.arsTypes[i];
			Type targetType = typeof(IEnumerable<>).MakeGenericType(t);
			Type sourceType = predicatDataSource.ReturnType;
			bool isInstance = targetType.IsInstanceOfType(Activator.CreateInstance(sourceType));
			if (!isInstance)
			{
				errors.Add("method " + predicatDataSourceName + " must habe return tupe that will be instance of" +
						   "IEnumerator<" + t.FullName + ">");
				return false;
			}
			predicat.argsValueGenerator.Add(name, predicatDataSource);
		}
		prediactsList.Add(predicat);
		return true;
	}

	private Type validateClass(ref Assembly assembly, ref List<string> errors)
	{
		var types = assembly.GetTypes();
		foreach (var t in types)
			if (t.FullName == Consts.rootClassName)
				return t;
		errors.Add("Can not found class " + Consts.rootClassName);
		return null;
	}

	public override string ToString()
	{
		return string.Format("[SourceCodeProccessor]");
	}

	public List<string> getPredicatsName()
	{
		List<string> result = new List<string>();
		foreach (var p in prediactsList)
			result.Add(p.ToString());
		return result;
	}

	private class PrdicatMethodInfo
	{
		public string name = "";
		public MethodInfo method = null;
		public List<string> argsNames = new List<string>();
		public List<Type> arsTypes = new List<Type>();
		public Dictionary<String, MethodInfo> argsValueGenerator = new Dictionary<string, MethodInfo>();
		public override string ToString()
		{
			string args = ""; ;
			if (argsNames.Count > 0)
				args = argsNames[0];
			for (int i = 1; i < argsNames.Count; i++)
				args += "," + argsNames[i];

			return string.Format(name + "(" + args + ")");
		}
	}

	public List<KeyValuePair<List<object>,bool>> getAllDataForTruth(String methodSignature)
	{
		List<List<object>> result = new List<List<object>>();
		List<KeyValuePair<List<object>, bool>> functionResult = new List<KeyValuePair<List<object>, bool>>();
		PrdicatMethodInfo currentPredicat = null;
		foreach (var p in prediactsList)
		{
			if (p.ToString() == methodSignature)
			{
				currentPredicat = p;
				break;
			}
		}
		if (currentPredicat == null)
			return functionResult;
		object predicatObject = results.CompiledAssembly.CreateInstance(Consts.rootClassName);

		List<IEnumerable<object>> sourceData = new List<IEnumerable<object>>();
		foreach (var n in currentPredicat.argsNames)
		{
			MethodInfo method = currentPredicat.argsValueGenerator[n];
			object methodResult = method.Invoke(predicatObject, new object[] { });
			sourceData.Add((IEnumerable<object>)methodResult);
		}
		result = getAllCombination(sourceData);
		foreach (var i in result)
		{
			MethodInfo method = currentPredicat.method;
			bool methodResult = (bool)method.Invoke(predicatObject, i.ToArray());
			functionResult.Add(new KeyValuePair<List<object>, bool>(i, methodResult));
		}
		return functionResult;
	}

	List<List<object>> getAllCombination(List<IEnumerable<object>> items)
	{
		List<List<object>> result = new List<List<object>>();
		foreach (var v in items[0])
		{
			List<object> item = new List<object>();
			item.Add(v);
			result.Add(item);
		}
		for (int i = 1; i < items.Count; i++)
			result = decart(result, items[i]);
		return result;
	}


	public List<List<object>> decart(List<List<object>> a, IEnumerable<object> b)
	{
		List<List<object>> result = new List<List<object>>();
		foreach (var v in a)
		{
			foreach (var c in b)
			{
				List<object> item = new List<object>(v);
				item.Add(c);
				result.Add(item);
			}
		}
		return result;
	}
}
