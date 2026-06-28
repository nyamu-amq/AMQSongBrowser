using System;
using System.Collections.Generic;
using System.Text;

namespace AMQSongBrowser {
	public static class IconContainer {
		public static bool IsDark { get; set; } = false;

		static readonly Dictionary<string, Bitmap> light = new Dictionary<string, Bitmap>() {
			{ "mute", Properties.Resources.mute },
			{ "next", Properties.Resources.next },
			{ "pause", Properties.Resources.pause },
			{ "play", Properties.Resources.play },
			{ "player", Properties.Resources.player },
			{ "prev", Properties.Resources.prev },
			{ "repeat1", Properties.Resources.repeat1 },
			{ "repeatall", Properties.Resources.repeatall },
			{ "repeatoff", Properties.Resources.repeatoff },
			{ "shuffle", Properties.Resources.shuffle },
			{ "shuffleoff", Properties.Resources.shuffleoff },
			{ "speaker", Properties.Resources.speaker },
		};
		static readonly Dictionary<string, Bitmap> dark = new Dictionary<string, Bitmap>() {
			{ "mute", Properties.Resources.muted },
			{ "next", Properties.Resources.nextd },
			{ "pause", Properties.Resources.paused },
			{ "play", Properties.Resources.playd },
			{ "player", Properties.Resources.playerd },
			{ "prev", Properties.Resources.prevd },
			{ "repeat1", Properties.Resources.repeat1d },
			{ "repeatall", Properties.Resources.repeatalld },
			{ "repeatoff", Properties.Resources.repeatoffd },
			{ "shuffle", Properties.Resources.shuffled },
			{ "shuffleoff", Properties.Resources.shuffleoffd },
			{ "speaker", Properties.Resources.speakerd },
		};
		public static Bitmap Get(string key) {
			return IsDark ? dark[key] : light[key];
		}
	}
}
