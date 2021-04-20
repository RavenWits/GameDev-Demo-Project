// TransformExtensions
using UnityEngine;

/// <summary>
/// This is a extension method to enhance existing classes. Makes transform methods easy to use.
/// </summary>

public static class TransformExtensions
{
	public static void SetPosX(this Transform transform, float x)
	{
		Vector3 newPosition = (transform.localPosition = new Vector2(x, transform.localPosition.y));
	}

	public static void SetPosY(this Transform transform, float y)
	{
		Vector3 newPosition = (transform.localPosition = new Vector2(transform.localPosition.x, y));
	}

	public static void SetScaleX(this Transform transform, float x)
	{
		Vector3 newScale = (transform.localScale = new Vector2(x, transform.localScale.y));
	}

	public static void SetScaleY(this Transform transform, float y)
	{
		Vector3 newScale = (transform.localScale = new Vector2(transform.localScale.x, y));
	}
}
