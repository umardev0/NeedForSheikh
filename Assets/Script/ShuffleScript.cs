using System;
using System.Collections.Generic;

public static class RandomExtensions
{
	static Random _random = new Random();
	
	/// <summary>
	/// Shuffle the array.
	/// </summary>
	/// <typeparam name="T">Array element type.</typeparam>
	/// <param name="array">Array to shuffle.</param>
	public static void Shuffle<T>(List<T> array)
	{
		int n = array.Count;
		for (int i = 0; i < n; i++)
		{
			// NextDouble returns a random number between 0 and 1.
			// ... It is equivalent to Math.random() in Java.
			int r = i + (int)(_random.NextDouble() * (n - i));
			T t = array[r];
			array[r] = array[i];
			array[i] = t;
		}
	}
	
}
