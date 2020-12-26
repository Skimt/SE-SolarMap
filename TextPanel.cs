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
using VRageRender;

namespace IngameScript
{
	partial class Program
	{
		public class TextPanel : Terminal<IMyTextPanel, TextPanelSetting>
		{

			private readonly Vector2 centerScreen = new Vector2(512 / 2, 307.2f / 2 * 1.6f);
			public readonly SpriteManager spriteManager;
			private readonly List<MySprite> orbitSprites = new List<MySprite>();
			private readonly List<MySprite> planetSprites = new List<MySprite>();
			private readonly List<MySprite> infoSprites = new List<MySprite>();

			MySpriteDrawFrame frame;
			private IMyTextPanel lcd;
			private TextPanelSetting setting;
			private MySprite grid;
			private bool isOdd;

			public TextPanel(Program program) : base(program)
			{
				spriteManager = new SpriteManager(program.world);
			}

			public override void OnCycle(IMyTextPanel lcd, TextPanelSetting setting)
			{

				this.lcd = lcd;
				this.setting = setting;

				// Setting error.
				if (setting.HasError)
				{
					SetError(setting.Status.ToString());
					return;
				}

				// LCD dimension error. 
				if (lcd.SurfaceSize.Y != 512)
				{
					SetError("Solar map cannot be displayed on 5:3 text panel.");
					return;
				}

				// Update propreties. 
				spriteManager.Update(lcd, setting);

				// Paint.
				PaintMap();
				PaintInfoPanel();

				// Add paint. Must be added in the correct order. 
				using (frame = lcd.DrawFrame())
				{

					// Used to force the texture surface cache to refresh. 
					// See Whiplash141's original post: https://support.keenswh.com/spaceengineers/pc/topic/1-192-021-lcd-scripts-using-sprites-dont-work-in-mp
					// See Georgik's original code: https://discordapp.com/channels/125011928711036928/216219467959500800/721065427140083832
					if (isOdd)
						frame.Add(new MySprite());

					frame.AddRange(orbitSprites); 
					frame.AddRange(planetSprites); 
					frame.Add(grid); 
					frame.AddRange(infoSprites); 

				}

				// Force texture cache to change every odd and even interval. 
				isOdd = !isOdd;

			}

			/// <summary>
			/// Sets up settings for each retrieved.
			/// </summary>
			public override TextPanelSetting CreateSetting(IMyTextPanel lcd)
			{

				MyIni = new MyIni();
				TextPanelSetting setting = new TextPanelSetting();

				// Try to parse CustomData. 
				MyIniParseResult status;
				if (!MyIni.TryParse(lcd.CustomData, out status))
				{
					setting.Status = status;
					return setting;
				}

				// GridColor
				string gridColorHex = MyIni.Get("SolarMap", "GridColor").ToString();
				setting.GridColor = ColorExtensions.FromHtml(gridColorHex);

				// Hide Grid
				setting.HideGrid = MyIni.Get("SolarMap", "HideGrid").ToBoolean();

				// Hide Map
				setting.HideMap = MyIni.Get("SolarMap", "HideMap").ToBoolean();

				// Hide InfoPanel
				setting.HideInfo = MyIni.Get("SolarMap", "HideInfo").ToBoolean();

				// Offset X and Y axis.
				int offsetX = MyIni.Get("SolarMap", "OffsetX").ToInt32();
				int offsetY = MyIni.Get("SolarMap", "OffsetY").ToInt32();
				setting.Offset = new Vector2I(offsetX, offsetY);

				return setting;

			}

			public override bool Collect(IMyTextPanel terminal)
			{

				// To be collected. 
				bool isSolarmap = terminal.IsSameConstructAs(program.Me) && MyIni.HasSection(terminal.CustomData, "SolarMap");

				// Set content type. 
				if (isSolarmap)
				{
					terminal.ContentType = ContentType.SCRIPT;
					terminal.Script = ""; // Resets any mistakes that the user might have done. 
				}

				return isSolarmap;

			}

			private void PaintMap()
			{

				// Hide.
				if (setting.HideMap)
				{
					orbitSprites.Clear();
					planetSprites.Clear();
					grid = new MySprite();
					return;
				}

				spriteManager.Map.UpdateMultipliers(lcd);
				spriteManager.Map.PaintOrbits(orbitSprites);
				spriteManager.Map.PaintPlanets(planetSprites);

				// Hide.
				if (setting.HideGrid)
				{
					grid = new MySprite();
					return;
				}

				grid = spriteManager.Map.PaintGrid(program.terminalManager.ShipController);

			}

			private void PaintInfoPanel()
			{

				// Hide.
				if (setting.HideInfo)
				{
					infoSprites.Clear();
					return;
				}

				spriteManager.InfoPanel.PaintInfo(infoSprites);

			}

			/// <summary>
			/// Sets error text and proceeds to dispose of the DrawFrame. 
			/// </summary>
			private void SetError(string text)
			{
				frame = lcd.DrawFrame();
				frame.Add(new MySprite(SpriteType.TEXT, text, centerScreen, null, lcd.ScriptForegroundColor, null, rotation: 0.6f));
				frame.Dispose();
			}

		}
	}
}
