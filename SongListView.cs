using System.Reflection;
using System.Text;
using Windows.Media.Playback;
using static AMQSongBrowser.DataCache;
using System.ComponentModel;

namespace AMQSongBrowser {
	public class SongListView : ListView {
		public SongListView() {
			typeof(System.Windows.Forms.ListView).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(this, true, null);
			VirtualMode = true;
			VirtualListSize = 0;
			RetrieveVirtualItem += ListResult_RetrieveVirtualItem;

			ColumnClick += OnClickColumn;
			ColumnWidthChanged += OnChangedColumnWidth;
			ColumnWidthChanging += OnChangingColumnWidth;
			SelectedIndexChanged += OnSelectedIndexChanged;
			Enter += OnEnter;
			MouseLeave += OnMouseLeave;

			toolTip = new ToolTip();
		}

		private ToolTip toolTip;
		public bool inited = false;
		public bool update = false;

		Dictionary<int, string> strtype = new Dictionary<int, string>() {
			{ 1,"Opening"},
			{ 2,"Ending"},
			{ 3,"Insert"}
		};
		Dictionary<int, string> strseason = new Dictionary<int, string>() {
			{ 0,"Winter"},
			{ 1,"Spring"},
			{ 2,"Summer"},
			{ 3,"Fall"}
		};

		private readonly object lockObj = new object();
		List<AllSongListData> songlist;
		private void ListResult_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e) {
			lock(lockObj) {
				var data = songlist[e.ItemIndex];

				e.Item = new ListViewItem(DataCache.Instance.GetSongData(data.songid).name);
				if(data.artistid > 0) e.Item.SubItems.Add(DataCache.Instance.GetArtistData(data.artistid).name);
				else e.Item.SubItems.Add(DataCache.Instance.GetGroupData(data.groupid).nameartists);
				if(MainForm.Instance.IsEnglish) e.Item.SubItems.Add(DataCache.Instance.GetAnimeData(data.animeid).nameen);
				else e.Item.SubItems.Add(DataCache.Instance.GetAnimeData(data.animeid).nameja);
				e.Item.SubItems.Add(data.type == 3 ? strtype[data.type] : string.Join(" ", strtype[data.type], data.number));
				e.Item.SubItems.Add(data.animecategory);
				e.Item.BackColor = data == playingSong ? IconContainer.IsDark ? Color.DarkRed: Color.Yellow : SystemColors.Window;
			}
		}
		public void UpdateList(List<AllSongListData> newlist) {
			lock(lockObj) {
				if(!update) return;
				songlist = newlist;
				SortResults();
				VirtualListSize = songlist.Count;
			}
			UpdateColumnHeaderSigns();
			Invalidate();
		}

		public AllSongListData GetFocusedSongData() {
			if(FocusedItem == null) return null;
			return songlist[FocusedItem.Index];
		}
		public List<AllSongListData> GetSelectedSongList() {
			if(SelectedIndices.Count > 0) {
				var ret = new List<AllSongListData>();
				for(var i=0;i<SelectedIndices.Count;i++)
					ret.Add(songlist[SelectedIndices[i]].Clone());
				return ret;
			}
			return null;
		}

		private int lastSortColumn = -1;
		private bool isAscending = true;
		private void UpdateColumnHeaderSigns() {
			for(int i = 0; i < Columns.Count; i++) {
				string cleanText = Columns[i].Text.Replace(" ▲", "").Replace(" ▼", "");
				if(i == lastSortColumn)
					Columns[i].Text = cleanText + (isAscending ? " ▲" : " ▼");
				else
					Columns[i].Text = cleanText;
			}
		}
		private void OnClickColumn(object sender, ColumnClickEventArgs e) {
			if(songlist.Count == 0) return;
			if(e.Column == lastSortColumn)
				isAscending = !isAscending;
			else {
				isAscending = true;
				lastSortColumn = e.Column;
			}
			lock(lockObj) {
				SortResults();
			}
			UpdateColumnHeaderSigns();
			Invalidate();
		}
		private bool isAdjusting = false;
		private void OnChangingColumnWidth(object sender, ColumnWidthChangingEventArgs e) {
			if(!inited) return;
			if(isAdjusting) return;
			isAdjusting = true;
			try {
				var index = e.ColumnIndex;
				if(index < 4)
					Columns[index + 1].Width += Columns[index].Width - e.NewWidth;
			}
			finally {
				isAdjusting = false;
			}
		}
		private void OnChangedColumnWidth(object sender, ColumnWidthChangedEventArgs e) {
			if(!inited) return;
			if(isAdjusting) return;
			isAdjusting = true;
			RecordColumnsWidthRatios();
			ResizeColumns();
			isAdjusting = false;
		}
		private double[] columnsWidthRatios = new double[3];
		private int fixedWidth = 0;

		public void RecordColumnsWidthRatios() {
			int totalWidth = 0;
			fixedWidth = 0;
			foreach(ColumnHeader col in Columns) {
				if(col.Index < 3) totalWidth += col.Width;
				else fixedWidth += col.Width;
			}
			if(totalWidth <= 0) return;
			for(int i = 0; i < 3; i++)
				columnsWidthRatios[i] = (double)Columns[i].Width / totalWidth;
		}
		public void ResizeColumns() {
			if(!inited) return;
			if(isAdjusting) return;
			isAdjusting = true;
			if(columnsWidthRatios[0] == 0) return;

			int usableWidth = ClientSize.Width - fixedWidth;
			if(usableWidth <= 0) return;

			for(int i = 0; i < 3; i++) {
				Columns[i].Width = (int)(usableWidth * columnsWidthRatios[i]);
			}
			isAdjusting = false;
		}

		public void SortResults() {
			if(lastSortColumn < 0) return;
			songlist.Sort((x, y) => {
				int result = 0;
				switch(lastSortColumn) {
					case 0:
						result = string.Compare(DataCache.Instance.GetSongData(x.songid).name, DataCache.Instance.GetSongData(y.songid).name);
						break;
					case 1:
						var strx = (x.artistid == 0) ? DataCache.Instance.GetGroupData(x.groupid).nameartists : DataCache.Instance.GetArtistData(x.artistid).name;
						var stry = (y.artistid == 0) ? DataCache.Instance.GetGroupData(y.groupid).nameartists : DataCache.Instance.GetArtistData(y.artistid).name;
						result = string.Compare(strx, stry);
						break;
					case 2:
						if(MainForm.Instance.IsEnglish)
							result = string.Compare(DataCache.Instance.GetAnimeData(x.animeid).nameen, DataCache.Instance.GetAnimeData(y.animeid).nameen);
						else
							result = string.Compare(DataCache.Instance.GetAnimeData(x.animeid).nameja, DataCache.Instance.GetAnimeData(y.animeid).nameja);
						break;
					case 3:
						result = (x.type * 1000 + x.number).CompareTo(y.type * 1000 + y.number);
						break;
					case 4:
						result = string.Compare(x.animecategory, y.animecategory);
						break;
				}

				return isAscending ? result : -result;
			});
			UpdatePlayingIndex();
		}
		private void OnMouseLeave(object sender, EventArgs e) {
			toolTip.RemoveAll();
		}
		private void OnSelectedIndexChanged(object sender, EventArgs e) {
			ShowTooltip();
		}
		private void ShowTooltip() {
			BeginInvoke(new Action(() => {
				if(FocusedItem == null) return;
				//if(SelectedIndices.Count == 0) return;
				var rowIndex = FocusedItem.Index;
				StringBuilder sb = new StringBuilder();
				var animedata = DataCache.Instance.GetAnimeData(songlist[rowIndex].animeid);
				var songdata = DataCache.Instance.GetSongData(songlist[rowIndex].songid);
				sb.AppendLine($"Song Name: {songdata.name}");
				sb.AppendLine();
				AppendArtist(sb, "Artist", songdata.artist);
				AppendGroup(sb, "Artist", songdata.group);
				AppendArtist(sb, "Composer", songdata.composerartist);
				AppendGroup(sb, "Composer", songdata.composergroup);
				AppendArtist(sb, "Arranger", songdata.arrangerartist);
				AppendGroup(sb, "Arranger", songdata.arrangergroup);
				sb.AppendLine();
				sb.AppendLine($"Anime English: {animedata.nameen}");
				sb.AppendLine($"Anime Romaji: {animedata.nameja}");
				sb.AppendLine();
				sb.AppendLine($"Category: {songlist[rowIndex].animecategory}");
				sb.AppendLine($"Year: {strseason[animedata.seasonid]} {animedata.year}");
				sb.AppendLine($"ANNID: {animedata.annid}");
				var celltext = sb.ToString();
				if(celltext.Length > 0) {
					var rowrect = GetItemRect(rowIndex, ItemBoundsPortion.Entire);
					toolTip.Show(celltext, this, rowrect.Width, rowrect.Y + rowrect.Height);
				}
			}));
		}
		private void AppendArtist(StringBuilder sb, string label, int id) {
			if(id > 0)
				sb.AppendLine($"{label}: {DataCache.Instance.GetArtistData(id).name}");
		}
		private void AppendGroup(StringBuilder sb, string label, int id) {
			if(id > 0) {
				var groupdata = DataCache.Instance.GetGroupData(id);
				sb.AppendLine($"{label}: {groupdata.name}");
				foreach(var artist in groupdata.artists)
					sb.AppendLine($"    {DataCache.Instance.GetArtistData(artist).name}");
			}
		}

		private void OnEnter(object sender, EventArgs e) {
			ShowTooltip();
		}

		AllSongListData playingSong = null;
		int playingIndex = -1;
		public int CurrentPlayingIndex { get { return playingIndex; } }
		public void SetPlayingSong(AllSongListData songdata) { playingSong = songdata; UpdatePlayingIndex(); Invalidate(); }
		public void UpdatePlayingIndex() { playingIndex = songlist.IndexOf(playingSong); }
		public List<AllSongListData> GetSongList { get { return songlist; } }
	}
}
