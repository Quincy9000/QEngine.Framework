using System;
using System.Collections.Generic;

namespace QEngine
{
	/// <summary>
	/// Helper class to manage QAnimations
	/// </summary>
	public class QAnimator
	{
		Dictionary<string, QAnimation> Animations { get; }

		public QAnimation CurrentFrame { get; private set; }

		public string CurrentFrameName { get; private set; }

		/// <summary>
		/// Adds the frame and sets it as the first frame in the animation
		/// returns false if the animation already exists
		/// </summary>
		/// <param name="name"></param>
		/// <param name="a"></param>
		/// <returns></returns>
		public bool AddAsFirstFrame(string name, QAnimation a)
		{
			if(!Animations.ContainsKey(name))
			{
				CurrentFrame = a;
				Animations.Add(name, a);
				return true;
			}
			return false;
		}

		/// <summary>
		///	Plays Current, checks if new play is available
		/// </summary>
		/// <returns></returns>
		public void Play(QSprite s)
		{
			s.Source = CurrentFrame.Play(QTime.Delta);
		}

		/// <summary>
		///	Plays Current, checks if new play is available
		/// </summary>
		/// <returns></returns>
		public void Play(QSprite s, float time)
		{
			s.Source = CurrentFrame.Play(time);
		}

		/// <summary>
		/// Change animation conditions
		/// </summary>
		/// <param name="name"></param>
		/// <param name="action"></param>
		/// <exception cref="Exception"></exception>
		public void EditAnimation(string name, Action<QAnimation> action)
		{
			if(Animations.TryGetValue(name, out QAnimation a))
			{
				action(a);
			}
			else
			{
				throw new Exception($"Animation {name} does not exist!");
			}
		}

		/// <summary>
		/// Switches to animation if it exists. returns false if does not exist
		/// </summary>
		/// <param name="n"></param>
		/// <returns></returns>
		public bool Swap(string n)
		{
			if(Animations.TryGetValue(n, out QAnimation a))
			{
				if(CurrentFrame == a)
				{
					return false;
				}
				CurrentFrame = a;
				CurrentFrameName = n;
				return true;
			}
			return false;
		}

		public QAnimator()
		{
			Animations = new Dictionary<string, QAnimation>();
		}

		/// <summary>
		/// Add one animation to the animator
		/// </summary>
		/// <param name="name"></param>
		/// <param name="firstFrame"></param>
		public QAnimator(string name, QAnimation firstFrame) : this()
		{
			AddAsFirstFrame(name, firstFrame);
		}
	}
}