using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Draco18s.AoCLib;

namespace AdventofCode2024
{
	internal static class Day4
	{
		internal static long Part1(string input)
		{
			long result = 0l;
			Grid grid = new Grid(input, true);

			for (int y = grid.MinY; y < grid.MaxY; y++)
			{
				for (int x = grid.MinX; x < grid.MaxX; x++)
				{
					if (x + 3 < grid.MaxX && 
					    grid[x, y] == 'X' && grid[x + 1, y] == 'M' && grid[x + 2, y] == 'A' && grid[x + 3, y] == 'S')
					{
						result++;
					}

					if (x + 3 < grid.MaxX && 
					    grid[x, y] == 'S' && grid[x + 1, y] == 'A' && grid[x + 2, y] == 'M' && grid[x + 3, y] == 'X')
					{
						result++;
					}

					if (y + 3 < grid.MaxY && 
					    grid[x, y] == 'X' && grid[x, y + 1] == 'M' && grid[x, y + 2] == 'A' && grid[x, y + 3] == 'S')
					{
						result++;
					}

					if (y + 3 < grid.MaxY && 
					    grid[x, y] == 'S' && grid[x, y + 1] == 'A' && grid[x, y + 2] == 'M' && grid[x, y + 3] == 'X')
					{
						result++;
					}

					if (y + 3 < grid.MaxY && x + 3 < grid.MaxX &&
					    grid[x, y] == 'X' && grid[x + 1, y + 1] == 'M' && grid[x + 2, y + 2] == 'A' && grid[x + 3, y + 3] == 'S')
					{
						result++;
					}

					if (y + 3 < grid.MaxY && x >= 3 &&
					    grid[x, y] == 'X' && grid[x - 1, y + 1] == 'M' && grid[x - 2, y + 2] == 'A' && grid[x - 3, y + 3] == 'S')
					{
						result++;
					}

					if (y >= 3 && x + 3 < grid.MaxX &&
					    grid[x, y] == 'X' && grid[x + 1, y - 1] == 'M' && grid[x + 2, y - 2] == 'A' && grid[x + 3, y - 3] == 'S')
					{
						result++;
					}

					if (y >= 3 && x >= 3 &&
					    grid[x, y] == 'X' && grid[x - 1, y - 1] == 'M' && grid[x - 2, y - 2] == 'A' && grid[x - 3, y - 3] == 'S')
					{
						result++;
					}
				}
			}

			return result;
		}

		internal static long Part2(string input)
		{
			long result = 0l;
			Grid grid = new Grid(input, true);

			for (int y = grid.MinY+1; y+1 < grid.MaxY; y++)
			{
				for (int x = grid.MinX+1; x+1 < grid.MaxX; x++)
				{
					if (CheckGridAt(grid, x, y))
					{
						result++;
					}
				}
			}
			/* Grid Feature Locator version. Now with fixed implementation!
			
			List<Vector2> x = grid.LocateFeature(v =>
			{
				return CheckGridAt(v, 1, 1);
			}, new List<Vector2>()
			{
				new Vector2(0, 0),
				new Vector2(-1, -1),
				new Vector2(-1, 1),
				new Vector2(1, -1),
				new Vector2(1, 1),
			}, Grid.returnZero);
			
			return x.Count*/

			return result;
		}

		private static bool CheckGridAt(Grid grid, int x, int y)
		{
			if (grid[x, y] != 'A') return false;
			if (grid[x + 1, y + 1] != 'M' && grid[x + 1, y + 1] != 'S') return false;
			if (grid[x + 1, y - 1] != 'M' && grid[x + 1, y - 1] != 'S') return false;
			if (grid[x - 1, y + 1] != 'M' && grid[x - 1, y + 1] != 'S') return false;
			if (grid[x - 1, y - 1] != 'M' && grid[x - 1, y - 1] != 'S') return false;

			if (grid[x - 1, y - 1] == 'M' && grid[x + 1, y + 1] == 'M') return false;
			if (grid[x - 1, y - 1] == 'S' && grid[x + 1, y + 1] == 'S') return false;

			if (grid[x + 1, y - 1] == 'M' && grid[x - 1, y + 1] == 'M') return false;
			if (grid[x + 1, y - 1] == 'S' && grid[x - 1, y + 1] == 'S') return false;

			return true;
		}

		private static bool CheckGridAt(int[,] grid, int x, int y)
		{
			if (grid[x, y] != 'A') return false;
			if (grid[x + 1, y + 1] != 'M' && grid[x + 1, y + 1] != 'S') return false;
			if (grid[x + 1, y - 1] != 'M' && grid[x + 1, y - 1] != 'S') return false;
			if (grid[x - 1, y + 1] != 'M' && grid[x - 1, y + 1] != 'S') return false;
			if (grid[x - 1, y - 1] != 'M' && grid[x - 1, y - 1] != 'S') return false;

			if (grid[x - 1, y - 1] == 'M' && grid[x + 1, y + 1] == 'M') return false;
			if (grid[x - 1, y - 1] == 'S' && grid[x + 1, y + 1] == 'S') return false;

			if (grid[x + 1, y - 1] == 'M' && grid[x - 1, y + 1] == 'M') return false;
			if (grid[x + 1, y - 1] == 'S' && grid[x - 1, y + 1] == 'S') return false;

			return true;
		}
	}
}
