using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Draco18s.AoCLib;

namespace AdventofCode2024
{
	internal static class Day6
	{
		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			Grid room = new Grid(input, true);
			Vector2 position = FindGuard(room);
			Vector2 face = new Vector2(0, -1);
			while (GuardMove(room, ref result, ref position, ref face)) ;
			Console.WriteLine(room);
			return result;
		}

		private static Vector2 FindGuard(Grid room)
		{
			for (int x = 0; x < room.Width; x++)
			{
				for (int y = 0; y < room.Height; y++)
				{
					if (room[x, y] == '^')
						return new Vector2(x, y);
				}
			}
			return new Vector2(-1, -1);
		}

		private static bool GuardMove(Grid room, ref long result, ref Vector2 pos, ref Vector2 facing)
		{
			if (!room.IsInside(pos + facing))
				return false;

			if (room[pos] != 'X') result++;
			room[pos] = 'X';
			if (room[pos + facing] == '#')
			{
				facing = new Vector2(-facing.y, facing.x);
			}
			else
			{
				pos += facing;
				if(!room.IsInside(pos+facing))
				{
					result++;
					room[pos] = 'X';
					return false;
				}
			}

			return true;
		}

		internal static long Part2(string input)
		{
			/*************************************************************************
			// This functions and spits out the right answer after about 10 minutes //
			// And is the code that was used for the actual submission. I was not   //
			// confident that I could revise for faster run time more quickly than  //
			// to just waiting for it to finish.                                    //
			*************************************************************************/

			/*string[] lines = input.Split('\n');
			long result = 0l;
			long ignore = 0l;
			Grid room = new Grid(input, true);
			Vector2 position = FindGuard(room);
			Vector2 face = new Vector2(0, -1);
			while (GuardMove(room, ref ignore, ref position, ref face)) ;

			Grid room2 = new Grid(input, true);
			position = FindGuard(room2);
			face = new Vector2(0, -1);
			room[position] = '.';
			Vector2 obst = GetObstaclePos(room);
			int progress = 0;
			do
			{
				if (obst.y == 9)
					;
				room2[obst] = '#';
				List<(Vector2 p, Vector2 f)> visits = new List<(Vector2 p, Vector2 f)>();
				while (GuardMove(room2, ref ignore, ref position, ref face))
				{
					(Vector2 p, Vector2 f) atNow = (position, face);
					if (visits.Contains(atNow))
					{
						//Console.WriteLine($"{obst} loops");
						result++;
						break;
					}
					visits.Add(atNow);
				}
				room[obst] = '.';
				obst = GetObstaclePos(room);
				room2 = new Grid(input, true);
				position = FindGuard(room2);
				face = new Vector2(0, -1);
				progress++;
				Console.WriteLine($"{progress}/4976");
			} while (obst.x >= 0);

			return result;*/

			/**********************************************
			// This solution runs significantly faster.  //
			**********************************************/

			long result = 0l;
			long ignore = 0l;
			Grid room = new Grid(input, true);
			Vector2 position = FindGuard(room);
			Vector2 face = new Vector2(0, -1);

			for (int x = 0; x < room.Width; x++)
			{
				for (int y = 0; y < room.Height; y++)
				{
					if (room[x, y] != '#')
					{
						room[new Vector2(x, y)] = 0;
					}
				}
			}

			while (GuardMove2(room, ref position, ref face)) ;
			room[position] = 4;
			List<Vector2> allValidLocations = new List<Vector2>();
			for (int x = 0; x < room.Width; x++)
			{
				for (int y = 0; y < room.Height; y++)
				{
					if (room[x, y] != '#' && room[x,y] != 0 && room[x, y] != '.')
					{
						allValidLocations.Add(new Vector2(x, y));
					}
				}
			}
			int progress = 0;
			foreach (Vector2 obst in allValidLocations)
			{
				progress++;
				foreach (Vector2 qfacing in FindFacings(room[obst]))
				{
					Vector2 query = obst - qfacing;
					Vector2 nfacing = new Vector2(-qfacing.y, qfacing.x);

					HashSet<(Vector2 p, Vector2 f)> visits = new HashSet<(Vector2 p, Vector2 f)>();
					Grid room2 = new Grid(input, true);
					room2[obst] = '#';
					while (GuardMove(room2, ref ignore, ref query, ref nfacing))
					{
						(Vector2 p, Vector2 f) atNow = (query, nfacing);
						if (!visits.Add(atNow))
						{
							//Console.WriteLine($"{obst} loops");
							result++;
							break;
						}
					}
				}
				Console.WriteLine($"{progress}/4976");
			}

			return result;
		}

		private static bool GuardMove2(Grid room, ref Vector2 pos, ref Vector2 facing)
		{
			if (!room.IsInside(pos + facing))
				return false;
			if(room[pos] == 0)
				room[pos] = GetFacingVal(facing);
			if (room[pos + facing] == '#')
			{
				facing = new Vector2(-facing.y, facing.x);
			}
			else
			{
				pos += facing;
				if (!room.IsInside(pos + facing))
				{
					if (room[pos] == 0)
						room[pos] = GetFacingVal(facing);
					return false;
				}
			}

			return true;
		}

		private static IEnumerable<Vector2> FindFacings(int i)
		{
			if ((i & 1) > 0) yield return new Vector2(-1, 0);
			if ((i & 2) > 0) yield return new Vector2(1, 0);
			if ((i & 4) > 0) yield return new Vector2(0, -1);
			if ((i & 8) > 0) yield return new Vector2(0, 1);
		}

		private static int GetFacingVal(Vector2 facing)
		{
			if (facing.x == -1) return 1;
			if (facing.x ==  1) return 2;
			if (facing.y == -1) return 4;
			if (facing.y ==  1) return 8;

			return 0;
		}

		/* Implementation 1 method not used for implementation 2 */
		private static Vector2 GetObstaclePos(Grid room)
		{
			for (int x = 0; x < room.Width; x++)
			{
				for (int y = 0; y < room.Height; y++)
				{
					if (room[x, y] == 'X')
						return new Vector2(x, y);
				}
			}
			return new Vector2(-1, -1);
		}
	}
}
