using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Graphics;

namespace QEngine.Pipeline
{
	[XmlRoot(ElementName = "SourceFileCollection")]
	public sealed class QSourceFileCollection
	{
		public GraphicsProfile Profile { get; set; }

		public TargetPlatform Platform { get; set; }

		public string Config { get; set; }

		[XmlArrayItem("File")]
		public List<string> SourceFiles { get; set; }

		public QSourceFileCollection()
		{
			SourceFiles = new List<string>();
			Config = string.Empty;
		}

		public static QSourceFileCollection Read(string filePath)
		{
			var deserializer = new XmlSerializer(typeof(QSourceFileCollection));
			try
			{
				using(var textReader = new StreamReader(filePath))
					return (QSourceFileCollection)deserializer.Deserialize(textReader);
			}
			catch(Exception) { }

			return new QSourceFileCollection();
		}

		public void Write(string filePath)
		{
			var serializer = new XmlSerializer(typeof(QSourceFileCollection));
			using(var textWriter = new StreamWriter(filePath, false, new UTF8Encoding(false)))
				serializer.Serialize(textWriter, this);
		}

		public void Merge(QSourceFileCollection other)
		{
			foreach(var sourceFile in other.SourceFiles)
			{
				var inContent = SourceFiles.Any(e => string.Equals(e, sourceFile, StringComparison.InvariantCultureIgnoreCase));
				if(!inContent)
					SourceFiles.Add(sourceFile);
			}
		}
	}
}