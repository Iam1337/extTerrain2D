/* Copyright (c) 2021 ExT (V.Sigalkin) */

using UnityEngine;

using UnityEditor;

namespace extTerrain2D.Editor
{
    public static class CustomEditorStyles
    {
        #region Static Private Vars

        private static GUIStyle _centerLabel;

        private static GUIStyle _centerBoldLabel;

        #endregion

        #region Static Public Vars

        public static GUIStyle CenterLabel
        {
            get
            {
                if (_centerLabel == null)
                {
                    _centerLabel = new GUIStyle(EditorStyles.label)
                    {
                        alignment = TextAnchor.MiddleCenter
                    };
                }

                return _centerLabel;
            }
        }

        public static GUIStyle CenterBoldLabel
        {
            get
            {
                if (_centerBoldLabel == null)
                {
                    _centerBoldLabel = new GUIStyle(EditorStyles.label)
                    {
                        alignment = TextAnchor.MiddleCenter,
                        fontStyle = FontStyle.Bold
                    };
                }

                return _centerBoldLabel;
            }
        }

        #endregion
    }
}