using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace QEngine
{
	[ImmutableObject(true)]
	public struct QTilePos
	{
		public QRect Source { get; }
		public QVec Position { get; }

		public QTilePos(QVec pos, QRect source)
		{
			Position = pos;
			Source = source;
		}
	}
	
	public static class QMapTools
	{
		public delegate QRect TileMapper(QColor color);

		public delegate void ObjectCreator(QColor color, QVec pos);

		//Instantiates objects at positions of a QMapTools
		static void ObjectSceneLoader(QColor[,] colors, QVec startingPos, QVec tileSize, ObjectCreator spawner)
		{
			QVec pos = startingPos;
			for(int i = 0; i < colors.GetLength(0); ++i, pos.X = startingPos.X, pos.Y += tileSize.Y)
			{
				for(int j = 0; j < colors.GetLength(1); ++j, pos.X += tileSize.X)
				{
					spawner(colors[i, j], pos);
				}
			}
		}

		//turn array into 2d array
		static T[,] ConvertArray2D<T>(IReadOnlyList<T> array, int width, int height)
		{
			if(width == 0 || height == 0)
				throw new DivideByZeroException();
			var element = 0;
			var array2D = new T[height, width];
			var w = (int)Math.Round((double)array.Count / height);
			var h = (int)Math.Round((double)array.Count / width);
			for(var i = 0; i < h; i++)
			{
				for(var j = 0; j < w; j++)
				{
					array2D[i, j] = array[element++];
				}
			}
			return array2D;
		}

		//turns list of colors into list of rectangles and positions
		static List<QTilePos> CompileLayer(QColor[,] colors, QVec startingPos, QVec scale, TileMapper layer)
		{
			var tiles = new List<QTilePos>();
			var pos = startingPos;
			for(int i = 0; i < colors.GetLength(0); i++)
			{
				for(int j = 0; j < colors.GetLength(1); j++)
				{
					var r = layer(colors[i, j]);
					if(r != QRect.Empty)
					{
						tiles.Add(new QTilePos(pos, r));
					}
					pos.X += scale.X;
				}
				pos.X = startingPos.X;
				pos.Y += scale.Y;
			}
			return tiles;
		}

		public static List<QTilePos> CreateSpriteLayer(QRetrieveContent content, string nameOfTexture, QVec startingPos, QVec scale, TileMapper layer)
		{
			var t = content.Texture(nameOfTexture);
			var colors = t.GetPixels();
			var c = ConvertArray2D(colors, t.Width, t.Height);
			return CompileLayer(c, startingPos, scale, layer);
		}

		public static void SpawnObjects(QRetrieveContent content, string nameOfTexture, QVec startingPos, QVec scale, ObjectCreator spawner)
		{
			var t = content.Texture(nameOfTexture);
			var colors = t.GetPixels();
			var c = ConvertArray2D(colors, t.Width, t.Height);
			ObjectSceneLoader(c, startingPos, scale, spawner);
		}
	}
}