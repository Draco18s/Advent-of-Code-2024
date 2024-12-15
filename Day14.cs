using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Draco18s.AoCLib;

namespace AdventofCode2024
{
	internal static class Day14
	{
		private class Robot
		{
			public Vector2 position;
			private Vector2 velocity;
			public Robot(Vector2 pos, Vector2 vel)
			{
				position = pos;
				velocity = vel;
			}

			public void Move(int w, int h)
			{
				position += velocity;
				position = new Vector2((w + position.x)% w, (h + position.y) % h);
			}

			public override string ToString()
			{
				return position.ToString();
			}
		}
		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			Grid room = new Grid(101, 103, 0, 0);
			Regex parse = new Regex(@"p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)");
			List<Robot> robots = new List<Robot>();
			foreach (string line in lines)
			{
				MatchCollection mat = parse.Matches(line);
				var a = int.Parse(mat[0].Groups[1].Value);
				var b = int.Parse(mat[0].Groups[2].Value);
				var c = int.Parse(mat[0].Groups[3].Value);
				var d = int.Parse(mat[0].Groups[4].Value);
				robots.Add(new Robot(new Vector2(a, b), new Vector2(c, d)));
			}

			for (int i = 0; i < 100; i++)
			{
				MoveRobots(robots, room.Width, room.Height);
			}

			int q1 = robots.Count(r => r.position.x < room.Width / 2 && r.position.y < room.Height / 2);
			int q2 = robots.Count(r => r.position.x > room.Width / 2 && r.position.y < room.Height / 2);
			int q3 = robots.Count(r => r.position.x < room.Width / 2 && r.position.y > room.Height / 2);
			int q4 = robots.Count(r => r.position.x > room.Width / 2 && r.position.y > room.Height / 2);
			return q1*q2*q3*q4;
		}

		private static void MoveRobots(List<Robot> robots, int w, int h)
		{
			foreach (Robot r in robots)
			{
				r.Move(w, h);
			}
		}

		internal static long Part2(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			Grid room = new Grid(101, 103, 0, 0);
			Regex parse = new Regex(@"p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)");
			List<Robot> robots = new List<Robot>();
			foreach (string line in lines)
			{
				MatchCollection mat = parse.Matches(line);
				var a = int.Parse(mat[0].Groups[1].Value);
				var b = int.Parse(mat[0].Groups[2].Value);
				var c = int.Parse(mat[0].Groups[3].Value);
				var d = int.Parse(mat[0].Groups[4].Value);
				robots.Add(new Robot(new Vector2(a, b), new Vector2(c, d)));
			}

			long totalDistance = 0;
			long lastDistance = 0;
			long bestDistance = long.MaxValue;
			int bestTick = 0;
			Vector2 mid = new Vector2(room.Width / 2, room.Height / 2);
			for (int i = 1; i<10000; i++)
			{
				MoveRobots(robots, room.Width, room.Height);
				/*************************************************************
				/  Actually used this "total distance to the center" method  /
				*************************************************************/
				/*lastDistance = totalDistance;
				totalDistance = 0;
				foreach (Robot r in robots)
				{
					totalDistance += Math.Abs((mid - r.position).x) + Math.Abs((mid - r.position).y);
				}

				if (totalDistance < bestDistance)
				{
					bestDistance = totalDistance;
					bestTick = i;
					room = new Grid(101, 103, 0, 0);
					foreach (Robot r in robots)
					{
						room[r.position] = '#' - 32;
						totalDistance += Math.Abs((mid - r.position).x) + Math.Abs((mid - r.position).y);
					}
					// print state for visual confirmation
					Console.WriteLine(room.ToString("char"));
					Console.WriteLine(i);
				}*/
				/*******************************************************************
				/  But using the "safety factor" from part 1 has the same result.  /
				/  Thought it was interesting and keeping around                   /
				*******************************************************************/
				int q1 = robots.Count(r => r.position.x < room.Width / 2 && r.position.y < room.Height / 2);
				int q2 = robots.Count(r => r.position.x > room.Width / 2 && r.position.y < room.Height / 2);
				int q3 = robots.Count(r => r.position.x < room.Width / 2 && r.position.y > room.Height / 2);
				int q4 = robots.Count(r => r.position.x > room.Width / 2 && r.position.y > room.Height / 2);
				totalDistance = q1 * q2 * q3 * q4;
				if (totalDistance < bestDistance)
				{
					bestDistance = totalDistance;
					bestTick = i;
				}
			}
			return bestTick;
		}
	}
}
