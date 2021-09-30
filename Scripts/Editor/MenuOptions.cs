/* Copyright (c) 2021 ExT (V.Sigalkin) */

using UnityEditor;

namespace extTerrain2D.Editor
{
    public static class MenuOptions
    {
        #region Static Public Methods

        [MenuItem("GameObject/extTerrain2D/Terrain 2D", false, 40)]
        public static void AddTerrain2D(MenuCommand menuCommand)
        {
			var line = EditorAssets.LineMaterial;
			var ground = EditorAssets.GroundMaterial;

			var terrain = Utils.CreateTerrain2D(ground, line);

			Undo.RegisterCreatedObjectUndo(terrain.gameObject, "Create Terrain2D");
        }

		[MenuItem("GameObject/extTerrain2D/Compound Terrain 2D", false, 42)]
		public static void AddCompoundTerrain2D(MenuCommand menuCommand)
		{
			var line = EditorAssets.LineMaterial;
			var ground = EditorAssets.GroundMaterial;

			var terrain = Utils.CreateCompoundTerrain2D(ground, line);

			Undo.RegisterCreatedObjectUndo(terrain.gameObject, "Create Compound Terrain2D");
		}

        #endregion
    }
}