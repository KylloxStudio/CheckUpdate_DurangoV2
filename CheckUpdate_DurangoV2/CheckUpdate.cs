using System;
using System.IO;
using System.Net;
using System.Diagnostics;
using HtmlAgilityPack;

namespace CheckUpdate
{
	class MainClass
	{
		private static bool FoundUpdate;
		private static double CurrentVersion;
		private static double LastVersion;

		static void Main(string[] args)
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

			HtmlWeb web = new HtmlWeb();
			HtmlDocument document = web.Load("https://github.com/KylloxStudio/Durango_V2/tags");
			HtmlNode h4Node = document.DocumentNode.SelectSingleNode("//h4[@class='flex-auto min-width-0 pr-2 pb-1 commit-title']");
			HtmlNode aNode = h4Node.SelectSingleNode("a");

			string[] version = aNode.InnerText.Split(new char[]
			{
				'v'
			});
			LastVersion = double.Parse(version[1]);

			string path = Directory.GetCurrentDirectory() + "\\DurangoV2_Data\\version.txt";

			if (new FileInfo(path).Exists)
			{
				string txt = File.ReadAllText(path);
				CurrentVersion = double.Parse(txt);
			}
			else
			{
				CurrentVersion = int.MaxValue;
				Console.WriteLine("Error: Cannot Found Required File.");
				foreach (Process process in Process.GetProcesses())
				{
					if (process.ProcessName.StartsWith("CheckUpdate_DurangoV2"))
					{
						process.Kill();
					}
				}
			}

			if (CurrentVersion < LastVersion)
				FoundUpdate = true;

			Console.WriteLine("Found Update: " + FoundUpdate.ToString());
			Console.WriteLine("Current Version: " + string.Format("{0:0.0}", CurrentVersion));
			Console.WriteLine("Last Version: " + string.Format("{0:0.0}", LastVersion));
		}
	}
}