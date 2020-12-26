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
		public class TerminalManager
		{

			protected Program program;
			private readonly IEnumerator<bool> cycle;

			public TerminalManager(Program program)
			{
				this.program = program;
				ShipController = new ShipController(program);
				TextPanel = new TextPanel(program);
				//Antenna = new Antenna(program);
				cycle = SetCycle();
			}

			public ShipController ShipController { get; private set; }
			public TextPanel TextPanel { get; private set; }
			//public Antenna Antenna { get; private set; }

			public void Run()
			{
				if (!cycle.MoveNext())
					cycle.Dispose();
			}

			private IEnumerator<bool> SetCycle()
			{
				while (true)
				{
					yield return ShipController.Run();
					yield return TextPanel.Run();
					//yield return Antenna.Run();
				}
			}

		}
	}
}
