using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using QEngine.Pipeline.MGCB;
using QEngine.Pipeline.MGCB.MGCB;

namespace QEngine.Pipeline
{
	public static class QImporter
	{
		static List<string> SearchDirectory(string dirOfFolder, string folderName)
		{
			List<string> files = new List<string>();
			if(Directory.Exists(dirOfFolder + "\\" + folderName))
			{
				DirSearch(files, dirOfFolder + "\\" + folderName);
			}
			return files;
		}

		static void DirSearch(List<string> files, string sDir)
		{
			try
			{
				foreach(string d in Directory.GetDirectories(sDir))
				{
					foreach(string f in Directory.GetFiles(d))
					{
						//Console.WriteLine(f);
						files.Add(f);
					}
					DirSearch(files, d);
				}
			}
			catch(System.Exception excpt)
			{
				Console.WriteLine(excpt.Message);
			}
		}

		public static int Start(string dirOfFolder, string folderName)
		{
			foreach(var s in SearchDirectory(dirOfFolder, folderName))
			{
				Console.WriteLine(s);
			}
			return 0;
		}

		static int Start(string[] args)
		{
			// We force all stderr to redirect to stdout
			// to avoid any out of order console output.
			Console.SetError(Console.Out);

			if(!Environment.Is64BitProcess && Environment.OSVersion.Platform != PlatformID.Unix)
			{
				Console.Error.WriteLine("The MonoGame content tools only work on a 64bit OS.");
				return -1;
			}

			var content = new QBuildContent();

			// Parse the command line.
			var parser = new QCommandLineBuildParser(content)
			{
				Title = "MonoGame Content Builder\n" +
				        "Builds optimized game content for MonoGame projects."
			};

			if(!parser.Parse(args))
				return -1;

			// Launch debugger if requested.
			if(content.LaunchDebugger)
			{
				try
				{
					System.Diagnostics.Debugger.Launch();
				}
				catch(NotImplementedException)
				{
					// not implemented under Mono
				}
			}

			// Print a startup message.            
			var buildStarted = DateTime.Now;
			if(!content.Quiet)
				Console.WriteLine("Build started {0}\n", buildStarted);

			// Let the content build.
			int successCount, errorCount;
			content.Build(out successCount, out errorCount);

			// Print the finishing info.
			if(!content.Quiet)
			{
				Console.WriteLine("\nBuild {0} succeeded, {1} failed.\n", successCount, errorCount);
				Console.WriteLine("Time elapsed {0:hh\\:mm\\:ss\\.ff}.", DateTime.Now - buildStarted);
			}

			// Return the error count.
			return errorCount;
		}
	}
}