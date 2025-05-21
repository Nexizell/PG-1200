using UnityEngine;

public struct FColor
{
	public float r;

	public float g;

	public float b;

	public float a;

	public FColor(float r, float g, float b, float a)
	{
		this.r = r;
		this.g = g;
		this.b = b;
		this.a = a;
	}

	public FColor(Color color)
	{
		r = color.r;
		g = color.g;
		b = color.b;
		a = color.a;
	}

	public Color GetColor()
	{
		return new Color(r, g, b, a);
	}
}
