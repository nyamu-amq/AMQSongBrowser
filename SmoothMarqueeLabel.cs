using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

public class SmoothMarqueeLabel : Control {
	private readonly Timer timer;

	private float currentX;
	private int textWidth;

	[DefaultValue(50)]
	public int Gap { get; set; } = 50;
	[DefaultValue(60f)]
	public float ScrollSpeed { get; set; } = 60f;
	[DefaultValue(16)]
	public int Interval {
		get { return timer.Interval; }
		set { timer.Interval = Math.Max(1, value); }
	}

	public SmoothMarqueeLabel() {
		SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
		BackColor = Color.Transparent;
		timer = new Timer();
		timer.Interval = 16;
		timer.Tick += Timer_Tick;
		UpdateTextMetrics();
		if(!DesignMode) timer.Start();
	}

	private bool MarqueeNeeded => textWidth > ClientSize.Width;
	private void Timer_Tick(object sender, EventArgs e) {
		if(!MarqueeNeeded) return;
		currentX -= ScrollSpeed * timer.Interval / 1000f;
		float cycleWidth = textWidth + Gap;
		if(currentX <= -cycleWidth) currentX += cycleWidth;
		Invalidate();
	}
	private bool wasMarqueeNeeded;
	private void UpdateTextMetrics() {
		using(Graphics g = CreateGraphics())
			textWidth = TextRenderer.MeasureText(g, Text, Font, Size.Empty, TextFormatFlags.SingleLine).Width;

		bool marqueeNeeded = textWidth > ClientSize.Width;

		if(wasMarqueeNeeded != marqueeNeeded) currentX = 0;
		wasMarqueeNeeded = marqueeNeeded;

		Invalidate();
	}
	protected override void OnTextChanged(EventArgs e) {
		base.OnTextChanged(e);
		UpdateTextMetrics();
	}
	protected override void OnFontChanged(EventArgs e) {
		base.OnFontChanged(e);
		UpdateTextMetrics();
	}
	protected override void OnSizeChanged(EventArgs e) {
		base.OnSizeChanged(e);
		UpdateTextMetrics();
	}
	protected override void OnPaint(PaintEventArgs e) {
		base.OnPaint(e);
		e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
		TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;
		if(!MarqueeNeeded) {
			TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, ForeColor, flags);
			return;
		}
		int x1 = (int)currentX;
		int x2 = x1 + textWidth + Gap;
		Rectangle rect1 = new Rectangle(x1, 0, textWidth, ClientSize.Height);
		Rectangle rect2 = new Rectangle(x2, 0, textWidth, ClientSize.Height);
		TextRenderer.DrawText(e.Graphics, Text, Font, rect1, ForeColor, flags);
		TextRenderer.DrawText(e.Graphics, Text, Font, rect2, ForeColor, flags);
	}

	protected override void Dispose(bool disposing) {
		if(disposing) timer?.Dispose();
		base.Dispose(disposing);
	}
}
