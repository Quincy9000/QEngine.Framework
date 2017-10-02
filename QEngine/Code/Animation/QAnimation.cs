using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace QEngine
{
	/// <summary>
	/// Creates an animation loop from rectangles
	/// </summary>
	public class QAnimation
	{
		/// <summary>
		///New frames will use this as the default loop option
		/// </summary>
		public static bool IsLoop { get; set; } = true;

		/// <summary>
		/// Check if the animation is in between frames
		/// </summary>
		public bool IsPlaying => CurrentFrame != 0;

		/// <summary>
		/// This will turn true if it will no longer update any frames
		/// Useful for attacking type animations
		/// </summary>
		public bool IsDone { get; private set; } = false;

		/// <summary>
		/// Does the animation loop at the end, if false IsDone can return true
		/// </summary>
		public bool Loop { get; set; } = true;

		/// <summary>
		/// Gets the current Source rectangle of the animation
		/// </summary>
		public QRectangle CurrentRectangle { get; private set; }

		public int CurrentFrame { get; private set; }

		List<QFrame> _frames { get; } = new List<QFrame>();

		float _accumulator;

		float _multiplyer = 1f;

		/// <summary>
		/// Adds a frame to the animation at the end
		/// </summary>
		/// <param name="rectangle"></param>
		/// <param name="time"></param>
		public void Add(QRectangle rectangle, float time)
		{
			_frames.Add(new QFrame(rectangle, time));
		}

		/// <summary>
		/// Adds frames from list, kind of like substring
		/// </summary>
		/// <param name="recs"></param>
		/// <param name="time"></param>
		/// <param name="start">inclusive</param>
		/// <param name="end">inclusive</param>
		public void SubList(List<QRectangle> recs, float time, int start, int end)
		{
			for(var i = start; i < end + 1; i++)
				Add(recs[i], time);
		}

		/// <summary>
		/// Sets the animation to the first frame
		/// </summary>
		public void Reset()
		{
			_accumulator = 0;
			CurrentFrame = 0;
			CurrentRectangle = _frames[CurrentFrame].SourceRectangle;
			IsDone = false;
		}

		/// <summary>
		/// Updates the animation smoothly no matter what the Fps is running at
		/// </summary>
		/// <param name="delta"></param>
		public QRectangle Play(float delta)
		{
			_accumulator += delta;
			//if loop is set to not loop then it will always return the last frame
			if(IsDone)
				return CurrentRectangle;
			//check how long the frame has been on the current frame(also check multiplyer), if long enough to change then 
			//we set the next frame
			if(_accumulator > _frames[CurrentFrame].Duration / Multiplyer)
			{
				if(CurrentFrame == _frames.Count - 1)
				{
					if(!Loop)
					{
						IsDone = true;
						return CurrentRectangle;
					}
					CurrentFrame = 0;
				}
				else
				{
					CurrentFrame++;
				}
				_accumulator = 0;
			}
			return CurrentRectangle = _frames[CurrentFrame].SourceRectangle;
		}

		/// <summary>
		/// Makes the animation take less time to play, so it goes faster or slower if lower number
		/// Cannot be zero
		/// </summary>
		public float Multiplyer
		{
			get => _multiplyer;
			set
			{
				if(value > 0)
					_multiplyer = value;
			}
		}

		/// <summary>
		/// Creates new Animation that starts at frame zero and you need to add all the frames manually
		/// </summary>
		public QAnimation()
		{
			CurrentRectangle = _frames[CurrentFrame].SourceRectangle;
		}

		/// <summary>
		/// Creates new animation from a list of frames, can you tweak what frames you want to include
		/// </summary>
		/// <param name="recs"></param>
		/// <param name="time"></param>
		/// <param name="start"></param>
		/// <param name="end"></param>
		public QAnimation(List<QRectangle> recs, double time, int start, int end)
		{
			SubList(recs, (float)time, start, end);
			CurrentRectangle = _frames[CurrentFrame].SourceRectangle;
			Loop = IsLoop;
		}

		/// <summary>
		/// One frame animation handler
		/// </summary>
		/// <param name="rectangle"></param>
		public QAnimation(QRectangle rectangle)
		{
			Add(rectangle, 1);
			CurrentRectangle = rectangle;
			Loop = IsLoop;
		}

		/// <summary>
		/// Creates new animation from all the frames in a list of frames
		/// </summary>
		/// <param name="recs"></param>
		/// <param name="time"></param>
		public QAnimation(List<QRectangle> recs, double time)
		{
			SubList(recs, (float)time, 0, recs.Count);
			CurrentRectangle = _frames[CurrentFrame].SourceRectangle;
			Loop = IsLoop;
		}

		public static bool operator ==(QAnimation left, QAnimation right)
		{
			if(left == null || right == null) return false;
			//if they dont have the same amount of frames they are not equal
			if(left._frames.Count == right._frames.Count)
			{
				//then if they do have the same amount then we check the actual qrects to see if they are equal
				for(int i = 0; i < left._frames.Count; i++)
				{
					//if any of them are false then we just return false
					if(left._frames[i] != right._frames[i])
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		public static bool operator !=(QAnimation left, QAnimation right)
		{
			return !(left == right);
		}

		protected bool Equals(QAnimation other)
		{
			return _accumulator.Equals(other._accumulator) && _multiplyer.Equals(other._multiplyer) && IsDone == other.IsDone && Loop == other.Loop && CurrentRectangle.Equals(other.CurrentRectangle) && CurrentFrame == other.CurrentFrame && Equals(_frames, other._frames);
		}

		public override bool Equals(object obj)
		{
			if(ReferenceEquals(null, obj)) return false;
			if(ReferenceEquals(this, obj)) return true;
			if(obj.GetType() != this.GetType()) return false;
			return Equals((QAnimation)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = _accumulator.GetHashCode();
				hashCode = (hashCode * 397) ^ _multiplyer.GetHashCode();
				hashCode = (hashCode * 397) ^ IsDone.GetHashCode();
				hashCode = (hashCode * 397) ^ Loop.GetHashCode();
				hashCode = (hashCode * 397) ^ CurrentRectangle.GetHashCode();
				hashCode = (hashCode * 397) ^ CurrentFrame;
				hashCode = (hashCode * 397) ^ (_frames != null ? _frames.GetHashCode() : 0);
				return hashCode;
			}
		}
	}
}