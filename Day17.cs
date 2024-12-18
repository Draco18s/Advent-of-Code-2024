using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace AdventofCode2024
{
	internal static class Day17
	{
		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			bool readingRegisters = true;
			Dictionary<string, long> registers = new Dictionary<string, long>();
			char r = 'A';
			int[] program = new int[0];
			foreach (string line in lines)
			{
				if (string.IsNullOrEmpty(line))
				{
					readingRegisters = false;
					continue;
				}

				if (readingRegisters)
				{
					int v = int.Parse(line.Split(": ")[1]);
					registers.Add(r+"", v);
					r = (char)(r + 1);
				}
				else
				{
					program = line.Split(": ")[1].Split(',').Select(int.Parse).ToArray();
				}
			}

			Console.WriteLine(RunProgram(registers, program));
			return result;
		}

		private static string RunProgram(Dictionary<string, long> registers, int[] program)
		{
			List<int> output = new List<int>();
			for (var i = 0; i+1 < program.Length;)
			{
				var op = program[i];
				var val = program[i+1];
				switch (op)
				{
					case 0:
						long cmb1 = Combo(val, registers);
						long div = (long)Math.Pow(2, cmb1);
						registers["A"] = (registers["A"] / div);
						break;
					case 1:
						registers["B"] = registers["B"] ^ val;
						break;
					case 2:
						registers["B"] = Combo(val, registers) % 8;
						break;
					case 3:
						if(registers["A"] == 0)
							break;
						else
						{
							i = val;
							continue;
						}
					case 4:
						registers["B"] = registers["B"] ^ registers["C"];
						break;
					case 5:
						output.Add((int)(Combo(val, registers) % 8));
						break;
					case 6:
						long cmb2 = Combo(val, registers);
						long div2 = (long)Math.Pow(2, cmb2);
						registers["B"] = (registers["A"] / div2);
						break;
					case 7:
						long cmb3 = Combo(val, registers);
						long div3 = (long)Math.Pow(2, cmb3);
						registers["C"] = (registers["A"] / div3);
						break;
					case 8:
					case 9:
						throw new Exception("Invalid program!");
						break;
				}

				i += 2;
			}

			return string.Join(",", output);
		}

		private static long Combo(int val, Dictionary<string, long> registers)
		{
			switch (val % 8)
			{
				case 0:
				case 1:
				case 2:
				case 3:
					return val % 8;
				case 4:
					return registers["A"];
				case 5:
					return registers["B"];
				case 6:
					return registers["C"];
				default:
					throw new Exception("Invalid program!");
			}

		}

		public static IEnumerable<long> Range(long start, long count, long step)
		{
			long max = start + count - 1;
			//if (count < 0 || max > Int32.MaxValue) throw Error.ArgumentOutOfRange("count");
			return RangeIterator(start, count, step);
		}

		private static IEnumerable<long> RangeIterator(long start, long count, long step)
		{
			for (long i = 0; i < count; i+= step) yield return start + i;
		}
		
		internal static long Part2(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			bool readingRegisters = true;
			Dictionary<string, long> registers = new Dictionary<string, long>();
			char r = 'A';
			int[] program = new int[0];
			string sprogram = "";
			foreach (string line in lines)
			{
				if (string.IsNullOrEmpty(line))
				{
					readingRegisters = false;
					continue;
				}

				if (readingRegisters)
				{
					int v = int.Parse(line.Split(": ")[1]);
					registers.Add(r + "", v);
					r = (char)(r + 1);
				}
				else
				{
					sprogram = line.Split(": ")[1];
					program = sprogram.Split(',').Select(int.Parse).ToArray();
				}
			}
			//result = 2503159376460815L; // too high
			//result =  251359562775567L; // too high

			/*************************************
			//  locating pattern bits by hand   //
			**************************************
			
			0111 0111 0001 1000 1001 1000 0000 0001 0011 1100 0000 1111 // first 6 digits
			0111 0111 0001 1000 1001 1001 0000 0001 0011 1100 0000 1111

			0111 0111 0001 0010 0011 0011 0011 1101 0011 1100 0000 1111 // first 7

			0111 0111 0001 0010 0011 0011 0000 0101 0011 1100 0000 1111 // first 8
			0111 0111 0001 0010 0011 0011 0001 1101 0011 1100 0000 1111
			0111 0111 0001 0010 0011 0011 0111 1101 0011 1100 0000 1111

			0111 0111 0001 0100 0000 0101 0111 0001 0011 1100 0000 1111 // first 9
			0111 0111 0001 0100 0000 0101 0111 0011 0011 1100 0000 1111
			0111 0111 0001 0100 0000 0101 1111 0001 0011 1100 0000 1111
			0111 0111 0001 0100 0000 0101 1111 0011 0011 1100 0000 1111
			0111 0111 0001 0100 0000 0101 1111 1001 0011 1100 0000 1111
			0111 0111 0001 0110 0010 0001 1101 1001 0011 1100 0000 1111
			0111 0111 0001 0110 0100 0111 0000 0101 0011 1100 0000 1111
			0111 0111 0001 0110 0100 0111 0001 1101 0011 1100 0000 1111
			0111 0111 0001 0110 0100 0111 0011 1101 0011 1100 0000 1111
			0111 0111 0001 1000 1010 0001 1101 1001 0011 1100 0000 1111

			???? ???? ???? ???? 0011 0101 0011 0001 0011 1100 0000 1111 // suspected hot bits
			1010 1101 0010 0111 0101 1111 0000 0101 0011 1100 0000 1111 // SOLUTION
			???? ???? ???? ???? ???? ???? ???? 1100 1100 0011 1111 0000 // suspected low bits

			0111 0110 1111 1111 0111 1111 1100 0111 0111 0001 1110 1111 // first 0
			0111 0111 0001 0010 0010 0110 1110 0101 0011 1100 0010 1111
			0111 0111 0001 0010 0010 0110 1110 0101 0011 1100 0011 1111

			0111 0111 0001 0010 0011 0010 1110 0101 1011 1101 0000 1111 // first 1
			0111 0111 0001 1000 1010 0001 1101 1001 0011 1101 0000 1111
			0111 0111 0001 1000 1001 1000 0000 0001 0011 1101 0000 1111
			0111 0111 0001 1000 1001 0000 1100 1011 0011 1101 0000 1111
			0111 0111 0001 0010 1100 1011 0110 1101 0011 1111 0000 1111
			0111 0111 0001 0011 1111 1001 0010 0101 1011 1101 0000 1111
			0111 0111 0001 0011 1111 1001 0010 0101 1011 1111 0000 1111
			0111 0111 0001 0011 1111 1001 0010 0101 1111 1101 0000 1111
			0111 0111 0001 0011 1111 1001 0010 0101 1111 1111 0000 1111
			0111 0111 0001 0011 1111 1001 0010 0111 0011 1111 0000 1111

			0111 0111 0001 1000 1001 1000 0000 0001 0011 1110 0000 1111 // first 2

			0111 0111 0001 1000 1001 1000 0000 0001 1011 1100 0000 1111 // first 3
			*/

			/***********************************************
			//  used to work out the majority of the LSB  //
			************************************************
			/*while (result < 251359562775567L)
			{
				Console.WriteLine($"Testing {result}");
				ResetRegisters(registers, result);
				if (RunProgram2(registers, program))
					break;

				result += 65536L; // avoid mutating known LSB
				result += 786432L;

				//result = result & ~(135054320);
				//result |= 18005179407L;
				result = result & ~(50160);
				result |= 20003855L;
			}*/
			/***********************************************
			//  used to work out the majority of the MSB  //
			************************************************
			/*var results1 = Range(190384609165327L, 190384609165327L + (1L<<20), 1L<<9).AsParallel();
			var results2 = results1.Select(a =>
			{
				ResetRegisters(registers, a);
				string str = RunProgram(registers, program);
				return (a, str);
			});
			var results3 = results2.First(a => a.Item2.Length == 31 && a.Item2.StartsWith("2,4,1,2,7,5,4,3"));
			
			Console.WriteLine("---");
			Console.WriteLine(sprogram);
			result = results3.a;*/
			/*result = 190384609508367L;
			ResetRegisters(registers, result);
			Console.WriteLine(RunProgram(registers, program));

			Console.WriteLine(sprogram);*/
			/*//ResetRegisters(registers, result);
			//Console.WriteLine(RunProgram(registers, program));

			/***************************************
			//  more by-hand to link MSB and LSB  //
			****************************************

			result += 1L << 16;
			result += 1L << 18;
			result += 1L << 19;
			result += 1L << 22;

			//2,4,1,2,7,5,5,4,3,1,1,7,5,5,3,0 just 19
			//2,4,1,2,7,5,5,4,3,1,1,7,5,5,3,0 both
			//2,4,1,2,7,5,2,4,3,1,1,7,5,5,3,0 just 18
			//result += 1L << 39;
			//result += 1L << 40;
			//result += 1L << 41;
			//result += 1L << 42;//y
			//result += 1L << 43;//y
			//result += 1L << 44;
			//result += 1L << 45;//y
			//result += 1L << 46;
			//result += 1L << 47;//n

			//result += 1L << 24;// 0000000000000000111100000000000
			//result += 1L << 25;// 0000000011111111000000000000000
			  result += 1L << 26;// 00001111000011110000000011111110001
			  result += 1L << 27;// 00110011001100110011001100111101010
			  result += 1L << 28;// 01010101010101010101010101011011100
			//result += 1L << 29;// 00000000000000000000111111111111111
			//result += 1L << 30;//                             1111111
			// # incorrect digits   32332232323233234343333433332443344
			//                                 ^  ^


			// 1010 1101 0011 0001 0000 1100 1001 1110 0101 0100 0000 1111 < 190426176705551L 2,4,1,5,3,5,3,0,5,0,2,3,7,5,3,0
			// 1010 1101 0010 0111 0110 0111 1111 0000 0000 0000 0000 1111 < 190384759111695L 2,4,5,5,5,5,6,3,1,2,1,7,5,5,3,0
			// 1010 1101 0010 0111 0100 1011 1010 1100 0011 1100 0000 1111 < 190384284908559L 2,4,1,2,7,3,3,4,3,1,1,7,5,5,3,0
			// 1010 1101 0010 0111 0101 1111 0000 0000 0000 0000 0000 1111 < 190384609165327L 2,4,5,5,5,5,5,3,0,3,1,7,5,5,3,0
			//                                                                                2,4,1,2,7,5,4,3,0,3,1,7,5,5,3,0 // GOAL

			ResetRegisters(registers, result);
			Console.WriteLine(RunProgram(registers, program));

			Console.WriteLine(" ");*/

			result = 190384609508367L;

			return result;
		}

		static List<int> part2output = new List<int>();
		private static bool RunProgram2(Dictionary<string, long> registers, int[] program)
		{
			part2output.Clear();

			for (var i = 0; i < program.Length;)
			{
				var op = program[i];
				var val = program[i + 1];
				switch (op)
				{
					case 0:
						long cmb1 = Combo(val, registers);
						long div = (long)Math.Pow(2, cmb1);
						registers["A"] = (registers["A"] / div);
						break;
					case 1:
						registers["B"] = registers["B"] ^ val;
						break;
					case 2:
						registers["B"] = Combo(val, registers) % 8;
						break;
					case 3:
						if (registers["A"] == 0)
							break;
						i = val;
						continue;
					case 4:
						registers["B"] = registers["B"] ^ registers["C"];
						break;
					case 5:
						int v = (int)(Combo(val, registers) % 8);
						if (part2output.Count < program.Length && program[part2output.Count] != v)
							return false;
						part2output.Add(v);
						break;
					case 6:
						long cmb2 = Combo(val, registers);
						long div2 = (long)Math.Pow(2, cmb2);
						registers["B"] = (registers["A"] / div2);
						break;
					case 7:
						long cmb3 = Combo(val, registers);
						long div3 = (long)Math.Pow(2, cmb3);
						registers["C"] = (registers["A"] / div3);
						break;
					case 8:
					case 9:
						throw new Exception("Invalid program!");
				}

				i += 2;
			}

			if (part2output.Count != program.Length)
				return false;

			string str = string.Join(",", part2output);
			Console.WriteLine(str);
			return string.Join(",", program).Equals(str);
		}

		private static void ResetRegisters(Dictionary<string, long> registers, long i)
		{
			registers["A"] = i;
			registers["B"] = 0;
			registers["C"] = 0;
		}
	}
}
