using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace QEngine
{
	/// <summary>
	/// Preferences class that can store data to local game location for non-important game data
	/// etc..Player location or music volume
	/// </summary>
	public static class QPrefs
	{
		static Dictionary<string, int> IntPreferences { get; set; } = new Dictionary<string, int>();

		static Dictionary<string, bool> BoolPreferences { get; set; } = new Dictionary<string, bool>();

		static Dictionary<string, float> FloatPreferences { get; set; } = new Dictionary<string, float>();

		static Dictionary<string, string> StringPreferences { get; set; } = new Dictionary<string, string>();

		static string IntSaveLocation = "Saves/PrefsI.bin";

		static string BoolSaveLocation = "Saves/PrefsB.bin";

		static string FloatSaveLocation = "Saves/PrefsF.bin";

		static string StringSaveLocation = "Saves/PrefsS.bin";

		internal static async Task Save()
		{
			if(!Directory.Exists("Saves"))
				Directory.CreateDirectory("Saves");
			await Task.Factory.StartNew(() =>
			{
				using(var fs = new FileStream(IntSaveLocation, FileMode.OpenOrCreate))
				using(var bw = new BinaryWriter(fs))
				{
					var count = IntPreferences.Count;

					bw.Write(count);

					foreach(var i in IntPreferences)
					{
						bw.Write(i.Key);
						bw.Write(i.Value);
					}
				}
				using(var fs = new FileStream(BoolSaveLocation, FileMode.OpenOrCreate))
				using(var bw = new BinaryWriter(fs))
				{
					var count = BoolPreferences.Count;

					bw.Write(count);

					foreach(var i in BoolPreferences)
					{
						bw.Write(i.Key);
						bw.Write(i.Value);
					}
				}
				using(var fs = new FileStream(FloatSaveLocation, FileMode.OpenOrCreate))
				using(var bw = new BinaryWriter(fs))
				{
					var count = FloatPreferences.Count;

					bw.Write(count);

					foreach(var i in FloatPreferences)
					{
						bw.Write(i.Key);
						bw.Write(i.Value);
					}
				}
				using(var fs = new FileStream(StringSaveLocation, FileMode.OpenOrCreate))
				using(var bw = new BinaryWriter(fs))
				{
					var count = StringPreferences.Count;

					bw.Write(count);

					foreach(var i in StringPreferences)
					{
						bw.Write(i.Key);
						bw.Write(i.Value);
					}
				}
			});
		}

		public static void ClearAll()
		{
			File.Delete(BoolSaveLocation);
			File.Delete(FloatSaveLocation);
			File.Delete(IntSaveLocation);
			File.Delete(StringSaveLocation);
		}

		internal static async Task Load()
		{
			if(!Directory.Exists("Saves"))
			{
				Directory.CreateDirectory("Saves");
				await Save();
			}
			await Task.Factory.StartNew(() =>
			{
				using(var fs = new FileStream(IntSaveLocation, FileMode.OpenOrCreate))
				using(var br = new BinaryReader(fs))
				{
					var count = br.Read();

					for(var i = 0; i < count; i++)
					{
						var key = br.ReadString();
						var value = br.Read();

						IntPreferences[key] = value;
					}
				}
				using(var fs = new FileStream(BoolSaveLocation, FileMode.OpenOrCreate))
				using(var br = new BinaryReader(fs))
				{
					var count = br.Read();

					for(var i = 0; i < count; i++)
					{
						var key = br.ReadString();
						var value = br.ReadBoolean();

						BoolPreferences[key] = value;
					}
				}
				using(var fs = new FileStream(FloatSaveLocation, FileMode.OpenOrCreate))
				using(var br = new BinaryReader(fs))
				{
					var count = br.ReadInt32();

					for(var i = 0; i < count; i++)
					{
						var key = br.ReadString();
						var value = br.ReadSingle();

						FloatPreferences[key] = value;
					}
				}
				using(var fs = new FileStream(StringSaveLocation, FileMode.OpenOrCreate))
				using(var br = new BinaryReader(fs))
				{
					var count = br.Read();

					for(var i = 0; i < count; i++)
					{
						var key = br.ReadString();
						var value = br.ReadString();

						StringPreferences[key] = value;
					}
				}
			});
		}

		public static void AddInt(string name, int value)
		{
			IntPreferences[name] = value;
		}

		public static void AddFloat(string name, float value)
		{
			FloatPreferences[name] = value;
		}

		public static void AddString(string name, string value)
		{
			StringPreferences[name] = value;
		}

		public static void AddBool(string name, bool value)
		{
			BoolPreferences[name] = value;
		}

		public static int GetInt(string name)
		{
			if(IntPreferences.TryGetValue(name, out int v))
				return v;
			throw new FileNotFoundException();
		}

		public static float GetFloat(string name)
		{
			if(FloatPreferences.TryGetValue(name, out float v))
				return v;
			throw new FileNotFoundException();
		}

		public static bool GetBool(string name)
		{
			if(BoolPreferences.TryGetValue(name, out bool v))
				return v;
			throw new FileNotFoundException();
		}

		public static string GetString(string name)
		{
			if(StringPreferences.TryGetValue(name, out string v))
				return v;
			throw new FileNotFoundException();
		}
	}
}