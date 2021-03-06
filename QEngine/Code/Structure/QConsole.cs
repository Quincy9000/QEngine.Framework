﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace QEngine
{
	/// <summary>
	/// Debug tool that can display Text inside the game scene area
	/// </summary>
	public class QConsole : QBehavior, IQLoad, IQStart, IQUpdate, IQGui
	{
		/*Structs*/

		[ImmutableObject(true)]
		struct MessageBox
		{
			public MessageBox(string s, QVector2 v)
			{
				Text = s;
				MeasureMent = v;
			}

			public string Text { get; }
			public QVector2 MeasureMent { get; }
		}

		/*Publics*/

		public int Width { get; set; } = 40;

		public int Height { get; set; } = 30;

		/// <summary>
		/// How long until the text starts to fade
		/// </summary>
		public double FadeStart { get; set; } = 2;

		public QLabel Label { get; set; }

		public QColor Color
		{
			get => Label.Color;
			set => Label.Color = value;
		}

		/*Privates*/

		double _startFade;

		double _fade;

		LinkedList<MessageBox> Messages { get; set; }

		QVector2 Measure(string s)
		{
			return Label.Font.MeasureString(s);
		}

		/// <summary>
		/// Writes to the screen and disappears after a certain amount of delta has passes useful for debugging
		/// </summary>
		/// <param name="message"></param>
		public void WriteLine(string message)
		{
			if(Label == null) return;
			_fade = FadeStart;
			_startFade = 0;
			var measure = Measure(message);
			if(measure.X > Width)
			{
				message = WordWrap(message, Width);
				measure = Measure(message);
			}
			Messages.AddFirst(new MessageBox(message, measure));
			while(Messages.Count > Height)
				Messages.RemoveLast();
		}

		public void WriteLine<T>(T obj)
		{
			WriteLine(obj.ToString());
		}

		/// <summary>
		/// Clears all the messages in the console
		/// </summary>
		public void Clear()
		{
			Messages = new LinkedList<MessageBox>();
			_startFade = 0;
		}

		public void OnLoad(QLoadContent load)
		{
			load.Font("Fonts/arial");
		}

		public void OnStart(QGetContent get)
		{
			Label = new QLabel(get.Font("arial"));
			Clear();
		}

		public void OnUpdate()
		{
			_startFade += QTime.Delta;
			if(_startFade > FadeStart)
				_fade -= QTime.Delta;
		}

		public void OnDrawGui(QGuiRenderer renderer)
		{
			var temp = Transform.Position;
			foreach(var m in Messages)
			{
				Label.Text = m.Text;
				renderer.DrawString(Label, temp, Transform, (float)_fade);
				temp += new QVector2(0, m.MeasureMent.Y); //??
			}
		}

		/*Statics*/

		//http://stackoverflow.com/questions/17586/best-word-wrap-algorithm
		static readonly char[] SplitChars = {' ', '-', '\t'};

		static string WordWrap(string str, int width)
		{
			var words = Explode(str, SplitChars);

			var curLineLength = 0;
			var strBuilder = new StringBuilder();
			for(var i = 0; i < words.Length; i += 1)
			{
				var word = words[i];
				// If adding the new word to the current line would be too long,
				// then put it on a new line (and split it up if it's too long).
				if(curLineLength + word.Length > width)
				{
					// Only move down to a new line if we have Text on the current line.
					// Avoids situation where wrapped whitespace causes emptylines in Text.
					if(curLineLength > 0)
					{
						strBuilder.Append(Environment.NewLine);
						curLineLength = 0;
					}

					// If the current word is too long to fit on a line even on it's own then
					// split the word up.
					while(word.Length > width)
					{
						strBuilder.Append(word.Substring(0, width - 1) + "-");
						word = word.Substring(width - 1);

						strBuilder.Append(Environment.NewLine);
					}

					// Remove leading whitespace from the word so the new line starts flush to the left.
					word = word.TrimStart();
				}
				strBuilder.Append(word);
				curLineLength += word.Length;
			}
			return strBuilder.ToString();
		}

		static string[] Explode(string str, char[] splitChars)
		{
			var parts = new List<string>();
			var startIndex = 0;
			while(true)
			{
				var index = str.IndexOfAny(splitChars, startIndex);

				if(index == -1)
				{
					parts.Add(str.Substring(startIndex));
					return parts.ToArray();
				}

				var word = str.Substring(startIndex, index - startIndex);
				var nextChar = str.Substring(index, 1)[0];
				// Dashes and the likes should stick to the word occuring before it. Whitespace doesn't have to.
				if(char.IsWhiteSpace(nextChar))
				{
					parts.Add(word);
					parts.Add(nextChar.ToString());
				}
				else
				{
					parts.Add(word + nextChar);
				}

				startIndex = index + 1;
			}
		}
	}
}