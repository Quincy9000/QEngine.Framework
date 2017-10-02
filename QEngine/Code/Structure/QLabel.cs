using System.Text;
using Microsoft.Xna.Framework;

namespace QEngine
{
	public class QLabel : QBehavior, IQGui
	{
		public QFont Font { get; set; }

		StringBuilder PerformanceString { get; set; } = new StringBuilder();

		public QVector2 Measure(string measureWith)
		{
			return Font.MeasureString(measureWith);
		}

		public QColor Color { get; set; } = QColor.White;

		public QVector2 Scale { get; set; } = Vector2.One;

		public float Layer { get; set; } = 0;

		public bool Visible { get; set; } = true;

		public void OnDrawGui(QGuiRenderer renderer)
		{
			if(Font != null)
				renderer.DrawString(this);
		}

		public static implicit operator string(QLabel l) => l.PerformanceString.ToString();

		public static QLabel operator +(QLabel l, string s)
		{
			l.Text = l.Text + s;
			return l;
		}

		public string Text
		{
			get => PerformanceString.ToString();
			set
			{
				PerformanceString.Clear();
				PerformanceString.Append(value);
			}
		}

		/// <summary>
		/// Adds the text to the next line on the output
		/// </summary>
		public void AppendLine(string value)
		{
			PerformanceString.AppendLine(value);
		}

		/// <summary>
		/// Adds the text to the next line on the output
		/// </summary>
		public void Append(string value)
		{
			PerformanceString.Append(value);
		}

		public void ClearText()
		{
			PerformanceString.Clear();
		}

		public QLabel(QFont font)
		{
			Font = font;
		}
	}
}