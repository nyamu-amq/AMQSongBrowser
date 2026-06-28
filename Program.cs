namespace AMQSongBrowser {

	internal static class Program {
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			// To customize application configuration such as set high DPI settings or default font,
			// see https://aka.ms/applicationconfiguration.
			ApplicationConfiguration.Initialize();
			Application.SetColorMode((SystemColorMode)Properties.Settings.Default.ThemeMode);
			IconContainer.IsDark = (Application.ColorMode == SystemColorMode.System ? Application.SystemColorMode : Application.ColorMode) == SystemColorMode.Dark;
			Application.Run(new MainForm());
		}
	}
}