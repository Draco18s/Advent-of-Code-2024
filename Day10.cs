using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Draco18s.AoCLib;

namespace AdventofCode2024
{
	internal static class Day10
	{
		internal static long Part1(string input)
		{
			long result = 0l;
			Grid map = new Grid(input, false);

			List<Vector2> trailheads = map.LocateFeature(v => v[0,0] == 0, new List<Vector2>(){new Vector2(0,0)}, Grid.returnNegInf);

			foreach (Vector2 head in trailheads)
			{
				result += CountDestinations(head, map);
			}

			return result;
		}

		private static long CountDestinations(Vector2 start, Grid map)
		{
			int count = 0;
			Vector2[] dir = new[] { new Vector2(-1, 0), new Vector2(1, 0), new Vector2(0, -1), new Vector2(0, 1) };
			List<Vector2> open = new List<Vector2>();
			open.Add(start);
			while (open.Count > 0)
			{
				Vector2 p = open[0];
				open.RemoveAt(0);
				if (map[p] == 9)
				{
					count++;
					continue;
				}
				foreach (Vector2 d in dir)
				{
					if (map.IsInside(p+d) && map[p + d] == map[p] + 1 && !open.Contains(p+d))
					{
						open.Add(p+d);
					}
				}
			}
			return count;
		}

		internal static long Part2(string input)
		{
			long result = 0l;
			Grid map = new Grid(input, false);

			List<Vector2> trailheads = map.LocateFeature(v => v[0, 0] == 0, new List<Vector2>() { new Vector2(0, 0) }, Grid.returnNegInf);

			foreach (Vector2 head in trailheads)
			{
				result += CountPaths(head, map);
			}

			return result;
		}

		private static long CountPaths(Vector2 start, Grid map)
		{
			int count = 0;
			Vector2[] dir = new[] { new Vector2(-1, 0), new Vector2(1, 0), new Vector2(0, -1), new Vector2(0, 1) };
			List<Vector2> open = new List<Vector2>();
			open.Add(start);
			while (open.Count > 0)
			{
				Vector2 p = open[0];
				open.RemoveAt(0);
				if (map[p] == 9)
				{
					count++;
					continue;
				}
				foreach (Vector2 d in dir)
				{
					if (map.IsInside(p + d) && map[p + d] == map[p] + 1)
					{
						open.Add(p + d);
					}
				}
			}
			return count;
		}
	}
}
