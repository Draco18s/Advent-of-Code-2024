using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Draco18s.AoCLib;

namespace AdventofCode2024
{
	internal static class Day8
	{
		internal static long Part1(string input)
		{
			long result = 0l;
			Grid grid = new Grid(input, true);
			Dictionary<char,List<Vector2>> antennas = new();
			HashSet<Vector2>  unique = new();
			for (int x = 0; x < grid.Width; x++)
			{
				for (int y = 0; y < grid.Height; y++)
				{
					char c = (char)grid[x, y];
					if (c != '.')
					{
						if (!antennas.ContainsKey(c))
						{
							antennas.Add(c,new List<Vector2>());
						}
						antennas[c].Add(new Vector2(x,y));
					}
				}
			}

			Grid grid2 = new Grid(grid.Width, grid.Height);
			foreach (var kvp in antennas)
			{
				result += FindNearest(grid, grid2, kvp, ref unique);
			}
			Console.WriteLine(grid2.ToString("char+0"));

			return result;
		}

		private static long FindNearest(Grid grid, Grid nGrid, KeyValuePair<char, List<Vector2>> kvp, ref HashSet<Vector2> unique)
		{
			long count = 0;
			for (int x = 0; x < grid.Width; x++)
			{
				for (int y = 0; y < grid.Height; y++)
				{
					if (nGrid[x, y] == 0)
						nGrid[x, y] = grid[x, y];

					foreach (Vector2 v1 in kvp.Value)
					{
						foreach (Vector2 v2 in kvp.Value)
						{
							if(v1 == v2) continue;
							var t = new Vector2(x, y);
							if (y == 4)
								;
							if (IsInLine(t, v1, v2) && !unique.Contains(t))
							{
								if (t.y == 11 && t.x >= 10)
									;
								double d1 = Distance(t, v1);
								double d2 = Distance(t, v2);
								if (Math.Abs(Math.Max(d1, d2) - 2 * Math.Min(d1, d2)) < 0.0001)
								{
									count++;
									unique.Add(t);
									nGrid[x, y] = '#';
								}
							}
						}
					}
				}
			}
			return count;
		}

		private static long FindNearest2(Grid grid, Grid nGrid, KeyValuePair<char, List<Vector2>> kvp, ref HashSet<Vector2> unique)
		{
			long count = 0;
			for (int x = 0; x < grid.Width; x++)
			{
				for (int y = 0; y < grid.Height; y++)
				{
					if(nGrid[x,y] == 0)
						nGrid[x, y] = grid[x, y];

					foreach (Vector2 v1 in kvp.Value)
					{
						foreach (Vector2 v2 in kvp.Value)
						{
							if (v1 == v2) continue;
							var t = new Vector2(x, y);
							if (IsInLine(t, v1, v2) && !unique.Contains(t))
							{
								count++;
								unique.Add(t);
								nGrid[x, y] = '#';
							}
						}
					}
				}
			}
			return count;
		}

		private static double Distance(Vector2 p0, Vector2 p1)
		{
			return Math.Sqrt((p0.x - p1.x) * (p0.x - p1.x) + (p0.y - p1.y) * (p0.y - p1.y));
		}

		private static bool IsInLine(Vector2 t, Vector2 a, Vector2 b)
		{
			if (t == a || t == b) return true;

			double mAB = int.MaxValue;
			if((a.y - b.y) != 0)
				mAB = (double)(a.x - b.x) / (a.y - b.y);

			double mTA = int.MaxValue;
			if ((a.y - t.y) != 0)
				mTA = (double)(a.x - t.x) / (a.y - t.y);

			double mTB = int.MaxValue;
			if ((b.y - t.y) != 0)
				mTB = (double)(b.x - t.x) / (b.y - t.y);

			return  Math.Abs(mAB - mTA) < 0.0001 && Math.Abs(mAB - mTB) < 0.0001;
		}

		internal static long Part2(string input)
		{
			long result = 0l;
			Grid grid = new Grid(input, true);
			Dictionary<char, List<Vector2>> antennas = new();
			HashSet<Vector2> unique = new();
			for (int x = 0; x < grid.Width; x++)
			{
				for (int y = 0; y < grid.Height; y++)
				{
					char c = (char)grid[x, y];
					if (c != '.')
					{
						if (!antennas.ContainsKey(c))
						{
							antennas.Add(c, new List<Vector2>());
						}
						antennas[c].Add(new Vector2(x, y));
					}
				}
			}

			Grid grid2 = new Grid(grid.Width, grid.Height);
			foreach (var kvp in antennas)
			{
				result += FindNearest2(grid, grid2, kvp, ref unique);
			}
			Console.WriteLine(grid2.ToString("char+0"));

			return result;
		}
	}
}
