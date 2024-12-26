using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;

namespace AdventofCode2024
{
	internal static class Day25
	{
		public class Key
		{
			public int[] pins;

			public override string ToString()
			{
				return string.Join(',', pins);
			}
		}
		public class Lock
		{
			public int[] pins;

			public override string ToString()
			{
				return string.Join(',', pins);
			}
		}
		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			List < Key > keys = new List<Key>();
			List <Lock> locks = new List<Lock>();
			for (var i = 0; i + 6 < lines.Length; i += 8)
			{
				var l1 = lines[i+0];
				var l2 = lines[i+1];
				var l3 = lines[i+2];
				var l4 = lines[i+3];
				var l5 = lines[i+4];
				var l6 = lines[i+5];
				var l7 = lines[i+6];
				//+7 is empty
				List<int> heights = new List<int>();
				if (l1[0] == '#')
				{
					for (int c = 0; c < 5; c++)
					{
						int c1 = (l1[c] == '#' ? (l2[c] == '#' ? (l3[c] == '#' ? (l4[c] == '#' ? (l5[c] == '#' ? (l6[c] == '#' ? 6 : 5) : 4) : 3) : 2) : 1) : 0);
						heights.Add(c1-1);
					}

					locks.Add(
						new Lock()
						{
							pins = heights.ToArray()
						});
				}

				if (l1[0] == '.')
				{
					for (int c = 0; c< 5; c++)
					{
						int c1 = (l7[c] == '#' ? (l6[c] == '#' ? (l5[c] == '#' ? (l4[c] == '#' ? (l3[c] == '#' ? (l2[c] == '#' ? 6 : 5) : 4) : 3) : 2) : 1) : 0);
						heights.Add(c1-1);
					}
					keys.Add(
						new Key()
						{
							pins = heights.ToArray()
						});
				}
			}

			foreach (var k in keys)
			{
				foreach (var l in locks)
				{
					if (CheckFit(k, l))
						result++;
				}
			}
			return result;
		}

		private static bool CheckFit(Key key, Lock _lock)
		{
			for (int c = 0; c < 5; c++)
			{
				if (key.pins[c] + _lock.pins[c] > 5)
				{
					return false;
				}
			}

			return true;
		}

		internal static long Part2(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			foreach (string line in lines)
			{

			}
			return result;
		}
	}
}
