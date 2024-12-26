using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading;

namespace AdventofCode2024
{
	internal static class Day24
	{
		private enum BoolOp
		{
			AND,OR,XOR,NOT,NAND,NOR
		}

		private class Gate
		{
			public BoolOp op;
			public string Input1;
			public string Input2;
			public string Output;
			public string OrigOutName;
		}

		internal static long Part1(string input)
		{
			return 0;
			string[] lines = input.Split('\n');
			long result = 0l;
			bool first = true;

			Dictionary<string, long> wires = new Dictionary<string, long>();
			List<Gate> gates = new List<Gate>();

			foreach (string line in lines)
			{
				if (string.IsNullOrEmpty(line))
				{
					first = false;
					continue;
				}

				if (first)
				{
					var w = line.Split(": ");
					wires.Add(w[0],int.Parse(w[1]));
				}
				else
				{
					var g = line.Split(" ");
					gates.Add(
						new Gate()
						{
							Input1 = g[0],
							Input2 = g[2],
							op = Enum.Parse<BoolOp>(g[1]),
							Output = g[4]
						});
					if(!wires.ContainsKey(g[0]))
						wires.Add(g[0], -1);
					if (!wires.ContainsKey(g[2]))
						wires.Add(g[2], -1);
					if (!wires.ContainsKey(g[4]))
						wires.Add(g[4], -1);
				}
			}

			return Simulate(gates, wires, result);
		}

		private static long Simulate(List<Gate> gates, Dictionary<string, long> wires, long result)
		{
			bool shouldExit = false;
			DateTime start = DateTime.Now;
			while (!shouldExit)
			{
				shouldExit = true;
				foreach (Gate g in gates)
				{
					if (wires[g.Input1] != -1 && wires[g.Input2] != -1)
					{
						var o = GetValue(g, wires[g.Input1], wires[g.Input2]);
						if(o != wires[g.Output])
							shouldExit = false;
						wires[g.Output] = o;
					}
					else
					{
						shouldExit = false;
					}
				}

				if ((DateTime.Now - start).TotalMilliseconds > 20)
				{
					return -1;
				}
			}
			var outwires = wires.Where(kvp => kvp.Key.StartsWith('z')).OrderByDescending(kvp => kvp.Key);

			foreach (Gate g in gates)
			{
				if (wires[g.Input1] != -1 && wires[g.Input2] != -1)
				{
					wires[g.Output] = GetValue(g, wires[g.Input1], wires[g.Input2]);
				}
			}

			foreach (KeyValuePair<string, long> v in outwires)
			{
				result = result << 1;
				result |= (uint)(v.Value == 1?1:0);
			}

			return result;
		}

		private static int GetValue(Gate gate, long a, long b)
		{
			switch (gate.op)
			{
				case BoolOp.AND:
					return a == 1 && b == 1 ? 1 : 0;
				case BoolOp.OR:
					return a == 1 || b == 1 ? 1 : 0;
				case BoolOp.NAND:
					return a == 1 && b == 1 ? 0 : 1;
				case BoolOp.NOR:
					return a == 1 || b == 1 ? 0 : 1;
				case BoolOp.XOR:
					return (a == 1)^(b == 1) ? 1 : 0;
			}

			return -1;
		}

		internal static long Part2(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			bool first = true;

			Dictionary<string, long> wires = new Dictionary<string, long>();
			Dictionary<string,Gate> gates = new Dictionary<string,Gate>();

			foreach (string line in lines)
			{
				if (string.IsNullOrEmpty(line))
				{
					first = false;
					continue;
				}

				if (first)
				{
					var w = line.Split(": ");
					wires.Add(w[0], int.Parse(w[1]));
				}
				else
				{
					var g = line.Split(" ");
					gates.Add(g[4],
						new Gate()
						{
							Input1 = g[0],
							Input2 = g[2],
							op = Enum.Parse<BoolOp>(g[1]),
							Output = g[4]
						});
					if (!wires.ContainsKey(g[0]))
						wires.Add(g[0], -1);
					if (!wires.ContainsKey(g[2]))
						wires.Add(g[2], -1);
					if (!wires.ContainsKey(g[4]))
						wires.Add(g[4], -1);
				}
			}

			long A = 0;
			long B = 0;
			Random rand = new Random();

			List<string> suspects = new List<string>();

			var outputBits = new Dictionary<string, Gate>();
			var xorInputs = new Dictionary<string, Gate>();
			var andInputs = new Dictionary<string, Gate>();

			//hnd,bks,nrn  -> XOR with one used output (and not ->z)
			//hnd,bks,tdv  -> XOR with one used output (and not: x^y)
			//jfs,tjp      -> AND with two used outputs
			//z23,z09      -> outputs that are AND gates
			//z16,z45      -> outputs that are OR gates
			//TODO

			(gates["tdv"].Output, gates["z16"].Output) = (gates["z16"].Output, gates["tdv"].Output);
			(gates["bks"].Output, gates["z23"].Output) = (gates["z23"].Output, gates["bks"].Output);
			//(gates["ghr"].Output, gates["nrn"].Output) = (gates["nrn"].Output, gates["ghr"].Output);
			(gates["hnd"].Output, gates["z09"].Output) = (gates["z09"].Output, gates["hnd"].Output);
			//(gates["dqt"].Output, gates["rqf"].Output) = (gates["rqf"].Output, gates["dqt"].Output); NO
			//(gates["nrn"].Output, gates["mdk"].Output) = (gates["mdk"].Output, gates["nrn"].Output);
			//(gates["nrn"].Output, gates["ghr"].Output) = (gates["ghr"].Output, gates["nrn"].Output); no
			//(gates["nrn"].Output, gates["z37"].Output) = (gates["z37"].Output, gates["nrn"].Output); NO
			//(gates["hnd"].Output, gates["rqf"].Output) = (gates["rqf"].Output, gates["hnd"].Output); NO
			//(gates["vqv"].Output, gates["rqf"].Output) = (gates["rqf"].Output, gates["vqv"].Output);
			//(gates["z01"].Output, gates["wsb"].Output) = (gates["wsb"].Output, gates["z01"].Output);
			(gates["tjp"].Output, gates["nrn"].Output) = (gates["nrn"].Output, gates["tjp"].Output);

			(gates["tdv"], gates["z16"]) = (gates["z16"], gates["tdv"]);
			(gates["bks"], gates["z23"]) = (gates["z23"], gates["bks"]);
			//(gates["ghr"], gates["nrn"]) = (gates["nrn"], gates["ghr"]);
			(gates["hnd"], gates["z09"]) = (gates["z09"], gates["hnd"]);
			//(gates["dqt"], gates["rqf"]) = (gates["rqf"], gates["dqt"]); NO
			//(gates["nrn"], gates["mdk"]) = (gates["mdk"], gates["nrn"]);
			//(gates["nrn"], gates["ghr"]) = (gates["ghr"], gates["nrn"]); no
			//(gates["nrn"], gates["z37"]) = (gates["z37"], gates["nrn"]); NO
			//(gates["hnd"], gates["rqf"]) = (gates["rqf"], gates["hnd"]); NO
			//(gates["vqv"], gates["z09"]) = (gates["z09"], gates["vqv"]);
			//(gates["z01"], gates["wsb"]) = (gates["wsb"], gates["z01"]);
			(gates["tjp"], gates["nrn"]) = (gates["nrn"], gates["tjp"]);

			// PAIR FOUND? nrn/ghr: 
			// jfs,dqt
			// PAIR FOUND? hnd/z09
			// PAIR FOUND? vqv/rqf no
			// nrn swapped with...
			// c36 ghr
			// p36 mdk
			// dqt,rqf
			// tjp/nrn

			Gate nrn = gates["nrn"];

			foreach (var g in gates.Where(G => G.Key[0] == 'z'))
			{
				outputBits.Add(g.Key,g.Value);
				if (g.Value.op != BoolOp.XOR)
				{
					suspects.Add(g.Key);
				}
			}

			foreach (var g in gates.Where(G => G.Value.op == BoolOp.XOR && (G.Value.Input1[0] == 'x' || G.Value.Input1[0] == 'y')).ToArray())
			{
				xorInputs.Add(g.Key, g.Value);
				var v1 = int.Parse(g.Value.Input1.Substring(1));
				var v2 = int.Parse(g.Value.Input2.Substring(1));
				RenameWire(g.Key, $"s{v1:00}", gates, wires); //partial sum bit
				if (v1 != v2)
				{
					suspects.Add(g.Value.Output);

					if (g.Key[0] == 'z')
						;
				}
			}

			foreach (var g in gates.Where(G => G.Value.op == BoolOp.AND && (G.Value.Input1[0] == 'x' || G.Value.Input1[0] == 'y')).ToArray())
			{
				andInputs.Add(g.Key, g.Value);
				var v1 = int.Parse(g.Value.Input1.Substring(1));
				var v2 = int.Parse(g.Value.Input2.Substring(1));
				RenameWire(g.Key, $"p{v1:00}", gates, wires);
				if (g.Key[0] == 'z')
					suspects.Add(g.Value.Output); //partial carry bit (step 1)
				if (v1 != v2)
				{
					suspects.Add(g.Value.Output);
				}
			}

			foreach (var g in gates.Where(G => G.Value.op == BoolOp.AND && (G.Value.Output[0] != 'p' || !char.IsDigit(G.Value.Output[1]))).ToArray())
			{
				Gate outZ = gates.FirstOrDefault(f => f.Value.Input1 == g.Key || f.Value.Input2 == g.Key).Value;

				if (outZ != null && int.TryParse(g.Value.Input1.Substring(1), out var v1) && int.TryParse(g.Value.Input2.Substring(1), out var v2))
				{
					RenameWire(g.Key, $"C{v1:00}", gates, wires); //partial carry bit (step 2)
					if (v1 != v2)
					{
						suspects.Add(g.Value.Input2);
					}
					continue;
				}

				else if (g.Key[0] == 'z')
					suspects.Add(g.Key);
				else
					suspects.Add(g.Key);
			}

			foreach (var g in gates.Where(G => G.Value.op == BoolOp.OR).ToArray())
			{
				if (int.TryParse(g.Value.Input1.Substring(1), out var v1) | int.TryParse(g.Value.Input2.Substring(1), out var v2))
				{
					var vv = Math.Max(v1, v2);
					RenameWire(g.Key, $"c{vv:00}", gates, wires);

					if(!char.IsDigit(g.Value.Input1[1]))
					{
						char inWire = g.Value.Input2[0] == 'p' ? 'C' : 'p';
						string nn = $"{inWire}{vv:00}";
						if (wires.ContainsKey(nn))
						{
							suspects.Add(g.Value.Input1);
						}
						else
							RenameWire(g.Value.Input1, nn, gates, wires);
					}
					if (!char.IsDigit(g.Value.Input2[1]))
					{
						char inWire = g.Value.Input1[0] == 'p' ? 'C' : 'p';
						string nn = $"{inWire}{vv:00}";
						if (wires.ContainsKey(nn))
						{
							suspects.Add(g.Value.Input2);
						}
						else 
							RenameWire(g.Value.Input2, nn, gates, wires);
					}
				}

				else if (g.Key[0] == 'z')
					suspects.Add(g.Key);
				else
					suspects.Add(g.Key);
			}

			var gatesOrd = gates.OrderBy(k => k.Key);
			/*var jfs = gates.Where(g => g.Value.OrigOutName.Equals("jfs"));
			var tjp = gates.Where(g => g.Value.OrigOutName.Equals("tjp"));
			var hnd = gates.Where(g => g.Value.OrigOutName.Equals("hnd"));
			var bks = gates.Where(g => g.Value.OrigOutName.Equals("bks"));
			var tdv = gates.Where(g => g.Value.OrigOutName.Equals("tdv"));
			var nrn = gates.Where(g => g.Value.OrigOutName.Equals("nrn"));
			;*/

			// bks: c22 XOR s23 -> z23
			// dch: qhm OR bks => c23 [correct]
			// kpb: p37 AND c36 
			// qhm: c22 AND s23 -> C23
			// tdv: c15 XOR s16 -> z16

			//x23 AND y23 -> z23
			//ncj OR pwk -> z16

			//PAIR FOUND: bks/z23
			//PAIR FOUND: tdv/z16
			//PAIR FOUND: nrn/ghr

			//suspects.Clear();
			//chv,wkc,fqf,jtf,sfv,jbb,rnw,pjv,dch,qcq,qjr,qdp,pbb
			//suspects.AddRange(new string[] { "bks","dch","hnd","jfs","kpb","nrn","tdv","tjp","z23","z09","z16","z45", });
			/*suspects = suspects.Distinct().ToList();
			suspects.RemoveAll(w => !gates.ContainsKey(w));*/
			;
			StringBuilder sb = new StringBuilder("Missing: ");
			for (int i = 0; i < 45; i++)
			{
				if (!gates.ContainsKey($"z{i:00}"))
					sb.Append($"z{i:00},");
				if (!gates.ContainsKey($"s{i:00}"))
					sb.Append($"s{i:00},");
				if (!gates.ContainsKey($"p{i:00}"))
					sb.Append($"p{i:00},");
				if (!gates.ContainsKey($"c{i:00}"))
					sb.Append($"c{i:00},");
				if (!gates.ContainsKey($"C{i:00}"))
					sb.Append($"C{i:00},");
			}
			Console.WriteLine(sb.ToString());

			//var s37 = gates.First(g => g.Value.Output == "s37");
			//var c36 = gates.First(g => g.Value.Output == "c36");
			//var C37 = gates.First(g => (g.Value.Input1 == "s37" && g.Value.Input2 == "c36") || (g.Value.Input2 == "s37" && g.Value.Input1 == "c36"));
			;
			;// nrn AND ghr / ghr AND nrn
			;// p37 AND c36
			//var dch = gates.First(g => g.Value.OrigOutName == "dch");
			//var kbp = gates.First(g => g.Value.Output == "kbp" || g.Value.OrigOutName == "kbp");
			//var ggh = gates.First(g => g.Value.OrigOutName == "ggh" || g.Value.Output == "ggh");
			//var nrn = gates.First(g => g.Value.OrigOutName == "nrn" || g.Value.Output == "nrn");
			;
			;
			var anyNonNumberedGates = gates.Where(g => !char.IsDigit(g.Key[1]));
			var anyNonNumberedWires = wires.Where(g => !char.IsDigit(g.Key[1]));
			
			;
			;
			;
			var wiresOrd = wires.OrderBy(x => x.Key);
			;
			var z00 = gates.First(g => g.Value.OrigOutName == "z00");
			RenameWire(z00.Key, "z00", gates, wires);
			var z45 = gates.First(g => g.Value.OrigOutName == "z45");
			RenameWire(z45.Key, "z45", gates, wires);
			;
			;
			// PAIR FOUND? kbp/nrn: unclear
			// PAIR FOUND? kbp/ghr: no

			/*suspects = suspects.Distinct().ToList();
			suspects.RemoveAll(w => !gates.ContainsKey(w));

			foreach (var sus in suspects.ToArray())
			{
				if (sus[0] == 'z') continue;
				var g = gates[sus];
				if (g.Input1[0] == 's' && g.Input2[0] == 'c')
				{
					suspects.Remove(sus);
					suspects.Add(g.Input1);
				}
				else if (g.Input1[0] == 'c' && g.Input2[0] == 's')
				{
					suspects.Remove(sus);
					suspects.Add(g.Input2);
				}
				else if ((g.Input1[0] == 'c' && g.Input2[0] == 'p') || (g.Input1[0] == 'p' && g.Input2[0] == 'c') && char.IsDigit(g.Input1[1]) && char.IsDigit(g.Input2[1]))
				{
					int v1 = int.Parse(g.Input1.Substring(1));
					int v2 = int.Parse(g.Input2.Substring(1));
					string newName = $"C{v1:00}";
					if (Math.Abs(v1-v2) == 1)
					{
						suspects.Remove(sus);
						RenameWire(sus, newName, gates, wires);
					}
					else
					{
						if (g.op != BoolOp.XOR)
						{
							suspects.Remove(sus);
						}
						else
						{
							;
						} //oh god
					}
				}
				else if (g.op == BoolOp.OR)
				{
					suspects.Remove(sus);
				}
				else
				{
					;
				}
			}*/
			//suspects = suspects.Distinct().ToList();
			//suspects.Sort();

			suspects.Clear();
			suspects.AddRange(gates.Keys);
			suspects = suspects.Distinct().ToList();
			suspects.Sort();

			List<(string In1, string In2)> susPairs = new List<(string In1, string In2)>();
			int x = 0;
			int y = 0;
			int z = 0;
			foreach (string sus1 in suspects)
			{
				x++;
				foreach (string sus2 in suspects.Skip(x))
				{
					if(sus1.CompareTo(sus2) >= 0) continue;
					//if(sus1.Equals(sus2)) continue;
					susPairs.Add((sus1, sus2));
				}
			}

			x = 0;
			HashSet<string> resultsFound = new HashSet<string>();
			bool wasFailureEver = false;
			int tests = 1000000;
			int testsOuter = 44;
			A = 1L;//(long)(rand.NextDouble() * (long.MaxValue >> 8));//68719476736
			B = 1L;//(long)(rand.NextDouble() * (long.MaxValue >> 8));
			while (testsOuter-->0)
			{
				//foreach ((string In1, string In2) pair1 in susPairs)
				{
					{
						SetWiresToValues(wires, A, B);
						long res = Simulate(gates.Values.ToList(), wires, result);
						long resLong = A + B;
						string ab = Convert.ToString(A + B, 2);
						string rs = Convert.ToString(res, 2);
						string resStr = rs.Substring(Math.Max(rs.Length - 45, 0)); //.Substring(8);//.Substring(3);
						string exp = ab.Substring(Math.Max(ab.Length - 44, 0)); //.Substring(8);

						if (resStr != exp)
						{
							;
							return 1111;
							;
							;
							;
							;

							// nrn swapped with z37
							// kpb ->
							// ggh?
							// c37 : C37 * p37
							// C37 : c32 * s37

						}

						A *= 2;
						B *= 2;
						/*Console.WriteLine($"{x}/{susPairs.Count}");
						x++;

						(gates[pair1.In1].Output, gates[pair1.In2].Output) = (gates[pair1.In2].Output, gates[pair1.In1].Output);
						(gates[pair1.In1], gates[pair1.In2]) = (gates[pair1.In2], gates[pair1.In1]);
						y = x - 1;
						//foreach ((string In1, string In2) pair2 in susPairs.Skip(x))
						{
							wasFailureEver = false;
							y++;
							z = y - 1;
							//Console.WriteLine($"{x}:{y}/{susPairs.Count}");
							//if (pair1.In1 == pair2.In1 || pair1.In1 == pair2.In2) continue;
							//if (pair1.In2 == pair2.In1 || pair1.In1 == pair2.In2) continue;

							//foreach ((string In1, string In2) pair3 in susPairs.Skip(x + y))
							{
								z++;
								//if (pair1.In1 == pair3.In1 || pair1.In1 == pair3.In2) continue;
								//if (pair1.In2 == pair3.In1 || pair1.In1 == pair3.In2) continue;
								//if (pair2.In1 == pair3.In1 || pair2.In1 == pair3.In2) continue;
								//if (pair2.In2 == pair3.In1 || pair2.In1 == pair3.In2) continue;

								//(gates[pair3.In1].Output, gates[pair3.In2].Output) = (gates[pair3.In2].Output, gates[pair3.In1].Output);
								//foreach ((string In1, string In2) pair4 in susPairs.Skip(x + y + z))

								List<string> set = new List<string>();
								set.Add(pair1.In1);
								set.Add(pair1.In2);
								//set.Add(pair2.In1);
								//set.Add(pair2.In2);
								/*set.Add(pair3.In1);
								set.Add(pair3.In2);
								set.Add(pair4.In1);
								set.Add(pair4.In2);*/
						/*if (set.Distinct().Count() != 2) continue;
						tests = 1000;
						while (tests-- > 0)
						{
							if(tests%50==0)
								Console.Write('.');
							A = (long)(rand.NextDouble() * (long.MaxValue >> 8)); //68719476736
							B = (long)(rand.NextDouble() * (long.MaxValue >> 8));

							//(gates[pair4.In1].Output, gates[pair4.In2].Output) = (gates[pair4.In2].Output, gates[pair4.In1].Output);

							SetWiresToValues(wires, A, B);
							long res = Simulate(gates.Values.ToList(), wires, result);
							if (res == -1)
							{
								continue;
							}

							//res ^= M;
							long resLong = A + B;
							string ab = Convert.ToString(A + B, 2);
							string rs = Convert.ToString(res, 2);
							string resStr = rs.Substring(Math.Max(rs.Length - 45, 0)); //.Substring(8);//.Substring(3);
							string exp = ab.Substring(Math.Max(ab.Length - 45, 0)); //.Substring(8);

							if (resStr == exp)
							{
								//Thread.Sleep(1000);
							}
							else
							{
								wasFailureEver = true;
								break;
							}

							//(gates[pair4.In1].Output, gates[pair4.In2].Output) = (gates[pair4.In2].Output, gates[pair4.In1].Output);
						}

						set = set.Select(g => string.IsNullOrEmpty(gates[g].OrigOutName) ? gates[g].Output : gates[g].OrigOutName).ToList();

						string str = string.Join(',', set.OrderBy(s => s));
						if (!wasFailureEver && !resultsFound.Contains(str))
						{
							resultsFound.Add(str);
							Console.WriteLine($"{str}");
						}
						Console.Write('\n');

						//(gates[pair3.In1].Output, gates[pair3.In2].Output) = (gates[pair3.In2].Output, gates[pair3.In1].Output);
					}
					//Console.Write('\n');

					//(gates[pair2.In1].Output, gates[pair2.In2].Output) = (gates[pair2.In2].Output, gates[pair2.In1].Output);
				}

				(gates[pair1.In1].Output, gates[pair1.In2].Output) = (gates[pair1.In2].Output, gates[pair1.In1].Output);
				(gates[pair1.In1], gates[pair1.In2]) = (gates[pair1.In2], gates[pair1.In1]);*/
					}
				}
			}

			return 0;
		}

		private static void RenameWire(string origName, string newName, Dictionary<string, Gate> gates, Dictionary<string, long> wires)
		{
			if (origName.Equals("x00"))
				;
			if (origName[0] == 'x' || origName[0] == 'y')
				return;
			long val = wires[origName];
			wires.Remove(origName);
			wires.Add(newName, val);
			Gate g0 = gates[origName];
			if(string.IsNullOrEmpty(g0.OrigOutName))
				g0.OrigOutName = g0.Output;
			g0.Output = newName;
			gates.Remove(origName);
			gates.Add(newName,g0);

			foreach (var g1 in gates.Where(g => g.Value.Input1 == origName))
			{
				g1.Value.Input1 = newName;
			}
			foreach (var g2 in gates.Where(g => g.Value.Input2 == origName))
			{
				g2.Value.Input2 = newName;
			}
		}

		private static void SetWiresToValues(Dictionary<string, long> wires, long x, long y)
		{
			var inwiresX = wires.Where(kvp => kvp.Key.StartsWith('x')).OrderBy(kvp => kvp.Key);

			foreach (KeyValuePair<string, long> v in inwiresX)
			{
				wires[v.Key] = (x % 2);
				x = x >> 1;
			}
			var inwiresY = wires.Where(kvp => kvp.Key.StartsWith('y')).OrderBy(kvp => kvp.Key);

			foreach (KeyValuePair<string, long> v in inwiresY)
			{
				wires[v.Key] = (y % 2);
				y = y >> 1;
			}
		}
	}
}
