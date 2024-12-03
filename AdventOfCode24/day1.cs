using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode24
{
	internal class day1
	{
		public static void Run()
		{
			const string filePath = "data/day1.txt";
			string[] lines = File.ReadAllLines(filePath);
			List<int> columnOne = new List<int>();
			List<int> columnTwo = new List<int>();
			foreach (string line in lines)
			{
				string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
				if (parts.Length == 2)
				{
					columnOne.Add(int.Parse(parts[0]));
					columnTwo.Add(int.Parse(parts[1]));
				}
			}
			columnOne.Sort();
			columnTwo.Sort();
			int totalDif = 0;
			Console.WriteLine("Calculating Difs:");
			for (int i = 0; i < columnOne.Count; i++)
			{
				Console.Write(string.Format("{0}, {1}", columnOne[i], columnTwo[i]));
				int dif = 0;
				if (columnOne[i] < columnTwo[i])
					dif = columnTwo[i] - columnOne[i];
				else
					dif = columnOne[i] - columnTwo[i];
				Console.WriteLine("  dif : {0}", dif);
				totalDif += dif;
			}


			Console.WriteLine(string.Format("Total Dif : {0}", totalDif));


			Console.WriteLine("Calculating Similarity");
			int lastFoundIndex = 0;
			int lastFoundScore = 0;
			int similarityScore = 0;
			for (int i = 0; i < columnOne.Count; i++)
			{
				Console.WriteLine(columnOne[i]);
				if (i > 0)
				{
					if (columnOne[i] == columnOne[i - 1])
					{
						similarityScore += lastFoundScore;
						Console.WriteLine("\t same as previous");
						continue;
					}
				}
				int start = lastFoundIndex;
				int countFound = 0;
				while (start < columnTwo.Count && columnTwo[start] <= columnOne[i])
				{
					Console.Write("\t{0}: ", columnTwo[start]);
					if (columnOne[i] == columnTwo[start])
					{
						countFound++;
						Console.Write("Add");
					}
					else
					{
						Console.Write("-");
					}
					Console.WriteLine();
					start++;
				}
				if (start < columnTwo.Count)
				{
					Console.WriteLine("\t breaking with {0}: ", columnTwo[start]);
				}
				else
				{
					Console.WriteLine("List searched");
				}
				lastFoundScore = countFound * columnOne[i];
				lastFoundIndex = start;
				similarityScore += lastFoundScore;
				Console.WriteLine("Similarity found : {0}, latest similarity : {1}", countFound, lastFoundScore);
			}
			Console.WriteLine(similarityScore);

			for (int i = 0; i < columnOne.Count - 1; i++)
			{
				if (columnOne[i] == columnOne[i + 1])
				{
					Console.WriteLine("Match found : {0} {1}, {2}", columnOne[i], i, i + 1);
				}
			}
		}
	}
}
