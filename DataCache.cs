using System.Globalization;
using System.Text;
using System.Text.Json;

namespace AMQSongBrowser {
	public class DataCache {
		private static readonly Lazy<DataCache> _instance= new Lazy<DataCache>(() => new DataCache());
		public static DataCache Instance => _instance.Value;
		private DataCache() {
			Init();
		}

		private readonly string jsonpath = @"libraryMasterList.json";
		private readonly string binpath = @"datacache.bin";
		public bool inited = false;
		private void Init() {
			if(!LoadCacheData()) {
				if(File.Exists(jsonpath))
					BuildData(File.ReadAllText(jsonpath));
			}
		}

		public class AnimeData {
			public string[] names;
			public string[] nameslower;
			public string nameja;
			public string nameen;
			public int annid;
			public int year;
			public int seasonid;
			public string category;

			public void Save(BinaryWriter writer) {
				DataCache.SaveStringArray(writer, names);
				DataCache.SaveStringArray(writer, nameslower);
				writer.Write(nameja);
				writer.Write(nameen);
				writer.Write(annid);
				writer.Write(year);
				writer.Write(seasonid);
				writer.Write(category);
			}
			public static AnimeData Load(BinaryReader reader) {
				var ret = new AnimeData();
				ret.names = DataCache.LoadStringArray(reader);
				ret.nameslower = DataCache.LoadStringArray(reader);
				ret.nameja = reader.ReadString();
				ret.nameen = reader.ReadString();
				ret.annid = reader.ReadInt32();
				ret.year = reader.ReadInt32();
				ret.seasonid = reader.ReadInt32();
				ret.category = reader.ReadString();
				return ret;
			}
		};
		public class SongData {
			public string name;
			public string namelower;
			public bool standard;
			public bool instrumental;
			public bool chanting;
			public bool character;
			public int artist;
			public int group;
			public int composerartist;
			public int composergroup;
			public int arrangerartist;
			public int arrangergroup;

			public void Save(BinaryWriter writer) {
				writer.Write(name);
				writer.Write(namelower);
				writer.Write(standard);
				writer.Write(instrumental);
				writer.Write(chanting);
				writer.Write(character);
				writer.Write(artist);
				writer.Write(group);
				writer.Write(composerartist);
				writer.Write(composergroup);
				writer.Write(arrangerartist);
				writer.Write(arrangergroup);
			}
			public static SongData Load(BinaryReader reader) {
				var ret = new SongData();
				ret.name = reader.ReadString();
				ret.namelower = reader.ReadString();
				ret.standard = reader.ReadBoolean();
				ret.instrumental = reader.ReadBoolean();
				ret.chanting = reader.ReadBoolean();
				ret.character = reader.ReadBoolean();
				ret.artist = reader.ReadInt32();
				ret.group = reader.ReadInt32();
				ret.composerartist = reader.ReadInt32();
				ret.composergroup = reader.ReadInt32();
				ret.arrangerartist = reader.ReadInt32();
				ret.arrangergroup = reader.ReadInt32();
				return ret;
			}
		};
		public class ArtistData {
			public string name;
			public string namelower;
			public HashSet<int> altnames;

			public void Save(BinaryWriter writer) {
				writer.Write(name);
				writer.Write(namelower);
				DataCache.SaveIntHashSet(writer, altnames);
			}
			public static ArtistData Load(BinaryReader reader) {
				var ret = new ArtistData();
				ret.name = reader.ReadString();
				ret.namelower = reader.ReadString();
				ret.altnames = DataCache.LoadIntHashSet(reader);
				return ret;
			}
		}
		public class GroupData {
			public string name;
			public string namelower;
			public string nameartists;
			public HashSet<int> artists;
			public HashSet<int> groups;
			public HashSet<int> altnames;

			public void Save(BinaryWriter writer) {
				writer.Write(name);
				writer.Write(namelower);
				writer.Write(nameartists);
				DataCache.SaveIntHashSet(writer, artists);
				DataCache.SaveIntHashSet(writer, groups);
				DataCache.SaveIntHashSet(writer, altnames);

			}
			public static GroupData Load(BinaryReader reader) {
				var ret = new GroupData();
				ret.name = reader.ReadString();
				ret.namelower = reader.ReadString();
				ret.nameartists = reader.ReadString();
				ret.artists = DataCache.LoadIntHashSet(reader);
				ret.groups = DataCache.LoadIntHashSet(reader);
				ret.altnames = DataCache.LoadIntHashSet(reader);
				return ret;
			}
		};

		public class AllSongListData {
			public int animeid;
			public int songid;
			public int artistid;
			public int groupid;
			public int type;
			public int number;
			public int annsongid;
			public bool standard;
			public bool instrumental;
			public bool chanting;
			public bool character;
			public bool dub;
			public bool rebroad;
			public string animecategory;

			public AllSongListData Clone() { return (AllSongListData)MemberwiseClone(); }
			public void Save(BinaryWriter writer) {
				writer.Write(animeid);
				writer.Write(songid);
				writer.Write(artistid);
				writer.Write(groupid);
				writer.Write(type);
				writer.Write(number);
				writer.Write(annsongid);
				writer.Write(standard);
				writer.Write(instrumental);
				writer.Write(chanting);
				writer.Write(character);
				writer.Write(dub);
				writer.Write(rebroad);
				writer.Write(animecategory);
			}
			public static AllSongListData Load(BinaryReader reader) {
				var ret = new AllSongListData();
				ret.animeid = reader.ReadInt32();
				ret.songid = reader.ReadInt32();
				ret.artistid = reader.ReadInt32();
				ret.groupid = reader.ReadInt32();
				ret.type = reader.ReadInt32();
				ret.number = reader.ReadInt32();
				ret.annsongid = reader.ReadInt32();
				ret.standard = reader.ReadBoolean();
				ret.instrumental = reader.ReadBoolean();
				ret.chanting = reader.ReadBoolean();
				ret.character = reader.ReadBoolean();
				ret.dub = reader.ReadBoolean();
				ret.rebroad = reader.ReadBoolean();
				ret.animecategory = reader.ReadString();
				return ret;
			}
		}

		static public void SaveStringArray(BinaryWriter writer, string[] arr) {
			writer.Write(arr.Length);
			foreach(string str in arr)
				writer.Write(str);
		}
		static public string[] LoadStringArray(BinaryReader reader) {
			var count = reader.ReadInt32();
			var arr = new string[count];
			for(int i = 0; i < count; i++)
				arr[i] = reader.ReadString();
			return arr;
		}
		static public void SaveIntHashSet(BinaryWriter writer, HashSet<int> hashset) {
			writer.Write(hashset.Count);
			foreach(var item in hashset)
				writer.Write(item);
		}
		static public HashSet<int> LoadIntHashSet(BinaryReader reader) {
			var ret = new HashSet<int>();
			var count = reader.ReadInt32();
			for(int i = 0; i < count; i++)
				ret.Add(reader.ReadInt32());
			return ret;
		}

		public DateTime timeLastUpdate { get; private set; }

		public bool isSongName { get; set; }
		public bool isArtist { get; set; }
		public bool isAnime { get; set; }

		public string strSongName { get; set; }
		public string strArtist { get; set; }
		public string strAnime { get; set; }

		public bool isOP { get; set; }
		public bool isED { get; set; }
		public bool isINS { get; set; }

		public bool isStandard { get; set; }
		public bool isInstrumental { get; set; }
		public bool isChanting { get; set; }
		public bool isCharacter { get; set; }
		public bool isDub { get; set; }
		public bool isRebroad { get; set; }

		public bool isSongNameDirty { get; set; }
		public bool isArtistDirty { get; set; }
		public bool isAnimeDirty { get; set; }

		private HashSet<int> matchedAnimeNames = new HashSet<int>();
		private HashSet<int> matchedSongNames = new HashSet<int>();
		private HashSet<int> matchedArtistNames = new HashSet<int>();
		private HashSet<int> matchedGroupNames = new HashSet<int>();

		private Dictionary<int, AnimeData> animeDataMap= new Dictionary<int, AnimeData>();
		private Dictionary<int, SongData> songDataMap = new Dictionary<int,	SongData>();
		private Dictionary<int, ArtistData> artistDataMap = new Dictionary<int, ArtistData>();
		private Dictionary<int, GroupData> groupDataMap = new Dictionary<int, GroupData>();

		public AnimeData GetAnimeData(int id) { return animeDataMap[id]; }
		public SongData GetSongData(int id) { return songDataMap[id]; }
		public ArtistData GetArtistData(int id) { return artistDataMap[id]; }
		public GroupData GetGroupData(int id) { return groupDataMap[id]; }

		List<AllSongListData> allSongList = new List<AllSongListData>();

		private Dictionary<string, JsonElement> GetDict(JsonElement element) {
			return element.Deserialize<Dictionary<string, JsonElement>>();
		}
		private int GetInt(string str) {
			int ret;
			if(int.TryParse(str, CultureInfo.InvariantCulture, out ret)) return ret;
			return 0;
		}
		private int GetInt(JsonElement element) {
			switch(element.ValueKind) {
				case JsonValueKind.Number:
					return element.GetInt32();
				case JsonValueKind.String:
					return GetInt(element.GetString());
			}
			return 0;
		}
		private string GetStr(JsonElement element) {
			switch(element.ValueKind) {
				case JsonValueKind.String:
					return element.GetString();
				case JsonValueKind.Number:
					return element.GetRawText();
			}
			return string.Empty;
		}
		private HashSet<int> GetIntHashSet(JsonElement element) {
			var result = new HashSet<int>();
			var array = element.Deserialize<int[]>();
			if(array != null) {
				foreach(var data in array)
					result.Add(data);
			}
			return result;
		}

		public bool BuildData(string jsonstr) {
			inited = false;
			try {
				var animedatamap = new Dictionary<int, AnimeData>();
				var songdatamap = new Dictionary<int, SongData>();
				var artistdatamap = new Dictionary<int, ArtistData>();
				var groupdatamap = new Dictionary<int, GroupData>();

				var rootmap = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonstr);

				var animemap = GetDict(rootmap["animeMap"]);
				var songmap = GetDict(rootmap["songMap"]);
				var artistmap = GetDict(rootmap["artistMap"]);
				var groupmap = GetDict(rootmap["groupMap"]);

				foreach(KeyValuePair<string, JsonElement> pair in artistmap) {
					var artistdata = new ArtistData();
					int key = GetInt(pair.Key);
					var value = GetDict(pair.Value);
					artistdata.name = value["name"].GetString();
					artistdata.namelower = RemoveDiacritics(artistdata.name).ToLower();
					artistdata.altnames = GetIntHashSet(value["altNameLinks"]);
					artistdatamap.Add(key, artistdata);
				}

				foreach(KeyValuePair<string, JsonElement> pair in groupmap) {
					var groupdata = new GroupData();
					int key = GetInt(pair.Key);
					var value = GetDict(pair.Value);
					groupdata.name = value["name"].GetString();
					groupdata.namelower = RemoveDiacritics(groupdata.name).ToLower();
					groupdata.artists = GetIntHashSet(value["artistMembers"]);
					groupdata.groups = GetIntHashSet(value["groupMembers"]);
					groupdata.altnames = GetIntHashSet(value["altNameLinks"]);
					groupdatamap.Add(key, groupdata);
				}
				var tempgroupdatamap = new Dictionary<int, GroupData>();
				foreach(var groupdata in groupdatamap) {
					var tempdata = groupdata.Value;
					var artistlist = new List<string>();
					foreach(var artist in tempdata.artists)
						artistlist.Add(artistdatamap[artist].name);
					foreach(var group in tempdata.groups)
						artistlist.Add(groupdatamap[group].name);
					if(artistlist.Count > 0)
						tempdata.nameartists = string.Concat(tempdata.name, " ( ", string.Join(", ", artistlist), " )");
					else tempdata.nameartists = tempdata.name;
					tempgroupdatamap.Add(groupdata.Key, tempdata);
				}
				groupdatamap = tempgroupdatamap;

				foreach(KeyValuePair<string, JsonElement> pair in songmap) {
					var songdata = new SongData();
					int key = GetInt(pair.Key);
					var value = GetDict(pair.Value);
					songdata.name = value["name"].GetString();
					songdata.namelower = RemoveDiacritics(songdata.name).ToLower();
					songdata.artist = GetInt(value["songArtistId"]);
					songdata.group = GetInt(value["songGroupId"]);
					songdata.standard = songdata.instrumental = songdata.chanting = songdata.character = false;
					var category = GetInt(value["category"]);
					switch(category) {
						case 1:
							songdata.instrumental = true;
							break;
						case 2:
							songdata.chanting = true;
							break;
						case 3:
							songdata.character = true;
							break;
						default:
							songdata.standard = true;
							break;
					}
					songdata.composerartist = GetInt(value["composerArtistId"]);
					songdata.composergroup = GetInt(value["composerGroupId"]);
					songdata.arrangerartist = GetInt(value["arrangerArtistId"]);
					songdata.arrangergroup = GetInt(value["arrangerGroupId"]);

					songdatamap.Add(key, songdata);
				}

				var allsonglist = new List<AllSongListData>();
				foreach(KeyValuePair<string, JsonElement> pair in animemap) {
					var animedata = new AnimeData();
					int key = GetInt(pair.Key);
					var value = GetDict(pair.Value);
					var names = value["names"].Deserialize<List<Dictionary<string, string>>>();
					var tempnames = new List<string>();
					foreach(var obj in names)
						tempnames.Add(obj["name"]);
					animedata.names = tempnames.ToArray();
					animedata.nameslower = tempnames.ConvertAll(s => RemoveDiacritics(s).ToLower()).ToArray();
					var mainnames = value["mainNames"].Deserialize<Dictionary<string, string>>();
					animedata.nameja = mainnames["JA"] ?? string.Empty;
					animedata.nameen = mainnames["EN"] ?? string.Empty;
					if(animedata.nameja.Length < 1) animedata.nameja = animedata.nameen;
					else if(animedata.nameen.Length < 1) animedata.nameen = animedata.nameja;
					animedata.annid = GetInt(value["annId"]);
					animedata.year = GetInt(value["year"]);
					animedata.seasonid = GetInt(value["seasonId"]);
					var category = value["category"].Deserialize<Dictionary<string, JsonElement>>();
					var cname = category["name"].GetString();
					var cnum = GetStr(category["number"]);
					animedata.category = string.Join(" ", cname, string.IsNullOrEmpty(cnum) ? "" : (cnum.Length > 1 && cnum[cnum.Length - 2] == '.' && cnum[cnum.Length - 1] == '0' ? cnum.Substring(0, cnum.Length - 2) : cnum));
					animedatamap.Add(key, animedata);

					var songlinks = value["songLinks"].Deserialize<Dictionary<string, JsonElement>>();
					ProcessSongLinks(songlinks["OP"], allsonglist, animedata, songdatamap, key);
					ProcessSongLinks(songlinks["ED"], allsonglist, animedata, songdatamap, key);
					ProcessSongLinks(songlinks["INS"], allsonglist, animedata, songdatamap, key);
				}

				animeDataMap = animedatamap;
				songDataMap = songdatamap;
				artistDataMap = artistdatamap;
				groupDataMap = groupdatamap;
				allSongList = allsonglist;

				SaveCacheData();
				strSongName = strArtist = strAnime = string.Empty;
				inited = true;
				return true;
			}
			catch { return false; }
		}
		void SaveCacheData() {
			using(FileStream fs = new FileStream(binpath, FileMode.Create, FileAccess.Write))
			using(BinaryWriter writer = new BinaryWriter(fs)) {
				timeLastUpdate = DateTime.Now;
				writer.Write(timeLastUpdate.ToUniversalTime().ToBinary());
				writer.Write(animeDataMap.Count);
				foreach(var data in animeDataMap) {
					writer.Write(data.Key);
					data.Value.Save(writer);
				}
				writer.Write(songDataMap.Count);
				foreach(var data in songDataMap) {
					writer.Write(data.Key);
					data.Value.Save(writer);
				}
				writer.Write(artistDataMap.Count);
				foreach(var data in artistDataMap) {
					writer.Write(data.Key);
					data.Value.Save(writer);
				}
				writer.Write(groupDataMap.Count);
				foreach(var data in groupDataMap) {
					writer.Write(data.Key);
					data.Value.Save(writer);
				}
				writer.Write(allSongList.Count);
				foreach(var data in allSongList)
					data.Save(writer);
			}
		}
		bool LoadCacheData() {
			try {
				using(FileStream fs = new FileStream(binpath, FileMode.Open, FileAccess.Read))
				using(BinaryReader reader = new BinaryReader(fs)) {
					long binarytime = reader.ReadInt64();
					var count = reader.ReadInt32();
					for(int i = 0; i < count; i++) {
						var key = reader.ReadInt32();
						var value = AnimeData.Load(reader);
						animeDataMap.Add(key, value);
					}
					count = reader.ReadInt32();
					for(int i = 0; i < count; i++) {
						var key = reader.ReadInt32();
						var value = SongData.Load(reader);
						songDataMap.Add(key, value);
					}
					count = reader.ReadInt32();
					for(int i = 0; i < count; i++) {
						var key = reader.ReadInt32();
						var value = ArtistData.Load(reader);
						artistDataMap.Add(key, value);
					}
					count = reader.ReadInt32();
					for(int i = 0; i < count; i++) {
						var key = reader.ReadInt32();
						var value = GroupData.Load(reader);
						groupDataMap.Add(key, value);
					}
					count = reader.ReadInt32();
					for(int i = 0; i < count; i++) {
						var data = AllSongListData.Load(reader);
						allSongList.Add(data);
					}
					timeLastUpdate = DateTime.FromBinary(binarytime).ToLocalTime();
					strSongName = strArtist = strAnime = string.Empty;
					inited = true;
					return true;
				}
			}
			catch { return false; }
		}

		private void ProcessSongLinks(JsonElement element, List<AllSongListData> allsonglist, AnimeData animedata, Dictionary<int, SongData> songdatamap, int animeid) {
			var songlinks = element.Deserialize<Dictionary<string, int>[]>();
			foreach(var songlink in songlinks) {
				if(songlink["uploaded"] == 0) continue;
				var allsonglistdata = new AllSongListData();
				var songid = songlink["songId"];
				var songdata = songdatamap[songid];

				allsonglistdata.animeid = animeid;
				allsonglistdata.songid = songid;
				allsonglistdata.artistid = songdata.artist;
				allsonglistdata.groupid = songdata.group;
				allsonglistdata.type = songlink["type"];
				allsonglistdata.number = songlink["number"];
				allsonglistdata.annsongid = songlink["annSongId"];
				allsonglistdata.standard = songdata.standard;
				allsonglistdata.instrumental = songdata.instrumental;
				allsonglistdata.chanting = songdata.chanting;
				allsonglistdata.character = songdata.character;
				allsonglistdata.dub = songlink["dub"] == 1;
				allsonglistdata.rebroad = songlink["rebroadcast"] == 1;
				allsonglistdata.animecategory = animedata.category;
				allsonglist.Add(allsonglistdata);
			}
		}

		private bool IsMatch(string a, string b) {
			if(string.IsNullOrEmpty(b)) return true;
			if(string.IsNullOrEmpty(a)) return false;

			int pointerB = 0;
			for(int pointerA = 0; pointerA < a.Length; pointerA++) {
				if(a[pointerA] == b[pointerB]) {
					pointerB++;
					if(pointerB == b.Length) return true;
				}
				else if(char.IsLetterOrDigit(a[pointerA])) {
					if(pointerB > 0)
						pointerB = (a[pointerA] == b[0]) ? 1 : 0;
				}
			}
			return false;
		}
		private static readonly char[] WordSeparators = new char[] { ' ' };
		public bool IsMatchAllWords(string a, string b) {
			if(string.IsNullOrEmpty(b)) return true;
			if(string.IsNullOrEmpty(a)) return false;
			string[] words = b.Split(WordSeparators, StringSplitOptions.RemoveEmptyEntries);
			for(int i = 0; i < words.Length; i++) {
				if(!IsMatch(a, words[i])) return false;
			}
			return true;
		}
		private HashSet<T> GetIntersection<T>(HashSet<T> source, HashSet<T> other) {
			if(source.Count > other.Count) {
				var result = new HashSet<T>(other, source.Comparer);
				result.IntersectWith(source);
				return result;
			}
			else {
				var result = new HashSet<T>(source, source.Comparer);
				result.IntersectWith(other);
				return result;
			}
		}
		public string RemoveDiacritics(string text) {
			if(string.IsNullOrEmpty(text)) return text;
			string normalizedString = text.Normalize(NormalizationForm.FormD);
			StringBuilder stringBuilder = new StringBuilder();
			for(int i = 0; i < normalizedString.Length; i++) {
				char c = normalizedString[i];
				if(CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
					stringBuilder.Append(c);
			}
			return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
		}

		void CheckMatchAnime() {
			if(!isAnimeDirty) return;
			isAnimeDirty = false;
			var str = strAnime;
			matchedAnimeNames.Clear();
			str=RemoveDiacritics(str.ToLower());
			foreach(var animedata in animeDataMap) {
				if(animedata.Value.names == null) continue;
				foreach(var name in animedata.Value.nameslower) {
					if(IsMatchAllWords(name, str)) {
						matchedAnimeNames.Add(animedata.Key);
						break;
					}
				}
			}
		}
		void CheckMatchArtist() {
			if(!isArtistDirty) return;
			isArtistDirty = false;
			var str = strArtist;
			matchedArtistNames.Clear();
			matchedGroupNames.Clear();
			str = RemoveDiacritics(str.ToLower());
			foreach(var artist in artistDataMap) {
				if(IsMatchAllWords(artist.Value.namelower, str))
					matchedArtistNames.Add(artist.Key);
			}
			foreach(var group in groupDataMap) {
				if(IsMatchAllWords(group.Value.namelower, str)) {
					matchedGroupNames.Add(group.Key);
					continue;
				}
				if(GetIntersection(matchedArtistNames, group.Value.artists).Count > 0)
					matchedGroupNames.Add(group.Key);
			}
		}
		void CheckMatchSongname() {
			if(!isSongNameDirty) return;
			isSongNameDirty = false;
			var str = strSongName;
			matchedSongNames.Clear();
			str = RemoveDiacritics(str.ToLower());
			foreach(var songdata in songDataMap) {
				if(IsMatchAllWords(songdata.Value.namelower, str))
					matchedSongNames.Add(songdata.Key);
			}
		}

		public List<AllSongListData> GetMatchedSongList() {
			var result = new List<AllSongListData>();
			if(!inited) return result;
			CheckMatchSongname();
			CheckMatchArtist();
			CheckMatchAnime();
			result = allSongList.FindAll(x => {
				if(!isOP && x.type == 1) return false;
				if(!isED && x.type == 2) return false;
				if(!isINS && x.type == 3) return false;
				if(!isStandard && x.standard) return false;
				if(!isInstrumental && x.instrumental) return false;
				if(!isChanting && x.chanting) return false;
				if(!isCharacter && x.character) return false;
				if(!isDub && x.dub) return false;
				if(!isRebroad && x.rebroad) return false;
				if(isAnime && !matchedAnimeNames.Contains(x.animeid)) return false;
				if(isSongName && !matchedSongNames.Contains(x.songid)) return false;
				if(isArtist && !(matchedArtistNames.Contains(x.artistid) || matchedGroupNames.Contains(x.groupid))) return false;
				return true;
			});
			return result;
		}
		public AllSongListData GetAllSongListData(int annsongid) {
			return allSongList.Find(x => x.annsongid == annsongid);
		}
	}
}
