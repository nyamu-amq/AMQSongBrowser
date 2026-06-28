using Windows.Media.Core;
using Windows.Media.Playback;

namespace AMQSongBrowser {
	public class MusicPlayer : IDisposable {
		private MediaPlayer mediaPlayer;

		public event EventHandler PlaybackEnded;

		public bool isPlaying { get; private set; } = false;
		public bool isMuted {
			get { return mediaPlayer?.IsMuted ?? false; }
			set { if(mediaPlayer != null) mediaPlayer.IsMuted = value; }
		}
		public double Volume {
			get { return (mediaPlayer?.Volume ?? 0) * 100.0; }
			set { if(mediaPlayer != null) mediaPlayer.Volume = value / 100.0; }
		}
		public double CurrentPosition { get { return mediaPlayer?.PlaybackSession?.Position.TotalSeconds ?? 0; } }
		public double TotalDuration { get { return mediaPlayer?.PlaybackSession?.NaturalDuration.TotalSeconds ?? 0; } }

		public MusicPlayer() {
			mediaPlayer = new MediaPlayer();
			mediaPlayer.MediaEnded += OnMediaEnded;
			mediaPlayer.MediaFailed += OnMediaFailed;
		}

		public async Task<bool> PlayAsync(Uri mediaUri) {
			Stop();
			if(mediaUri == null) return false;
			try {
				var mediaSource = MediaSource.CreateFromUri(mediaUri);
				mediaPlayer.Source = mediaSource;
				await mediaSource.OpenAsync();
				mediaPlayer.Play();
				isPlaying = true;
				return true;
			}
			catch {
				Stop();
				return false;
			}
		}
		public void TogglePlayPause() {
			var session = mediaPlayer?.PlaybackSession;
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
		}
		public void Stop() {
			if(mediaPlayer != null) mediaPlayer.Source = null;
			isPlaying = false;
		}
		public void Seek(double seconds) {
			var session = mediaPlayer?.PlaybackSession;
			if(session != null)
				session.Position = TimeSpan.FromSeconds(seconds);
		}

		private void OnMediaEnded(MediaPlayer sender, object args) {
			PlaybackEnded?.Invoke(this, EventArgs.Empty);
		}

		private void OnMediaFailed(MediaPlayer sender, MediaPlayerFailedEventArgs args) {
			Stop();
			PlaybackEnded?.Invoke(this, EventArgs.Empty);
		}

		public void Dispose() {
			if(mediaPlayer != null) {
				mediaPlayer.Source = null;
				mediaPlayer.MediaEnded -= OnMediaEnded;
				mediaPlayer.MediaFailed -= OnMediaFailed;
				mediaPlayer.Dispose();
				mediaPlayer = null;
			}
		}
	}
}
