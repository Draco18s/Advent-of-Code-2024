using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Draco18s.AoCLib;

namespace AdventofCode2024
{
	internal static class Day15
	{
		internal static long Part1(string input)
		{
			string[] lines = input.Split("\n\n");
			long result = 0l;
			Grid warehouse = new Grid(lines[0], true);
			string moves = lines[1];
			Vector2 robot = warehouse.FindFirst('@');
			warehouse[robot] = '.';

			int i = 0;
			int l = moves.Length;
			foreach (char c in moves)
			{
				robot = MoveRobot(warehouse, robot, c);
				//Console.WriteLine(warehouse);
				//Console.WriteLine($"{++i}/{l}");
			}

			for (int y = warehouse.MinY; y < warehouse.MaxY; y++)
			{
				for (int x = warehouse.MinX; x < warehouse.MaxX; x++)
				{
					if (warehouse[x, y] == 'O')
					{
						long gps = 100 * y + x;
						result += gps;
					}
				}
			}

			return result;
		}

		private static Vector2 MoveRobot(Grid warehouse, Vector2 robot, char c)
		{
			if (c == '\n') return robot;
			Dictionary<char, Vector2> moves = new Dictionary<char, Vector2>();
			moves.Add('<', new Vector2(-1, 0));
			moves.Add('>', new Vector2(1, 0));
			moves.Add('^', new Vector2(0, -1));
			moves.Add('v', new Vector2(0, 1));

			Vector2 dir = moves[c];
			Vector2 check = new Vector2(robot.x, robot.y);
			bool canMove = true;
			while (canMove)
			{
				check += dir;
				if (warehouse.IsInside(check))
				{
					if (warehouse[check] == '#')
					{
						canMove = false;
					}
					if (warehouse[check] == '.')
						break;
				}
				else
				{
					canMove = false;
				}
			}

			if (canMove)
				robot = ShiftBoxes(warehouse, robot, dir, check);

			return robot;
		}

		private static Vector2 ShiftBoxes(Grid warehouse, Vector2 robot, Vector2 dir, Vector2 upTo)
		{
			Vector2 pos = robot + dir;
			bool foundbox = warehouse[pos] == 'O';
			warehouse[pos] = '.';
			while (true)
			{
				if (warehouse[pos] == 'O')
					foundbox = true;
				if (pos == upTo)
					break;
				pos += dir;
			}

			if (foundbox)
				warehouse[pos] = 'O';
			robot += dir;
			return robot;
		}

		private static Vector2 MoveRobot2(Grid warehouse, Vector2 robot, char c)
		{
			if (c == '\n') return robot;
			Dictionary<char, Vector2> moves = new Dictionary<char, Vector2>();
			moves.Add('<', new Vector2(-1, 0));
			moves.Add('>', new Vector2(1, 0));
			moves.Add('^', new Vector2(0, -1));
			moves.Add('v', new Vector2(0, 1));

			List<Vector2> checkPos = new List<Vector2>();
			List<Vector2> skipPos = new List<Vector2>();
			List<Vector2> newCheckPos = new List<Vector2>();
			Vector2 dir = moves[c];
			checkPos.Add(robot + dir);
			bool canMove = true;
			while (canMove)
			{
				foreach (Vector2 check in checkPos)
				{
					if (!warehouse.IsInside(check)) continue;
					Vector2 ncheck = check + dir;
					if (warehouse[check] == '#')
					{
						canMove = false;
						break;
					}

					if (dir.x == 0)
					{
						if (warehouse[check] == '[' && !skipPos.Contains(check + new Vector2(1, 0)))
						{
							newCheckPos.Add(check + new Vector2(1, 0));
							skipPos.Add(check);
						}

						else if (warehouse[check] == ']' && !skipPos.Contains(check + new Vector2(-1, 0)))
						{
							newCheckPos.Add(check + new Vector2(-1, 0));
							skipPos.Add(check);
						}
					}
					if (warehouse[check] == '.')
					{
						;
					}
					else
					{
						newCheckPos.Add(ncheck);
					}
				}

				if (newCheckPos.Count == 0)
					break;
				checkPos.Clear();
				checkPos.AddRange(newCheckPos.Distinct());
				newCheckPos.Clear();
			}

			if (canMove)
				robot = ShiftBoxes2(warehouse, robot, dir);

			return robot;
		}

		private static Vector2 ShiftBoxes2(Grid warehouse, Vector2 robot, Vector2 dir)
		{
			Vector2 pos = robot + dir;
			if (!warehouse.IsInside(pos)) return robot;
			bool foundbox = warehouse[pos] == '[' || warehouse[pos] == ']';

			List<Vector2> checkPos = new List<Vector2>();
			List<Vector2> newCheckPos = new List<Vector2>();

			List<(Vector2 s, Vector2 e, char c)> boxMoves = new List<(Vector2, Vector2, char)>();

			checkPos.Add(robot);
			for (int j = 1; true; j++)
			{
				bool foundAllEmpty = true;
				foreach (Vector2 check in checkPos)
				{
					Vector2 ncheck = check + dir;
					if (warehouse[ncheck] == '#')
					{
						foundAllEmpty = false;
						if (dir.y == 0)
						{
							foundAllEmpty = true;
							newCheckPos.Clear();
							boxMoves.Clear();
							break;
						}
					}
					else
					{
						if (dir.x == 0 && warehouse[ncheck] == '[')
						{
							foundAllEmpty = false;
							newCheckPos.Add(ncheck);
							newCheckPos.Add(ncheck + new Vector2(1, 0));
						}
						else if (dir.x == 0 && warehouse[ncheck] == ']')
						{
							foundAllEmpty = false;
							newCheckPos.Add(ncheck);
							newCheckPos.Add(ncheck + new Vector2(-1, 0));
						}
						else if (warehouse[ncheck] == '.')
						{
						}
						else
						{
							foundAllEmpty = false;
							newCheckPos.Add(ncheck);
							boxMoves.Add((ncheck, ncheck + dir, (char)warehouse[ncheck]));
						}

						if (warehouse[check] == '[')
						{
							foundAllEmpty = false;
							boxMoves.Add((check, check + dir, '['));
						}

						if (warehouse[check] == ']')
						{
							foundAllEmpty = false;
							boxMoves.Add((check, check + dir, ']'));
						}
					}
				}

				checkPos.Clear();
				checkPos.AddRange(newCheckPos.Distinct());
				newCheckPos.Clear();
				if (foundAllEmpty)
					break;
			}

			foreach (var m in boxMoves)
			{
				warehouse[m.e] = m.c;
				if (boxMoves.All(p => p.e != m.s))
					warehouse[m.s] = '.';
			}

			robot += dir;
			return robot;
		}

		internal static long Part2(string input)
		{
			string[] lines = input.Split("\n\n");
			long result = 0l;
			string layout = "";
			foreach (char c in lines[0])
			{
				switch (c)
				{
					case '.':
					case '#':
						layout += $"{c}{c}";
						break;
					case 'O':
						layout += "[]";
						break;
					case '@':
						layout += "@.";
						break;
					case '\n':
						layout += c;
						break;
				}
			}

			Grid warehouse = new Grid(layout, true);
			Console.WriteLine(warehouse);
			string moves = lines[1];
			Vector2 robot = warehouse.FindFirst('@');
			warehouse[robot] = '.';

			int i = 0;
			int l = moves.Length;
			foreach (char c in moves)
			{
				if (c == '\n') continue;
				i++;
				//warehouse[robot] = c;
				//Console.WriteLine($"{i}/{l}");
				//Console.WriteLine(warehouse);
				//warehouse[robot] = '.';
				//Thread.Sleep(100);
				robot = MoveRobot2(warehouse, robot, c);
				//warehouse[robot] = c;
				//Console.WriteLine(warehouse);
				//warehouse[robot] = '.';

				//Thread.Sleep(100);
			}

			for (int y = warehouse.MinY; y < warehouse.MaxY; y++)
			{
				for (int x = warehouse.MinX; x < warehouse.MaxX; x++)
				{
					if (warehouse[x, y] == '[')
					{
						long gps = 100 * y + x;
						result += gps;
					}
				}
			}
			warehouse[robot] = '@';
			Console.WriteLine(warehouse);
			warehouse[robot] = '.';
			return result;
		}
	}
}
