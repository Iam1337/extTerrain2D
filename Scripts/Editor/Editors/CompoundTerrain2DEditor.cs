/* Copyright (c) 2018 ExT (V.Sigalkin) */

using UnityEngine;

using UnityEditor;
using UnityEditor.SceneManagement;

namespace extTerrain2D.Editor.Editors
{
	[CustomEditor(typeof(CompoundTerrain2D))]
	public class CompoundTerrain2DEditor : UnityEditor.Editor
	{
		#region Static Private Vars

		private static readonly GUIContent _nameContent = new GUIContent("Name:");

		private static readonly GUIContent _rebuildContent = new GUIContent("Rebuild");

		private static readonly GUIContent _selectContent = new GUIContent("Select");

		private static readonly GUIContent _removeContent = new GUIContent("Remove");

		private static readonly GUIContent _refreshContent = new GUIContent("Refresh");

		private static readonly GUIContent _groundMaterialContent = new GUIContent("Ground Material:");

		private static readonly GUIContent _lineMaterialContent = new GUIContent("Line Material:");

		private static readonly GUIContent _addSegmentContent = new GUIContent("Add Segment");

		#endregion

		#region Public Vars

		#endregion

		#region Protected Vars

		#endregion

		#region Private Vars

		private CompoundTerrain2D _compoundTerrain;

		private SerializedProperty _groundMaterialProperty;

		private SerializedProperty _lineMaterialProperty;

		#endregion

		#region Unity Methods

		protected void OnEnable()
		{
			_compoundTerrain = target as CompoundTerrain2D;

			_groundMaterialProperty = serializedObject.FindProperty("_groundMaterial");
			_lineMaterialProperty = serializedObject.FindProperty("_lineMaterial");
		}

		public override void OnInspectorGUI()
		{
			var defaultColor = GUI.color;

			serializedObject.Update();

			// LOGO
			GUILayout.Space(10);
			EditorUtils.DrawLogo();
			GUILayout.Space(5);

			EditorGUI.BeginChangeCheck();

			// SETTINGS BLOCK
			EditorGUILayout.LabelField("Compound Terrain 2D", EditorStyles.boldLabel);
			GUILayout.BeginVertical("box");

			// COMPOUND TERRAIN SETTINGS BOX
			EditorGUILayout.LabelField("Compound Terrain Settings:", EditorStyles.boldLabel);
			GUILayout.BeginVertical("box");

			// GROUND MATERIAL
			EditorGUILayout.PropertyField(_groundMaterialProperty, _groundMaterialContent);

			// LINE MATERIAL 
			EditorGUILayout.PropertyField(_lineMaterialProperty, _lineMaterialContent);

			// COMPOUND TERRAIN SETTINGS BOX END
			EditorGUILayout.EndVertical();

			// TERRAIN SETTINGS BOX
			EditorGUILayout.LabelField("Terrains Segments:", EditorStyles.boldLabel);
			GUILayout.BeginVertical("box");

			GUI.color = Color.yellow;
			var refresh = GUILayout.Button(_refreshContent);
			GUI.color = defaultColor;
			if (refresh) _compoundTerrain.RebuildChilds();

			EditorGUILayout.EndVertical();

			var terrainsCount = _compoundTerrain.GetTerrainsCount();

			GUILayout.BeginVertical("box");

			if (terrainsCount > 0)
			{
				for (var index = 0; index < terrainsCount; index++)
				{
					var removeButton = false;

					DrawTerrainSettings(index, out removeButton);

					if (removeButton)
					{
						RemoveTerrainSegment(index);
						break;
					}
				}
			}
			else
			{
				EditorGUILayout.LabelField("- none -", CustomEditorStyles.CenterLabel);
			}

			EditorGUILayout.EndVertical();


			GUILayout.BeginVertical("box");

			GUI.color = Color.green;
			var addSegment = GUILayout.Button(_addSegmentContent);
			GUI.color = defaultColor;
			if (addSegment) CreateTerrainSegment();

			// TERRAIN SETTINGS BOX END
			EditorGUILayout.EndVertical();

			// SETTINGS BLOCK END
			EditorGUILayout.EndVertical();

			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
			}
		}

		protected void OnSceneGUI()
		{
			var terrainsCount = _compoundTerrain.GetTerrainsCount();
			for (var index = 0; index < terrainsCount; index++)
			{
				var terrain = _compoundTerrain.GetTerrain(index);
				var terrainPosition = terrain.transform.position;

				var x = terrainPosition.x + (terrain.Width * terrain.transform.localScale.x) / 2f;

				var position = new Vector3(x, terrainPosition.y, terrainPosition.z);
				var size = HandleUtility.GetHandleSize(position) * 0.35f;

				Handles.color = Color.white;
				var removeButton = Handles.Button(position, Quaternion.identity, size, size, EditorUtils.RemoveSegmentHandleCap);
				if (removeButton)
				{
					RemoveTerrainSegment(index);
					break;
				}

				if (index == terrainsCount - 1)
				{
					var keypointsCount = terrain.GetKeyPointsCount();
					var keypoint = terrain.GetKeyPoint(keypointsCount - 1);

					if (keypoint != null)
					{
						var worldPosition = terrain.KeyPointToWorld(keypoint.Position);
						var y = worldPosition.y - (worldPosition.y - terrainPosition.y) / 2f;

						position = new Vector3(worldPosition.x, y, terrainPosition.z);

						Handles.color = Color.white;
						var createButton = Handles.Button(position, Quaternion.identity, size, size, EditorUtils.AddSegmentHandleCap);
						if (createButton)
						{
							CreateTerrainSegment();
						}
					}
				}

				var offset = Vector3.up * HandleUtility.GetHandleSize(position) * 0.35f;
				var startPosition = terrainPosition;
				var endPosition = new Vector3(terrainPosition.x + (terrain.Width * terrain.transform.localScale.x), terrainPosition.y, terrainPosition.z);

				Handles.color = Color.red;
				Handles.DrawLine(startPosition, startPosition - offset);
				Handles.DrawLine(endPosition, endPosition - offset);
			}
		}

		#endregion

		#region Public Methods

		#endregion

		#region Protected Methods

		#endregion

		#region Private Methods

		private void CreateTerrainSegment()
		{
			_compoundTerrain.AddTerrain();

			var terrainCount = _compoundTerrain.GetTerrainsCount();
			var terrain = _compoundTerrain.GetTerrain(terrainCount - 1);
			terrain.name = "Terrain " + terrainCount;

			var neighborsTerrain = _compoundTerrain.GetTerrain(terrainCount - 2);
			if (neighborsTerrain != null)
			{
				terrain.Width = neighborsTerrain.Width;
				terrain.Height = neighborsTerrain.Height;
				terrain.LineWidth = neighborsTerrain.LineWidth;
				terrain.Collider = neighborsTerrain.Collider;
				terrain.GroundUVOffset = neighborsTerrain.GroundUVOffset;
				terrain.GroundUVScale = neighborsTerrain.GroundUVScale;
				terrain.Quality = neighborsTerrain.Quality;
			}

			terrain.Rebuild();

			EditorUtility.SetDirty(terrain);

			_compoundTerrain.RebuildNeighbors(terrainCount - 2);

			Undo.RegisterCreatedObjectUndo(terrain.gameObject, "Add Terrain Segment");
		}

		private void RemoveTerrainSegment(int index)
		{
			var terrain = _compoundTerrain.GetTerrain(index);

			_compoundTerrain.RemoveTerrain(index);

			Undo.DestroyObjectImmediate(terrain.gameObject);
		}

		private void DrawTerrainSettings(int index, out bool remove)
		{
			var defaultColor = GUI.color;
			var terrain = _compoundTerrain.GetTerrain(index);

			EditorGUILayout.BeginVertical("box");

			EditorGUILayout.BeginHorizontal();

			EditorGUI.BeginChangeCheck();
			terrain.name = EditorGUILayout.TextField(_nameContent, terrain.name);
			if (EditorGUI.EndChangeCheck())
			{
				EditorSceneManager.MarkSceneDirty(terrain.gameObject.scene);
			}

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();

			var select = GUILayout.Button(_selectContent);
			if (select) Selection.activeGameObject = terrain.gameObject;

			GUI.color = Color.yellow;
			var rebuild = GUILayout.Button(_rebuildContent);
			if (rebuild) _compoundTerrain.RebuildNeighbors(index);

			GUI.color = Color.red;
			remove = GUILayout.Button(_removeContent);

			GUI.color = defaultColor;

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.EndVertical();
		}

		#endregion
	}
}