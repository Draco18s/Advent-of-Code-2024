using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Channels;
using Draco18s.AoCLib;

namespace AdventofCode2024
{
	internal static class Day16
	{
		private static Vector2[] FACING = new[] { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1) };
		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			Grid maze = new Grid(input, true);
			
			Vector2 start = maze.FindFirst('S');
			Vector2 end = maze.FindFirst('E');
			Vector2 dir = new Vector2(1, 0);

			return Pathfind(start, end, dir, maze, out _);

			//return result;
		}

		private class PathNode
		{
			public Vector2 pos;
			public Vector2 dir;
			public int cost;
			public PathNode parent;
		}

		private static long Pathfind(Vector2 start, Vector2 end, Vector2 dir, Grid maze, out Grid path)
		{
			Grid pathCost = new Grid(maze.Width, maze.Height);
			
			List<PathNode> open = new List<PathNode>(); 
			open.Add(new PathNode()
			{
				pos = start, dir = dir,
				cost = 0,
				parent = null
			});
			List<PathNode> closed = new List<PathNode>();
			path = new Grid(maze.Width, maze.Height);
			while (open.Count > 0)
			{
				PathNode cur = open[0];
				open.RemoveAt(0);

				if (pathCost[cur.pos] > 0 && pathCost[cur.pos]+1001 < cur.cost)
				{
					continue;
				}
				pathCost[cur.pos] = cur.cost;
				if (maze[cur.pos] == 'E')
				{
					if (closed.Exists(c => c.cost < cur.cost))
					{
						break;
					}
					closed.Add(cur);
				}
				if (maze[cur.pos + cur.dir] == 'E')
				{
					closed.Add(new PathNode()
					{
						pos = cur.pos + cur.dir,
						dir = cur.dir,
						cost = cur.cost + 1,
						parent = cur
					});
					if (closed.Exists(c => c.cost < cur.cost))
					{
						break;
					}
				}
				if (maze[cur.pos + cur.dir] == '.')
				{
					open.Add(new PathNode()
					{
						pos = cur.pos + cur.dir,
						dir = cur.dir,
						cost = cur.cost + 1,
						parent = cur
					});
				}
				if (maze[cur.pos + new Vector2(-cur.dir.y,cur.dir.x)] == '.')
				{
					PathNode n = new PathNode()
					{
						pos = cur.pos + new Vector2(-cur.dir.y, cur.dir.x),
						dir = new Vector2(-cur.dir.y, cur.dir.x),
						cost = cur.cost + 1001,
						parent = cur
					};
					open.Add(n);
				}
				if (maze[cur.pos + new Vector2(cur.dir.y, -cur.dir.x)] == '.')
				{
					PathNode n = new PathNode()
					{
						pos = cur.pos + new Vector2(cur.dir.y, -cur.dir.x),
						dir = new Vector2(cur.dir.y, -cur.dir.x),
						cost = cur.cost + 1001,
						parent = cur
					};
					open.Add(n);
				}

				open.Sort((a,b) => a.cost.CompareTo(b.cost));
			}

			closed.Sort((a, b) => a.cost.CompareTo(b.cost));
			closed.RemoveAll(p => p.cost > closed[0].cost);

			
			foreach (PathNode e in closed)
			{
				PathNode p = e;
				while (p.parent != null)
				{
					path[p.pos] = 'O';
					p = p.parent;
				}
			}
			return closed[0].cost;
		}

		internal static long Part2(string input)
		{
			long result = 0l;
			Grid maze = new Grid(input, true);

			Vector2 start = maze.FindFirst('S');
			Vector2 end = maze.FindFirst('E');
			Vector2 dir = new Vector2(1, 0);

			Pathfind(start, end, dir, maze, out Grid path);
			for (int x = 0; x < path.Width; x++)
			{
				for (int y = 0; y < path.Height; y++)
				{
					if (path[x, y] == 'O')
						result++;
				}
			}
			return result+1;
		}
	}
}
