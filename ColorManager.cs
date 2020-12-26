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
		public class ColorManager
		{

			private Vector3 hsv = Vector3.Zero; // Hue, Saturation, Value

			public Color Fill { get; private set; }
			public Color Border { get; private set; }
			public Color Grid { get; private set; }
			public Color Text { get; private set; }
			public Color PanelBackground = new Color(0, 0, 0, 50);
			public Color TitleBackground = new Color(0, 0, 0, 150);

			private bool BgIsDark => hsv.Z < 0.125 || (hsv.Y > 0.5 && hsv.Z < 0.25);
			private bool BgIsRed => (hsv.X < 0.1 || hsv.X > 0.9) && hsv.Y > 0.65 && hsv.Z > 0.125;
			private bool BgIsBlue => hsv.X > 0.5 && hsv.X < 0.75 && hsv.Y > 0.25 && hsv.Z > 0.25;

			public void UpdateGeneralColors(IMyTextPanel lcd)
			{
				Fill = lcd.ScriptBackgroundColor;
				Border = new Color(lcd.ScriptForegroundColor, 0.5f);
				Text = lcd.ScriptForegroundColor;
			}

			public void UpdateGridArrowColor(IMyTextPanel lcd, Color? gridColor)
			{

				if (gridColor.HasValue)
				{
					Grid = gridColor.Value;
					return;
				}

				hsv = lcd.ScriptBackgroundColor.ColorToHSV();

				if (BgIsBlue)
				{
					Grid = Color.Red;
				}
				else if (BgIsRed)
				{
					Grid = Color.LimeGreen;
				}
				else
				{
					Grid = Color.Black;
					if (BgIsDark)
					{
						Grid = Color.Red;
					}
				}

			}

		}
	}
}
