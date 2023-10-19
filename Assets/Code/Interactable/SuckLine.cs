using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckLine : PooledItem
{
	public LineRenderer lineRenderer;
	public float speed;

	VacuumCleaner vacuumHead;
	Vector3 initialPos;
	Vector2 initialVacuumDir2D;
	Color initialCol;
	Color targetCol;
	float initialDst;
	float t;

	public void Init(VacuumCleaner vacuumHead)
	{
		this.vacuumHead = vacuumHead;
		initialDst = (vacuumHead.flyingToPosition.position - transform.position).magnitude;
		initialPos = transform.position;
		initialVacuumDir2D = new Vector2(vacuumHead.flyingToPosition.forward.x, vacuumHead.flyingToPosition.forward.z).normalized;
		transform.forward = (vacuumHead.flyingToPosition.position - transform.position).normalized;
		initialCol = lineRenderer.material.color;
		targetCol = new Color(initialCol.r, initialCol.g, initialCol.b, 0);
		UpdateMovement();

	}

	protected override void Reset()
	{
		lineRenderer.material.color = initialCol;
		t = 0;
	}

	void Update()
	{
		UpdateMovement();
	}

	void UpdateMovement()
	{
		t += Time.deltaTime * speed / initialDst;

		transform.position = Vector3.Lerp(initialPos, vacuumHead.flyingToPosition.position, t);
		Vector2 vacuumDir2D = new Vector2(vacuumHead.flyingToPosition.forward.x, vacuumHead.flyingToPosition.forward.z).normalized;
		lineRenderer.material.color = Color.Lerp(initialCol, targetCol, t);


		if (t >= 1 || Vector2.Dot(initialVacuumDir2D, vacuumDir2D) < 0.8f)
		{
			//print("Destroy: " + t);
			DestroyPooled();
			//Destroy(gameObject);
		}

	}
}
