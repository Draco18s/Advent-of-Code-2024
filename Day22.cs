using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AdventofCode2024
{
	internal static class Day22
	{
		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			foreach (string line in lines)
			{
				long sec = long.Parse(line);
				for (int i = 0; i < 2000; i++)
				{
					sec = Mix(sec, sec * 64);
					sec = Prune(sec);
					sec = Mix(sec, sec / 32);
					sec = Prune(sec);
					sec = Mix(sec, sec * 2048);
					sec = Prune(sec);
				}

				//Console.WriteLine($"{line}: {sec}");
				result += sec;
			}
			return result;
		}

		private static bool CheckBuy((int a, int b, int c, int d) s, int[] priceChange)
		{
			return priceChange[0] == s.a && priceChange[1] == s.b && priceChange[2] == s.c && priceChange[3] == s.d;
		}

		private static long Mix(long sec, long val)
		{
			return sec = sec ^ val;

		}

		private static long Prune(long sec)
		{
			return sec % 16777216;
		}
		internal static long Part2(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;

			Dictionary<(int, int, int, int), int> sequence = new();
			Dictionary<(int, int, int, int), bool> alreadyCounted = new();
			(int, int, int, int) sequ = (2, 0, -1, 2);
			int monk = 0;
			int totBuys = 0;
			foreach (string line in lines)
			{
				monk++;
				alreadyCounted = new();
				long sec = long.Parse(line);
				int[] priceChange = new int[4];
				int[] priceHistory = new int[5];
				int lastPrice = (int)(sec % 10);
				priceChange[0] = int.MinValue;
				priceChange[1] = int.MinValue;
				priceChange[2] = int.MinValue;
				priceChange[3] = int.MinValue;

				bool bought = false;
				for (int i = 0; i < 2000; i++)
				{
					sec = Mix(sec, sec * 64);
					sec = Prune(sec);
					sec = Mix(sec, sec / 32);
					sec = Prune(sec);
					sec = Mix(sec, sec * 2048);
					sec = Prune(sec);

					int price = (int)(sec % 10);

					priceChange[0] = priceChange[1];
					priceChange[1] = priceChange[2];
					priceChange[2] = priceChange[3];
					priceChange[3] = price - lastPrice;

					priceHistory[0] = priceHistory[1];
					priceHistory[1] = priceHistory[2];
					priceHistory[2] = priceHistory[3];
					priceHistory[3] = priceHistory[4];
					priceHistory[4] = price;

					(int, int, int, int) hist = (priceChange[0], priceChange[1], priceChange[2], priceChange[3]);

					lastPrice = price;
					if (priceChange[0] < -50)
						continue;

					if (!alreadyCounted.TryAdd(hist, true))
						continue;
					if (!sequence.TryAdd(hist, price))
						sequence[hist] += price;

					/*if (!bought && CheckBuy(sequ, priceChange))
					{
						bought = true;
						result += price;
						totBuys++;
						Console.WriteLine($"Monkey {monk} bought at {price}");
					}*/
				}
			}
			var desc = sequence.OrderByDescending(kvp => kvp.Value);
			var p = desc.First();
			return p.Value; //sequence.Max(kvp => kvp.Value);
		}
	}
}
