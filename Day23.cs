using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AdventofCode2024
{

	internal static class Day23
	{
		class Computer
		{
			public string ID;
			public HashSet<string> connections;

			public Computer(string id)
			{
				ID = id;
				connections = new HashSet<string>();
			}
		}

		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			Dictionary<string, Computer> lookup = new Dictionary<string, Computer>();
			foreach (string line in lines)
			{
				string[] comps = line.Split('-');
				if (lookup.ContainsKey(comps[0]))
				{
					lookup[comps[0]].connections.Add(comps[1]);
				}
				else
				{
					Computer cmp = new Computer(comps[0]);
					cmp.connections.Add(comps[1]);
					lookup.Add(comps[0], cmp);
				}
				if (lookup.ContainsKey(comps[1]))
				{
					lookup[comps[1]].connections.Add(comps[0]);
				}
				else
				{
					Computer cmp = new Computer(comps[1]);
					cmp.connections.Add(comps[0]);
					lookup.Add(comps[1], cmp);
				}
			}

			HashSet< (string,string,string)> found = new HashSet<(string, string, string)>();

			foreach (Computer cm in lookup.Values)
			{
				if(cm.ID[0] != 't') continue;
				foreach (var cn in lookup[cm.ID].connections)
				{
					if (lookup[cn].connections.Any(x => cm.connections.Contains(x)))
					{
						foreach (var vvv in lookup[cn].connections.Where(x => cm.connections.Contains(x)))
						{
							List<string> loop = new List<string>() { vvv, cm.ID, cn };
							loop.Sort();
							(string, string, string) v = (loop[0], loop[1], loop[2]);
							if (!found.Contains(v))
							{
								found.Add(v);
								result++;
							}
						}
					}
				}
			}

			return result;
		}

		internal static long Part2(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			Dictionary<string, Computer> lookup = new Dictionary<string, Computer>();
			foreach (string line in lines)
			{
				string[] comps = line.Split('-');
				if (lookup.ContainsKey(comps[0]))
				{
					lookup[comps[0]].connections.Add(comps[1]);
				}
				else
				{
					Computer cmp = new Computer(comps[0]);
					cmp.connections.Add(comps[1]);
					lookup.Add(comps[0], cmp);
				}
				if (lookup.ContainsKey(comps[1]))
				{
					lookup[comps[1]].connections.Add(comps[0]);
				}
				else
				{
					Computer cmp = new Computer(comps[1]);
					cmp.connections.Add(comps[0]);
					lookup.Add(comps[1], cmp);
				}
			}

			HashSet<string> fullNets = new HashSet<string>();

			foreach (Computer cm in lookup.Values)
			{
				//assumption that worked on my input but not all
				//if (cm.ID[0] != 't') continue;
				foreach (string cn in cm.connections)
				{
					HashSet<string> found = new HashSet<string>();
					found.Add(cm.ID);
					if (lookup[cn].connections.Any(x => cm.connections.Contains(x)))
					{
						found.Add(cn);
						IEnumerable<string> search = lookup[cn].connections.Where(x => cm.connections.Contains(x));
						
						foreach (var test in search)
						{
							bool passed = true;
							foreach (var ex in found)
							{
								if (!lookup[ex].connections.Contains(test))
									passed = false;
							}

							if (passed)
								found.Add(test);
						}
					}
					fullNets.Add(string.Join(",", found.OrderBy(x => x)));
				}

			}

			IOrderedEnumerable<string> ord = fullNets.OrderByDescending(pw => pw.Length);
			Console.WriteLine(ord.First());
			return 0;
		}
	}
}
