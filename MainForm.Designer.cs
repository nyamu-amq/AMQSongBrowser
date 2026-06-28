namespace AMQSongBrowser {
	partial class MainForm {
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			textSongName = new TextBox();
			checkSongName = new CheckBox();
			checkArtist = new CheckBox();
			checkAnime = new CheckBox();
			textArtist = new TextBox();
			textAnime = new TextBox();
			checkOP = new CheckBox();
			checkED = new CheckBox();
			checkINS = new CheckBox();
			checkInstrumental = new CheckBox();
			checkDub = new CheckBox();
			checkRebroad = new CheckBox();
			listResult = new SongListView();
			columnHeader1 = new ColumnHeader();
			columnHeader2 = new ColumnHeader();
			columnHeader3 = new ColumnHeader();
			columnHeader4 = new ColumnHeader();
			columnHeader5 = new ColumnHeader();
			radioEnglish = new RadioButton();
			radioRomaji = new RadioButton();
			checkChanting = new CheckBox();
			checkCharacter = new CheckBox();
			checkStandard = new CheckBox();
			button1 = new Button();
			labelLastUpdate = new Label();
			buttonPlayer = new Button();
			SuspendLayout();
			// 
			// textSongName
			// 
			textSongName.Location = new Point(12, 43);
			textSongName.Name = "textSongName";
			textSongName.Size = new Size(190, 23);
			textSongName.TabIndex = 0;
			textSongName.TextChanged += OnSongNameChanged;
			textSongName.Enter += OnTextboxEnter;
			textSongName.KeyDown += OnKeyDown;
			// 
			// checkSongName
			// 
			checkSongName.Appearance = Appearance.Button;
			checkSongName.Checked = true;
			checkSongName.CheckState = CheckState.Checked;
			checkSongName.Location = new Point(12, 12);
			checkSongName.Name = "checkSongName";
			checkSongName.Size = new Size(190, 25);
			checkSongName.TabIndex = 4;
			checkSongName.TabStop = false;
			checkSongName.Text = "Song Name";
			checkSongName.TextAlign = ContentAlignment.MiddleCenter;
			checkSongName.UseVisualStyleBackColor = true;
			checkSongName.CheckedChanged += OnSongNameChanged;
			// 
			// checkArtist
			// 
			checkArtist.Appearance = Appearance.Button;
			checkArtist.Checked = true;
			checkArtist.CheckState = CheckState.Checked;
			checkArtist.Location = new Point(208, 12);
			checkArtist.Name = "checkArtist";
			checkArtist.Size = new Size(190, 25);
			checkArtist.TabIndex = 4;
			checkArtist.TabStop = false;
			checkArtist.Text = "Artist";
			checkArtist.TextAlign = ContentAlignment.MiddleCenter;
			checkArtist.UseVisualStyleBackColor = true;
			checkArtist.CheckedChanged += OnArtistChanged;
			// 
			// checkAnime
			// 
			checkAnime.Appearance = Appearance.Button;
			checkAnime.Checked = true;
			checkAnime.CheckState = CheckState.Checked;
			checkAnime.Location = new Point(404, 12);
			checkAnime.Name = "checkAnime";
			checkAnime.Size = new Size(190, 25);
			checkAnime.TabIndex = 4;
			checkAnime.TabStop = false;
			checkAnime.Text = "Anime";
			checkAnime.TextAlign = ContentAlignment.MiddleCenter;
			checkAnime.UseVisualStyleBackColor = true;
			checkAnime.CheckedChanged += OnAnimeChanged;
			checkAnime.TextChanged += OnAnimeChanged;
			// 
			// textArtist
			// 
			textArtist.Location = new Point(208, 43);
			textArtist.Name = "textArtist";
			textArtist.Size = new Size(190, 23);
			textArtist.TabIndex = 1;
			textArtist.TextChanged += OnArtistChanged;
			textArtist.Enter += OnTextboxEnter;
			textArtist.KeyDown += OnKeyDown;
			// 
			// textAnime
			// 
			textAnime.Location = new Point(404, 43);
			textAnime.Name = "textAnime";
			textAnime.Size = new Size(190, 23);
			textAnime.TabIndex = 2;
			textAnime.TextChanged += OnAnimeChanged;
			textAnime.Enter += OnTextboxEnter;
			textAnime.KeyDown += OnKeyDown;
			// 
			// checkOP
			// 
			checkOP.Appearance = Appearance.Button;
			checkOP.Checked = true;
			checkOP.CheckState = CheckState.Checked;
			checkOP.Location = new Point(600, 12);
			checkOP.Name = "checkOP";
			checkOP.Size = new Size(73, 25);
			checkOP.TabIndex = 4;
			checkOP.TabStop = false;
			checkOP.Text = "OP";
			checkOP.TextAlign = ContentAlignment.MiddleCenter;
			checkOP.UseVisualStyleBackColor = true;
			checkOP.CheckedChanged += OnSongTypeChanged;
			// 
			// checkED
			// 
			checkED.Appearance = Appearance.Button;
			checkED.Checked = true;
			checkED.CheckState = CheckState.Checked;
			checkED.Location = new Point(679, 12);
			checkED.Name = "checkED";
			checkED.Size = new Size(73, 25);
			checkED.TabIndex = 4;
			checkED.TabStop = false;
			checkED.Text = "ED";
			checkED.TextAlign = ContentAlignment.MiddleCenter;
			checkED.UseVisualStyleBackColor = true;
			checkED.CheckedChanged += OnSongTypeChanged;
			// 
			// checkINS
			// 
			checkINS.Appearance = Appearance.Button;
			checkINS.Checked = true;
			checkINS.CheckState = CheckState.Checked;
			checkINS.Location = new Point(758, 12);
			checkINS.Name = "checkINS";
			checkINS.Size = new Size(73, 25);
			checkINS.TabIndex = 4;
			checkINS.TabStop = false;
			checkINS.Text = "INS";
			checkINS.TextAlign = ContentAlignment.MiddleCenter;
			checkINS.UseVisualStyleBackColor = true;
			checkINS.CheckedChanged += OnSongTypeChanged;
			// 
			// checkInstrumental
			// 
			checkInstrumental.Appearance = Appearance.Button;
			checkInstrumental.Checked = true;
			checkInstrumental.CheckState = CheckState.Checked;
			checkInstrumental.Location = new Point(679, 41);
			checkInstrumental.Name = "checkInstrumental";
			checkInstrumental.Size = new Size(73, 25);
			checkInstrumental.TabIndex = 4;
			checkInstrumental.TabStop = false;
			checkInstrumental.Text = "Instrument";
			checkInstrumental.TextAlign = ContentAlignment.MiddleCenter;
			checkInstrumental.UseVisualStyleBackColor = true;
			checkInstrumental.CheckedChanged += OnSongTypeChanged;
			// 
			// checkDub
			// 
			checkDub.Appearance = Appearance.Button;
			checkDub.Checked = true;
			checkDub.CheckState = CheckState.Checked;
			checkDub.Location = new Point(916, 41);
			checkDub.Name = "checkDub";
			checkDub.Size = new Size(73, 25);
			checkDub.TabIndex = 4;
			checkDub.TabStop = false;
			checkDub.Text = "Dub";
			checkDub.TextAlign = ContentAlignment.MiddleCenter;
			checkDub.UseVisualStyleBackColor = true;
			checkDub.CheckedChanged += OnSongTypeChanged;
			// 
			// checkRebroad
			// 
			checkRebroad.Appearance = Appearance.Button;
			checkRebroad.Checked = true;
			checkRebroad.CheckState = CheckState.Checked;
			checkRebroad.Location = new Point(995, 41);
			checkRebroad.Name = "checkRebroad";
			checkRebroad.Size = new Size(73, 25);
			checkRebroad.TabIndex = 4;
			checkRebroad.TabStop = false;
			checkRebroad.Text = "Rebroad";
			checkRebroad.TextAlign = ContentAlignment.MiddleCenter;
			checkRebroad.UseVisualStyleBackColor = true;
			checkRebroad.CheckedChanged += OnSongTypeChanged;
			// 
			// listResult
			// 
			listResult.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			listResult.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4, columnHeader5 });
			listResult.FullRowSelect = true;
			listResult.GridLines = true;
			listResult.Location = new Point(12, 72);
			listResult.Name = "listResult";
			listResult.ShowGroups = false;
			listResult.Size = new Size(1360, 257);
			listResult.TabIndex = 3;
			listResult.TabStop = false;
			listResult.UseCompatibleStateImageBehavior = false;
			listResult.View = View.Details;
			listResult.VirtualMode = true;
			listResult.KeyDown += OnResultKeyDown;
			listResult.MouseDoubleClick += OnResultDoubleClick;
			// 
			// columnHeader1
			// 
			columnHeader1.Text = "SongName";
			columnHeader1.Width = 385;
			// 
			// columnHeader2
			// 
			columnHeader2.Text = "Artist";
			columnHeader2.Width = 385;
			// 
			// columnHeader3
			// 
			columnHeader3.Text = "Anime";
			columnHeader3.Width = 385;
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
			// radioEnglish
			// 
			radioEnglish.Appearance = Appearance.Button;
			radioEnglish.Location = new Point(916, 12);
			radioEnglish.Name = "radioEnglish";
			radioEnglish.Size = new Size(73, 25);
			radioEnglish.TabIndex = 4;
			radioEnglish.Text = "English";
			radioEnglish.TextAlign = ContentAlignment.MiddleCenter;
			radioEnglish.UseVisualStyleBackColor = true;
			radioEnglish.Click += OnLanguageClick;
			radioEnglish.Enter += DisableRadioTabstop;
			radioEnglish.Leave += DisableRadioTabstop;
			// 
			// radioRomaji
			// 
			radioRomaji.Appearance = Appearance.Button;
			radioRomaji.Checked = true;
			radioRomaji.Location = new Point(995, 12);
			radioRomaji.Name = "radioRomaji";
			radioRomaji.Size = new Size(73, 25);
			radioRomaji.TabIndex = 4;
			radioRomaji.TabStop = true;
			radioRomaji.Text = "Romaji";
			radioRomaji.TextAlign = ContentAlignment.MiddleCenter;
			radioRomaji.UseVisualStyleBackColor = true;
			radioRomaji.Click += OnLanguageClick;
			radioRomaji.Enter += DisableRadioTabstop;
			radioRomaji.Leave += DisableRadioTabstop;
			// 
			// checkChanting
			// 
			checkChanting.Appearance = Appearance.Button;
			checkChanting.Checked = true;
			checkChanting.CheckState = CheckState.Checked;
			checkChanting.Location = new Point(758, 41);
			checkChanting.Name = "checkChanting";
			checkChanting.Size = new Size(73, 25);
			checkChanting.TabIndex = 4;
			checkChanting.TabStop = false;
			checkChanting.Text = "Chanting";
			checkChanting.TextAlign = ContentAlignment.MiddleCenter;
			checkChanting.UseVisualStyleBackColor = true;
			checkChanting.CheckedChanged += OnSongTypeChanged;
			// 
			// checkCharacter
			// 
			checkCharacter.Appearance = Appearance.Button;
			checkCharacter.Checked = true;
			checkCharacter.CheckState = CheckState.Checked;
			checkCharacter.Location = new Point(837, 41);
			checkCharacter.Name = "checkCharacter";
			checkCharacter.Size = new Size(73, 25);
			checkCharacter.TabIndex = 4;
			checkCharacter.TabStop = false;
			checkCharacter.Text = "Character";
			checkCharacter.TextAlign = ContentAlignment.MiddleCenter;
			checkCharacter.UseVisualStyleBackColor = true;
			checkCharacter.CheckedChanged += OnSongTypeChanged;
			// 
			// checkStandard
			// 
			checkStandard.Appearance = Appearance.Button;
			checkStandard.Checked = true;
			checkStandard.CheckState = CheckState.Checked;
			checkStandard.Location = new Point(600, 41);
			checkStandard.Name = "checkStandard";
			checkStandard.Size = new Size(73, 25);
			checkStandard.TabIndex = 4;
			checkStandard.TabStop = false;
			checkStandard.Text = "Standard";
			checkStandard.TextAlign = ContentAlignment.MiddleCenter;
			checkStandard.UseVisualStyleBackColor = true;
			checkStandard.CheckedChanged += OnSongTypeChanged;
			// 
			// button1
			// 
			button1.Location = new Point(1268, 12);
			button1.Name = "button1";
			button1.Size = new Size(104, 25);
			button1.TabIndex = 5;
			button1.TabStop = false;
			button1.Text = "Update Cache";
			button1.UseVisualStyleBackColor = true;
			button1.Click += OnUpdateCacheClick;
			// 
			// labelLastUpdate
			// 
			labelLastUpdate.Location = new Point(1234, 40);
			labelLastUpdate.Name = "labelLastUpdate";
			labelLastUpdate.Size = new Size(138, 15);
			labelLastUpdate.TabIndex = 6;
			labelLastUpdate.TextAlign = ContentAlignment.MiddleRight;
			// 
			// buttonPlayer
			// 
			buttonPlayer.FlatAppearance.BorderSize = 0;
			buttonPlayer.FlatStyle = FlatStyle.Flat;
			buttonPlayer.Image = Properties.Resources.player;
			buttonPlayer.Location = new Point(1074, 12);
			buttonPlayer.Name = "buttonPlayer";
			buttonPlayer.Size = new Size(54, 54);
			buttonPlayer.TabIndex = 11;
			buttonPlayer.TabStop = false;
			buttonPlayer.UseVisualStyleBackColor = true;
			buttonPlayer.Click += OnPlayerClick;
			// 
			// MainForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1384, 341);
			Controls.Add(buttonPlayer);
			Controls.Add(labelLastUpdate);
			Controls.Add(button1);
			Controls.Add(radioRomaji);
			Controls.Add(radioEnglish);
			Controls.Add(listResult);
			Controls.Add(checkAnime);
			Controls.Add(checkArtist);
			Controls.Add(checkINS);
			Controls.Add(checkED);
			Controls.Add(checkRebroad);
			Controls.Add(checkDub);
			Controls.Add(checkStandard);
			Controls.Add(checkCharacter);
			Controls.Add(checkChanting);
			Controls.Add(checkInstrumental);
			Controls.Add(checkOP);
			Controls.Add(checkSongName);
			Controls.Add(textAnime);
			Controls.Add(textArtist);
			Controls.Add(textSongName);
			Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
			MinimumSize = new Size(1400, 266);
			Name = "MainForm";
			Text = "AMQ Song Browser";
			FormClosing += OnFormClosing;
			Load += OnFormLoad;
			ResizeBegin += OnFormResizeBegin;
			ResizeEnd += OnFormResizeEnd;
			SizeChanged += OnFormSizeChanged;
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private TextBox textSongName;
		private CheckBox checkSongName;
		private CheckBox checkArtist;
		private CheckBox checkAnime;
		private TextBox textArtist;
		private TextBox textAnime;
		private CheckBox checkOP;
		private CheckBox checkED;
		private CheckBox checkINS;
		private CheckBox checkInstrumental;
		private CheckBox checkDub;
		private CheckBox checkRebroad;
		private SongListView listResult;
		private RadioButton radioEnglish;
		private RadioButton radioRomaji;
		private ColumnHeader columnHeader1;
		private ColumnHeader columnHeader2;
		private ColumnHeader columnHeader3;
		private ColumnHeader columnHeader4;
		private ColumnHeader columnHeader5;
		private CheckBox checkChanting;
		private CheckBox checkCharacter;
		private CheckBox checkStandard;
		private Button button1;
		private Label labelLastUpdate;
		private Button buttonPlayer;
	}
}
