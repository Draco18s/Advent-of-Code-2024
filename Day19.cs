using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace AdventofCode2024
{
	internal static class Day19
	{
		internal static long Part1(string input)
		{
			return 0;
			var lines = input.Split('\n').Skip(2).AsParallel();//.AsParallel();

			long result = 0l;
			var allpat = input.Split('\n')[0].Split(", ");
			var patterns = allpat.Where(p => p.Contains('w')).OrderBy(l => l.Length)
				.Concat(allpat.Where(p => !p.Contains('w')).OrderBy(l => l.Length))
				.ToArray();
			
			result = lines.Count(l => !l.Contains('w') || CheckPattern(l, 0, l.Length, patterns));
			return result;
		}

		private static ConcurrentDictionary<string, long> cache = new ConcurrentDictionary<string, long>();

		private static bool CheckPattern(string line, int s, int len, string[] patterns)
		{
			if (len-s <= 0)
			{
				//Console.WriteLine(line);
				return true;
			}

			if (line.EndsWith("rgw") || line.EndsWith("rgwbgw"))
				return false;

			bool b = patterns.Any(p =>
			{
				bool cont = true;
				for (var i = 0; cont && i < p.Length; i++)
				{
					var c = p[i];
					if (i + s >= len || line[i + s] != c) cont = false;
				}

				return cont && CheckPattern(line, s + p.Length, len, patterns);
			});

			return b;
		}

		internal static long Part2(string input)
		{
			var lines = input.Split('\n').Skip(2);//.AsParallel();

			var allpat = input.Split('\n')[0].Split(", ");
			var allpatOrd = allpat.OrderByDescending(p => p.Length).ToArray();

			long result = lines.Sum(l => CountPatterns(l, 0, l.Length, allpatOrd));
			
			return result;
		}

		private static long CountPatterns(string line, int s, int len, string[] patterns)
		{
			if (len - s <= 0)
			{
				return 1;
			}

			if (line.EndsWith("rgw") || line.EndsWith("rgwbgw"))
				return 0;

			if (cache.TryGetValue(line, out long ch))
			{
				if (line.Equals("gbbr"))
					;
				return ch;
			}

			long b = 0;
			foreach (string p in patterns)
			{
				if (line.StartsWith(p))
				{
					b += CountPatterns(line.Substring(p.Length), 0, len-p.Length, patterns);
				}
			}

			cache.TryAdd(line, b);
			return b;
		}
	}
}
