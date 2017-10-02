 using System;
using Microsoft.Xna.Framework.Audio;

namespace QEngine
{
	public class QSound
	{
		SoundEffect sound;

		//TODO WORK ON THIS, MAKE IT AWESOME, not tested
		public void Play(float volume, float pitch, float pan)
		{
			using(var i = sound.CreateInstance())
			{
				i.Volume = volume;
				i.Pitch = pitch;
				i.Pan = pan;
				i.Play();
			}
		}

		public TimeSpan Duration => sound.Duration;

		public static implicit operator QSound(SoundEffect e) => new QSound(e);

		public static implicit operator SoundEffect(QSound s) => s.sound;

		internal QSound(SoundEffect s)
		{
			sound = s;
		}

		//
		//		static QSound()
		//		{
		//			Instances = new Queue<SoundEffectInstance>();
		//		}
	}
}