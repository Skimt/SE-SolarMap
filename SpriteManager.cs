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
		public class SpriteManager
		{

			private readonly ColorManager colorManager = new ColorManager();
			private readonly World world;

			public SpriteManager(World world)
			{

				this.world = world;
				Map = new Map(colorManager, world);
				InfoPanel = new InfoPanel(colorManager, world);

			}

			public Map Map;
			public InfoPanel InfoPanel;

			/// <summary>
			/// Updates the grid position and various colors. 
			/// </summary>
			public void Update(IMyTextPanel lcd, TextPanelSetting setting)
			{
				Map.UpdateSetting(setting);
				InfoPanel.UpdateSetting(setting);
				world.UpdateGridPosition();
				colorManager.UpdateGeneralColors(lcd);
				colorManager.UpdateGridArrowColor(lcd, setting.GridColor);
			}

		}
	}
}
