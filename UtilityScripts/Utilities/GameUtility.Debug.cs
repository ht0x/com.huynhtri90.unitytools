using UnityEngine;

public static partial class CGameUtility
{
	#region ===== Fields =====

	#endregion

	#region ===== Methods =====

	public static void DEBUG_DrawLine(Vector3 vFrom, Vector3 vTo, float distance, Color color)
	{
		#if UNITY_EDITOR
		vFrom.y = 5f;
		vTo.y   = 5f;
		Debug.DrawLine(vFrom, vFrom + (vTo - vFrom).normalized * distance, color, 5);
		#endif
	}

	public static void DEBUG_DrawCircle(Vector3 actPos, float range)
	{
		#if UNITY_EDITOR
		Debug.DrawLine(actPos - Vector3.right * range, actPos + Vector3.right * range, Color.yellow, 5);
		Debug.DrawLine(actPos - Vector3.forward * range, actPos + Vector3.forward * range, Color.yellow, 5);

		Vector3 cross1 = (Vector3.right + Vector3.forward);
		cross1.Normalize();
		Debug.DrawLine(actPos - cross1 * range, actPos + cross1 * range, Color.yellow, 5);

		Vector3 cross2 = (Vector3.right - Vector3.forward);
		cross2.Normalize();
		Debug.DrawLine(actPos - cross2 * range, actPos + cross2 * range, Color.yellow, 5);
		#endif
	}

	public static void DEBUG_DrawCircle(Vector3 actPos, float range, Color color, float duration = 5)
	{
		#if UNITY_EDITOR
		Debug.DrawLine(actPos - Vector3.right * range, actPos + Vector3.right * range, color, duration);
		Debug.DrawLine(actPos - Vector3.forward * range, actPos + Vector3.forward * range, color, duration);

		Vector3 cross1 = (Vector3.right + Vector3.forward);
		cross1.Normalize();
		Debug.DrawLine(actPos - cross1 * range, actPos + cross1 * range, color, duration);

		Vector3 cross2 = (Vector3.right - Vector3.forward);
		cross2.Normalize();
		Debug.DrawLine(actPos - cross2 * range, actPos + cross2 * range, color, duration);
		#endif
	}

	public static void DEBUG_DrawArc(Transform obj, float range, float arc)
	{
		#if UNITY_EDITOR
		float halfAngle = arc / 2.0f;

		Vector3 rightPos = Quaternion.AngleAxis(halfAngle, Vector3.up) * obj.forward;
		rightPos.Normalize();
		Debug.DrawLine(obj.transform.position, obj.transform.position + rightPos * range, Color.yellow, 5);


		Vector3 leftPos = Quaternion.AngleAxis(-halfAngle, Vector3.up) * (obj.forward);
		leftPos.Normalize();
		Debug.DrawLine(obj.transform.position, obj.transform.position + leftPos * range, Color.yellow, 5);


		Debug.DrawLine(obj.transform.position, obj.transform.position + obj.forward * range, Color.yellow, 5);
		#endif
	}

	#endregion
}