/* Copyright (c) 2018 ExT (V.Sigalkin) */

using UnityEngine;

namespace extTerrain2D
{
	public static class Utils
	{
		#region Static Public Methods

		public static float Map(float value, float inputMin, float inputMax, float outputMin, float outputMax, bool clamp = true)
		{
			if (Mathf.Abs(inputMin - inputMax) < Mathf.Epsilon) return outputMin;

			float outputValue = ((value - inputMin) / (inputMax - inputMin) * (outputMax - outputMin) + outputMin);

			if (clamp)
			{
				if (outputMax < outputMin) outputValue = Mathf.Clamp(outputValue, outputMax, outputMin);
				else outputValue = Mathf.Clamp(outputValue, outputMin, outputMax);
			}

			return outputValue;
		}

		public static Vector2 Beizer(KeyPoint startKey, KeyPoint endKey, float time)
		{
			var startPoint = startKey.Position;
			var startHandle = startPoint + startKey.RightHandlePosition;
			var endPoint = endKey.Position;
			var endHandle = endPoint + endKey.LeftHandlePosition;
			var inverseTime = 1f - time;

			var step1 = inverseTime * startPoint + time * startHandle;
			var step2 = inverseTime * startHandle + time * endHandle;
			var step3 = inverseTime * endHandle + time * endPoint;
			var step4 = inverseTime * step1 + time * step2;
			var step5 = inverseTime * step2 + time * step3;

			return inverseTime* step4 + time * step5;
		}

		public static Terrain2D CreateTerrain2D(Material groundMaterial, Material lineMaterial)
		{
			var root = new GameObject("Terrain 2D");

			root.AddComponent<MeshFilter>();
			var renderer = root.AddComponent<MeshRenderer>();
			renderer.sharedMaterials = new Material[] { groundMaterial, lineMaterial };

			var terrain = root.AddComponent<Terrain2D>();
			if (terrain.GetKeyPointsCount() == 0) // Fix for editor OnValidate.
			{
				terrain.AddKeyPoint();
				var keypoint = terrain.GetKeyPoint(0);
				keypoint.Position = new Vector2(0, 0);

				terrain.AddKeyPoint();
				keypoint = terrain.GetKeyPoint(1);
				keypoint.Position = new Vector2(1, 0);
			}

			terrain.Rebuild();

			return terrain;
		}

		public static CompoundTerrain2D CreateCompoundTerrain2D(Material groundMaterial, Material lineMaterial)
		{
			var root = new GameObject("Compound Terrain 2D");

			var compoundTerrain = root.AddComponent<CompoundTerrain2D>();
			compoundTerrain.GroundMaterial = groundMaterial;
			compoundTerrain.LineMaterial = lineMaterial;

			var terrain = CreateTerrain2D(groundMaterial, lineMaterial);
			terrain.name = "Terrain 0";
			terrain.transform.parent = root.transform;
			terrain.transform.localPosition = Vector3.zero;
			terrain.transform.localRotation = Quaternion.identity;
			terrain.transform.localScale = Vector3.one;

			return compoundTerrain;
		}

		#endregion

		#region Static Protected Methods

		#endregion

		#region Static Private Methods

		#endregion
	}
}