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
		public class CelestialBody
		{

			public CelestialType Type;
			public Oxygen Oxygen;
			public Vector3 Position;
			public bool HasAtmosphere;
			public float Radius;
			public float Gravity;
			public string Name;
			public string Resources;

			
			/// <summary>
			/// OrbitPosition uses standard LCD size. 
			/// </summary>
			public Vector2 OrbitPosition = new Vector2(512); // Standard LCD size.

			// Properties used in map.
			public Vector2 PlanetPosition;
			public Vector2 OrbitSize;
			public Vector2 PlanetSize;
			public Vector2 LblTitlePos;
			public Vector2 LblDistancePos;

		}
	}
}
