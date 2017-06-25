using System;
using Microsoft.Xna.Framework.Media;

namespace QEngine
{
	//not thouroughly tested
	public class QMusic
	{
		Song song;

		//not tested
		public void Play()
		{
			MediaPlayer.Stop();
			MediaPlayer.Play(this);
		}

		//not tested
		public void Stop()
		{
			MediaPlayer.Stop();
		}

		//not tested
		public void Pause()
		{
			MediaPlayer.Pause();
		}

		//not tested
		public QSoundStates SoundState()
		{
			if(!Enum.TryParse(MediaPlayer.State.ToString(), out QSoundStates s))
			{
				return s;
			}
			return QSoundStates.Idle;
		}

		public static implicit operator QMusic(Song s) => new QMusic(s);

		public static implicit operator Song(QMusic m) => m.song;

		internal QMusic(Song s)
		{
			song = s;
		}
	}
}
