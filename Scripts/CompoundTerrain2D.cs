/* Copyright (c) 2018 ExT (V.Sigalkin) */

using UnityEngine;

using System.Collections.Generic;

namespace extTerrain2D
{
	[ExecuteInEditMode]
	public class CompoundTerrain2D : MonoBehaviour
	{
		#region Public Vars

		public Material GroundMaterial
		{
			get { return _groundMaterial; }
			set { _groundMaterial = value; }
		}

		public Material LineMaterial
		{
			get { return _lineMaterial; }
			set { _lineMaterial = value; }
		}

		#endregion

		#region Private Vars

		[SerializeField]
		private Material _groundMaterial;

		[SerializeField]
		private Material _lineMaterial;

		private List<Terrain2D> _terrains = new List<Terrain2D>();

		#endregion

		#region Unity Methods

		protected void Awake()
		{
			RebuildChilds();
		}

		protected void Update()
		{
			RebuildPositions();
		}

		protected void OnEnable()
		{
			RebuildChilds();
		}

		protected void OnTransformChildrenChanged()
		{
			RebuildChilds();

			for (var index = 0; index < _terrains.Count; index++)
			{
				RebuildNeighbors(index);
			}
		}

		#endregion

		#region Public Methods

		public void Rebuild(Terrain2D terrain)
		{
			if (!_terrains.Contains(terrain))
				return;

			RebuildNeighbors(_terrains.IndexOf(terrain));
		}

		//TODO: Editor only?
		public void RebuildChilds()
		{
			_terrains.Clear();

			foreach (Transform child in transform)
			{
				var terrain = child.GetComponent<Terrain2D>();
				if (terrain != null) _terrains.Add(terrain);
			}
		}

		//TODO: Editor only?
		public void RebuildPositions()
		{
			var localPosition = Vector3.zero;

			foreach (var terrain in _terrains)
			{
				terrain.transform.localPosition = localPosition;

				localPosition.x += terrain.Width * terrain.transform.localScale.x;
			}
		}

		public void RebuildNeighbors(int index)
		{
			if (index < 0 || index >= _terrains.Count)
				return;

			var terrain = _terrains[index];

			if (index > 0)
			{
				var neighborIndex = index - 1;
				var neighborTerrain = _terrains[neighborIndex];

				var neighborKeypointsCount = neighborTerrain.GetKeyPointsCount();
				var neighborKeypoint = neighborTerrain.GetKeyPoint(neighborKeypointsCount - 1);

				var keypoint = terrain.GetKeyPoint(0);
				var worldPosition = terrain.KeyPointToWorld(keypoint.Position);
				var localPositon = neighborTerrain.WorldToKeyPoint(worldPosition);
				localPositon.x = neighborKeypoint.Position.x;
				neighborKeypoint.Position = localPositon;
				neighborKeypoint.HandlesType = keypoint.HandlesType;
				neighborKeypoint.SetRightHandle(keypoint.RightHandlePosition);

				neighborTerrain.Rebuild();
			}

			if (index <= _terrains.Count - 2)
			{
				var neighborIndex = index + 1;
				var neighborTerrain = _terrains[neighborIndex];

				var neighborKeypoint = neighborTerrain.GetKeyPoint(0);

				var keypointsCount = terrain.GetKeyPointsCount();
				var keypoint = terrain.GetKeyPoint(keypointsCount - 1);
				var worldPosition = terrain.KeyPointToWorld(keypoint.Position);
				var localPositon = neighborTerrain.WorldToKeyPoint(worldPosition);
				localPositon.x = neighborKeypoint.Position.x;
				neighborKeypoint.Position = localPositon;
				neighborKeypoint.HandlesType = keypoint.HandlesType;
				neighborKeypoint.SetLeftHandle(keypoint.LeftHandlePosition);

				neighborTerrain.Rebuild();
			}
		}

		public void AddTerrain()
		{
			InsertTerrain(_terrains.Count);
		}

		public void InsertTerrain(int index)
		{
			var terrain = Utils.CreateTerrain2D(_groundMaterial, _lineMaterial);
			terrain.transform.parent = transform;
			terrain.transform.SetSiblingIndex(index);

			RebuildChilds();
			RebuildPositions();
		}

		public void RemoveTerrain(int index)
		{
			if (index == 0 || index >= _terrains.Count)
				return;

			var terrain = GetTerrain(index);

			_terrains.RemoveAt(index);
		}

		public int GetTerrainsCount()
		{
			return _terrains.Count;
		}

		public Terrain2D GetTerrain(int index)
		{
			if (index < 0 || index >= _terrains.Count)
				return null;

			return _terrains[index];
		}

		#endregion

		#region Private Methods

		#endregion
	}
}