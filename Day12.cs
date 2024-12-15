using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Draco18s.AoCLib;

namespace AdventofCode2024
{
	internal static class Day12
	{
		internal static long Part1(string input)
		{
			Grid farm = new Grid(input, true);
			Dictionary<char, long> values = new Dictionary<char, long>();
			while (true)
			{
				Vector2 point = FindPlot(farm);
				if (point.x < 0 || point.y < 0) break;
				char c = (char)farm[point];
				(long a, long p) = GetAreaAndPerimeter(farm, point);
				if (values.ContainsKey(c))
				{
					values[c] += a * p;
				}
				else
					values.Add(c, a*p);
			}

			return values.Values.Sum();
		}

		private static (long a, long p) GetAreaAndPerimeter(Grid farm, Vector2 point)
		{
			char v = (char)farm[point];
			long perimeter = 0;
			long sz = farm.FloodFill(point, (p1, p2) =>
			{
				return -v;
			}, (p1, p2, arg3, arg4) =>
			{
				if (arg4 == Int32.MinValue)
				{
					perimeter++;
					return false;
				}
				perimeter += v != Math.Abs(arg4) ? 1 : 0;
				return v == arg4;
			}, Grid.returnNegInf);
			return (sz, perimeter);
		}

		private static Vector2 FindPlot(Grid farm)
		{
			for (int x = 0; x < farm.Width; x++)
			{
				for (int y = 0; y < farm.Height; y++)
				{
					if (farm[x, y] > 0)
						return new Vector2(x, y);
				}
			}
			return new Vector2(-1, -1);
		}

		internal static long Part2(string input)
		{
			Grid farm = new Grid(input, true);
			Dictionary<char, long> values = new Dictionary<char, long>();
			while (true)
			{
				Vector2 point = FindPlot(farm);
				if (point.x < 0 || point.y < 0) break;
				char c = (char)farm[point];
				(long a, long s) = GetAreaAndSideCount(farm, point);
				if (values.ContainsKey(c))
				{
					values[c] += a * s;
				}
				else
					values.Add(c, a * s);
			}

			return values.Values.Sum();
		}

		private static (long a, long s) GetAreaAndSideCount(Grid farm, Vector2 point)
		{
			char v = (char)farm[point];
			List<Vector2> region = new List<Vector2>();
			region.Add(point);
			long sz = farm.FloodFill(point, (p1, p2) =>
			{
				return -v;
			}, (p1, p2, arg3, arg4) =>
			{
				if (!region.Contains(p2)) region.Add(p2);
				return v == arg4;
			}, Grid.returnNegInf);

			List<Edge> sides = new List<Edge>();
			Vector2[] dirs = new[] { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1) };
			foreach (Vector2 p in region)
			{
				foreach (Vector2 d in dirs)
				{
					Vector2 edge = p + d;
					if (farm.IsInside(edge) && farm[edge] == -v)
					{
						continue;
					}

					List<Edge> matched = new List<Edge>();
					foreach (Edge e in sides)
					{
						if (!e.Adjacent(p, d)) continue;

						// Found matching Edge group
						matched.Add(e);
					}

					if (matched.Count == 0)
					{
						// Didn't find an adjacent Edge group, create one
						Edge e = new Edge(d);
						e.Add(p);
						sides.Add(e);
					}
					else if (matched.Count == 1)
					{
						matched[0].cells.Add(p);
					}
					else
					{
						// Found more than one adjacent Edge group, merge them
						Edge e = matched[0];
						e.Add(p);
						for (int i = 1; i < matched.Count; i++)
						{
							foreach (Vector2 cell in matched[i].cells)
							{
								e.Add(cell);
							}
							sides.Remove(matched[i]);
						}
					}
				}
			}

			return (sz, sides.Count);
		}

		private class Edge
		{
			public List<Vector2> cells = new List<Vector2>();
			public Vector2 dir;

			public Edge(Vector2 dir) { this.dir = dir; }

			public bool Adjacent(Vector2 pos, Vector2 facing)
			{
				// Adjacent edges must be facing the same direction
				if (dir != facing) 
					return false;

				foreach (Vector2 cell in cells)
				{
					// if pos is neighbor in the perpendicular to the facing direction
					if (cell + new Vector2(facing.y, -facing.x) == pos || cell + new Vector2(-facing.y, facing.x) == pos)
					{
						return true;
					}
				}
				return false;
			}

			public void Add(Vector2 p) => cells.Add(p);
		}
	}
}
