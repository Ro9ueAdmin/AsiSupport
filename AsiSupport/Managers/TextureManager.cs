﻿using Rage;
using System;
using System.Collections.Generic;

namespace AsiSupport.Managers
{
	public class TextureManager
	{
		private Dictionary<int, Texture> textures;
		private int idCount;

		public TextureManager()
		{
			this.textures = new Dictionary<int, Texture>();
			this.idCount = 0;
		}

		public int CreateTexture(string fileName)
		{
			this.textures.Add(this.idCount, Game.CreateTextureFromFile(fileName));
			this.idCount++;

			return this.idCount - 1;
		}

		public void DrawTexture(int id, int time, float x, float y, float width, float height, float rotation, float rotationCenterX, float rotationCenterY)
		{
			TextureDrawer drawer = new TextureDrawer(id, time, x, y, width, height, rotation, rotationCenterX, rotationCenterY);
			Game.RawFrameRender += drawer.Draw;
		}

		private struct TextureDrawer
		{
			public int ValidUntil { get; private set; }
			public int TextureId { get; private set; }
			public float RenderX { get; private set; }
			public float RenderY { get; private set; }
			public float Width { get; private set; }
			public float Height { get; private set; }
			public float Rotation { get; private set; }
			public float RotationCenterX { get; private set; }
			public float RotationCenterY { get; private set; }

			public TextureDrawer(int id, int time, float x, float y, float width, float height, float rotation, float rotationCenterX, float rotationCenterY)
			{
				this.ValidUntil = Environment.TickCount + time;
				this.TextureId = id;
				this.RenderX = x;
				this.RenderY = y;
				this.Width = width;
				this.Height = height;
				this.Rotation = rotation;
				this.RotationCenterX = rotationCenterX;
				this.RotationCenterY = rotationCenterY;
			}

			public void Draw(object sender, GraphicsEventArgs args)
			{
				if(Environment.TickCount <= this.ValidUntil)
				{
					Vector2 position = this.Scale(this.RenderX, this.RenderY);
					Vector2 size = this.Scale(this.Width, this.Height);
					Vector2 rotationCenter = new Vector2(position.X + (size.X * this.RotationCenterX), position.Y + (size.Y * this.RotationCenterY));
					args.Graphics.DrawTexture(Support.Instance.TextureManager.textures[this.TextureId], position, size, 0.0f, 0.0f, 1.0f, 1.0f, this.Rotation, rotationCenter);
				}
				else
				{
					Game.RawFrameRender -= Draw;
				}
			}

			private Vector2 Scale(float x, float y)
			{
				return new Vector2(x * Game.Resolution.Width, y * Game.Resolution.Height);
			}
		}
	}
}
