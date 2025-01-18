using System.Numerics;

namespace Velura.iOS.Models;

public class Cluster(
	Vector3 center)
{
	static Cluster FindClosest(Vector3 p, List<Cluster> clusters) =>
		clusters.OrderBy(cluster => Vector3.DistanceSquared(cluster.Center, p)).First();
	
	public static List<Cluster> Create(
		List<Vector3> points,
		int k)
	{
		List<Cluster> clusters = [];
		for (int i = 0; i < k; i++)
		{
			Vector3 p;
			do
				p = points[Random.Shared.Next(points.Count)];
			while (clusters.Any(c => c.Center == p));
			
			clusters.Add(new(p));
		}
		
		foreach (Vector3 p in points)
		{
			Cluster closest = FindClosest(p, clusters);
			closest.Points.Add(p);
		}
		
		foreach (Cluster c in clusters)
			c.UpdateCenter();

		for (int i = 0; i < 10; i++)
		{
			foreach (Cluster c in clusters)
				c.Points.Clear();

			foreach (Vector3 p in points)
			{
				Cluster closest = FindClosest(p, clusters);
				closest.Points.Add(p);
			}

			bool converged = true;
			foreach (Cluster c in clusters)
			{
				Vector3 oldCenter = c.Center;
				c.UpdateCenter();
				if (Vector3.DistanceSquared(oldCenter, c.Center) > 0.001f)
					converged = false;
			}

			if (converged)
				break;
		}
		
		return clusters;
	}
	
	
	public Vector3 Center { get; set; } = center;

	public List<Vector3> Points { get; set; } = [];
	
	
	public Vector3 CalculateCurrentCenter()
	{
		if (Points.Count == 0)
			return Vector3.Zero;

		return Points.Aggregate(Vector3.Zero, (acc, point) => acc + point) / Points.Count;
	}
	
	public void UpdateCenter()
	{
		if (Points.Count == 0)
			return;

		Vector3 currentCenter = CalculateCurrentCenter();
		Center = Points.OrderBy(point => Vector3.DistanceSquared(point, currentCenter)).First();
	}

}