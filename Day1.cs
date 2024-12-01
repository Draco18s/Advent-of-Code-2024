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
			List<int> listA = new List<int>();
			List<int> listB = new List<int>();
			foreach (string line in lines)
			{
				string[] val = line.Split(' ');
				int a = int.Parse(val[0]);
				int b = int.Parse(val[^1]);
				listA.Add(a);
				listB.Add(b);
			}
			listA.Sort();
			listB.Sort();
			for (int i = 0; i < listA.Count; i++)
			{
				int dif = Math.Abs(listA[i] - listB[i]);
				result += dif;
			}
			return result;
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			long result = 0l;
			List<int> listA = new List<int>();
			List<int> listB = new List<int>();
			foreach (string line in lines)
			{
				string[] val = line.Split(' ');
				int a = int.Parse(val[0]);
				int b = int.Parse(val[^1]);
				listA.Add(a);
				listB.Add(b);
			}

			foreach (int i in listA)
			{
				result += listB.Count(j => j == i) * i;
			}
			return result;
		}
	}
}