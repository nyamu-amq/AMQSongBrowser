using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using static AMQSongBrowser.DataCache;
using static System.Windows.Forms.AxHost;
using Timer = System.Windows.Forms.Timer;
namespace AMQSongBrowser {
	public partial class MainForm : Form {
		public static MainForm Instance { get; private set; }
		public bool inited = false;
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
			public Point playerwindowlocation { get; set; }
			public Size playerwindowsize { get; set; }
			public int volume { get; set; }
			public bool mute { get; set; }
			public bool shuffle { get; set; }
			public int repeattype { get; set; }
			public List<int> playercolumnwidths { get; set; } = new();
		}

		private MusicPlayerForm musicplayerform;
		public MainForm() {
			Instance = this;
			InitializeComponent();
			radioEnglish.TabStop = false;
			radioRomaji.TabStop = false;
			httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");

			musicplayerform = new MusicPlayerForm(httpClient);
		}

		private ContextMenuStrip contextmenu;
		private void InitContextMenu() {
			contextmenu = new ContextMenuStrip();
			((ToolStripMenuItem)contextmenu.Items.Add("Add to Playlist and Play", null, AddToPlaylistAndPlay)).ShortcutKeyDisplayString = "Enter";
			((ToolStripMenuItem)contextmenu.Items.Add("Add to Playlist", null, AddToPlaylist)).ShortcutKeyDisplayString = "Space";
			((ToolStripMenuItem)contextmenu.Items.Add("Find Songs from Songname", null, FindSongsFromSongname)).ShortcutKeyDisplayString = "1";
			((ToolStripMenuItem)contextmenu.Items.Add("Find Songs from Artist", null, FindSongsFromArtist)).ShortcutKeyDisplayString = "2";
			((ToolStripMenuItem)contextmenu.Items.Add("Find Songs from Anime", null, FindSongsFromAnime)).ShortcutKeyDisplayString = "3";

			contextmenu.Opening += (s, e) => { e.Cancel = listResult.SelectedIndices.Count == 0; };
			listResult.ContextMenuStrip = contextmenu;
		}

		private readonly string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");
		private void OnFormLoad(object sender, EventArgs e) {
			try {
				if(!File.Exists(configPath)) return;
				string json = File.ReadAllText(configPath);
				UiState state = JsonSerializer.Deserialize<UiState>(json);

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

					musicplayerform.LoadState(state);
				}
			}
			finally {
				inited = true;
				listResult.inited = true;
				listResult.update = true;
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
				if(DataCache.Instance.inited)
					labelLastUpdate.Text = DataCache.Instance.timeLastUpdate.ToString("g");
				InitContextMenu();
				UpdateList();
				buttonPlayer.Image = IconContainer.Get("player");
				musicplayerform.Init();
			}
		}

		private bool closecompleted = false;
		private async void OnFormClosing(object sender, FormClosingEventArgs e) {
			if(closecompleted) return;
			e.Cancel = true;
			Hide();
			musicplayerform.Hide();

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
			};
			foreach(ColumnHeader col in listResult.Columns)
				state.columnwidths.Add(col.Width);
			musicplayerform.SaveState(state);
			string json = JsonSerializer.Serialize(state, new JsonSerializerOptions { WriteIndented = true });
			File.WriteAllText(configPath, json);

			await musicplayerform.DisposeAll();
			closecompleted = true;
			Close();
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
					labelLastUpdate.Text = DataCache.Instance.timeLastUpdate.ToString("g");
					UpdateList();
				}
				else if(result == DialogResult.Cancel)
					MessageBox.Show(this, "Update canceled", "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		List<AllSongListData> matched;
		public bool IsEnglish { get { return radioEnglish.Checked; } }
		void UpdateList() {
			if(!listResult.update) return;
			matched = DataCache.Instance.GetMatchedSongList();
			listResult.UpdateList(matched);
			if(DataCache.Instance.inited)
				labelLastUpdate.Text = DataCache.Instance.timeLastUpdate.ToString("g");
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
		private void OnTextboxEnter(object sender, EventArgs e) {
			if(sender is System.Windows.Forms.TextBox textbox)
				textbox.SelectAll();
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

		private void OnLanguageClick(object sender, EventArgs e) {
			listResult.Invalidate();
			musicplayerform.LanguageChanged();
		}
		private void DisableRadioTabstop(object sender, EventArgs e) {
			if(sender is RadioButton radio)
				radio.TabStop = false;
		}

		private void OnFormResizeBegin(object sender, EventArgs e) {
			listResult.RecordColumnsWidthRatios();
		}
		private void OnFormResizeEnd(object sender, EventArgs e) {
			if(!inited) return;
			listResult.ResizeColumns();
		}

		private void OnResultDoubleClick(object sender, MouseEventArgs e) {
			if(!inited) return;
			if(e.Button == MouseButtons.Left) {
				AddToPlaylistAndPlay(sender, e);
			}
		}
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
			if(keyData == Keys.Escape) {
				if(!this.ContainsFocus)
					return base.ProcessCmdKey(ref msg, keyData);
				listResult.update = false;
				textSongName.Text = textArtist.Text = textAnime.Text = string.Empty;
				DataCache.Instance.isSongNameDirty = true;
				DataCache.Instance.isArtistDirty = true;
				DataCache.Instance.isAnimeDirty = true;
				listResult.update = true;
				UpdateList();
				return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void OnResultKeyDown(object sender, KeyEventArgs e) {
			if(e.KeyCode == Keys.Enter) AddToPlaylistAndPlay(sender, e);
			else if(e.KeyCode == Keys.Space) AddToPlaylist(sender, e);
			else if(e.KeyCode == Keys.D1) FindSongsFromSongname(sender, e);
			else if(e.KeyCode == Keys.D2) FindSongsFromArtist(sender, e);
			else if(e.KeyCode == Keys.D3) FindSongsFromAnime(sender, e);
			else if(e.KeyCode == Keys.Q) { checkSongName.Checked = !checkSongName.Checked; OnSongNameChanged(sender, e); }
			else if(e.KeyCode == Keys.W) { checkArtist.Checked = !checkArtist.Checked; OnArtistChanged(sender, e); }
			else if(e.KeyCode == Keys.E) { checkAnime.Checked = !checkAnime.Checked; OnAnimeChanged(sender, e); }
			else if(e.KeyCode == Keys.A) { checkOP.Checked = !checkOP.Checked; OnSongTypeChanged(sender, e); }
			else if(e.KeyCode == Keys.S) { checkED.Checked = !checkED.Checked; OnSongTypeChanged(sender, e); }
			else if(e.KeyCode == Keys.D) { checkINS.Checked = !checkINS.Checked; OnSongTypeChanged(sender, e); }
			else if(e.KeyCode == Keys.Z) { checkStandard.Checked = !checkStandard.Checked; OnSongTypeChanged(sender, e); }
			else if(e.KeyCode == Keys.X) { checkInstrumental.Checked = !checkInstrumental.Checked; OnSongTypeChanged(sender, e); }
			else if(e.KeyCode == Keys.C) { checkChanting.Checked = !checkChanting.Checked; OnSongTypeChanged(sender, e); }
			else if(e.KeyCode == Keys.V) { checkCharacter.Checked = !checkCharacter.Checked; OnSongTypeChanged(sender, e); }
			else if(e.KeyCode == Keys.B) { checkDub.Checked = !checkDub.Checked; OnSongTypeChanged(sender, e); }
			else if(e.KeyCode == Keys.N) { checkRebroad.Checked = !checkRebroad.Checked; OnSongTypeChanged(sender, e); }
			else if(e.KeyCode == Keys.L) { if(radioEnglish.Checked) radioRomaji.Checked = true; else radioEnglish.Checked = true; OnLanguageClick(sender, e); }
			else if(e.KeyCode == Keys.P) OnPlayerClick(sender, e);
			else return;
			e.Handled = true;
			e.SuppressKeyPress = true;
		}

		private bool CheckMaximumSelectedSongs() {
			if(listResult.SelectedIndices.Count > 99) {
				MessageBox.Show("Cannot add more than 99 songs at once.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return true;
			}
			return false;
		}
		private void AddToPlaylistAndPlay(object sender, EventArgs e) {
			if(CheckMaximumSelectedSongs()) return;
			if(listResult.SelectedIndices.Count > 0) {
				musicplayerform.AddToPlaylistAndPlay(listResult.GetSelectedSongList());
				ShowPlayer();
			}
		}
		private void AddToPlaylist(object sender, EventArgs e) {
			if(CheckMaximumSelectedSongs()) return;
			if(listResult.SelectedIndices.Count > 0) {
				musicplayerform.AddToPlaylist(listResult.GetSelectedSongList());
				ShowPlayer();
			}
		}
		private void FindSongsFromSongname(object sender, EventArgs e) {
			var focuseditem = listResult.FocusedItem;
			if(focuseditem != null) {
				textSongName.Text = string.Empty; textArtist.Text = string.Empty; textAnime.Text = string.Empty;
				textSongName.Text = focuseditem.SubItems[0].Text;
				checkSongName.Checked = true;
				textSongName.Focus();
			}
		}
		private void FindSongsFromArtist(object sender, EventArgs e) {
			var songdata = listResult.GetFocusedSongData();
			if(songdata != null) {
				textSongName.Text = string.Empty; textArtist.Text = string.Empty; textAnime.Text = string.Empty;
				if(songdata.artistid > 0) textArtist.Text = DataCache.Instance.GetArtistData(songdata.artistid).name;
				else textArtist.Text = DataCache.Instance.GetGroupData(songdata.groupid).name;
				checkArtist.Checked = true;
				textArtist.Focus();
			}
		}
		private void FindSongsFromAnime(object sender, EventArgs e) {
			var focuseditem = listResult.FocusedItem;
			if(focuseditem != null) {
				textSongName.Text = string.Empty; textArtist.Text = string.Empty; textAnime.Text = string.Empty;
				textAnime.Text = focuseditem.SubItems[2].Text;
				checkAnime.Checked = true;
				textAnime.Focus();
			}
		}

		private void OnPlayerClick(object sender, EventArgs e) {
			ShowHidePlayer();
		}
		private void ShowPlayer() {
			if(!musicplayerform.Visible) musicplayerform.Show(this);
		}
		private void ShowHidePlayer() {
			if(musicplayerform.Visible) musicplayerform.Hide();
			else musicplayerform.Show(this);
		}
		private bool wasplayervisible = false;
		private void OnFormSizeChanged(object sender, EventArgs e) {
			if(musicplayerform != null) {
				if(WindowState == FormWindowState.Minimized) {
					wasplayervisible = musicplayerform.Visible;
					musicplayerform.Hide();
				}
				else if(WindowState == FormWindowState.Normal || WindowState == FormWindowState.Maximized) {
					if(wasplayervisible)
						ShowPlayer();
				}
			}
		}
	}
}
