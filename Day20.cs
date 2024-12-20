using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Draco18s.AoCLib;

namespace AdventofCode2024
{
	internal static class Day20
	{
		internal static long Part1(string input)
		{
			return 0;
			string[] lines = input.Split('\n');
			Grid track = new Grid(input, true);
			Grid track2 = new Grid(input, true);
			Vector2 start = track.FindFirst('S');
			Vector2 end = track.FindFirst('E');
			track[start] = '.';
			track[end] = '.';
			long result = 0;
			Grid.PathNode p = track.FindShortestPath(start, end, v => track[v] == '.').FirstOrDefault();
			Grid.PathNode endNode = p;
			while (p != null)
			{
				track[p.pos] = 'O';
				track2[p.pos] = p.cost;
				p = p.parent;
			}

			Console.WriteLine(track.ToString("char+0"));
			p = endNode;
			while (p != null)
			{
				foreach(Vector2 dir in Grid.FACING)
				{
					if (track.IsInside(p.pos + dir * 2) && track[p.pos + dir] == '#' && track[p.pos + dir * 2] == 'O')
					{
						long v = Math.Abs(track2[p.pos] - track2[p.pos + dir * 2]) - 2;
						//long v = GetShortcutDist(input, p, p.pos + dir * 2);
						if (v >= 100)
							result++;
					}
				}
				p = p.parent;
			} 

			return result/2;
		}

		internal static long Part2(string input)
		{
			string[] lines = input.Split('\n');
			Grid track = new Grid(input, true);
			Grid track2 = new Grid(input, true);
			Vector2 start = track.FindFirst('S');
			Vector2 end = track.FindFirst('E');


			for (int x = 0; x < track.Width; x++)
			{
				for (int y = 0; y < track.Width; y++)
				{
					track2[x, y] = int.MinValue;
				}
			}

			track[start] = '.';
			track[end] = '.';
			long result = 0;
			Grid.PathNode p = track.FindShortestPath(start, end, v => track[v] == '.').FirstOrDefault();
			Grid.PathNode endNode = p;
			while (p != null)
			{
				track[p.pos] = ' ';
				track2[p.pos] = p.cost;
				p = p.parent;
			}
			p = endNode;
			
			while (p != null)
			{
				for (int x = 1; x < track.Width - 1; x++)
				{
					for (int y = 1; y < track.Width - 1; y++)
					{
						int a = track2[x, y];
						int b = track2[p.pos];
						if (a == int.MinValue || b == int.MinValue) continue;

						int v = Math.Abs(a - b) - 2;
						int dist = Math.Abs(x - p.pos.x) + Math.Abs(y - p.pos.y)-2;
						if (Math.Abs(x - p.pos.x) + Math.Abs(y - p.pos.y) <= 20 && (v-dist) >= 100 )
						{
							result++;
						}
					}
				}

				p = p.parent;
			}
			return result/2;
		}
	}
}
