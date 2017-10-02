using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace QEngine
{
	[ImmutableObject(true)]
	public struct QTiles
	{
		public QRectangle Source { get; }
		public QVector2 Position { get; }

		public QTiles(QVector2 pos, QRectangle source)
		{
			Position = pos;
			Source = source;
		}
	}
	
	public static class QMapTools
	{
		public delegate QRectangle TileMapper(QColor color);

		public delegate void ObjectCreator(QColor color, QVector2 pos);

		//Instantiates objects at positions of a QMapTools
		static void ObjectSceneLoader(QColor[,] colors, QVector2 startingPos, QVector2 tileSize, ObjectCreator spawner)
		{
			QVector2 pos = startingPos;
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
		static List<QTiles> CompileLayer(QColor[,] colors, QVector2 startingPos, QVector2 scale, TileMapper layer)
		{
			var tiles = new List<QTiles>();
			var pos = startingPos;
			for(int i = 0; i < colors.GetLength(0); i++)
			{
				for(int j = 0; j < colors.GetLength(1); j++)
				{
					var r = layer(colors[i, j]);
					if(r != QRectangle.Empty)
					{
						tiles.Add(new QTiles(pos, r));
					}
					pos.X += scale.X;
				}
				pos.X = startingPos.X;
				pos.Y += scale.Y;
			}
			return tiles;
		}

		public static List<QTiles> CreateSpriteLayer(QGetContent content, string nameOfTexture, QVector2 startingPos, QVector2 scale, TileMapper layer)
		{
			var t = content.Texture(nameOfTexture);
			var colors = t.GetPixels();
			var c = ConvertArray2D(colors, t.Width, t.Height);
			return CompileLayer(c, startingPos, scale, layer);
		}

		public static void SpawnObjects(QGetContent content, string nameOfTexture, QVector2 startingPos, QVector2 scale, ObjectCreator spawner)
		{
			var t = content.Texture(nameOfTexture);
			var colors = t.GetPixels();
			var c = ConvertArray2D(colors, t.Width, t.Height);
			ObjectSceneLoader(c, startingPos, scale, spawner);
		}
	}
}