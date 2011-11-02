// Copyright (C) 2010, 2011 Zeno Gantner
//
// This file is part of MyMediaLite.
//
// MyMediaLite is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// MyMediaLite is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with MyMediaLite.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using MathNet.Numerics.Distributions;

namespace MyMediaLite.DataType
{
	/// <summary>Extensions for vector-like data</summary>
	public static class VectorExtensions
	{
		/// <summary>Compute the Euclidean norm of a collection of doubles</summary>
		/// <param name="vector">the vector to compute the norm for</param>
		/// <returns>the Euclidean norm of the vector</returns>
		static public double EuclideanNorm(this ICollection<double> vector)
		{
			double sum = 0;
			foreach (double v in vector)
				sum += Math.Pow(v, 2);
			return Math.Sqrt(sum);
		}

		/// <summary>Compute the L1 norm of a collection of doubles</summary>
		/// <param name="vector">the vector to compute the norm for</param>
		/// <returns>the L1 norm of the vector</returns>
		static public double L1Norm(this ICollection<double> vector)
		{
			double sum = 0;
			foreach (double v in vector)
				sum += Math.Abs(v);
			return sum;
		}

		/// <summary>Initialize a collection of doubles with values from a normal distribution</summary>
		/// <param name="vector">the vector to initialize</param>
		/// <param name="mean">the mean of the normal distribution</param>
		/// <param name="stddev">the standard deviation of the normal distribution</param>
		static public void InitNormal(this IList<double> vector, double mean, double stddev)
		{
			var nd = new Normal(mean, stddev);
			nd.RandomSource = Util.Random.GetInstance();
			
			for (int i = 0; i < vector.Count; i++)
				vector[i] = nd.Sample();
		}
	}
}