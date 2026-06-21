using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Windows.Media.Core;
using Windows.Media.Playback;
using static AMQSongBrowser.DataCache;
using static System.Windows.Forms.AxHost;

namespace AMQSongBrowser {
	public partial class MainForm : Form {
		bool inited = false;
		bool update = false;
		public class UiState {
			public Point windowlocation { get; set; }
			public Size windowsize { get; set; }
			public List<int> columnwidths { get; set; } = new();
			public bool checkSongName { get; set; }
			public bool checkArtist { get; set; }
			public bool checkAnime { get; set; }
			public bool checkOP { get; set; }
			public bool checkED { get; set; }
			public bool checkINS { get; set; }
			public bool checkStandard { get; set; }
			public bool checkInstrumental { get; set; }
			public bool checkChanting { get; set; }
			public bool checkCharacter { get; set; }
			public bool checkDub { get; set; }
			public bool checkRebroad { get; set; }
			public bool radioEnglish { get; set; }
			public bool radioRomaji { get; set; }
			public int volume { get; set; }
			public bool mute { get; set; }
		}
		private readonly string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");

		public MainForm() {
			InitializeComponent();
			InitMediaPlayer();
			radioEnglish.TabStop = false;
			radioRomaji.TabStop = false;
			typeof(System.Windows.Forms.ListView).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(listResult, true, null);
			listResult.VirtualMode = true;
			listResult.VirtualListSize = 0;
			listResult.RetrieveVirtualItem += ListResult_RetrieveVirtualItem;
			httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
		}

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
		List<AllSongListData> matched;
		private void ListResult_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e) {
			lock(lockObj) {
				var data = matched[e.ItemIndex];

				e.Item = new ListViewItem(DataCache.Instance.GetSongData(data.songid).name);
				if(data.artistid > 0) e.Item.SubItems.Add(DataCache.Instance.GetArtistData(data.artistid).name);
				else e.Item.SubItems.Add(DataCache.Instance.GetGroupData(data.groupid).nameartists);
				if(radioEnglish.Checked) e.Item.SubItems.Add(DataCache.Instance.GetAnimeData(data.animeid).nameen);
				else e.Item.SubItems.Add(DataCache.Instance.GetAnimeData(data.animeid).nameja);
				e.Item.SubItems.Add(data.type == 3 ? strtype[data.type] : string.Join(" ", strtype[data.type], data.number));
				e.Item.SubItems.Add(data.animecategory);
			}
		}
		void UpdateList() {
			lock(lockObj) {
				if(!update) return;
				matched = DataCache.Instance.GetMatchedSongList();
				SortResults();
				listResult.VirtualListSize = matched.Count;
			}
			if(DataCache.Instance.inited)
				labelLastUpdate.Text = DataCache.Instance.timeLastUpdate.ToString("g");
			UpdateColumnHeaderSigns();
			listResult.Invalidate();
		}
		private int lastSortColumn = -1;
		private bool isAscending = true;

		public void SortResults() {
			if(lastSortColumn < 0) return;
			matched.Sort((x, y) => {
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
						if(radioEnglish.Checked)
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
		}

		private void OnResultClickColumn(object sender, ColumnClickEventArgs e) {
			if(matched.Count == 0) return;
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
			listResult.Invalidate();
		}
		private void UpdateColumnHeaderSigns() {
			for(int i = 0; i < listResult.Columns.Count; i++) {
				string cleanText = listResult.Columns[i].Text.Replace(" ▲", "").Replace(" ▼", "");
				if(i == lastSortColumn)
					listResult.Columns[i].Text = cleanText + (isAscending ? " ▲" : " ▼");
				else
					listResult.Columns[i].Text = cleanText;
			}
		}

		private void OnKeyDown(object sender, KeyEventArgs e) {
			string send = string.Empty;
			if(e.KeyCode == Keys.PageUp) {
				send = "PGUP";
			}
			else if(e.KeyCode == Keys.PageDown) {
				send = "PGDN";
			}
			else if(e.KeyCode == Keys.Up) {
				send = "UP";
			}
			else if(e.KeyCode == Keys.Down) {
				send = "DOWN";
			}
			else {
				return;
			}
			listResult.Focus();
			SendKeys.SendWait("{" + send + "}");
			e.Handled = true;
		}
		private void OnResultKeyDown(object sender, KeyEventArgs e) {
			if(e.KeyCode == Keys.Enter) {
				if(listResult.SelectedIndices.Count > 0)
					PlaySong(matched[listResult.SelectedIndices[0]]);
			}
		}
		private void OnTextboxEnter(object sender, EventArgs e) {
			if(sender is System.Windows.Forms.TextBox textbox)
				textbox.SelectAll();
		}

		private void OnFormLoad(object sender, EventArgs e) {
			try {
				if(!File.Exists(configPath)) return;
				string json = File.ReadAllText(configPath);
				UiState? state = JsonSerializer.Deserialize<UiState>(json);

				if(state != null) {
					Location = state.windowlocation;
					Size = state.windowsize;

					for(int i = 0; i < state.columnwidths.Count && i < listResult.Columns.Count; i++) {
						listResult.Columns[i].Width = state.columnwidths[i];
					}
					checkSongName.Checked = state.checkSongName;
					checkArtist.Checked = state.checkArtist;
					checkAnime.Checked = state.checkAnime;
					checkOP.Checked = state.checkOP;
					checkED.Checked = state.checkED;
					checkINS.Checked = state.checkINS;
					checkStandard.Checked = state.checkStandard;
					checkInstrumental.Checked = state.checkInstrumental;
					checkChanting.Checked = state.checkChanting;
					checkCharacter.Checked = state.checkCharacter;
					checkDub.Checked = state.checkDub;
					checkRebroad.Checked = state.checkRebroad;
					radioEnglish.Checked = state.radioEnglish;
					radioRomaji.Checked = state.radioRomaji;
					trackVolume.Value = state.volume;
					mediaPlayer.Volume = state.volume / 100.0;
					mediaPlayer.IsMuted = state.mute;
					UpdateMuteButton();
				}
			}
			finally {
				inited = true;
				update = true;
				DataCache.Instance.isSongName = checkSongName.Checked;
				DataCache.Instance.isArtist = checkArtist.Checked;
				DataCache.Instance.isAnime = checkAnime.Checked;
				DataCache.Instance.isOP = checkOP.Checked;
				DataCache.Instance.isED = checkED.Checked;
				DataCache.Instance.isINS = checkINS.Checked;
				DataCache.Instance.isStandard = checkStandard.Checked;
				DataCache.Instance.isInstrumental = checkInstrumental.Checked;
				DataCache.Instance.isChanting = checkChanting.Checked;
				DataCache.Instance.isCharacter = checkCharacter.Checked;
				DataCache.Instance.isDub = checkDub.Checked;
				DataCache.Instance.isRebroad = checkRebroad.Checked;
				DataCache.Instance.isSongNameDirty = true;
				DataCache.Instance.isArtistDirty = true;
				DataCache.Instance.isAnimeDirty = true;
				UpdateList();
			}
		}

		private bool closecompleted = false;
		private async void OnFormClosing(object sender, FormClosingEventArgs e) {
			if(closecompleted) return;
			e.Cancel = true;
			Hide();

			var state = new UiState {
				windowlocation = Location,
				windowsize = Size,
				checkSongName = checkSongName.Checked,
				checkArtist = checkArtist.Checked,
				checkAnime = checkAnime.Checked,
				checkOP = checkOP.Checked,
				checkED = checkED.Checked,
				checkINS = checkINS.Checked,
				checkStandard = checkStandard.Checked,
				checkInstrumental = checkInstrumental.Checked,
				checkChanting = checkChanting.Checked,
				checkCharacter = checkCharacter.Checked,
				checkDub = checkDub.Checked,
				checkRebroad = checkRebroad.Checked,
				radioEnglish = radioEnglish.Checked,
				radioRomaji = radioRomaji.Checked,
				volume = trackVolume.Value,
				mute = mediaPlayer.IsMuted
			};
			foreach(ColumnHeader col in listResult.Columns) {
				state.columnwidths.Add(col.Width);
			}
			string json = JsonSerializer.Serialize(state, new JsonSerializerOptions { WriteIndented = true });
			File.WriteAllText(configPath, json);

			try {
				while(isRetrievingMedia)
					await Task.Delay(100);
				// save medialinkcache!
			}
			finally {
				closecompleted = true;
				DisposeMediaPlayer();
				Close();
			}
		}

		private static readonly HttpClient httpClient = new HttpClient();
		private void OnUpdateCacheClick(object sender, EventArgs e) {
			if(!inited) return;
			using(var progressForm = new UpdateCache(httpClient)) {
				progressForm.StartPosition = FormStartPosition.CenterScreen;
				DialogResult result = progressForm.ShowDialog(this);

				if(result == DialogResult.OK) {
					MessageBox.Show(this, "Update completed!");
					DataCache.Instance.isSongNameDirty = true;
					DataCache.Instance.isArtistDirty = true;
					DataCache.Instance.isAnimeDirty = true;
					UpdateList();
				}
				else if(result == DialogResult.Cancel)
					MessageBox.Show(this, "Update canceled", "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void OnSongNameChanged(object sender, EventArgs e) {
			if(!inited) return;
			DataCache.Instance.isSongName = checkSongName.Checked;
			DataCache.Instance.strSongName = textSongName.Text;
			DataCache.Instance.isSongNameDirty = true;
			UpdateList();
		}

		private void OnArtistChanged(object sender, EventArgs e) {
			if(!inited) return;
			DataCache.Instance.isArtist = checkArtist.Checked;
			DataCache.Instance.strArtist = textArtist.Text;
			DataCache.Instance.isArtistDirty = true;
			UpdateList();
		}

		private void OnAnimeChanged(object sender, EventArgs e) {
			if(!inited) return;
			DataCache.Instance.isAnime = checkAnime.Checked;
			DataCache.Instance.strAnime = textAnime.Text;
			DataCache.Instance.isAnimeDirty = true;
			UpdateList();
		}

		private void OnSongTypeChanged(object sender, EventArgs e) {
			if(!inited) return;
			DataCache.Instance.isOP = checkOP.Checked;
			DataCache.Instance.isED = checkED.Checked;
			DataCache.Instance.isINS = checkINS.Checked;
			DataCache.Instance.isStandard = checkStandard.Checked;
			DataCache.Instance.isInstrumental = checkInstrumental.Checked;
			DataCache.Instance.isChanting = checkChanting.Checked;
			DataCache.Instance.isCharacter = checkCharacter.Checked;
			DataCache.Instance.isDub = checkDub.Checked;
			DataCache.Instance.isRebroad = checkRebroad.Checked;
			UpdateList();
		}

		private void OnEnglishClick(object sender, EventArgs e) {
			listResult.Invalidate();
			if(!inited) return;
		}

		private void OnRomajiClick(object sender, EventArgs e) {
			listResult.Invalidate();
			if(!inited) return;
		}

		private void DisableRadioTabstop(object sender, EventArgs e) {
			if(sender is RadioButton radio) {
				radio.TabStop = false;
			}
		}

		private double[] columnsWidthRatios = new double[3];
		private int fixedWidth = 0;

		private void RecordColumnsWidthRatios() {
			int totalWidth = 0;
			fixedWidth = 0;
			foreach(ColumnHeader col in listResult.Columns) {
				if(col.Index < 3) totalWidth += col.Width;
				else fixedWidth += col.Width;
			}
			if(totalWidth <= 0) return;
			for(int i = 0; i < 3; i++)
				columnsWidthRatios[i] = (double)listResult.Columns[i].Width / totalWidth;
		}
		private void ResizeColumns() {
			if(WindowState == FormWindowState.Minimized) return;
			if(columnsWidthRatios[0] == 0) return;

			int usableWidth = listResult.ClientSize.Width - fixedWidth;
			if(usableWidth <= 0) return;

			for(int i = 0; i < 3; i++) {
				listResult.Columns[i].Width = (int)(usableWidth * columnsWidthRatios[i]);
			}
		}

		private void OnFormResizeBegin(object sender, EventArgs e) {
			RecordColumnsWidthRatios();
		}
		private void OnFormResizeEnd(object sender, EventArgs e) {
			if(!inited) return;
			if(isAdjusting) return;
			isAdjusting = true;
			ResizeColumns();
			isAdjusting = false;
		}

		private bool isAdjusting = false;
		private void OnResultChangingColumnWidth(object sender, ColumnWidthChangingEventArgs e) {
			if(!inited) return;
			if(isAdjusting) return;
			isAdjusting = true;
			try {
				var index = e.ColumnIndex;
				if(index < 4)
					listResult.Columns[index + 1].Width += listResult.Columns[index].Width - e.NewWidth;
			}
			finally {
				isAdjusting = false;
			}
		}

		private void OnResultChangedColumnWidth(object sender, ColumnWidthChangedEventArgs e) {
			if(!inited) return;
			if(isAdjusting) return;
			isAdjusting = true;
			RecordColumnsWidthRatios();
			ResizeColumns();
			isAdjusting = false;
		}

		private void OnResultClick(object sender, MouseEventArgs e) {
			if(!inited) return;
			if(e.Button == MouseButtons.Right) {
				Point mousePos = listResult.PointToClient(Control.MousePosition);
				ListViewHitTestInfo hitTest = listResult.HitTest(mousePos);
				int columnIndex = hitTest.Item.SubItems.IndexOf(hitTest.SubItem);
				update = false;

				int selected = listResult.SelectedIndices[0];
				if(columnIndex > 2) return;

				textSongName.Text = string.Empty; textArtist.Text = string.Empty; textAnime.Text = string.Empty;
				if(columnIndex == 0) {
					textSongName.Text = listResult.Items[selected].SubItems[columnIndex].Text;
					checkSongName.Checked = true;
					textSongName.Focus();
				}
				else if(columnIndex == 1) {
					textArtist.Text = listResult.Items[selected].SubItems[columnIndex].Text;
					checkArtist.Checked = true;
					textArtist.Focus();
				}
				else if(columnIndex == 2) {
					textAnime.Text = listResult.Items[selected].SubItems[columnIndex].Text;
					checkAnime.Checked = true;
					textAnime.Focus();
				}
				DataCache.Instance.isSongNameDirty = true;
				DataCache.Instance.isArtistDirty = true;
				DataCache.Instance.isAnimeDirty = true;
				update = true;
				UpdateList();
			}
		}
		private void OnResultDoubleClick(object sender, MouseEventArgs e) {
			if(!inited) return;
			if(e.Button == MouseButtons.Left) {
				if(listResult.SelectedIndices.Count > 0)
					PlaySong(matched[listResult.SelectedIndices[0]]);
			}
		}
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
			if(keyData == Keys.Escape) {
				if(!this.ContainsFocus)
					return base.ProcessCmdKey(ref msg, keyData);
				update = false;
				textSongName.Text = textArtist.Text = textAnime.Text = string.Empty;
				DataCache.Instance.isSongNameDirty = true;
				DataCache.Instance.isArtistDirty = true;
				DataCache.Instance.isAnimeDirty = true;
				update = true;
				UpdateList();
				return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void OnResultMouseLeave(object sender, EventArgs e) {
			toolTip1.RemoveAll();
		}

		private void OnResultSelectedIndexChanged(object sender, EventArgs e) {
			ShowTooltip();
		}
		private void ShowTooltip() {
			BeginInvoke(new Action(() => {
				if(listResult.SelectedIndices.Count == 0) return;
				var rowIndex = listResult.SelectedIndices[0];
				StringBuilder sb = new StringBuilder();
				var animedata = DataCache.Instance.GetAnimeData(matched[rowIndex].animeid);
				var songdata = DataCache.Instance.GetSongData(matched[rowIndex].songid);
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
				sb.AppendLine($"Category: {matched[rowIndex].animecategory}");
				sb.AppendLine($"Year: {strseason[animedata.seasonid]} {animedata.year}");
				sb.AppendLine($"ANNID: {animedata.annid}");
				var celltext = sb.ToString();
				if(celltext.Length > 0) {
					var rowrect = listResult.GetItemRect(rowIndex, ItemBoundsPortion.Entire);
					toolTip1.Show(celltext, listResult, rowrect.Width, rowrect.Y + rowrect.Height);
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

		private void OnEnterResult(object sender, EventArgs e) {
			ShowTooltip();
		}

		private bool isRetrievingMedia = false;

		private async void PlaySong(AllSongListData allsonglistdata) {
			if(isRetrievingMedia) return;
			isRetrievingMedia = true;
			DisableMediaControls();

			var animedata = DataCache.Instance.GetAnimeData(allsonglistdata.animeid);
			var medialink = MediaLinkCache.Instance.GetMediaLink(allsonglistdata.annsongid);
			if(!await PlayMediaLink(medialink)) {
				await RetrieveAnisongdbInfo(animedata.annid);
				medialink = MediaLinkCache.Instance.GetMediaLink(allsonglistdata.annsongid);
				await PlayMediaLink(medialink);
			}
			isRetrievingMedia = false;
		}
		private int GetInt(in JsonElement element, string key) {
			if(element.TryGetProperty(key, out var value))
				return value.GetInt32();
			return 0;
		}
		private string GetStr(in JsonElement element, string key) {
			if(element.TryGetProperty(key, out var value))
				return value.GetString();
			return string.Empty;
		}
		private async Task RetrieveAnisongdbInfo(int annid) {
			var content = new StringContent(JsonSerializer.Serialize(new { ann_ids = new int[] { annid }, ignore_duplicate = false }), Encoding.UTF8, "application/json");
			HttpResponseMessage response = await httpClient.PostAsync("https://anisongdb.com/api/ann_ids_request", content);
			if(response.IsSuccessStatusCode) {
				string json = await response.Content.ReadAsStringAsync();
				using(JsonDocument doc = JsonDocument.Parse(json)) {
					JsonElement root = doc.RootElement;
					foreach(JsonElement element in root.EnumerateArray()) {
						int annsongid = GetInt(element, "annSongId");
						string audio = GetStr(element, "audio");
						string MQ = GetStr(element, "MQ");
						string HQ = GetStr(element, "HQ");
						MediaLinkCache.Instance.UpdateData(annsongid, audio, string.IsNullOrEmpty(HQ) ? MQ : HQ);
					}
					MediaLinkCache.Instance.SaveCacheData();
				}
			}
		}

		private void DisableMediaControls() {
			isPlaying = false;
			buttonPlay.Enabled = false;
			trackPlayingPos.Enabled = false;
			UpdatePlayButton();
		}
		private void EnableMediaControls() {
			isPlaying = true;
			buttonPlay.Enabled = true;
			trackPlayingPos.Enabled = true;
			UpdatePlayButton();
		}
		private void UpdateMuteButton() {
			buttonMute.Text = mediaPlayer.IsMuted ? "🔇" : buttonMute.Text = "🔊";
		}

		MediaPlayer mediaPlayer;
		private System.Windows.Forms.Timer timerPlayPos = new System.Windows.Forms.Timer();
		private void InitMediaPlayer() {
			mediaPlayer = new MediaPlayer();
			mediaPlayer.MediaEnded += OnMediaEnded;
			mediaPlayer.MediaFailed += OnMediaFailed;
			timerPlayPos.Interval = 200;
			timerPlayPos.Tick += UpdatePlayPos;
		}
		private void DisposeMediaPlayer() {
			if(mediaPlayer != null) {
				mediaPlayer.MediaEnded -= OnMediaEnded;
				mediaPlayer.MediaFailed -= OnMediaFailed;
				mediaPlayer.Source = null;
				mediaPlayer.Dispose();
				mediaPlayer = null;
			}
		}
		private void UpdatePlayPos(object sender, EventArgs e) {
			if(isDraggingPlayPos) return;
			var session = mediaPlayer.PlaybackSession;
			if(session != null && session.PlaybackState == MediaPlaybackState.Playing) {
				double total = session.NaturalDuration.TotalSeconds;
				double current = session.Position.TotalSeconds;
				if(total > 0) {
					trackPlayingPos.Maximum = (int)total;
					trackPlayingPos.Value = (int)current;
				}
			}
		}
		private void OnMediaEnded(MediaPlayer sender, object args) {
			Invoke(new Action(() => {
				StopPlaying();
			}));
		}
		private void OnMediaFailed(MediaPlayer sender, MediaPlayerFailedEventArgs args) {
		}
		private void StopPlaying() {
			timerPlayPos.Stop();
			mediaPlayer.Source = null;
			DisableMediaControls();
		}

		private async Task<bool> PlayMediaLink(string medialink) {
			if(string.IsNullOrEmpty(medialink)) return false;
			try {
				medialink = string.Concat(@"https://naedist.animemusicquiz.com/", medialink);
				mediaPlayer.Source = null;
				var mediaUri = new Uri(medialink);
				var mediaSource = MediaSource.CreateFromUri(mediaUri);
				mediaPlayer.Source = mediaSource;
				await mediaSource.OpenAsync();
				mediaPlayer.Play();
				isPlaying = true;
				trackPlayingPos.Value = 0;
				EnableMediaControls();
				timerPlayPos.Start();
				return true;
			}
			catch {
				StopPlaying();
				return false;
			}
		}

		private bool isPlaying = false;
		private void OnPauseResumeClick(object sender, EventArgs e) {
			var session = mediaPlayer.PlaybackSession;
			if(session != null) {
				if(session.PlaybackState == MediaPlaybackState.Playing) {
					mediaPlayer.Pause();
					isPlaying = false;
				}
				else {
					mediaPlayer.Play();
					isPlaying = true;
				}
			}
			UpdatePlayButton();
		}
		private void UpdatePlayButton() {
			buttonPlay.Text = isPlaying ? "||" : "▶️";
		}

		private void OnMuteClick(object sender, EventArgs e) {
			mediaPlayer.IsMuted = !mediaPlayer.IsMuted;
			UpdateMuteButton();
		}

		private void OnVolumeMouseDown(object sender, MouseEventArgs e) {
			SyncTrackBarClickLocation(trackVolume, e);
			mediaPlayer.Volume = trackVolume.Value / 100.0;
		}
		private void OnVolumeValueChanged(object sender, EventArgs e) {
			mediaPlayer.Volume = trackVolume.Value / 100.0;
		}

		private bool isDraggingPlayPos = false;
		private void OnPlayPosMouseDown(object sender, MouseEventArgs e) {
			isDraggingPlayPos = true;
			SyncTrackBarClickLocation(trackPlayingPos, e);
		}
		private void OnPlayPosMouseUp(object sender, MouseEventArgs e) {
			var session = mediaPlayer.PlaybackSession;
			if(session != null) {
				session.Position = TimeSpan.FromSeconds(trackPlayingPos.Value);
			}
			isDraggingPlayPos = false;
		}
		private void SyncTrackBarClickLocation(System.Windows.Forms.TrackBar trackbar, MouseEventArgs e) {
			int trackWidth = trackbar.Width - 20;
			int mouseX = e.X - 10;

			if(mouseX < 0) mouseX = 0;
			if(mouseX > trackWidth) mouseX = trackWidth;

			double percent = (double)mouseX / trackWidth;
			int newValue = (int)(trackbar.Minimum + (percent * (trackbar.Maximum - trackbar.Minimum)));

			trackbar.Value = newValue;
		}
	}
}
