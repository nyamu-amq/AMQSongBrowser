using System.Security.Policy;
using System.Text;
using System.Text.Json;
using Windows.Media.Playback;
using static AMQSongBrowser.DataCache;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Timer = System.Windows.Forms.Timer;

namespace AMQSongBrowser {
	public partial class MusicPlayerForm : Form {
		public MusicPlayerForm(HttpClient httpclient) {
			InitializeComponent();
			InitMediaPlayer();
			httpClient = httpclient;
		}

		private ContextMenuStrip contextmenu;
		private void InitContextMenu() {
			contextmenu = new ContextMenuStrip();
			//contextmenu.Items.Add(new ToolStripMenuItem("Play", null, PlaySong, Keys.Enter));

			((ToolStripMenuItem)contextmenu.Items.Add("Play", null, PlaySong)).ShortcutKeyDisplayString = "Enter";
			((ToolStripMenuItem)contextmenu.Items.Add("Pause / Resume", null, PauseResume)).ShortcutKeyDisplayString = "Space";
			((ToolStripMenuItem)contextmenu.Items.Add("Play Previous Song", null, OnPrevClick)).ShortcutKeyDisplayString = "Q";
			((ToolStripMenuItem)contextmenu.Items.Add("Play Next Song", null, OnNextClick)).ShortcutKeyDisplayString = "W";
			((ToolStripMenuItem)contextmenu.Items.Add("Remove", null, RemoveSongs)).ShortcutKeyDisplayString = "Delete";
			((ToolStripMenuItem)contextmenu.Items.Add("Copy", null, CopySong)).ShortcutKeyDisplayString = "Insert";
			((ToolStripMenuItem)contextmenu.Items.Add("Move Up", null, MoveUp)).ShortcutKeyDisplayString = "Alt+Up";
			((ToolStripMenuItem)contextmenu.Items.Add("Move Down", null, MoveDown)).ShortcutKeyDisplayString = "Alt+Down";

			contextmenu.Opening += (s, e) => { e.Cancel = listSongs.SelectedIndices.Count == 0; };
			listSongs.ContextMenuStrip = contextmenu;
		}
		private void PlaySong(object sender, EventArgs e) {
			var focusedsongdata = listSongs.GetFocusedSongData();
			if(focusedsongdata != null)
				PlaySong(focusedsongdata);
		}
		private void PauseResume(object sender, EventArgs e) {
			OnPauseResumeClick(sender, e);
		}
		private void RemoveSongs(object sender, EventArgs e) {
			var focusedindex = listSongs.FocusedItem?.Index ?? -1;
			for(var i = listSongs.SelectedIndices.Count - 1; i >= 0; i--)
				songlist.RemoveAt(listSongs.SelectedIndices[i]);
			UpdateList();
			if(songlist.Count > 0) {
				focusedindex = Math.Min(focusedindex, songlist.Count - 1);
				listSongs.SelectedIndices.Clear();
				listSongs.Items[focusedindex].Focused = true;
				listSongs.Items[focusedindex].Selected = true;
				listSongs.EnsureVisible(focusedindex);
			}
		}
		private void CopySong(object sender, EventArgs e) {
			var focusedindex = listSongs.FocusedItem?.Index ?? -1;
			if(focusedindex > -1) {
				var temp = songlist[focusedindex].Clone();
				songlist.Insert(focusedindex+1, temp);
				UpdateList();
			}
		}
		private void MoveUp(object sender, EventArgs e) {
			var focusedindex = listSongs.FocusedItem?.Index ?? -1;
			if(songlist.Count > 0) {
				focusedindex = Math.Min(focusedindex, songlist.Count - 1);
				if(focusedindex > 0) {
					var temp = songlist[focusedindex];
					songlist[focusedindex] = songlist[focusedindex - 1];
					songlist[focusedindex - 1] = temp;
					listSongs.SelectedIndices.Clear();
					listSongs.Items[focusedindex - 1].Focused = true;
					listSongs.Items[focusedindex - 1].Selected = true;
					listSongs.EnsureVisible(focusedindex - 1);
				}
			}
		}
		private void MoveDown(object sender, EventArgs e) {
			var focusedindex = listSongs.FocusedItem?.Index ?? -1;
			if(songlist.Count > 0) {
				focusedindex = Math.Min(focusedindex, songlist.Count - 1);
				if(focusedindex < songlist.Count - 1) {
					var temp = songlist[focusedindex];
					songlist[focusedindex] = songlist[focusedindex + 1];
					songlist[focusedindex + 1] = temp;
					listSongs.SelectedIndices.Clear();
					listSongs.Items[focusedindex + 1].Focused = true;
					listSongs.Items[focusedindex + 1].Selected = true;
					listSongs.EnsureVisible(focusedindex + 1);
				}
			}
		}

		protected override bool ShowWithoutActivation { get { return true; } }

		bool IsShuffle = false;
		int RepeatType = 0;
		HttpClient httpClient;
		List<AllSongListData> songlist = new List<AllSongListData>();
		public bool inited = false;
		public void LoadState(MainForm.UiState state) {
			try {
				Location = state.playerwindowlocation;
				Size = state.playerwindowsize;
				trackVolume.Value = state.volume;
				musicPlayer.Volume = state.volume;
				musicPlayer.isMuted = state.mute;
				IsShuffle = state.shuffle;
				RepeatType = state.repeattype;

				for(int i = 0; i < state.playercolumnwidths.Count && i < listSongs.Columns.Count; i++) {
					listSongs.Columns[i].Width = state.playercolumnwidths[i];
				}
			}
			catch { }
		}
		public void SaveState(MainForm.UiState state) {
			state.playerwindowlocation = Location;
			state.playerwindowsize = Size;
			state.volume = trackVolume.Value;
			state.mute = musicPlayer.isMuted;
			state.shuffle = IsShuffle;
			state.repeattype = RepeatType;

			foreach(ColumnHeader col in listSongs.Columns)
				state.playercolumnwidths.Add(col.Width);
		}
		private void OnFormLoad(object sender, EventArgs e) {
			UpdateList();
		}
		public void Init() {
			buttonPlay.Image = IconContainer.Get("play");
			buttonPrev.Image = IconContainer.Get("prev");
			buttonNext.Image = IconContainer.Get("next");
			UpdateMuteButton();
			UpdateShuffleButton();
			UpdateRepeatButton();
			InitContextMenu();
			inited = true;
			listSongs.inited = true;
			listSongs.update = true;
			LoadPlaylist();
		}

		private readonly string playlistpath = @"playlist.bin";
		private bool LoadPlaylist() {
			try {
				using(FileStream fs = new FileStream(playlistpath, FileMode.Open, FileAccess.Read))
				using(BinaryReader reader = new BinaryReader(fs)) {
					var count = reader.ReadInt32();
					for(int i = 0; i < count; i++) {
						var data = AllSongListData.Load(reader);
						songlist.Add(data);
					}
					return true;
				}
			}
			catch { return false; }
		}
		private void SavePlaylist() {
			using(FileStream fs = new FileStream(playlistpath, FileMode.Create, FileAccess.Write))
			using(BinaryWriter writer = new BinaryWriter(fs)) {
				writer.Write(songlist.Count);
				foreach(var data in songlist)
					data.Save(writer);
			}
		}

		private void OnFormClosing(object sender, FormClosingEventArgs e) {
			if(e.CloseReason == CloseReason.UserClosing) {
				e.Cancel = true;
				Hide();
			}
		}
		public async Task DisposeAll() {
			try {
				SavePlaylist();
				while(isRetrievingMedia)
					await Task.Delay(100);
			}
			finally {
				DisposeMediaPlayer();
			}
		}

		void UpdateList() {
			listSongs.UpdateList(songlist);
		}

		public void LanguageChanged() {
			listSongs.Invalidate();
		}

		private void OnFormResizeBegin(object sender, EventArgs e) {
			listSongs.RecordColumnsWidthRatios();
		}
		private void OnFormResizeEnd(object sender, EventArgs e) {
			if(!inited) return;
			listSongs.ResizeColumns();
		}

		private void OnListKeyDown(object sender, KeyEventArgs e) {
			if(e.KeyCode == Keys.Enter) {
				var songdata = listSongs.GetFocusedSongData();
				if(songdata != null) PlaySong(songdata);
			}
			else if(e.KeyCode == Keys.Space) OnPauseResumeClick(sender, e);
			else if(e.KeyCode == Keys.Q) OnPrevClick(sender, e);
			else if(e.KeyCode == Keys.W) OnNextClick(sender, e);
			else if(e.KeyCode == Keys.Delete) RemoveSongs(sender, e);
			else if(e.KeyCode == Keys.Insert) CopySong(sender, e);
			else if(e.Alt && e.KeyCode == Keys.Up) MoveUp(sender, e);
			else if(e.Alt && e.KeyCode == Keys.Down) MoveDown(sender, e);
			else return;
			e.Handled = true;
			e.SuppressKeyPress = true;
		}
		private void OnListClick(object sender, MouseEventArgs e) {
			if(!inited) return;
			if(e.Button == MouseButtons.Right) {
				var songdata = listSongs.GetFocusedSongData();
				//if(songdata != null) PlaySong(songdata);
			}
		}
		private void OnListDoubleClick(object sender, MouseEventArgs e) {
			if(!inited) return;
			if(e.Button == MouseButtons.Left) {
				var songdata = listSongs.GetFocusedSongData();
				if(songdata != null) PlaySong(songdata);
			}
		}

		public void AddToPlaylistAndPlay(List<AllSongListData> songstoadd) {
			if(songstoadd.Count > 0) {
				AddToPlaylist(songstoadd);
				PlaySong(songstoadd[0]);
			}
		}
		public void AddToPlaylist(List<AllSongListData> songstoadd) {
			if(songstoadd.Count > 0) {
				songlist.AddRange(songstoadd);
				listSongs.UpdateList(songlist);
				listSongs.EnsureVisible(songlist.Count - 1);
			}
		}

		private bool isRetrievingMedia = false;
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
		private async void PlaySong(AllSongListData allsonglistdata) {
			if(isRetrievingMedia) return;
			isRetrievingMedia = true;
			StopPlaying();

			var animedata = DataCache.Instance.GetAnimeData(allsonglistdata.animeid);
			var medialink = MediaLinkCache.Instance.GetMediaLink(allsonglistdata.annsongid);
			if(!await PlayMediaLink(medialink)) {
				await RetrieveAnisongdbInfo(animedata.annid);
				medialink = MediaLinkCache.Instance.GetMediaLink(allsonglistdata.annsongid);
				await PlayMediaLink(medialink);
			}
			isRetrievingMedia = false;
			listSongs.SetPlayingSong(allsonglistdata);
			var songdata = DataCache.Instance.GetSongData(allsonglistdata.songid);
			var artistname = songdata.artist > 0 ? DataCache.Instance.GetArtistData(songdata.artist).name : DataCache.Instance.GetGroupData(songdata.group).name;
			labelSongname.Text = string.Join(" - ", songdata.name, artistname);
		}

		private MusicPlayer musicPlayer = new MusicPlayer();
		private Timer timerPlayPos = new Timer();
		private void InitMediaPlayer() {
			timerPlayPos.Interval = 200;
			timerPlayPos.Tick += UpdatePlayPos;
			musicPlayer.PlaybackEnded += (s, ev) => {
				Invoke(() => {
					timerPlayPos.Stop();
					OnMediaEnded(s, ev);
					UpdateMediaControls();
				});
			};
		}
		private void DisposeMediaPlayer() {
			if(timerPlayPos != null) {
				timerPlayPos.Stop();
				timerPlayPos.Tick -= UpdatePlayPos;
				timerPlayPos.Dispose();
				timerPlayPos = null;
			}
			if(musicPlayer != null) {
				musicPlayer.Stop();
				musicPlayer.Dispose();
				musicPlayer.PlaybackEnded -= OnMediaEnded;
				musicPlayer = null;
			}
		}
		private void OnMediaEnded(object sender, EventArgs e) {
			var nextsongindex = GetNextSongIndex();
			if(nextsongindex == -1) StopPlaying();
			else if(nextsongindex == listSongs.CurrentPlayingIndex) {
				timerPlayPos.Start();
				musicPlayer.Seek(0);
				musicPlayer.TogglePlayPause();
			}
			else PlaySong(songlist[nextsongindex]);
		}
		private int GetNextSongIndex() {
			var currentsongindex = listSongs.CurrentPlayingIndex;
			if(currentsongindex < 0) return -1;
			if(RepeatType == 1) return currentsongindex;
			if(IsShuffle) return Random.Shared.Next(0, songlist.Count - 1);
			if(currentsongindex == songlist.Count - 1) {
				if(RepeatType == 0) return -1;
				return 0;
			}
			return currentsongindex + 1;
		}
		private int GetPrevSongIndex() {
			var currentsongindex = listSongs.CurrentPlayingIndex;
			if(currentsongindex < 0) return -1;
			if(RepeatType == 1) return currentsongindex;
			if(IsShuffle) return Random.Shared.Next(0, songlist.Count - 1);
			if(currentsongindex < 1) {
				if(RepeatType == 0) return -1;
				return songlist.Count - 1;
			}
			return currentsongindex - 1;
		}

		private async Task<bool> PlayMediaLink(string medialink) {
			if(string.IsNullOrEmpty(medialink)) return false;
			try {
				medialink = string.Concat(@"https://naedist.animemusicquiz.com/", medialink);
				await musicPlayer.PlayAsync(new Uri(medialink));
				trackPlayingPos.Value = 0;
				UpdateMediaControls();
				timerPlayPos.Start();
				return true;
			}
			catch {
				StopPlaying();
				return false;
			}
		}

		private void StopPlaying() {
			timerPlayPos.Stop();
			musicPlayer.Stop();
			trackPlayingPos.Value = 0;
			UpdateMediaControls();
			listSongs.UpdatePlayingIndex();
			listSongs.Invalidate();
		}

		private void UpdatePlayPos(object sender, EventArgs e) {
			if(isDraggingPlayPos) return;
			if(musicPlayer == null) return;
			trackPlayingPos.Maximum = (int)musicPlayer.TotalDuration;
			trackPlayingPos.Value = (int)musicPlayer.CurrentPosition;
			labelTime.Text = $"{trackPlayingPos.Value / 60:D2}:{trackPlayingPos.Value % 60:D2} / {trackPlayingPos.Maximum / 60:D2}:{trackPlayingPos.Maximum % 60:D2}";

		}
		private void UpdateMediaControls() {
			buttonPlay.Enabled = musicPlayer.isPlaying;
			trackPlayingPos.Enabled = musicPlayer.isPlaying;
			buttonPrev.Enabled = musicPlayer.isPlaying;
			buttonNext.Enabled = musicPlayer.isPlaying;
			UpdatePlayButton();
		}
		private void UpdateMuteButton() {
			buttonMute.Image = musicPlayer.isMuted ? IconContainer.Get("mute") : IconContainer.Get("speaker");
		}
		private void UpdateShuffleButton() {
			buttonShuffle.Image = IsShuffle ? IconContainer.Get("shuffle") : IconContainer.Get("shuffleoff");
		}
		private void UpdateRepeatButton() {
			buttonRepeat.Image = RepeatType == 0 ? IconContainer.Get("repeatoff") : RepeatType == 1 ? IconContainer.Get("repeat1") : IconContainer.Get("repeatall");
		}

		private void OnPauseResumeClick(object sender, EventArgs e) {
			musicPlayer.TogglePlayPause();
			UpdatePlayButton();
		}
		private void UpdatePlayButton() {
			buttonPlay.Image = musicPlayer.isPlaying ? IconContainer.Get("pause") : IconContainer.Get("play");
		}
		private void OnMuteClick(object sender, EventArgs e) {
			musicPlayer.isMuted = !musicPlayer.isMuted;
			UpdateMuteButton();
		}
		private void OnVolumeMouseDown(object sender, MouseEventArgs e) {
			SyncTrackBarClickLocation(trackVolume, e);
			musicPlayer.Volume = trackVolume.Value;
		}
		private void OnVolumeValueChanged(object sender, EventArgs e) {
			musicPlayer.Volume = trackVolume.Value;
		}
		private bool isDraggingPlayPos = false;
		private void OnPlayPosMouseDown(object sender, MouseEventArgs e) {
			isDraggingPlayPos = true;
			SyncTrackBarClickLocation(trackPlayingPos, e);
		}
		private void OnPlayPosMouseUp(object sender, MouseEventArgs e) {
			musicPlayer.Seek(trackPlayingPos.Value);
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

		private void OnPrevClick(object sender, EventArgs e) {
			var prevsongindex = GetPrevSongIndex();
			if(prevsongindex != -1) {
				if(prevsongindex == listSongs.CurrentPlayingIndex) musicPlayer.Seek(0);
				else PlaySong(songlist[prevsongindex]);
			}
		}

		private void OnNextClick(object sender, EventArgs e) {
			var nextsongindex = GetNextSongIndex();
			if(nextsongindex != -1) {
				if(nextsongindex == listSongs.CurrentPlayingIndex) musicPlayer.Seek(0);
				else PlaySong(songlist[nextsongindex]);
			}
		}

		private void OnShuffleClick(object sender, EventArgs e) {
			IsShuffle = !IsShuffle;
			UpdateShuffleButton();
		}

		private void OnRepeatClick(object sender, EventArgs e) {
			RepeatType++;
			if(RepeatType > 2) RepeatType = 0;
			UpdateRepeatButton();
		}
	}
}
