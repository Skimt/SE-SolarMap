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
	partial class Program : MyGridProgram
	{

		private const UpdateFrequency FREQUENCY = UpdateFrequency.Update10;

		public World world;
		public List<CelestialBody> celestialBodies;
		private readonly TerminalManager terminalManager;
		private readonly ProgrammableBlock programmableBlock;

		public Program()
		{

			// ---------------------------------------------------------------
			// Celestial bodies - Start. 
			celestialBodies = new List<CelestialBody>()
			{
				new CelestialBody
				{
					Name = "EarthLike",
					Radius = 60000,
					Gravity = 1,
					HasAtmosphere = true,
					Oxygen = Oxygen.High,
					Type = CelestialType.Planet,
					Position = new Vector3(0.5f, 0.5f, 0.5f),
					Resources = "All"
				},
				new CelestialBody
				{
					Name = "Moon",
					Radius = 9500,
					Gravity = 0.25f,
					HasAtmosphere = false,
					Oxygen = Oxygen.None,
					Type = CelestialType.Moon,
					Position = new Vector3(16384.5f, 136384.5f, -113615.5f),
					Resources = "All"
				},
				new CelestialBody
				{
					Name = "Mars",
					Radius = 60000,
					Gravity = 0.9f,
					HasAtmosphere = true,
					Oxygen = Oxygen.None,
					Type = CelestialType.Planet,
					Position = new Vector3(1031072.5f, 131072.5f, 1631072.5f),
					Resources = "All"
				},
				new CelestialBody
				{
					Name = "Europa",
					Radius = 9500,
					Gravity = 0.25f,
					HasAtmosphere = true,
					Oxygen = Oxygen.None,
					Type = CelestialType.Moon,
					Position = new Vector3(916384.5f, 16384.5f, 1616384.5f),
					Resources = "All"
				},
				new CelestialBody
				{
					Name = "Alien",
					Radius = 60000,
					Gravity = 1.1f,
					HasAtmosphere = true,
					Oxygen = Oxygen.Low,
					Type = CelestialType.Planet,
					Position = new Vector3(131072.5f, 131072.5f, 5731072.5f),
					Resources = "All"
				},
				//new CelestialBody
				//{
				//	Name = "Alien2",
				//	Radius = 60000,
				//	Gravity = 1.1f,
				//	HasAtmosphere = true,
				//	Oxygen = Oxygen.Low,
				//	Type = CelestialType.Planet,
				//	Position = new Vector3(131072.5f, 131072.5f, -4731072.5f),
				//	Resources = "All"
				//},
				//new CelestialBody
				//{
				//	Name = "Alien3",
				//	Radius = 60000,
				//	Gravity = 1.1f,
				//	HasAtmosphere = true,
				//	Oxygen = Oxygen.Low,
				//	Type = CelestialType.Planet,
				//	Position = new Vector3(-831072.5f, 131072.5f, -8731072.5f),
				//	Resources = "All"
				//},
				new CelestialBody
				{
					Name = "Titan",
					Radius = 9500,
					Gravity = 0.25f,
					HasAtmosphere = true,
					Oxygen = Oxygen.None,
					Type = CelestialType.Moon,
					Position = new Vector3(36384.5f, 226384.5f, 5796384.5f),
					Resources = "All"
				}
				/*,
				new CelestialBody
				{
					Name = "Triton",
					Radius = 40126.5f,
					Gravity = 1,
					HasAtmosphere = true,
					Oxygen = Oxygen.High,
					Type = CelestialType.Planet,
					Position = new Vector3(-284463.5f, -2434463.5, 365536.5f),
					Resources = "All"
				}
				*/
			};
			// Celestial bodies - End.
			// ---------------------------------------------------------------

			programmableBlock = new ProgrammableBlock(this, FREQUENCY);
			world = new World(this);
			terminalManager = new TerminalManager(this);

		}

		public void Main(string arg, UpdateType updateType)
		{

			// The update type is binary. Must look it up on Malware's wikia to figure out how to manipulate it.
			if ((updateType & FrequencyByUpdateType[FREQUENCY]) == 0)
				return;

			terminalManager.Run();
			programmableBlock.Draw();

		}

		private readonly Dictionary<UpdateFrequency, UpdateType> FrequencyByUpdateType = new Dictionary<UpdateFrequency, UpdateType>
		{
			{ UpdateFrequency.Update1, UpdateType.Update1 },
			{ UpdateFrequency.Update10, UpdateType.Update10 }
		};

	}
}
