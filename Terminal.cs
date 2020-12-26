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
		/// <summary>
		/// Common behavior to all terminals.
		/// </summary>
		public abstract class Terminal<T, U> 
			where T : class, IMyTerminalBlock 
			where U : struct {

			protected Program program;
			private readonly List<T> terminals = new List<T>();
			private readonly List<U> settings = new List<U>();
			private int tIndex, tUpdate;

			public Terminal(Program program)
			{
				this.program = program;
			}

			public int UpdateCount { get; private set; }
			public MyIni MyIni { get; set; }

			public bool IsListEmpty => terminals.Count == 0;

			/// <summary>
			/// Runs through each terminal in list and attempts to update at the end of the cycle. 
			/// Returns a booleans, which makes it possible to enumerate. 
			/// </summary>
			public bool Run()
			{

				// Run a terminal, or update list and settings. 
				if (tIndex < terminals.Count)
				{

					T terminal = terminals[tIndex];
					U setting = settings[tIndex];

					// Start the cycle or remove the corrupt terminal and setting. 
					if (!IsCorrupt(terminal))
					{
						OnCycle(terminal, setting);
					}
					else
					{
						terminals.Remove(terminal);
						settings.Remove(setting);
					}

					tIndex++;

				}
				else
				{

					// Only update terminals and create settings / parse MyIni every ~10 seconds or so.
					if (tUpdate % (32 / (terminals.Count + 1)) == 0)
					{

						program.GridTerminalSystem.GetBlocksOfType(terminals, Collect);
						UpdateCount++;
						settings.Clear();

						foreach (T item in terminals)
						{
							U setting = CreateSetting(item);
							settings.Add(setting);
						}

					}

					tUpdate++;
					tIndex = 0;

				}

				return true;

			}

			/// <summary>
			/// The main method that cycles through terminals and their settings. 
			/// </summary>
			public abstract void OnCycle(T terminal, U setting);

			/// <summary>
			/// Specificies which terminals to be collected. Use override. 
			/// </summary>
			public virtual bool Collect(T terminal)
			{
				return terminal.IsSameConstructAs(program.Me);
			}

			/// <summary>
			/// For parsing terminal's customdata.
			/// </summary>
			public abstract U CreateSetting(T item);

			/// <summary>
			/// Checks if the terminal is null, gone from world, or broken off from grid.
			/// </summary>
			public bool IsCorrupt(T block)
			{
				if (block == null || block.WorldMatrix == MatrixD.Identity) return true;
				return !(program.GridTerminalSystem.GetBlockWithId(block.EntityId) == block);
			}


		}

	}
}
