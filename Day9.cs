using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AdventofCode2024
{
	internal static class Day9
	{
		public class MemoryBlock
		{
			public readonly int FileID;
			public int size;
			public MemoryBlock(int size, int ID)
			{
				this.size = size;
				FileID = ID;
			}

			public override string ToString()
			{
				if(FileID < 0)
					return new string('.', size);
				if (FileID < 10)
					return new string(FileID.ToString()[0], size);
				return new string(((char)((FileID%(128-32))+32)), size);
			}
		}

		internal static long Part1(string input)
		{
			long result = 0;
			List<MemoryBlock> mem = new List<MemoryBlock>();
			bool isFile = true;
			int currentFileId = 0;
			foreach (char c in input)
			{
				int size = int.Parse(""+c);
				if (isFile)
				{
					mem.Add(new MemoryBlock(size,currentFileId));
					currentFileId++;
				}
				else if(size > 0)
				{
					mem.Add(new MemoryBlock(size, -1));
				}

				isFile = !isFile;
			}

			long maxSize = mem.Sum((a) => a.size);
			CompactBlocks(mem);
			result = Checksum(mem, maxSize);
			return result;
		}

		private static long Checksum(List<MemoryBlock> mem, long max)
		{
			long total = 0;
			int pos = 0;
			int idx = 0;
			int i = 0;
			for (; i < max && idx < mem.Count; i++)
			{
				if(mem[idx].FileID>=0)
				{
					total += mem[idx].FileID * i;
				}
				pos++;
				if (mem[idx].size == pos)
				{
					pos = 0;
					idx++;
				}
			}

			return total;
		}

		private static void CompactBlocks(List<MemoryBlock> mem)
		{
			if (mem.Last().FileID >= 0)
			{
				mem.Add(new MemoryBlock(0, -1));
			}

			MemoryBlock lastFreeSpace = mem.Last();
			int j = 0;
			while (mem[j].FileID >= 0) j++;
			for (int i = mem.Count - 1; i >= 0 && j < mem.Count; i = mem.FindLastIndex(f => f.FileID >= 0))
			{
				MemoryBlock file = mem[i];
				if(file == lastFreeSpace)
					continue;
				MemoryBlock free = mem[j];
				if (free == lastFreeSpace)
					break;

				int s = Math.Min(free.size, file.size);

				if (s < free.size)
				{
					mem.Insert(j, new MemoryBlock(s, file.FileID));
				}
				else
				{
					mem[j] = new MemoryBlock(s, file.FileID);
					i++;
				}

				file.size -= mem[j].size;
				free.size -= mem[j].size;

				if (file.size <= 0)
				{
					mem.Remove(file);
				}
				if (file.size <= 0)
				{
					mem.Remove(file);
				}

				lastFreeSpace.size += mem[j].size;

				j = mem.FindIndex(f => f.FileID < 0);
				if (mem[j] == lastFreeSpace)
					break;
			}
			//Console.WriteLine(string.Join("", mem));
		}

		internal static long Part2(string input)
		{
			long result = 0;
			List<MemoryBlock> mem = new List<MemoryBlock>();
			bool isFile = true;
			int currentFileId = 0;
			foreach (char c in input)
			{
				int size = int.Parse("" + c);
				if (isFile)
				{
					mem.Add(new MemoryBlock(size, currentFileId));
					currentFileId++;
				}
				else
				{
					if(size > 0)
						mem.Add(new MemoryBlock(size, -1));
				}

				isFile = !isFile;
			}

			long maxSize = mem.Sum((a) => a.size);
			CompactFiles(mem);
			result = Checksum(mem, maxSize);
			return result;
		}

		private static void CombineFree(List<MemoryBlock> mem)
		{
			for (int i = mem.Count - 2; i >= 0; i--)
			{
				if (mem[i].FileID == mem[i + 1].FileID)
				{
					mem[i] = new MemoryBlock(mem[i].size + mem[i + 1].size, mem[i].FileID);
					mem.RemoveAt(i+1);
				}
			}

			;
		}

		private static void CompactFiles(List<MemoryBlock> mem)
		{
			int fileID = mem.Last(f => f.FileID >= 0).FileID;
			if (mem.Last().FileID >= 0)
			{
				mem.Add(new MemoryBlock(0, -1));
			}

			MemoryBlock lastFreeSpace = mem.Last();

			for (; fileID >= 0; fileID--)
			{
				MemoryBlock file = mem.FindLast(f => f.FileID == fileID);

				if (file == null)
				{
					continue;
				}

				int j = mem.FindIndex(f => f.FileID < 0 && f.size >= file.size);
				if(j < 0)
					continue;
				MemoryBlock free = mem[j];
				int i = mem.IndexOf(file);
				if (j > i)
					continue;

				if(free.size < file.size)
					continue;

				int s = Math.Min(free.size, file.size);
				if (s < free.size)
				{
					mem.Insert(j, new MemoryBlock(file.size, file.FileID));
					free.size -= file.size;
				}
				else
				{
					mem[j] = new MemoryBlock(file.size, file.FileID);
				}

				mem[mem.IndexOf(file)] = new MemoryBlock(file.size, -1);
				//Console.WriteLine(string.Join("", mem));
			}
		}
	}
}
