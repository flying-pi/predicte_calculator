using System;
using System.Collections.Generic;

public class PredicatеSource
{
	public bool predicateP(string x)
	{
		return (x).Length > 1;
	}

	public List<string> argxOfP()
	{
		List<string> result = new List<string>();
		for (int i = 1; i < 10; result.Add((i++ + 7).ToString())) ;
		return result;
	}


	public List<string> argyOfP()
	{
		List<string> result = new List<string>();
		for (int i = 1; i < 10; result.Add(i++.ToString())) ;
		return result;
	}

	public bool predicateStringPredicat(string Str)
	{
		string[] puts = Str.Split(new char[] { ' ' });
		foreach (var s in puts)
		{
			if (getCountSubItem(puts,s) > 2)
				return true;
		}
		return false;
	}

	public int getCountSubItem(string[] source, string item)
	{
		int result = 0;
		foreach (var s in source)
			result += (s == item) ? 1 : 0;
		return result;
	}

	public List<string> argStrOfStringPredicat()
	{
		string filePath = "/Users/yurabraiko/Documents/source.txt";
		return new List<string>(System.IO.File.ReadAllLines(filePath));
	}

}