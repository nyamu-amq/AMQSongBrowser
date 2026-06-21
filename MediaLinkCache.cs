namespace AMQSongBrowser {
	internal class MediaLinkCache {
		private static readonly Lazy<MediaLinkCache> _instance = new Lazy<MediaLinkCache>(() => new MediaLinkCache());
		public static MediaLinkCache Instance => _instance.Value;
		private MediaLinkCache() {
			Init();
		}

		private readonly string binpath = @"medialinkcache.bin";
		public bool inited = false;
		private void Init() {
			LoadCacheData();
		}

		private Dictionary<int, string[]> mediaLinks = new Dictionary<int, string[]>();

		public void SaveCacheData() {
			try {
				using(FileStream fs = new FileStream(binpath, FileMode.Create, FileAccess.Write))
				using(BinaryWriter writer = new BinaryWriter(fs)) {
					writer.Write(mediaLinks.Count);
					foreach(var kvp in mediaLinks) {
						writer.Write(kvp.Key);
						writer.Write(kvp.Value[0]);
						writer.Write(kvp.Value[1]);
					}
				}
			}
			catch { }
		}
		private void LoadCacheData() {
			try {
				using(FileStream fs = new FileStream(binpath, FileMode.Open, FileAccess.Read))
				using(BinaryReader reader = new BinaryReader(fs)) {
					var medialinks = new Dictionary<int, string[]>();
					int count = reader.ReadInt32();
					for(int i = 0; i < count; i++) {
						int annsongid = reader.ReadInt32();
						string audio = reader.ReadString();
						string video = reader.ReadString();

						medialinks.Add(annsongid, [audio, video]);
					}
					mediaLinks = medialinks;
				}
			}
			catch { }
		}

		public string GetMediaLink(int annsongid) {
			if(mediaLinks.ContainsKey(annsongid)) {
				if(string.IsNullOrEmpty(mediaLinks[annsongid][0]))
					return mediaLinks[annsongid][1];
				return mediaLinks[annsongid][0];
			}
			return string.Empty;
		}
		public string GetAudioLink(int annsongid) {
			if(mediaLinks.ContainsKey(annsongid))
				return mediaLinks[annsongid][0];
			return string.Empty;
		}
		public string GetVideoLink(int annsongid) {
			if(mediaLinks.ContainsKey(annsongid))
				return mediaLinks[annsongid][1];
			return string.Empty;
		}
		public void Invalidate(int annsongid) {
			if(mediaLinks.ContainsKey(annsongid))
				mediaLinks.Remove(annsongid);
		}
		public void UpdateData(int annsongid, string audio, string video) {
		if(annsongid < 1) return;
			string[] links = [audio ?? "", video ?? ""];
			if(mediaLinks.ContainsKey(annsongid))
				mediaLinks[annsongid] = links;
			else
				mediaLinks.Add(annsongid, links);
		}
	}
}
