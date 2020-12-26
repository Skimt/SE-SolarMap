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
using System.Diagnostics;
using System.Timers;

namespace IngameScript
{
	partial class Program
	{
		public class ProgrammableBlock
		{

			protected Program program;
			private readonly IMyTextSurface textSurface;
			private readonly double[] runTimes = new double[10];
			private readonly int[] instructionCounts = new int[10];
			private int tick = 0;
			private TimeSpan time = new TimeSpan();

			public ProgrammableBlock(Program program, UpdateFrequency updateFrequency = UpdateFrequency.Update10)
			{
				this.program = program;
				program.Runtime.UpdateFrequency = updateFrequency;
				textSurface = program.Me.GetSurface(0);
				textSurface.ContentType = ContentType.SCRIPT;
			}

			private string AverageRunTime => runTimes.Average().ToString("F2");
			private int AverageInstructions => (int)instructionCounts.Average();
			private int HighestInstruction => instructionCounts.Max();

			public void Draw()
			{

				if (program.Me.CubeGrid.GridSizeEnum == MyCubeSize.Small)
					return;

				using (MySpriteDrawFrame frame = textSurface.DrawFrame())
				{

					frame.Add(new MySprite(SpriteType.TEXT, "SolarMap v0.9.1", new Vector2(10, 100), null, Color.White, null, TextAlignment.LEFT, 0.7f));
					frame.Add(new MySprite(SpriteType.TEXT, "Average instructions:\nInstruction peak:\nAverage runtime:\nRuntime:\nUpdates:", new Vector2(10, 125), null, Color.White, null, TextAlignment.LEFT, 0.6f));

					// Calculate various. 
					runTimes[tick] = program.Runtime.LastRunTimeMs;
					instructionCounts[tick] = program.Runtime.CurrentInstructionCount;
					time += program.Runtime.TimeSinceLastRun;
					tick = tick > 8 ? 0 : ++tick;

					frame.Add(new MySprite(SpriteType.TEXT,
						AverageInstructions + "/" + program.Runtime.MaxInstructionCount + "\n" +
						HighestInstruction + "\n" +
						AverageRunTime + " ms\n" + 
						time.ToString(@"dd\.hh\:mm\:ss") + "\n" +
						program.terminalManager.ShipController.UpdateCount, 
						new Vector2(200, 125), null, Color.White, null, TextAlignment.LEFT, 0.6f));

					if (!program.terminalManager.ShipController.HasController)
					{
						frame.Add(new MySprite(SpriteType.TEXT, "Status", new Vector2(10, 250), null, Color.White, null, TextAlignment.LEFT, 0.7f));
						frame.Add(new MySprite(SpriteType.TEXT, "- Controller does not exist.", new Vector2(10, 275), null, Color.White, null, TextAlignment.LEFT, 0.6f));
					}

				}

			}

		}
	}
}
