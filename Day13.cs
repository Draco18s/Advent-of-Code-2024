using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AdventofCode2024
{
	internal static class Day13
	{
		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			for (int index = 0; index < lines.Length; index+=4)
			{
				string buttonA = lines[index];
				string buttonB = lines[index+1];
				string prize = lines[index+2];
				var res = ComputeMinPresses(buttonA, buttonB, prize);
				if (res < int.MaxValue)
					result += res;
			}

			return result;
		}

		private static Regex buttonReg = new Regex(@"Button .: X\+(\d+), Y\+(\d+)");
		private static Regex prizeReg = new Regex(@"Prize: X=(\d+), Y=(\d+)");

		private static long ComputeMinPresses(string buttonA, string buttonB, string prize)
		{
			(int ax, int ay) = ParseButton(buttonA);
			(int bx, int by) = ParseButton(buttonB);
			MatchCollection col = prizeReg.Matches(prize);
			(int px, int py) = (int.Parse(col[0].Groups[1].Value), int.Parse(col[0].Groups[2].Value));

			int best = int.MaxValue;

			for (int a = 0; a <= 100; a++)
			{
				int X = a * ax;
				int Y = a * ay;
				if((px - X)%bx != 0 || (py - Y) % by != 0) continue;

				int b1 = (px - X) / bx;
				int b2 = (py - Y) / by;
				if(b1 != b2) continue;
				if(b1 > 100) continue;

				if (3*a + b1 < best)
				{
					best = 3*a + b1;
				}
			}

			return best;
		}

		private static long ComputeMinPresses2(string buttonA, string buttonB, string prize)
		{
			(long ax, long ay) = ParseButton(buttonA);
			(long bx, long by) = ParseButton(buttonB);
			MatchCollection col = prizeReg.Matches(prize);
			(long px, long py) = (10000000000000 + int.Parse(col[0].Groups[1].Value), 10000000000000 + int.Parse(col[0].Groups[2].Value));

			long best = long.MaxValue;

			double slopeA = (double)ay / ax;
			double slopeB = (double)by / bx;

			double c1 = -slopeA * px + py;
			double c2 = -slopeB;

			double ix = (c1 - c2) / (slopeB - slopeA);
			double iy = c2 + slopeB * ix;

			long tbx = (long)Math.Round(ix / bx);
			long tby = (long)Math.Round(iy / by);

			long tax = (long)Math.Round((px - ix) / ax);
			long tay = (long)Math.Round((py - iy) / ay);

			if (tbx != tby || tax != tay) return best;

			if (tax * ax + tbx * bx != px || tay * ay + tby * by != py) return best;

			best = 3 * tax + tbx;

			return best;
		}

		private static (int x, int y) ParseButton(string button)
		{
			MatchCollection col = buttonReg.Matches(button);
			
			return (int.Parse(col[0].Groups[1].Value), int.Parse(col[0].Groups[2].Value));
		}

		internal static long Part2(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			for (int index = 0; index < lines.Length; index += 4)
			{
				string buttonA = lines[index];
				string buttonB = lines[index + 1];
				string prize = lines[index + 2];
				long res = ComputeMinPresses2(buttonA, buttonB, prize);
				if (res < long.MaxValue)
					result += res;
				else 
					;
			}

			return result;
			return result;
		}
	}
}
