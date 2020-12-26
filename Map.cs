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
		public class Map
		{

			private readonly IEnumerable<CelestialBody> planets;
			//private readonly IEnumerable<CelestialBody> moons;
			private readonly World world;
			private readonly ColorManager colorManager;

			private Vector2 planetPositionMultiplier = Vector2.One; 
			private Vector2 orbitPositionMultiplier = Vector2.One;
			private Vector2 lcdSize = new Vector2(512);
			private TextPanelSetting setting;

			public Map(ColorManager colorManager, World world)
			{

				this.colorManager = colorManager;
				this.world = world;

				planets = world.CelestialMap.Where(cb => cb.Type == CelestialType.Planet);
				//moons = world.CelestialMap.Where(cb => cb.Type == CelestialType.Moon);

				foreach (CelestialBody planet in planets)
				{

					planet.PlanetPosition = planet.OrbitPosition * world.WorldToMapPercent(planet.Position);
					planet.OrbitSize = new Vector2(Vector2.Distance(planet.PlanetPosition, planet.OrbitPosition)) * 2;
					planet.PlanetSize = planet.OrbitPosition * planet.Radius * 0.000001f - 0.01f;
					planet.LblTitlePos = new Vector2(planet.PlanetPosition.X, planet.PlanetPosition.Y - planet.OrbitPosition.Y * 0.1f);
					planet.LblDistancePos = new Vector2(planet.PlanetPosition.X, planet.PlanetPosition.Y - planet.OrbitPosition.Y * 0.065f);

				}

			}

			public void UpdateSetting(TextPanelSetting setting)
			{
				this.setting = setting;
			}

			public void PaintOrbits(List<MySprite> sprites)
			{

				sprites.Clear();

				foreach (CelestialBody planet in planets)
				{

					Vector2 orbitPosition = planet.OrbitPosition * orbitPositionMultiplier;
					planet.OrbitSize = new Vector2(Vector2.Distance((planet.PlanetPosition + setting.Offset) * planetPositionMultiplier, orbitPosition)) * 2;

					// Border, then fill.
					sprites.Add(new MySprite(SpriteType.TEXTURE, "Circle", orbitPosition, planet.OrbitSize + 3, colorManager.Border));
					sprites.Add(new MySprite(SpriteType.TEXTURE, "Circle", orbitPosition, planet.OrbitSize, colorManager.Fill));

				}

			}

			public void PaintPlanets(List<MySprite> sprites)
			{

				sprites.Clear();

				foreach (CelestialBody planet in planets)
				{

					Vector2 planetPosition = (planet.PlanetPosition + setting.Offset) * planetPositionMultiplier;
					float distance = Vector3.Distance(planet.Position, world.GridPosition) / 1000;

					// Text.
					sprites.Add(new MySprite(SpriteType.TEXT, planet.Name, (planet.LblTitlePos + setting.Offset) * planetPositionMultiplier, null, colorManager.Text, null, rotation: 0.7f));
					sprites.Add(new MySprite(SpriteType.TEXT, distance.ToString("F1") + " km", (planet.LblDistancePos + setting.Offset) * planetPositionMultiplier, null, colorManager.Text, null, rotation: 0.55f));

					// Border, then fill.
					sprites.Add(new MySprite(SpriteType.TEXTURE, "Circle", planetPosition, planet.PlanetSize + 3, colorManager.Border));
					sprites.Add(new MySprite(SpriteType.TEXTURE, "Circle", planetPosition, planet.PlanetSize, colorManager.Fill));

				}

			}

			public MySprite PaintGrid(ShipController shipController)
			{

				Vector2 position = (lcdSize * world.WorldToMapPercent(world.GridPosition) + setting.Offset) * planetPositionMultiplier;

				if (shipController != null && shipController.IsMoveable)
				{
					float azimuth, elevation;
					Vector3.GetAzimuthAndElevation(shipController.Main.WorldMatrix.Forward, out azimuth, out elevation);
					return new MySprite(SpriteType.TEXTURE, "AH_BoreSight", position, lcdSize * 0.05f + 3, colorManager.Grid, null, rotation: -azimuth + (float)(Math.PI / 2f));
				}

				return new MySprite(SpriteType.TEXTURE, "Circle", position, lcdSize * 0.01f, colorManager.Grid);

			}

			/// <summary>
			/// Adjust multipliers for widescreens. 
			/// </summary>
			public void UpdateMultipliers(IMyTextPanel lcd)
			{
				if (lcd.SurfaceSize.X > 512)
				{
					planetPositionMultiplier = setting.HideInfo ? Vector2.One : new Vector2(2, 1);
					orbitPositionMultiplier = setting.HideInfo ? Vector2.One : new Vector2(1.5f, 1);
				}
				else
				{
					planetPositionMultiplier = setting.HideInfo ? Vector2.One : new Vector2(1.25f, 1);
					orbitPositionMultiplier = Vector2.One;
				}
			}

		}
	}
}
