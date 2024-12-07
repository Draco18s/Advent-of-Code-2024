using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AdventofCode2024
{
	internal static class Day5
	{
		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			bool firstSection = true;
			Dictionary<int, List<int>> orderRules = new Dictionary<int, List<int>>();
			//Dictionary<int, int> orderRulesRev = new Dictionary<int, int>();
			List<int> middlePages = new List<int>();
			foreach (string line in lines)
			{
				if (string.IsNullOrEmpty(line))
				{
					firstSection = false;
					continue;
				}
				if (firstSection)
				{
					string[] nums = line.Split('|');
					int k = int.Parse(nums[0]);
					int v = int.Parse(nums[1]);

					if(orderRules.ContainsKey(k))
						orderRules[k].Add(v);
					else
						orderRules.Add(k, new List<int>(){v});
				}
				else
				{
					string[] nums = line.Split(',');
					List<int> pages = new List<int>();
					pages.AddRange(nums.Select(int.Parse));
					if (IsCorrect(orderRules, pages))
					{
						middlePages.Add(pages[pages.Count / 2]);
					}
				}
			}
			return middlePages.Sum();
		}

		private static bool IsCorrect(Dictionary<int, List<int>> orderRules, List<int> pages)
		{
			for (int i = 0; i < pages.Count; i++)
			{
				if(!orderRules.ContainsKey(pages[i])) continue;
				foreach (int V in orderRules[pages[i]])
				{
					int ind = pages.IndexOf(V);
					if (ind >= 0 && ind < i)
						return false;
				}
			}

			return true;
		}

		internal static long Part2(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			bool firstSection = true;
			Dictionary<int, List<int>> orderRules = new Dictionary<int, List<int>>();
			//Dictionary<int, int> orderRulesRev = new Dictionary<int, int>();
			List<int> middlePages = new List<int>();
			foreach (string line in lines)
			{
				if (string.IsNullOrEmpty(line))
				{
					firstSection = false;
					continue;
				}
				if (firstSection)
				{
					string[] nums = line.Split('|');
					int k = int.Parse(nums[0]);
					int v = int.Parse(nums[1]);

					if (orderRules.ContainsKey(k))
						orderRules[k].Add(v);
					else
						orderRules.Add(k, new List<int>() { v });
				}
				else
				{
					string[] nums = line.Split(',');
					List<int> pages = new List<int>();
					pages.AddRange(nums.Select(int.Parse));
					if (!IsCorrect(orderRules, pages))
					{
						FixOrdering(orderRules, ref pages);
						middlePages.Add(pages[pages.Count / 2]);
					}
				}
			}
			return middlePages.Sum();
		}

		private static void FixOrdering(Dictionary<int, List<int>> orderRules, ref List<int> pages)
		{
			pages.Sort((a, b) =>
			{
				if (orderRules.ContainsKey(a) && orderRules[a].Contains(b))
				{
					return -1;
				}
				if (orderRules.ContainsKey(b) && orderRules[b].Contains(a))
				{
					return 1;
				}

				return 0;
			});
		}
	}
}
