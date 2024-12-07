using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode24
{
	internal class day3
	{
		static bool GetNumber(int start, string line, out int number, out int numberOfDigits, bool second = false)
		{
			number = -1;
			bool validNumber = true;
			numberOfDigits = 0;
			for (numberOfDigits = 0; numberOfDigits < 4; numberOfDigits++)
			{
				string digit = new string(line[start + numberOfDigits], 1);
				if (digit == "," || (second && digit == ")"))
				{
					break;
				}
				if (!int.TryParse(digit, out int blah))
				{
					validNumber = false;
					break;
				}
			}
			if (!validNumber || numberOfDigits < 1)
			{
				return false;
			}
			string sNumber = line.Substring(start, numberOfDigits);
			return int.TryParse(sNumber, out number);
		}

		public static void Run()
		{
			StringBuilder invalids = new StringBuilder();
			StringBuilder valids = new StringBuilder();
			const string filePath = "data/day3.txt";
			string[] lines = File.ReadAllLines(filePath);
			long total = 0;
			bool on = true;
			Console.BackgroundColor = ConsoleColor.Red;
			foreach (string line in lines)
			{
				int head = 0;
				while (head < line.Length)
				{
					for (int i = head; i < line.Length; i++)
					{
						char c = line[i];
						if (c == 'm')
						{
							//Console.WriteLine("\tRejecting \"{0}\"", line.Substring(head, i - head));
							Console.Write(line.Substring(head, i - head));
							head = i;
							Console.BackgroundColor = ConsoleColor.Red;
							break;
						}
						else if(c == 'd')
						{
							if (i + 4 < line.Length)
							{
								string doString = line.Substring(i, 4);
								if (doString == "do()")
								{
									Console.BackgroundColor = ConsoleColor.Red;
									Console.Write(line.Substring(head, i - head));
									on = true;
									i += 4;
									Console.BackgroundColor = ConsoleColor.Blue;
									Console.Write(doString);
									head = i;
									i--;
									Console.BackgroundColor = ConsoleColor.Red;
									continue;
								}
								if (i + 7 < line.Length)
								{
									string dontString = line.Substring(i, 7);
									if (dontString == "don't()")
									{
										Console.BackgroundColor = ConsoleColor.Red;
										Console.Write(line.Substring(head, i - head));
										on = false;
										i += 7;
										Console.BackgroundColor = ConsoleColor.Magenta;
										Console.Write(dontString);
										head = i;
										i--;
										Console.BackgroundColor = ConsoleColor.Red;
										continue;
									}
								}
							}
						}
					}
					if (head + 6 >= line.Length)
					{
						head += 1;
						break; // end of line 
					}
					int read = 4;
					string supposedMul = line.Substring(head, read);
					if(supposedMul != "mul(")
					{
						//invalids.AppendFormat("\t\tRejecting \"{0}\"\r\n", line.Substring(head, read + 1));
						Console.BackgroundColor = ConsoleColor.DarkYellow;
						Console.Write(line[head]);
						head = head + 1;
						continue;
					}
					if(!GetNumber(head + read, line, out int numberOne, out int numberOfDigitsOne))
					{
						invalids.AppendFormat("\t\tRejecting \"{0}\"\r\n", line.Substring(head, read + numberOfDigitsOne+1));
						Console.BackgroundColor = ConsoleColor.DarkYellow;
						Console.Write(line[head]);
						head += 1;
						continue;
					}
					read += numberOfDigitsOne+1; // start after comma
					if (!GetNumber(head + read, line, out int numberTwo, out int numberOfDigitsTwo, true))
					{
						invalids.AppendFormat("\t\tRejecting \"{0}\"\r\n", line.Substring(head, read + numberOfDigitsTwo+1));
						Console.BackgroundColor = ConsoleColor.DarkYellow;
						Console.Write(line[head]);
						head += 1;
						continue;
					}
					read += numberOfDigitsTwo;
					if (line[head + read] != ')')
					{
						invalids.AppendFormat("\t\tRejecting \"{0}\"\r\n", line.Substring(head, read + 1));
						Console.BackgroundColor = ConsoleColor.DarkYellow;
						Console.Write(line[head]);
						head += 1;
						continue;
					}
					if(on)
						total += (long)(numberOne * numberTwo);

					Console.BackgroundColor = ConsoleColor.Green;
					string found = line.Substring(head, read + 1);
					Console.Write(found);
					valids.AppendFormat("\tFound \"{0}\", {1} x {2}, {3}\r\n", found, numberOne, numberTwo, (on ? "Added" : "ignored"));
					head += found.Length;
					Console.BackgroundColor = ConsoleColor.Red;
				}
			}
			Console.BackgroundColor = ConsoleColor.Red;
			Console.WriteLine();
			Console.WriteLine(invalids.ToString());
			Console.WriteLine();

			Console.BackgroundColor = ConsoleColor.Black;
			Console.WriteLine();
			Console.WriteLine(valids.ToString());
			Console.WriteLine();
			Console.WriteLine("Total found: {0}",total);
		}
	}
}
