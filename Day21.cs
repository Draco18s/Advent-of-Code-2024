using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using Draco18s.AoCLib;
using RestSharp.Deserializers;

namespace AdventofCode2024
{
	internal static class Day21
	{
		private static Dictionary<Vector2, char> lookup = new Dictionary<Vector2, char>();
		internal static long Part1(string input)
		{
			lookup.Add(Vector2.LEFT, '<');
			lookup.Add(Vector2.RIGHT, '>');
			lookup.Add(Vector2.UP, '^');
			lookup.Add(Vector2.DOWN, 'v');
			string[] lines = input.Split('\n');
			long result = 0l;
			List<(string code, long seq)> lengths = new List<(string, long)>();
			Grid codePad = new Grid("789\n456\n123\n#0A", true);
			Grid controlPad = new Grid("#^A\n<v>", true);
			Regex test = new Regex(@"A[<>^v]{2,}A");
			foreach (string l in lines)
			{
				if (l == "085A")
					;
				string s = ComputeSequence(l, codePad);
				s = ComputeSequence(s, controlPad);
				s = ComputeSequence(s, controlPad);
				s = ComputeSequence(s, controlPad);
				s = ComputeSequence(s, controlPad);
				s = ComputeSequence(s, controlPad);

				lengths.Add((l, s.Length));
				//break;
			}

			result = lengths.Select(pair =>
			{
				int a = int.Parse(pair.code.Replace("A", ""));
				long b = pair.seq;
				return a * b;
			}).Sum();

			return result;
		}

		private static string ComputeSequence(string s, Grid pad, bool startAtA = true)
		{
			StringBuilder sb = new StringBuilder();
			Vector2 pos = startAtA ? pad.FindFirst('A') : pad.FindFirst(s[0]);
			Vector2 facing = Vector2.ZERO;
			foreach (char c in s)
			{
				if (c == 'A')
					;
				StringBuilder b2 = new StringBuilder();
				Vector2 dest = pad.FindFirst(c);
				if (dest == pos)
				{
					b2.Append('A');
					sb.Append(b2.ToString());
					continue;
				}
				var apath = pad.FindShortestPath(pos, dest, facing, (_, b) => (pad[b] != '#'), (p, n, d, x) =>
				{
					if (d == x)
						return 0;
					
					if (pad.Height == 4)
					{
						if (p.x == 0 && x.y > 0)
							return 5;

						if (dest.x != p.x && dest.y > p.y && x.y != 1)
						{
							return 100;
						}
						if (d.y == 0)
						{
							return 1;
						}
						return 10;
					}

					// need these two biases
					// to avoid A<vA and Av<A (and similar sequences)
					// having the same cost, as they don't expand similarly
					if (x.x == -1 && d.x != -1)
					{
						return 15;
					}

					if ((d.x != x.x || d.y != x.y) && (p.x != dest.x && p.y != dest.y))
						return 15;

					return 10;
				});
				var path = apath.First();
				facing = path.dir;
				pos = path.pos;
				while (path != null && path.parent != null)
				{
					char d = GetDirection(path);
					b2.Insert(0, d);
					path = path.parent;
				}
				//Console.WriteLine(seq.ToString("char+0"));
				b2.Append('A');
				sb.Append(b2.ToString());
			}

			return sb.ToString();
		}

		private static char GetDirection(Grid.PathNode path)
		{
			return lookup[(path.pos - path.parent.pos)];
		}

		internal static long Part2(string input)
		{
			long result = 0l;
			string[] lines = input.Split('\n');
			Grid codePad = new Grid("789\n456\n123\n#0A", true);
			Grid controlPad = new Grid("#^A\n<v>", true);
			ConcurrentDictionary<(string, int), long> finalLenths = new ConcurrentDictionary<(string, int), long>();
			
			int depth = 25;
			foreach (string l in lines)
			{
				long count = 0;
				string s = ComputeSequence(l, codePad);
				
				var splt = s.Split('A').Select(p => $"A{p}A").Reverse().Skip(1).Reverse().ToArray();
				foreach (string sq in splt)
				{
					long cl = GetTotalLength(sq, depth, finalLenths, controlPad);
					count += cl;
				}
				result += count * int.Parse(l.Replace("A", ""));
			}
			return result;
		}

		private static long GetTotalLength(string seq, int depth, ConcurrentDictionary<(string, int), long> finalLenths, Grid pad)
		{
			if (finalLenths.TryGetValue((seq, depth), out long len)) return len;

			if (depth == 0) return seq.Length - 1;
			if (depth == 1)
			{
				long l = ComputeSequence(seq, pad).Length-1;
				finalLenths.TryAdd((seq, 1), l);
				return l;
			}

			string s = ComputeSequence(seq, pad);
			finalLenths.TryAdd((seq, 1), s.Length-1); //saves a recursion

			// split into memoizable chunks
			var splt = s.Split('A').Select(p => $"A{p}A").Reverse().Skip(1).Reverse().ToArray();
			long count = -1; //ignore leading A, doing it once here rather than doing a count-- at the end
			foreach (string sq in splt)
			{
				long cl = GetTotalLength(sq, depth-1, finalLenths, pad);
				count += cl;
			}
			
			finalLenths.TryAdd((seq, depth), count);

			return count;
		}
	}
}
