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
		public class ShipController : Terminal<IMyShipController, ShipControllerSetting>
		{

			private IMyShipController main;

			public ShipController(Program program) : base(program) { }

			public IMyShipController Main {
				get {
					return IsListEmpty || (main != null && (main.WorldMatrix == MatrixD.Identity || !main.IsWorking)) ? null : main;
				}
			}

			public bool HasController => !IsListEmpty && main != null && main.IsWorking;
			public bool IsMoveable => HasController && !program.Me.CubeGrid.IsStatic;

			public override void OnCycle(IMyShipController terminal, ShipControllerSetting settings)
			{

				if (terminal.IsMainCockpit)
				{
					main = terminal;
					return;
				}

				main = terminal;

			}

			public override bool Collect(IMyShipController terminal)
			{
				return terminal.IsSameConstructAs(program.Me) && terminal.CanControlShip && terminal.IsWorking;
			}

			public override ShipControllerSetting CreateSetting(IMyShipController item)
			{
				return new ShipControllerSetting();
			}
		}
	}
}
