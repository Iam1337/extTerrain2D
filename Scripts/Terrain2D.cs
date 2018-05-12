/* Copyright (c) 2018 ExT (V.Sigalkin) */

using UnityEngine;

using System.Collections.Generic;

namespace extTerrain2D
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	public class Terrain2D : MonoBehaviour
	{
		#region Public Vars

		public float Width
		{
			get { return _width; }
			set { _width = value; }
		}

		public float Height
		{
			get { return _height; }
			set { _height = value; }
		}

		public float LineWidth
		{
			get { return _lineWidth; }
			set { _lineWidth = value; }
		}

		public int Quality
		{
			get { return _quality; }
			set { _quality = value; }
		}

		public Vector2 GroundUVOffset
		{
			get { return _groundUVOffset; }
			set { _groundUVOffset = value; }
		}

		public Vector2 GroundUVScale
		{
			get { return _groundUVScale; }
			set { _groundUVScale = value; }
		}

		public ColliderType Collider
		{
			get { return _colliderType; }
			set { _colliderType = value; }
		}

		#endregion

		#region Private Vars

		[SerializeField]
		private float _width = 10;

		[SerializeField]
		private float _height = 2;

		[SerializeField]
		private float _lineWidth = 0.2f;

		[Range(1, 50)]
		[SerializeField]
		private int _quality = 5;

		[SerializeField]
		private ColliderType _colliderType = ColliderType.Polygon;

		[SerializeField]
		private Vector2 _groundUVOffset = Vector2.zero;

		[SerializeField]
		private Vector2 _groundUVScale = Vector2.one;

		[SerializeField]
		private List<KeyPoint> _keyPoints = new List<KeyPoint>();

		private Mesh _mesh;

		private List<float> _heights = new List<float>();

		private MeshFilter _meshFilter;

		#endregion

		#region Unity Methods

		protected void Awake()
		{
			if (_mesh == null)
			{
				_mesh = new Mesh();
				_mesh.name = "extTerrain2D";
			}

			_meshFilter = GetComponent<MeshFilter>();
			_meshFilter.sharedMesh = _mesh;
		}

#if UNITY_EDITOR

		protected void OnDrawGizmos()
		{
			Gizmos.color = Color.red;

			var fromPosition = KeyPointToWorld(GetPosition(0));

			for (var i = 0; i < _heights.Count; i++)
			{
				var time = (1f / (_quality * _width)) * i;

				var toPosition = KeyPointToWorld(GetPosition(time));

				Gizmos.DrawLine(fromPosition, toPosition);

				fromPosition = toPosition;
			}
		}

		protected void OnValidate()
		{
			for (var i = _keyPoints.Count; i < 2; i++)
			{
				_keyPoints.Add(new KeyPoint());
			}

			if (_keyPoints.Count >= 2)
			{
				var keypoint = _keyPoints[0];
				var position = keypoint.Position;
				position.x = 0;
				keypoint.Position = position;

				keypoint = _keyPoints[_keyPoints.Count - 1];
				position = keypoint.Position;
				position.x = 1;
				keypoint.Position = position;
			}

			Rebuild();

			var compound = GetComponentInParent<CompoundTerrain2D>();
			if (compound != null) compound.Rebuild(this);
		}

#endif

		#endregion

		#region Public Methods

		public void Rebuild()
		{
			var segmentsCount = Mathf.CeilToInt(_width) * _quality + 1;

			RebuildHeights(segmentsCount);
			RebuildMesh(segmentsCount);
			RebuildCollider(segmentsCount);
		}

		public void AddKeyPoint()
		{
			InsertKeyPoint(_keyPoints.Count);
		}

		public void InsertKeyPoint(int index)
		{
			var keypoint = new KeyPoint();

			_keyPoints.Insert(index, keypoint);
		}

		public void RemoveKeyPoint(int index)
		{
			if (index == 0 || index >= _keyPoints.Count - 1)
				return;

			_keyPoints.RemoveAt(index);
		}

		public int GetKeyPointsCount()
		{
			return _keyPoints.Count;
		}

		public KeyPoint GetKeyPoint(int index)
		{
			if (index < 0 || index >= _keyPoints.Count)
				return null;

			return _keyPoints[index];
		}

		public Vector2 GetPosition(float time)
		{
			KeyPoint startKey = null;
			KeyPoint endKey = null;

			GetSegment(time, out startKey, out endKey);

			time = Utils.Map(time, startKey.Position.x, endKey.Position.x, 0f, 1f);

			return Utils.Beizer(startKey, endKey, time);
		}

		public Vector3 KeyPointToWorld(Vector2 vector)
		{
			var terrainPosition = transform.position;
			var terrainScale = transform.localScale;

			return new Vector3(
				terrainPosition.x + (vector.x * _width * terrainScale.x),
				terrainPosition.y + (_height * terrainScale.y) + (vector.y * terrainScale.y),
				terrainPosition.z);
		}

		public Vector2 WorldToKeyPoint(Vector3 vector)
		{
			var terrainPosition = transform.position;
			var terrainScale = transform.localScale;

			return new Vector2(
				(vector.x - terrainPosition.x) / _width / terrainScale.x,
				(vector.y - terrainPosition.y - (_height * terrainScale.y)) / terrainScale.y);
		}

		public void RefreshKeyPointsIndexes()
		{
			// Hehehehe
			_keyPoints.Sort((x, y) => x.Position.x.CompareTo(y.Position.x));
		}

		#endregion

		#region Private Methods

		private void GetSegment(float time, out KeyPoint startKey, out KeyPoint endKey)
		{
			startKey = null;
			endKey = null;

			foreach (var key in _keyPoints)
			{
				if (time < key.Position.x)
				{
					endKey = key;
					break;
				}

				if (time >= key.Position.x)
				{
					startKey = key;
				}
			}

			if (endKey == null)
				endKey = startKey;
		}

		private void RebuildHeights(int segmentsCount)
		{
			_heights.Clear();

			for (var i = 0; i < segmentsCount; i++)
			{
				var time = (1f / (_quality * _width)) * i;
				var position = GetPosition(time);

				_heights.Add(_height + position.y);
			}
		}

		private void RebuildMesh(int segmentsCount)
		{
			if (_mesh == null)
			{
				_mesh = new Mesh();
				_mesh.name = "extTerrain2D";
			}

			if (_meshFilter == null)
				_meshFilter = GetComponent<MeshFilter>();

			var vertices = new List<Vector3>();
			var maxHeight = 0f;

			for (var i = 0; i < segmentsCount; i++)
			{
				var time = (1f / (_quality * _width)) * i;
				var position = GetPosition(time);

				var x = _width * position.x;
				var y = Mathf.Max(0, _heights[i] - _lineWidth);

				vertices.Add(new Vector3(x, 0f, 0f));
				vertices.Add(new Vector3(x, y, 0f));

				if (maxHeight < y)
					maxHeight = y;
			}

			for (var i = 0; i < segmentsCount; i++)
			{
				var time = (1f / (_quality * _width)) * i;
				var position = GetPosition(time);

				var x = _width * position.x;
				var y = Mathf.Max(0, _heights[i]);

				vertices.Add(new Vector3(x, Mathf.Max(0, y - _lineWidth), 0f));
				vertices.Add(new Vector3(x, y, 0f));
			}

			var triangles = new List<int>();
			var uv = new List<Vector2>();

			for (var i = 0; i < vertices.Count; i++)
			{
				var even = i % 2 == 0;

				if (i < vertices.Count/2 - 2 || (i >= vertices.Count / 2 && i < vertices.Count - 2))
				{
					if (!even)
					{
						triangles.Add(i + 2);
						triangles.Add(i + 1);
						triangles.Add(i);
					}
					else
					{
						triangles.Add(i);
						triangles.Add(i + 1);
						triangles.Add(i + 2);
					}
				}

				if (i < vertices.Count / 2)
				{
					if (even)
					{
						uv.Add(new Vector2(vertices[i].x / _width * _groundUVScale.x + _groundUVOffset.x, 0f + _groundUVOffset.y));
					}
					else
					{
						var y = Utils.Map(vertices[i].y, 0, maxHeight, 0, 1);
						uv.Add(new Vector2(vertices[i].x / _width * _groundUVScale.x + _groundUVOffset.x, y * _groundUVScale.y + _groundUVOffset.y));
					}
				}
				else
				{
					if (even)
					{
						uv.Add(new Vector2(vertices[i].x / _width, 0f));
					}
					else
					{
						uv.Add(new Vector2(vertices[i].x / _width, 1f));
					}
				}
			}


			_mesh.Clear();
			_mesh.subMeshCount = 2;
			_mesh.vertices = vertices.ToArray();

			_mesh.SetTriangles(triangles.GetRange(0, triangles.Count / 2), 0, true);
			_mesh.SetTriangles(triangles.GetRange(triangles.Count / 2, triangles.Count / 2), 1, true);

			_mesh.uv = uv.ToArray();
		}

		private void RebuildCollider(int segmentsCount)
		{
			if (_colliderType == ColliderType.None)
			{
				var colliders = GetComponents<Collider2D>();
				for (var i = 0; i < colliders.Length; i++)
				{
					colliders[i].enabled = false;
				}
			}
			if (_colliderType == ColliderType.Polygon)
			{
				var collider = GetComponent<PolygonCollider2D>();
				if (collider == null) collider = gameObject.AddComponent<PolygonCollider2D>();

				collider.enabled = true;

				var points = new Vector2[segmentsCount + 5];
				points[0] = Vector2.zero;

				for (int i = 0; i < segmentsCount; i++)
				{
					var time = (1f / (_quality * _width)) * i;
					var position = GetPosition(time);

					var x = _width * position.x;
					var y = Mathf.Max(0, _heights[i]);

					points[i + 1] = new Vector2(x, y);
				}

				points[points.Length - 4] = new Vector2(_width, 0);
				points[points.Length - 3] = new Vector2(_width * 0.75f, 0);
				points[points.Length - 2] = new Vector2(_width * 0.50f, 0);
				points[points.Length - 1] = new Vector2(_width * 0.25f, 0);

				collider.SetPath(0, points);
			}
		}

		#endregion
	}
}