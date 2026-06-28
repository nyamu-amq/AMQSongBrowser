namespace AMQSongBrowser {
	partial class MusicPlayerForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			listSongs = new SongListView();
			columnHeader1 = new ColumnHeader();
			columnHeader2 = new ColumnHeader();
			columnHeader3 = new ColumnHeader();
			columnHeader4 = new ColumnHeader();
			columnHeader5 = new ColumnHeader();
			buttonMute = new Button();
			buttonPlay = new Button();
			trackVolume = new TrackBar();
			trackPlayingPos = new TrackBar();
			labelSongname = new SmoothMarqueeLabel();
			buttonShuffle = new Button();
			buttonRepeat = new Button();
			buttonPrev = new Button();
			buttonNext = new Button();
			labelTime = new Label();
			((System.ComponentModel.ISupportInitialize)trackVolume).BeginInit();
			((System.ComponentModel.ISupportInitialize)trackPlayingPos).BeginInit();
			SuspendLayout();
			// 
			// listSongs
			// 
			listSongs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			listSongs.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4, columnHeader5 });
			listSongs.FullRowSelect = true;
			listSongs.GridLines = true;
			listSongs.HideSelection = true;
			listSongs.Location = new Point(12, 51);
			listSongs.Name = "listSongs";
			listSongs.ShowGroups = false;
			listSongs.Size = new Size(660, 218);
			listSongs.TabIndex = 0;
			listSongs.TabStop = false;
			listSongs.UseCompatibleStateImageBehavior = false;
			listSongs.View = View.Details;
			listSongs.VirtualMode = true;
			listSongs.KeyDown += OnListKeyDown;
			listSongs.MouseClick += OnListClick;
			listSongs.MouseDoubleClick += OnListDoubleClick;
			// 
			// columnHeader1
			// 
			columnHeader1.Text = "SongName";
			columnHeader1.Width = 150;
			// 
			// columnHeader2
			// 
			columnHeader2.Text = "Artist";
			columnHeader2.Width = 150;
			// 
			// columnHeader3
			// 
			columnHeader3.Text = "Anime";
			columnHeader3.Width = 150;
			// 
			// columnHeader4
			// 
			columnHeader4.Text = "SongType";
			columnHeader4.Width = 100;
			// 
			// columnHeader5
			// 
			columnHeader5.Text = "Category";
			columnHeader5.Width = 80;
			// 
			// buttonMute
			// 
			buttonMute.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			buttonMute.BackColor = SystemColors.Control;
			buttonMute.FlatAppearance.BorderSize = 0;
			buttonMute.FlatAppearance.MouseDownBackColor = SystemColors.Control;
			buttonMute.FlatAppearance.MouseOverBackColor = SystemColors.Control;
			buttonMute.FlatStyle = FlatStyle.Flat;
			buttonMute.Font = new Font("Segoe UI", 12F);
			buttonMute.Image = Properties.Resources.speaker;
			buttonMute.Location = new Point(538, 12);
			buttonMute.Name = "buttonMute";
			buttonMute.Size = new Size(28, 28);
			buttonMute.TabIndex = 12;
			buttonMute.TabStop = false;
			buttonMute.UseVisualStyleBackColor = false;
			buttonMute.Click += OnMuteClick;
			// 
			// buttonPlay
			// 
			buttonPlay.BackColor = SystemColors.Control;
			buttonPlay.Enabled = false;
			buttonPlay.FlatAppearance.BorderSize = 0;
			buttonPlay.FlatAppearance.MouseDownBackColor = SystemColors.Control;
			buttonPlay.FlatAppearance.MouseOverBackColor = SystemColors.Control;
			buttonPlay.FlatStyle = FlatStyle.Flat;
			buttonPlay.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
			buttonPlay.Image = Properties.Resources.play;
			buttonPlay.Location = new Point(46, 12);
			buttonPlay.Name = "buttonPlay";
			buttonPlay.Size = new Size(28, 28);
			buttonPlay.TabIndex = 13;
			buttonPlay.TabStop = false;
			buttonPlay.UseVisualStyleBackColor = false;
			buttonPlay.Click += OnPauseResumeClick;
			// 
			// trackVolume
			// 
			trackVolume.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			trackVolume.AutoSize = false;
			trackVolume.BackColor = SystemColors.Control;
			trackVolume.Location = new Point(572, 15);
			trackVolume.Maximum = 100;
			trackVolume.Name = "trackVolume";
			trackVolume.Size = new Size(100, 30);
			trackVolume.SmallChange = 5;
			trackVolume.TabIndex = 10;
			trackVolume.TabStop = false;
			trackVolume.TickFrequency = 10;
			trackVolume.TickStyle = TickStyle.None;
			trackVolume.Value = 50;
			trackVolume.MouseDown += OnVolumeMouseDown;
			trackVolume.MouseUp += OnVolumeMouseDown;
			// 
			// trackPlayingPos
			// 
			trackPlayingPos.AutoSize = false;
			trackPlayingPos.BackColor = SystemColors.Control;
			trackPlayingPos.Enabled = false;
			trackPlayingPos.LargeChange = 10;
			trackPlayingPos.Location = new Point(114, 15);
			trackPlayingPos.Name = "trackPlayingPos";
			trackPlayingPos.Size = new Size(100, 30);
			trackPlayingPos.TabIndex = 11;
			trackPlayingPos.TabStop = false;
			trackPlayingPos.TickStyle = TickStyle.None;
			trackPlayingPos.MouseDown += OnPlayPosMouseDown;
			trackPlayingPos.MouseUp += OnPlayPosMouseUp;
			// 
			// labelSongname
			// 
			labelSongname.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			labelSongname.BackColor = Color.Transparent;
			labelSongname.Interval = 20;
			labelSongname.Location = new Point(392, 19);
			labelSongname.Name = "labelSongname";
			labelSongname.Size = new Size(129, 15);
			labelSongname.TabIndex = 14;
			labelSongname.TabStop = false;
			// 
			// buttonShuffle
			// 
			buttonShuffle.BackColor = SystemColors.Control;
			buttonShuffle.FlatAppearance.BorderSize = 0;
			buttonShuffle.FlatAppearance.MouseDownBackColor = SystemColors.Control;
			buttonShuffle.FlatAppearance.MouseOverBackColor = SystemColors.Control;
			buttonShuffle.FlatStyle = FlatStyle.Flat;
			buttonShuffle.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
			buttonShuffle.Image = Properties.Resources.shuffle;
			buttonShuffle.Location = new Point(313, 12);
			buttonShuffle.Name = "buttonShuffle";
			buttonShuffle.Size = new Size(28, 28);
			buttonShuffle.TabIndex = 13;
			buttonShuffle.TabStop = false;
			buttonShuffle.UseVisualStyleBackColor = false;
			buttonShuffle.Click += OnShuffleClick;
			// 
			// buttonRepeat
			// 
			buttonRepeat.BackColor = SystemColors.Control;
			buttonRepeat.FlatAppearance.BorderSize = 0;
			buttonRepeat.FlatAppearance.MouseDownBackColor = SystemColors.Control;
			buttonRepeat.FlatAppearance.MouseOverBackColor = SystemColors.Control;
			buttonRepeat.FlatStyle = FlatStyle.Flat;
			buttonRepeat.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
			buttonRepeat.Image = Properties.Resources.repeatall;
			buttonRepeat.Location = new Point(347, 12);
			buttonRepeat.Name = "buttonRepeat";
			buttonRepeat.Size = new Size(28, 28);
			buttonRepeat.TabIndex = 13;
			buttonRepeat.TabStop = false;
			buttonRepeat.UseVisualStyleBackColor = false;
			buttonRepeat.Click += OnRepeatClick;
			// 
			// buttonPrev
			// 
			buttonPrev.BackColor = SystemColors.Control;
			buttonPrev.Enabled = false;
			buttonPrev.FlatAppearance.BorderSize = 0;
			buttonPrev.FlatAppearance.MouseDownBackColor = SystemColors.Control;
			buttonPrev.FlatAppearance.MouseOverBackColor = SystemColors.Control;
			buttonPrev.FlatStyle = FlatStyle.Flat;
			buttonPrev.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
			buttonPrev.Image = Properties.Resources.prev;
			buttonPrev.Location = new Point(12, 12);
			buttonPrev.Name = "buttonPrev";
			buttonPrev.Size = new Size(28, 28);
			buttonPrev.TabIndex = 13;
			buttonPrev.TabStop = false;
			buttonPrev.UseVisualStyleBackColor = false;
			buttonPrev.Click += OnPrevClick;
			// 
			// buttonNext
			// 
			buttonNext.BackColor = SystemColors.Control;
			buttonNext.Enabled = false;
			buttonNext.FlatAppearance.BorderSize = 0;
			buttonNext.FlatAppearance.MouseDownBackColor = SystemColors.Control;
			buttonNext.FlatAppearance.MouseOverBackColor = SystemColors.Control;
			buttonNext.FlatStyle = FlatStyle.Flat;
			buttonNext.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
			buttonNext.Image = Properties.Resources.next;
			buttonNext.Location = new Point(80, 12);
			buttonNext.Name = "buttonNext";
			buttonNext.Size = new Size(28, 28);
			buttonNext.TabIndex = 13;
			buttonNext.TabStop = false;
			buttonNext.UseVisualStyleBackColor = false;
			buttonNext.Click += OnNextClick;
			// 
			// labelTime
			// 
			labelTime.Location = new Point(220, 19);
			labelTime.Name = "labelTime";
			labelTime.Size = new Size(87, 15);
			labelTime.TabIndex = 15;
			labelTime.Text = "00:00 / 00:00";
			labelTime.TextAlign = ContentAlignment.MiddleLeft;
			// 
			// MusicPlayerForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(684, 281);
			Controls.Add(labelTime);
			Controls.Add(labelSongname);
			Controls.Add(buttonMute);
			Controls.Add(buttonRepeat);
			Controls.Add(buttonShuffle);
			Controls.Add(buttonNext);
			Controls.Add(buttonPrev);
			Controls.Add(buttonPlay);
			Controls.Add(trackVolume);
			Controls.Add(trackPlayingPos);
			Controls.Add(listSongs);
			Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
			MaximizeBox = false;
			MinimizeBox = false;
			MinimumSize = new Size(700, 320);
			Name = "MusicPlayerForm";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.Manual;
			Text = "Music Player";
			FormClosing += OnFormClosing;
			Load += OnFormLoad;
			ResizeBegin += OnFormResizeBegin;
			ResizeEnd += OnFormResizeEnd;
			((System.ComponentModel.ISupportInitialize)trackVolume).EndInit();
			((System.ComponentModel.ISupportInitialize)trackPlayingPos).EndInit();
			ResumeLayout(false);
		}

		#endregion

		private SongListView listSongs;
		private ColumnHeader columnHeader1;
		private ColumnHeader columnHeader2;
		private ColumnHeader columnHeader3;
		private ColumnHeader columnHeader4;
		private ColumnHeader columnHeader5;
		private Button buttonMute;
		private Button buttonPlay;
		private TrackBar trackVolume;
		private TrackBar trackPlayingPos;
		private SmoothMarqueeLabel labelSongname;
		private Button buttonShuffle;
		private Button buttonRepeat;
		private Button buttonPrev;
		private Button buttonNext;
		private Label labelTime;
	}
}