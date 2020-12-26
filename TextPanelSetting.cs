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
		public struct TextPanelSetting
		{

			public MyIniParseResult Status { get; set; }
			public Color? GridColor { get; set; }
			public bool HideGrid { get; set; }
			public bool HideMap { get; set; }
			public bool HideInfo { get; set; }
			public Vector2I Offset { get; set; }

			public bool HasError => Status.IsDefined && !Status.Success;

		}
	}
}
