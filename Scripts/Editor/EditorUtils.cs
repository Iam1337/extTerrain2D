/* Copyright (c) 2018 ExT (V.Sigalkin) */

using UnityEngine;

using UnityEditor;

namespace extTerrain2D.Editor
{
    public static class EditorUtils
    {
        #region Static Private Vars

        #endregion

        #region Static Public Methods

        public static void DrawLogo()
        {
            if (EditorAssets.IronWall != null)
            {
                EditorGUILayout.Space();

                var rect = GUILayoutUtility.GetRect(0, 0);
                var width = EditorAssets.IronWall.width * 0.2f;
                var height = EditorAssets.IronWall.height * 0.2f;

                rect.x = rect.width * 0.5f - width * 0.5f;
                rect.y = rect.y + rect.height * 0.5f - height * 0.5f;
                rect.width = width;
                rect.height = height;

                GUI.DrawTexture(rect, EditorAssets.IronWall);
                EditorGUILayout.Space();
            }
        }

		public static void AddSegmentHandleCap(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
		{
			TextureHandleCap(controlID, position, rotation, size, eventType, EditorAssets.AddSegmentMaterial);
		}

		public static void RemoveSegmentHandleCap(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
		{
			TextureHandleCap(controlID, position, rotation, size, eventType, EditorAssets.RemoveSegmentMaterial);
		}

		public static void KeyPointHandleCap(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
		{
			TextureHandleCap(controlID, position, rotation, size, eventType, EditorAssets.KeyPointMaterial);
		}

		public static void KeyPointHandleHandleCap(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
		{
			TextureHandleCap(controlID, position, rotation, size, eventType, EditorAssets.KeyPointHandleMaterial);
		}

		#endregion

		#region Static Private Methods

		public static void TextureHandleCap(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType, Material material)
		{
			if (eventType != EventType.Layout)
			{
				if (eventType == EventType.Repaint)
				{
					position = Handles.matrix.MultiplyPoint(position);

					var color = Handles.color * new Color(1f, 1f, 1f, 0.99f);
					var matrix = Handles.matrix * Matrix4x4.TRS(position, rotation, Vector3.one * size);

					material.SetColor("_Color", color);
					material.SetPass(0);

					Graphics.DrawMeshNow(EditorAssets.QuadMesh, matrix);
				}
			}
			else
			{
				HandleUtility.AddControl(controlID, HandleUtility.DistanceToRectangle(position, rotation, size));
			}
		}

        #endregion
    }
}