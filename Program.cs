//*******************************************************************
//       XML Doc Filter	                                     
//                                                                   
//       Copyright © 2016 ByteScout - http://www.bytescout.com       
//       ALL RIGHTS RESERVED                                         
//                                                                   
//*******************************************************************


using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace XmlDocFilter
{
	class Program
	{
		static int Main(string[] args)
		{
			int result = 0;
			string inputFile = null; 
			string outputFile = null;
			string inputFilters = null;

			if (args.Length < 1)
			{
				ShowUsage();
				result = 1;
			}
			else
			{
				PrintVersion();

				try
				{
					for (int i = 0; i < args.Length; i++)
					{
						string arg = args[i];

						if (String.Compare(arg, "/input", true) == 0)
						{
							if (i + 1 < args.Length)
							{
								inputFile = args[i + 1];
							}
							else
							{
								throw new Exception("Invalid \"/input\" parameter.");
							}
						}
						else if (String.Compare(arg, "/output", true) == 0)
						{
							if (i + 1 < args.Length)
							{
								outputFile = args[i + 1];
							}
							else
							{
								throw new Exception("Invalid \"/output\" parameter.");
							}
						}
						else if (String.Compare(arg, "/filters", true) == 0)
						{
							if (i + 1 < args.Length)
							{
								inputFilters = args[i + 1];
							}
							else
							{
								throw new Exception("Invalid \"/filters\" parameter.");
							}
						}
					}

					if (String.IsNullOrEmpty(inputFile))
					{
						throw new Exception("Input file is not specified.");
					}

                    if (!File.Exists(inputFile)) {

                        Console.WriteLine("ERROR: Input file not found: \"" + inputFile + "\"");
                        return 0;

                    }

					if (String.IsNullOrEmpty(outputFile))
					{
						outputFile = inputFile;
					}

					if (String.IsNullOrEmpty(inputFilters))
					{
						throw new Exception("Filters are not specified.");
					}



					
					Console.WriteLine("Processing \"" + inputFile + "\"...");

					string[] filters = inputFilters.Split(',');
					string xml = null;
					
					using (StreamReader streamReader = new StreamReader(inputFile))
					{
						xml = streamReader.ReadToEnd();
						streamReader.Close();
					}

					XmlDocument doc = new XmlDocument();

					doc.LoadXml(xml);

					XmlNodeList members = doc.SelectNodes("//member[@name]");

					if (members != null)
					{
						foreach (XmlNode member in members)
						{
							foreach (string filter in filters)
							{
								if (member.Attributes["name"].Value.Contains(filter))
								{
									member.RemoveAll();

									if (member.ParentNode != null)
									{
										member.ParentNode.RemoveChild(member);
									}

									break;
								}
							}
						}
					}

					doc.Save(outputFile);
					
					Console.WriteLine("Result saved to \"" + outputFile + "\"...");

				}
				catch (Exception exception)
				{
					Console.WriteLine();
					Console.WriteLine(exception.Message);
					result = 2;
				}
			}

#if DEBUG
			Console.WriteLine();
			Console.WriteLine("Press any key...");
			Console.Read();
#endif

			return result;
		}

		private static void PrintVersion()
		{
			string name = Assembly.GetExecutingAssembly().GetName().Name;
			string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			Console.WriteLine(name + " " + version);
		}

		private static void ShowUsage()
		{
			PrintVersion();
			Console.WriteLine();
			Console.WriteLine("Usage:  XmlDocFilter.exe /input <filename> output <filename> /filters <filters>");
			Console.WriteLine();
			Console.WriteLine("   /input <filename>   Input XML file.");
			Console.WriteLine("   /output <filename>  Output XML file.");
			Console.WriteLine("   /filter <filters>   Comma-separated filter strings.");
			Console.WriteLine();
			Console.WriteLine("Example:");
			Console.WriteLine();
			Console.WriteLine("   XmlDocFilter.exe /input:MyProject.xml output:MyProject.Filtered.xml /filters:.Internal.,Core.");

		}
	}
}
