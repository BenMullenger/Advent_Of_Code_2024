using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode24
{
	internal class day2
	{
		enum InvalidReason
		{
			LargeGap,
			NoChange,
			ChangeInDirection,
			Success
		}
		static InvalidReason ValidLevelChange(int one, int two, int lastDif, bool verbose = false)
		{
			int dif = two - one;
			InvalidReason reason = InvalidReason.Success;
			Console.ForegroundColor = ConsoleColor.Yellow;
			if (Math.Abs(dif) > 3)
			{
				if (verbose)
					Console.WriteLine("\t\tPotentially unsafe due to large gap : {0} - {1} = {2}", two, one, dif);
				reason = InvalidReason.LargeGap;
			}
			else if (dif == 0)
			{
				if (verbose)
					Console.WriteLine("\t\tPotentially unsafe as no change in reactor level");
				reason = InvalidReason.NoChange;
			}
			else
			{
				bool lastPos = Math.Abs(lastDif) == lastDif;
				bool pos = Math.Abs(dif) == dif;
				if (lastPos != pos)
				{
					if (verbose)
						Console.WriteLine("\t\tPotentially unsafe due to changing direction : {0} , {1}", lastDif, dif);
					reason = InvalidReason.ChangeInDirection;
				}
			}
			Console.ForegroundColor = ConsoleColor.White;
			return reason;
		}
		
		static void AnalyzeReports(List<List<int>> reports, bool problemDampenerTollerance = false, bool verbose = false)
		{
			int numberSafeMethodic = 0;
			int numberSafeBrute = 0;
			foreach (List<int> report in reports)
			{
				if (verbose)
				{
					Console.Write("\t");
					foreach (int number in report)
					{
						Console.Write("{0}, ", number);
					}
					Console.WriteLine();
				}
				if (verbose)
					Console.WriteLine("\t Methodic:");
				bool isSafeMethodic = CheckReportMethodic(problemDampenerTollerance, report, verbose);
				if (isSafeMethodic)
				{
					numberSafeMethodic++;
				}
				else if (verbose)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("\t\tUNSAFE");
					Console.ForegroundColor = ConsoleColor.White;
				}
				if (verbose)
					Console.WriteLine("\tBrute:");
				bool isSafeBrute = CheckReportBrute(report, verbose);
				if (problemDampenerTollerance && !isSafeBrute)
				{
					for(int i = 0; i < report.Count; i++)
					{
						List<int> subReport = new List<int>(report);
						subReport.RemoveAt(i);
						
						isSafeBrute = CheckReportBrute(subReport);
						if(isSafeBrute)
						{
							if (verbose)
							{
								Console.WriteLine("\t\tSub:");
								Console.Write("\t\t");
								foreach (int number in subReport)
								{
									Console.Write("{0}, ", number);
								}
								Console.WriteLine();
							}
							if (verbose)
								Console.WriteLine("\t\tRemoving at {0} makes for safe report...", i);
							break;
						}
					}
				}
				if(isSafeBrute)
				{
					numberSafeBrute++;
				}
				else if (verbose)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("\t\tUNSAFE");
					Console.ForegroundColor = ConsoleColor.White;
				}
				if (verbose && isSafeBrute != isSafeMethodic)
				{
					Console.ForegroundColor = ConsoleColor.Magenta;
					Console.WriteLine("\tDISCREPENCY!!!");
					Console.ForegroundColor = ConsoleColor.White;
				}

			}
			// I tried so hard not to brute force it, but here we are...
			Console.WriteLine("\tNumber safe Methodic : {0}", numberSafeMethodic);
			Console.WriteLine("\tNumber safe Brute : {0}", numberSafeBrute);
		}

		private static bool CheckReportBrute(List<int> report, bool verbose = false)
		{
			bool isSafe = true;
			int lastDif = 0;
			bool first = true;
			for (int i = 1; i < report.Count; i++)
			{
				InvalidReason reason = InvalidReason.Success;
				int dif = report[i] - report[i - 1];
				bool isProblem = false;
				if (first) // no previous dif to find
				{
					first = false;
					if (Math.Abs(dif) > 3 || dif == 0)
					{
						reason = InvalidReason.LargeGap;
						Console.ForegroundColor = ConsoleColor.Yellow;
						if (verbose)
							Console.WriteLine("\t\tPotentially unsafe due to large gap or no change in reactor level : {0} - {1} = {2}", report[i], report[i - 1], dif);

						Console.ForegroundColor = ConsoleColor.White;
						isProblem = true;
					}
				}
				else
				{
					reason = ValidLevelChange(report[i - 1], report[i], lastDif, verbose);
					if (reason != InvalidReason.Success)
					{
						isProblem = true;
					}
				}
				if (isProblem)
				{
					isSafe = false;
					break;
				}
				lastDif = dif;
			}
			return isSafe;
		}

		private static bool CheckReportMethodic(bool problemDampenerTollerance, List<int> report, bool verbose = false)
		{
			bool isSafe = true;
			int lastDif = 0;
			int problemsCounted = 0;
			bool first = true;
			int previusIndex = 0;
			for (int i = 1; i < report.Count; i++)
			{
				InvalidReason reason = InvalidReason.Success;
				int dif = report[i] - report[previusIndex];
				bool isProblem = false;
				if (first) // no previous dif to find
				{
					first = false;
					if (Math.Abs(dif) > 3 || dif == 0)
					{
						reason = InvalidReason.LargeGap;
						Console.ForegroundColor = ConsoleColor.Yellow;
						if (verbose)
							Console.WriteLine("\t\tPotentially unsafe due to large gap or no change in reactor level : {0} - {1} = {2}", report[i], report[i - 1], dif);

						Console.ForegroundColor = ConsoleColor.White;
						isProblem = true;
						problemsCounted++;
					}
					previusIndex = i;
				}
				else
				{
					reason = ValidLevelChange(report[previusIndex], report[i], lastDif, verbose);
					if (reason != InvalidReason.Success)
					{
						isProblem = true;
						problemsCounted++;
					}
					previusIndex = i;
				}
				if (isProblem)
				{
					if (problemDampenerTollerance && problemsCounted <= 1)
					{
						if (i == report.Count - 1) // can just chop off last number
						{
							// no need for last dif
							if (verbose)
								Console.WriteLine("\t\t\tBut we could skip because can just chop off LAST number");
							continue;
						}
						if (i < 2)
						{
							first = true;
							lastDif = dif;
							if (verbose)
								Console.WriteLine("\t\t\tBut we could skip because we could chop off FIRST number");
							continue;
						}
						reason = ValidLevelChange(report[previusIndex], report[i + 1], lastDif);
						if (reason == InvalidReason.Success) // check we could skip current
						{
							// last dif remains the same
							if (verbose)
								Console.WriteLine("\t\t\tBut we could skip because we could skip current");
							previusIndex = i - 1;
							continue;
						}
						if (i > 2)
						{
							int prevLastDif = report[previusIndex - 1] - report[previusIndex - 2];
							reason = ValidLevelChange(report[i - 2], report[i], prevLastDif);
							if (reason == InvalidReason.Success) // check we could skip previous
							{
								lastDif = report[i] - report[previusIndex - 1];
								if (verbose)
									Console.WriteLine("\t\t\tBut we could skip because  we could skip previous");
								continue;
							}
						}
						if (reason == InvalidReason.ChangeInDirection)
						{
							// in the case of change direction, we might be able to get rid of one 2 places ago
							if (i == 2)
							{
								lastDif = dif;
								if (verbose)
									Console.WriteLine("\t\t\tBut as we are only on the third number we could chop off FIRST number");
								continue;
							}
						}
					}
					isSafe = false;
					break;
				}
				lastDif = dif;
			}

			return isSafe;
		}

		public static void Run()
		{
			const string filePath = "data/day2.txt";
			string[] lines = File.ReadAllLines(filePath);
			List<List<int>> reports = new List<List<int>>(lines.Length);
			foreach (string line in lines)
			{
				int currentReport = reports.Count;
				reports.Add(new List<int>());
				string[] numbers = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
				foreach (string number in numbers)
				{
					reports[currentReport].Add(int.Parse(number));
				}
			}

			Console.WriteLine("Zero Tollerance report:");
			AnalyzeReports(reports);
			Console.WriteLine("One Tollerance report:");
			AnalyzeReports(reports, true);
		}
			
	}
}
