using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AdventofCode2024
{
	internal static class Day3
	{
		internal static long Part1(string input)
		{
			long result = 0l;
			Regex mul = new Regex(@"mul\((\d+),(\d+)\)");
			MatchCollection matches = mul.Matches(input);
			foreach (Match c in matches)
			{
				result += long.Parse(c.Groups[1].Value) * long.Parse(c.Groups[2].Value);
			}
			return result;
		}

		internal static long Part2(string input)
		{
			long result = 0l;
			Regex mul = new Regex(@"(?:mul\((\d+),(\d+)\))|(?:do\(\))|(?:don't\(\))");
			//Regex _do = new Regex(@"do\(\)");
			//Regex _dont = new Regex(@"don't\(\)");
			MatchCollection matches = mul.Matches(input);
			bool execute = true;
			foreach (Match c in matches)
			{
				if (c.Value == "do()")
				{
					execute = true;
				}
				else if (c.Value == "don't()")
				{
					execute = false;
				}
				else if(execute)
					result += long.Parse(c.Groups[1].Value) * long.Parse(c.Groups[2].Value);
			}
			
			return result;
		}
	}
}
