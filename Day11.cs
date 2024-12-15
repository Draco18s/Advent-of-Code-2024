using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;

namespace AdventofCode2024
{
	internal static class Day11
	{
		internal static long Part1(string input)
		{
			string[] lines = input.Split(' ');
			long result = 0l;
			List<long> stones = new List<long>();
			foreach (string num in lines)
			{
				stones.Add(long.Parse(num));
			}

			List<long> nstones = new List<long>();
			for (int i = 0; i < 25; i++)
			{
				foreach(long s in stones)
				{
					if (s == 0)
					{
						nstones.Add(1);
						continue;
					}

					string str = s.ToString();
					if (str.Length % 2 == 0)
					{
						nstones.Add(long.Parse(str.Substring(0, str.Length / 2)));
						nstones.Add(long.Parse(str.Substring(str.Length / 2, str.Length / 2)));
						continue;
					}

					nstones.Add(s * 2024);
				}
				stones.Clear();
				(stones, nstones) = (nstones, stones);
			}
			return stones.Count;
		}

		internal static long Part2(string input)
		{
			string[] lines = input.Split(' ');
			Dictionary<long, long> stones = new Dictionary<long, long>();
			foreach (string num in lines)
			{
				stones.Add(long.Parse(num), 1);
			}
			Dictionary<long, long> nstones = new Dictionary<long, long>();
			for (int i = 0; i < 75; i++)
			{
				foreach (KeyValuePair<long, long> kvp in stones)
				{
					if (kvp.Key == 0)
					{
						if (nstones.ContainsKey(1))
							nstones[1] += kvp.Value;
						else
							nstones[1] = kvp.Value;
						continue;
					}
					string str = kvp.Key.ToString();
					if (str.Length % 2 == 0)
					{
						long a = long.Parse(str.Substring(0, str.Length / 2));
						long b = long.Parse(str.Substring(str.Length / 2, str.Length / 2));
						if (nstones.ContainsKey(a))
							nstones[a] += kvp.Value;
						else
							nstones[a] = kvp.Value;
						if (nstones.ContainsKey(b))
							nstones[b] += kvp.Value;
						else
							nstones[b] = kvp.Value;
						continue;
					}
					if (nstones.ContainsKey(kvp.Key * 2024))
						nstones[kvp.Key * 2024] += kvp.Value;
					else
						nstones[kvp.Key * 2024] = kvp.Value;
				}
				stones.Clear();
				(stones, nstones) = (nstones, stones);
			}

			return stones.Select(k => k.Value).Sum();
		}
	}
}
