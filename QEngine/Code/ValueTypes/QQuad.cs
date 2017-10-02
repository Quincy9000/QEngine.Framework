using System.Collections.Generic;

namespace QEngine
{
	/// <summary>
	/// https://gamedevelopment.tutsplus.com/tutorials/quick-tip-use-quadtrees-to-detect-likely-collisions-in-2d-space--gamedev-374
	/// </summary>
	public class QQuad
	{
		int MAX_OBJECTS = 10;

		int MAX_LEVELS = 5;

		int level;

		List<QRectangle> objects;
		QRectangle bounds;
		QQuad[] nodes;

		QQuad(int plevel, QRectangle pBounds)
		{
			level = plevel;
			objects = new List<QRectangle>();
			bounds = pBounds;
			nodes = new QQuad[4];
		}

		public QQuad(QRectangle pBounds)
		{
			level = 0;
			objects = new List<QRectangle>();
			bounds = pBounds;
			nodes = new QQuad[4];
		}

		public void Clear()
		{
			objects.Clear();
			for(int i = 0; i < nodes.Length; i++)
			{
				if(nodes[i] != null)
				{
					nodes[i].Clear();
					nodes[i] = null;
				}
			}
		}

		void Split()
		{
			int subWidth = bounds.Width / 2;
			int subHeight = bounds.Height / 2;
			int x = bounds.X;
			int y = bounds.Y;

			nodes[0] = new QQuad(level + 1, new QRectangle(x + subWidth, y, subWidth, subHeight));
			nodes[1] = new QQuad(level + 1, new QRectangle(x, y, subWidth, subHeight));
			nodes[2] = new QQuad(level + 1, new QRectangle(x, y + subHeight, subWidth, subHeight));
			nodes[3] = new QQuad(level + 1, new QRectangle(x + subWidth, y + subHeight, subWidth, subHeight));
		}

		int Index(QRectangle r)
		{
			int i = -1;
			double vMidpoint = bounds.X + bounds.Width / 2.0;
			double hMidpoint = bounds.Y + bounds.Height / 2.0;

			bool topQuad = r.Y < hMidpoint && r.Y + r.Height < hMidpoint;
			bool botQuad = r.Y > hMidpoint;

			if(r.X < vMidpoint && r.X + r.Width < vMidpoint)
			{
				if(topQuad)
				{
					i = 1;
				}
				else if(botQuad)
				{
					i = 2;
				}
			}
			else if(r.X > vMidpoint)
			{
				if(topQuad)
				{
					i = 0;
				}
				else if(botQuad)
				{
					i = 3;
				}
			}

			return i;
		}

		public void Insert(QRectangle r)
		{
			if(nodes[0] != null)
			{
				int index = Index(r);

				if(index != -1)
				{
					nodes[index].Insert(r);
				}

				return;
			}

			objects.Add(r);

			if(objects.Count > MAX_OBJECTS && level < MAX_LEVELS)
			{
				if(nodes[0] == null)
				{
					Split();
				}

				int i = 0;
				while(i < objects.Count)
				{
					int index = Index(objects[i]);
					if(index != -1)
					{
						nodes[index].Insert(objects[i]);
						objects.RemoveAt(i);
					}
					else
					{
						i++;
					}
				}
			}
		}

		public List<QRectangle> Retrieve(List<QRectangle> returnObjects, QRectangle rectangle)
		{
			int index = Index(rectangle);

			if(index != -1 && nodes[0] != null)
			{
				nodes[index].Retrieve(returnObjects, rectangle);
			}

			returnObjects.AddRange(objects);

			return returnObjects;
		}
	}
}