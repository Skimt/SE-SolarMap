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
		public class World
		{

			private const int SCALE = 2;

			protected readonly Program program;
			private readonly float radius;
			private readonly Vector2 maxOffset;

			public World(Program program)
			{

				this.program = program;

				CelestialMap = program.celestialBodies;
				CelestialInfo = new List<CelestialBody>(CelestialMap); // Copied due to sorting.

				// Setup map specfic properties. 
				foreach (CelestialBody celestialBody in CelestialMap)
				{

					// Finds the farthest point from origo in the solar system, and scales it (used to create a sort of margin on the LCD).
					radius = celestialBody.Position.X > radius ? celestialBody.Position.X * SCALE : radius;
					radius = celestialBody.Position.Z > radius ? celestialBody.Position.Z * SCALE : radius;

					// Finds the farthest points from origo in the solar system, to center the map. 
					maxOffset.X = celestialBody.Position.X > maxOffset.X ? celestialBody.Position.X : maxOffset.X;
					maxOffset.Y = celestialBody.Position.Z > maxOffset.Y ? celestialBody.Position.Z : maxOffset.Y;

				}

				// The map object needs to be sorted.
				CelestialMap.Sort(SortByDistance);

			}

			public List<CelestialBody> CelestialMap { get; }
			public List<CelestialBody> CelestialInfo { get; }
			public Vector3 GridPosition { get; private set; }

			/// <summary>
			/// Retrieve Vector2 equivalent in percent to be multipled to orbital position later.
			/// </summary>
			public Vector2 WorldToMapPercent(Vector3 position)
			{

				Vector2 mapCoordinates = new Vector2(2 * position.X + radius - maxOffset.X, 2 * position.Z + radius - maxOffset.Y);

				// Turns the mapCoordinates into a percentage between 0 and 1, and reverse so that the coordinates will be flipped on screen.  
				Vector2 reversedPosition = Vector2.One - (mapCoordinates / (2 * radius)); 

				return reversedPosition; 

			}

			/// <summary>
			/// Updates the position of the grid. 
			/// </summary>
			public void UpdateGridPosition()
			{
				if (program.terminalManager.ShipController.HasController)
				{
					GridPosition = program.terminalManager.ShipController.Main.CenterOfMass;
				}
				else
				{
					GridPosition = program.Me.GetPosition();
				}
			}

			/// <summary>
			/// Used to sort celestial bodies by distance so that sprites will stack nicely. 
			/// </summary>
			private int SortByDistance(CelestialBody a, CelestialBody b)
			{

				float distanceA = (new Vector2(a.Position.X, a.Position.Z) - Vector2.One).LengthSquared();
				float distanceB = (new Vector2(b.Position.X, b.Position.Z) - Vector2.One).LengthSquared();

				if (distanceA > distanceB)
					return -1;
				else if (distanceB > distanceA)
					return 1;
				return 0;

			}

			#region Not used right now.
			private int SortByType(CelestialBody a, CelestialBody b)
			{
				if (a.Type == CelestialType.Moon && b.Type == CelestialType.Planet)
					return -1;
				else if (b.Type == CelestialType.Moon && a.Type == CelestialType.Planet)
					return 1;
				return 0;
			}
			#endregion

		}
	}
}
