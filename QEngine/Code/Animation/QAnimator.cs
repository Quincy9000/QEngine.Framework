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

		public QAnimation Current { get; private set; }

		public string CurrentName { get; private set; }

		/// <summary>
		/// Returns true if successfully added animation, sets currentAnimation to this one
		/// </summary>
		/// <param name="name"></param>
		/// <param name="a"></param>
		/// <returns></returns>
		public bool AddAnimation(string name, QAnimation a)
		{
			if(!Animations.ContainsKey(name))
			{
				Current = a;
				Animations.Add(name, a);
				return true;
			}
			return false;
		}

		/// <summary>
		///	Plays Current, checks if new play is available
		/// </summary>
		/// <returns></returns>
		public void Play(QSprite s, QTime time)
		{
			s.Source = Current.Play(time.Delta);
		}

		/// <summary>
		///	Plays Current, checks if new play is available
		/// </summary>
		/// <returns></returns>
		public void Play(QSprite s, float time)
		{
			s.Source = Current.Play(time);
		}

		public void EditAnimation(string name, Action<QAnimation> action)
		{
			if(Animations.TryGetValue(name, out QAnimation a))
			{
				action(a);
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
				if(Current == a)
				{
					return false;
				}
				Current = a;
				CurrentName = n;
				return true;
			}
			return false;
		}

		public QAnimator()
		{
			Animations = new Dictionary<string, QAnimation>();
		}
	}
}