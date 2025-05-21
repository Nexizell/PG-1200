using System;
using UnityEngine;

public class GaussianBlur
{
	public Texture2D BlurTexture(Texture2D texture, int iterations)
	{
		FColor[,] array = new FColor[texture.width, texture.height];
		for (int i = 0; i < texture.width; i++)
		{
			for (int j = 0; j < texture.height; j++)
			{
				array[i, j] = new FColor(texture.GetPixel(i, j));
			}
		}
		while (iterations > 0)
		{
			for (int k = 0; k < array.GetLength(0); k++)
			{
				for (int l = 0; l < array.GetLength(1); l++)
				{
					Compress(array, k, l);
				}
			}
			iterations--;
		}
		Texture2D texture2D = new Texture2D(texture.width, texture.height);
		for (int m = 0; m < array.GetLength(0); m++)
		{
			for (int n = 0; n < array.GetLength(1); n++)
			{
				texture2D.SetPixel(m, n, array[m, n].GetColor());
			}
		}
		texture2D.Apply();
		return texture2D;
	}

	private void Compress(FColor[,] matrix, int i, int j)
	{
		FColor fColor = matrix[i, j];
		int num = 1;
		float num2 = fColor.r;
		float num3 = fColor.g;
		float num4 = fColor.b;
		float num5 = fColor.a;
		if (i > 0)
		{
			FColor fColor2 = matrix[i - 1, j];
			num2 += fColor2.r;
			num3 += fColor2.g;
			num4 += fColor2.b;
			num5 += fColor2.a;
			num++;
		}
		if (i + 1 < matrix.GetLength(0))
		{
			FColor fColor3 = matrix[i + 1, j];
			num2 += fColor3.r;
			num3 += fColor3.g;
			num4 += fColor3.b;
			num5 += fColor3.a;
			num++;
		}
		if (j > 0)
		{
			FColor fColor4 = matrix[i, j - 1];
			num2 += fColor4.r;
			num3 += fColor4.g;
			num4 += fColor4.b;
			num5 += fColor4.a;
			num++;
		}
		if (j + 1 < matrix.GetLength(1))
		{
			FColor fColor5 = matrix[i, j + 1];
			num2 += fColor5.r;
			num3 += fColor5.g;
			num4 += fColor5.b;
			num5 += fColor5.a;
			num++;
		}
		num2 /= (float)num;
		num3 /= (float)num;
		num4 /= (float)num;
		num5 /= (float)num;
		matrix[i, j] = new FColor(num2, num3, num4, Math.Min(1f, num5));
	}

	public FColor[,] CalculateBlurMatrix(Texture2D texture, int iterations)
	{
		FColor[,] array = new FColor[texture.width, texture.height];
		for (int i = 0; i < texture.width; i++)
		{
			for (int j = 0; j < texture.height; j++)
			{
				array[i, j] = new FColor(texture.GetPixel(i, j));
			}
		}
		while (iterations > 0)
		{
			for (int k = 0; k < array.GetLength(0); k++)
			{
				for (int l = 0; l < array.GetLength(1); l++)
				{
					Compress(array, k, l);
				}
			}
			iterations--;
		}
		return array;
	}

	public FColor[,] CalculateBlurEffect(FColor[,] matrix, int iterations)
	{
		while (iterations > 0)
		{
			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				for (int j = 0; j < matrix.GetLength(1); j++)
				{
					Compress(matrix, i, j);
				}
			}
			iterations--;
		}
		return matrix;
	}

	public FColor[,] CreateMatrix(Texture2D texture)
	{
		FColor[,] array = new FColor[texture.width, texture.height];
		for (int i = 0; i < texture.width; i++)
		{
			for (int j = 0; j < texture.height; j++)
			{
				array[i, j] = new FColor(texture.GetPixel(i, j));
			}
		}
		return array;
	}

	public Texture2D CreateTextureFromMatrix(FColor[,] matrix)
	{
		Texture2D texture2D = new Texture2D(matrix.GetLength(0), matrix.GetLength(1), TextureFormat.RGB24, false);
		for (int i = 0; i < matrix.GetLength(0); i++)
		{
			for (int j = 0; j < matrix.GetLength(1); j++)
			{
				texture2D.SetPixel(i, j, matrix[i, j].GetColor());
			}
		}
		texture2D.Apply();
		return texture2D;
	}
}
