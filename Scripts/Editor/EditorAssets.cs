/* Copyright (c) 2021 ExT (V.Sigalkin) */

using UnityEditor;
using UnityEngine;

namespace extTerrain2D.Editor
{
	public static class EditorAssets
	{
		#region Static Private Vars

		private const string _defaultFolder = "extTerrain2D/";

		private static Texture2D _iwIcon;

		private static bool _isProSkin;

		private static Mesh _quadMesh;

		private static Material _terrainLine;

		private static Material _terrainGround;

		private static Material _addSegment;

		private static Material _removeSegment;

		private static Material _keypointMaterial;

		public static Material _keypointHandleMaterial;

		#endregion

		#region Static Public Vars

		public static Texture2D IronWall
		{
			get
			{
				if (_iwIcon == null || EditorGUIUtility.isProSkin != _isProSkin)
				{
					_isProSkin = EditorGUIUtility.isProSkin;

					if (_iwIcon != null)
					{
						Resources.UnloadAsset(_iwIcon);
					}

					_iwIcon = LoadTexture(_isProSkin ? "IW_logo_light" : "IW_logo_dark");
				}

				return _iwIcon;
			}
		}

		public static Mesh QuadMesh
		{
			get
			{
				if (_quadMesh == null)
				{
					_quadMesh = new Mesh();

					var vertices = new Vector3[4];
					vertices[0] = new Vector3(-0.5f, -0.5f, 0);
					vertices[1] = new Vector3(-0.5f, 0.5f, 0);
					vertices[2] = new Vector3(0.5f, -0.5f, 0);
					vertices[3] = new Vector3(0.5f, 0.5f, 0);

					var triangles = new int[] { 0, 1, 2, 3, 2, 1 };

					var uv = new Vector2[vertices.Length];
					uv[0] = new Vector2(0, 0);
					uv[1] = new Vector2(0, 1);
					uv[2] = new Vector2(1, 0);
					uv[3] = new Vector2(1, 1);

					_quadMesh.vertices = vertices;
					_quadMesh.uv = uv;
					_quadMesh.triangles = triangles;
				}

				return _quadMesh;
			}
		}

		public static Material LineMaterial
		{
			get
			{
				if (_terrainLine == null)
					_terrainLine = LoadMaterial("terrain-line");

				return _terrainLine;
			}
		}

		public static Material GroundMaterial
		{
			get
			{
				if (_terrainGround == null)
					_terrainGround = LoadMaterial("terrain-ground");

				return _terrainGround;
			}
		}

		public static Material AddSegmentMaterial
		{
			get
			{
				if (_addSegment == null)
				{
					_addSegment = LoadTextureToMaterial("terrain-addsegment");
				}

				return _addSegment;
			}
		}

		public static Material RemoveSegmentMaterial
		{
			get
			{
				if (_removeSegment == null)
				{
					_removeSegment = LoadTextureToMaterial("terrain-removesegment");
				}

				return _removeSegment;
			}
		}

		public static Material KeyPointMaterial
		{
			get
			{
				if (_keypointMaterial == null)
				{
					_keypointMaterial = LoadTextureToMaterial("terrain-keypoint");
				}

				return _keypointMaterial;
			}
		}

		public static Material KeyPointHandleMaterial
		{
			get
			{
				if (_keypointHandleMaterial == null)
				{
					_keypointHandleMaterial = LoadTextureToMaterial("terrain-keypoint-handle");
				}

				return _keypointHandleMaterial;
			}
		}

		#endregion

		#region Static Private Methods

		private static Texture2D LoadTexture(string fileName)
		{
			return Resources.Load<Texture2D>(_defaultFolder + fileName);
		}

		private static Material LoadMaterial(string fileName)
		{
			return Resources.Load<Material>(_defaultFolder + fileName);
		}

		private static Material LoadTextureToMaterial(string fileName)
		{
			var texture = LoadTexture(fileName);

			var material = new Material(Shader.Find("Hidden/extTerrain2D-Handles"));
			material.SetTexture("_MainTex", texture);
			material.SetColor("_Color", Color.white);

			return material;
		}

		#endregion
	}
}