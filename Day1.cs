using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AdventofCode2024 {
	internal static class Day1 {
		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			foreach (string line in lines)
			{
				string val = "";
				foreach (var c in line)
				{
					if (char.IsDigit(c))
						val += c;
				}

				result += int.Parse(val[0].ToString() + val[^1].ToString());
			}
			return result;
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			long result = 0l;
			foreach (string line in lines)
			{
				
			}
			return result;
		}
	}
}