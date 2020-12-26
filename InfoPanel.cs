using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game;
using VRage;
using VRageMath;

namespace IngameScript
{
	partial class Program
	{
		public class InfoPanel
		{

			private readonly ColorManager colorManager;
			private readonly World world;

			private Vector2I panelLayout = Vector2I.One;
			private Vector2 size = new Vector2(512 - 14, 20);
			private Vector2 position = Vector2.Zero;
			private Vector2 margin = new Vector2(7, 2);
			private int tick;
			private TextPanelSetting setting;

			public InfoPanel(ColorManager colorManager, World world)
			{
				this.colorManager = colorManager;
				this.world = world;
			}

			public void PaintInfo(List<MySprite> sprites)
			{

				// Create a panel layout and sort them panel roughly every ~5 seconds. 
				if (tick % 27 == 0)
				{
					world.CelestialInfo.Sort(SortByDistance);
				}
				tick++;

				sprites.Clear();

				// Panel background. 
				if (!setting.HideMap)
				{
					sprites.Add(new MySprite(SpriteType.TEXTURE, "SquareSimple", new Vector2(95, 256), new Vector2(183, 505), colorManager.PanelBackground));
				}

				int cbCount = world.CelestialInfo.Count > 5 ? 6 : world.CelestialInfo.Count;
				panelLayout = setting.HideMap ? GetPanelLayout(world.CelestialInfo.Count) : new Vector2I(1, cbCount);

				// Position multiplier.
				int xPosMult = 0;
				int yPosMult = -1;
				float ySizeMult = 1.75f - (panelLayout.Y * (0.125f + (panelLayout.X - 1) * 0.01f)); // 1.75f when there are 1 and 2 rows.

				// Panel content.
				for (int i = 0; i < world.CelestialInfo.Count; i++)
				{

					if (panelLayout.Y - 1 == yPosMult)
					{
						yPosMult = 0;
						xPosMult++;
					}
					else
					{
						yPosMult++;
					}

					CelestialBody cb = world.CelestialInfo[i];

					// General.
					float marginX = setting.HideMap ? margin.X : 0;
					float sizeX = setting.HideMap ? size.X / panelLayout.X : (size.X / 3) + margin.X + 1;
					position.X = margin.X + (sizeX * xPosMult);
					position.Y = margin.X + (size.X / panelLayout.Y * yPosMult);

					// Title: Background.
					Vector2 titleBgPosition = new Vector2(position.X + (sizeX / 2), position.Y + (size.Y * ySizeMult / 2));
					Vector2 titleBgSize = new Vector2(sizeX - marginX, size.Y * ySizeMult);
					sprites.Add(new MySprite(SpriteType.TEXTURE, "SquareSimple", titleBgPosition, titleBgSize, colorManager.TitleBackground));

					// Title: Text.
					Vector2 titleTxtPosition = new Vector2(margin.X + position.X, margin.Y * ySizeMult + position.Y);
					float distance = Vector3.Distance(cb.Position, world.GridPosition) / 1000;
					sprites.Add(new MySprite(SpriteType.TEXT, cb.Name + " (" + distance.ToString("F1") + " km" + ")", titleTxtPosition, null, colorManager.Text, null, TextAlignment.LEFT, 0.5f * ySizeMult));

					// Body: Text.
					Vector2 bodyTxtPosition = new Vector2(margin.X + position.X, size.Y * ySizeMult + position.Y);
					string txt = "Radius: " + (cb.Radius / 1000).ToString("F1") + " km\n" +
								"Gravity: " + cb.Gravity.ToString("F1") + " G\n" +
								"Atmosphere: " + cb.HasAtmosphere + "\n" +
								"Oxygen: " + cb.Oxygen + "\n" +
								"Resources: " + cb.Resources + "\n";
					sprites.Add(new MySprite(SpriteType.TEXT, txt, bodyTxtPosition, null, colorManager.Text, null, TextAlignment.LEFT, 0.4f * ySizeMult));

					if (!setting.HideMap && i == 5)
					{
						break;
					}

				}

			}

			public void UpdateSetting(TextPanelSetting setting)
			{
				this.setting = setting;
			}

			/// <summary>
			/// Returns the amount of columns and rows that the information panel will have. 
			/// </summary>
			private Vector2I GetPanelLayout(int num)
			{

				int x, y;

				// X
				if (num < 3)
					x = 1;
				else if (num > 8)
					x = 3;
				else
					x = 2;

				// Y
				if (num < 9)
					y = (int)Math.Round((0.42f * num) + 0.75f);
				else if (num > 9)
					y = (int)Math.Round((0.3f * num) + 0.8f);
				else
					y = 3;

				return new Vector2I(x, y);

			}

			private int SortByDistance(CelestialBody a, CelestialBody b)
			{

				float distanceA = Vector3.Distance(a.Position, world.GridPosition);
				float distanceB = Vector3.Distance(b.Position, world.GridPosition);

				if (distanceA < distanceB)
					return -1;
				else if (distanceB < distanceA)
					return 1;
				return 0;

			}

		}
	}
}
