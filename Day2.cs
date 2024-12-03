using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AdventofCode2024
{
	internal static class Day2
	{
		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			foreach (string line in lines)
			{
				long[] values = line.Split(' ').Select(long.Parse).ToArray();
				if (CheckSafe(values, out _))
					result++;
			}
			return result;
		}

		private static bool CheckSafe(long[] values, out int idx)
		{
			idx = -1;
			bool? isDecreasing = null;
			for (int i = 0; i < values.Length; i++)
			{
				idx = i;
				if (i == 0)
					continue;
				if (i == 1)
				{
					isDecreasing = values[0] > values[1];
				}

				if (values[i] < values[i - 1] != isDecreasing)
					return false;
				long dif = values[i] - values[i - 1];
				if (Math.Abs(dif) < 1 || Math.Abs(dif) > 3)
					return false;
			}

			return true;
		}

		internal static long Part2(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			foreach (string line in lines)
			{
				long[] values = line.Split(' ').Select(long.Parse).ToArray();
				if (CheckSafe(values, out int idx))
				{
					result++;
				}
				else
				{
					List<long> l = values.ToList();
					l.RemoveAt(idx);
					if (CheckSafe(l.ToArray(), out _))
					{
						result++;
					}
					else {
						l = values.ToList();
						l.RemoveAt(idx-1);
						if (CheckSafe(l.ToArray(), out _))
						{
							result++;
						}
						else if(idx >= 2)
						{
							l = values.ToList();
							l.RemoveAt(idx - 2);
							if (CheckSafe(l.ToArray(), out _))
							{
								result++;
							}
						}
					}
				}
			}
			return result;
		}
	}
}
