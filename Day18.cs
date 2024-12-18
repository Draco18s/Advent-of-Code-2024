using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Draco18s.AoCLib;

namespace AdventofCode2024
{
	internal static class Day18
	{
		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			Grid mem = new Grid(71,71);
			long result = 0l;


			for (int x = 0; x < 71; x++)
			{
				for (int y = 0; y < 71; y++)
				{
					mem[x, y] = int.MaxValue;
				}
			}
			for (var i = 0; i < lines.Length && i < 1024; i++)
			{
				var line = lines[i];
				var l = line.Split(',');
				int x = int.Parse(l[0]);
				int y = int.Parse(l[1]);

				mem[x, y] = -1;
			}
			var r = Pathfind(new Vector2(0, 0), new Vector2(70, 70), mem);
			for (int x = 0; x < 71; x++)
			{
				for (int y = 0; y < 71; y++)
				{
					int i = Array.IndexOf(lines, ($"{x},{y}"));
					if (i >=0 && i < 1024)
						mem[x, y] = '#';
					else 
						mem[x, y] = '.';
				}
			}
			while (r != null)
			{
				mem[r.pos] = 'O';
				r = r.parent;
			}
			Console.WriteLine(mem);
			return mem[70, 70];
		}

		private class Node
		{
			public Vector2 pos;
			public int cost;
			public Node parent;
		}

		private static Node Pathfind(Vector2 start, Vector2 end, Grid map)
		{
			int count = 0;
			Vector2[] dir = new[] { new Vector2(-1, 0), new Vector2(1, 0), new Vector2(0, -1), new Vector2(0, 1) };
			List<Node> open = new List<Node>();
			List<Node> closed = new List<Node>();
			open.Add(new Node()
			{
				pos = start,
				cost = 0,
				parent = null
			});
			while (open.Count > 0)
			{
				Node p = open[^1];
				open.Remove(p);
				closed.Add(p);
				map[p.pos] = Math.Min(map[p.pos],p.cost);
				
				foreach (Vector2 d in dir)
				{
					if (map.IsInside(p.pos + d) && map[p.pos + d] > 0)
					{
						int q = map[p.pos + d];
						if (q > p.cost + 1)
						{
							open.Add(new Node()
							{
								pos = p.pos+d,
								cost = p.cost +1,
								parent = p
							});
						}
					}
				}
				//if(open.Count > 0)
				//	Console.WriteLine($"{open.Count}: {open[^1].pos}: {open[^1].cost}");
			}
			return closed.Where(c => c.pos == end).OrderBy(c => c.cost).FirstOrDefault();
		}

		private static int Distance(Node p)
		{
			return p.cost + (int)Math.Sqrt((70-p.pos.x)* (70 - p.pos.x) + (70-p.pos.y)* (70 - p.pos.y));
		}

		internal static long Part2(string input)
		{
			int result = 0;
			int bytes = 3028; //done iteratively from at 2048(steps 100), 2948(steps 10), 3028(steps 1)
			while (true)
			{
				bytes+=1;
				int len = DoPart2(input, bytes);
				if (len == int.MaxValue)
					break;
				result = bytes;
			}
			string[] lines = input.Split('\n');
			Console.WriteLine(lines[result]);
			return result;
		}

		private static int DoPart2(string input, int bytes)
		{
			string[] lines = input.Split('\n');
			Grid mem = new Grid(71, 71);
			long result = 0l;


			for (int x = 0; x < 71; x++)
			{
				for (int y = 0; y < 71; y++)
				{
					mem[x, y] = int.MaxValue;
				}
			}
			for (var i = 0; i < lines.Length && i < bytes; i++)
			{
				var line = lines[i];
				var l = line.Split(',');
				int x = int.Parse(l[0]);
				int y = int.Parse(l[1]);

				mem[x, y] = -1;
			}
			Pathfind(new Vector2(0, 0), new Vector2(70, 70), mem);
			return mem[70, 70];
		}
	}
}
