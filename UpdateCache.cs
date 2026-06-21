using System.Text;

namespace AMQSongBrowser {
	public partial class UpdateCache : Form {
		private readonly string url= "https://animemusicquiz.com/libraryMasterList";
		private readonly HttpClient httpclient;
		private CancellationTokenSource cts;

		public UpdateCache(HttpClient client) {
			InitializeComponent();
			httpclient = client;

			Shown += OnFormShown;
			FormClosing += OnFormClosing;
		}
		private async void OnFormShown(object sender, EventArgs e) {
			cts = new CancellationTokenSource();

			try {
				using(var response = await httpclient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cts.Token)) {
					response.EnsureSuccessStatusCode();

					long? totalBytes = response.Content.Headers.ContentLength;

					using(var downloadStream = await response.Content.ReadAsStreamAsync())
					using(var memoryStream = new MemoryStream()) {
						byte[] buffer = new byte[8192];
						long totalReadBytes = 0;
						int readBytes;

						while((readBytes = await downloadStream.ReadAsync(buffer, 0, buffer.Length, cts.Token)) > 0) {
							await memoryStream.WriteAsync(buffer, 0, readBytes, cts.Token);
							totalReadBytes += readBytes;

							if(totalBytes.HasValue && totalBytes.Value > 0) {
								int progressPercentage = (int)((double)totalReadBytes / totalBytes.Value * 100);

								BeginInvoke(new Action(() => {
									if(progressPercentage <= progressBar1.Maximum)
										progressBar1.Value = progressPercentage;
								}));
							}
						}
						if(!DataCache.Instance.BuildData(Encoding.UTF8.GetString(memoryStream.ToArray())))
							throw (new Exception("Parsing failed"));
					}
				}

				DialogResult = DialogResult.OK;
				Close();
			}
			catch(OperationCanceledException) { }
			catch(Exception ex) {
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				DialogResult = DialogResult.Abort;
				Close();
			}
		}

		private void OnFormClosing(object sender, FormClosingEventArgs e) {
			if(DialogResult == DialogResult.OK || DialogResult == DialogResult.Abort) {
				cts?.Dispose();
				return;
			}

			DialogResult check = MessageBox.Show(this, "Do you want to cancel?", "Confirm",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question);

			if(check == DialogResult.Yes) {
				cts?.Cancel();
				DialogResult = DialogResult.Cancel;
			}
			else
				e.Cancel = true;
		}

		private void OnCancelClick(object sender, EventArgs e) {
			Close();
		}
	}
}
