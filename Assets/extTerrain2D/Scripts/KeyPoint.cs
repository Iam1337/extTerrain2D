/* Copyright (c) 2021 ExT (V.Sigalkin) */

using UnityEngine;

using System;

namespace extTerrain2D
{
	[Serializable]
	public class KeyPoint
	{
		#region Public Vars

		public Vector2 Position
		{
			get { return _position; }
			set { _position = value; }
		}

		public Vector2 LeftHandlePosition
		{
			get { return _leftHandlePosition; }
		}

		public Vector2 RightHandlePosition
		{
			get { return _rightHandlePosition; }
		}

		public HandleType HandlesType
		{
			get { return _handlesType; }
			set { _handlesType = value; }
		}

		#endregion

		#region Private Vars

		[SerializeField]
		private Vector2 _position;

		[SerializeField]
		private Vector2 _leftHandlePosition = new Vector2(-0.1f, 0f);

		[SerializeField]
		private Vector2 _rightHandlePosition = new Vector2(0.1f, 0);

		[SerializeField]
		private HandleType _handlesType;

		#endregion

		#region Public Methods

		public void SetLeftHandle(Vector2 position)
		{
			_leftHandlePosition = position;

			if (_handlesType == HandleType.FreeSmooth)
			{
				_rightHandlePosition = -position;
			}
		}

		public void SetRightHandle(Vector2 position)
		{
			_rightHandlePosition = position;

			if (_handlesType == HandleType.FreeSmooth)
			{
				_leftHandlePosition = -position;
			}
		}

		#endregion
	}
}