using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AdventofCode2024
{
	internal static class Day7
	{
		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			foreach (string line in lines)
			{
				string[] parts = line.Split(':');
				long target = long.Parse(parts[0]);
				long[] nums = parts[1].Split(' ').Where(v => !string.IsNullOrEmpty(v)).Select(long.Parse).ToArray();
				if (Combine1(nums, 0, target))
				{
					result += target;
				}
			}
			return result;
		}

		private static long Parse(string lin)
		{
			lin += " ";
			long workingNum = 0;
			long result = 0;
			char exp = ' ';
			foreach (char c in lin)
			{
				if (c == ' ')
				{
					if (exp == '+')
						result += workingNum;
					else if (exp == '*')
						result *= workingNum;
				} 
				else if (c == '+' || c == '*')
				{
					if (result == 0)
					{
						result = workingNum;
					}
					else
					{
						if(exp == '+')
							result += workingNum;
						else if(exp == '*')
							result *= workingNum;

					}
					exp = c;
					workingNum = 0;
				}
				else
				{
					workingNum = (workingNum * 10) + long.Parse(""+c);
				}
			}

			return result;
		}

		private static bool Combine1(long[] nums, long working, long target)
		{
			if (nums.Length == 0) return working == target;

			if (working > target) return false;

			long v = nums.Take(1).First();
			nums = nums.Skip(1).ToArray();

			if (working == 0)
				return Combine1(nums, v, target);
			else
			{
				return Combine1(nums, working + v, target) ||
				       Combine1(nums, working * v, target);
			}
		}

		private static bool Combine2(long[] nums, long working, long target)
		{
			if (nums.Length == 0) return working == target;

			if (working > target) return false;

			long v = nums.Take(1).First();
			nums = nums.Skip(1).ToArray();

			if(working == 0)
				return Combine2(nums, v, target);
			else
			{
				return Combine2(nums, working + v, target) ||
				Combine2(nums, working * v, target) ||
				Combine2(nums, long.Parse($"{working}{v}"), target);
			}
		}

		internal static long Part2(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			
			foreach (string line in lines)
			{
				string[] parts = line.Split(':');
				long target = long.Parse(parts[0]);
				long[] nums = parts[1].Split(' ').Where(v => !string.IsNullOrEmpty(v)).Select(long.Parse).ToArray();
				if (Combine2(nums, 0, target))
				{
					result += target;
				}
			}
			return result;
		}
	}
}
