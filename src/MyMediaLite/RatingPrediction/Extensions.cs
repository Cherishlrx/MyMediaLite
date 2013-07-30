// Copyright (C) 2010, 2011, 2012, 2013 Zeno Gantner
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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using MyMediaLite.Data;
using MyMediaLite.IO;
using MyMediaLite.ItemRecommendation;

namespace MyMediaLite.RatingPrediction
{
	/// <summary>Class that contains static methods for rating prediction</summary>
	public static class Extensions
	{
		/// <summary>Rate a given set of instances and write it to a TextWriter</summary>
		/// <param name="recommender">rating predictor</param>
		/// <param name="ratings">test cases</param>
		/// <param name="writer">the TextWriter to write the predictions to</param>
		/// <param name="user_mapping">an <see cref="Mapping"/> object for the user IDs</param>
		/// <param name="item_mapping">an <see cref="Mapping"/> object for the item IDs</param>
		/// <param name="line_format">a format string specifying the line format; {0} is the user ID, {1} the item ID, {2} the rating</param>
		/// <param name="header">if specified, write this string at the start of the output</param>
		public static void WritePredictions(
			this IRecommender recommender,
			IInteractions interactions,
			TextWriter writer,
			IMapping user_mapping = null,
			IMapping item_mapping = null,
			string line_format = "{0}\t{1}\t{2}",
			string header = null)
		{
			if (user_mapping == null)
				user_mapping = new IdentityMapping();
			if (item_mapping == null)
				item_mapping = new IdentityMapping();

			if (header != null)
				writer.WriteLine(header);

			if (line_format == "ranking") // TODO is this used at all?
			{
				foreach (int user_id in interactions.Users)
				{
					int num_ratings_by_user = interactions.ByUser(user_id).Count;
					if (num_ratings_by_user > 0)
					{
						recommender.WritePredictions(
							user_id,
							interactions.ByUser(user_id).Items,
							new int[] { },
							num_ratings_by_user,
							writer,
							user_mapping, item_mapping);
					}
				}
			}
			else
			{
				var reader = interactions.Sequential;
				while (reader.Read())
					writer.WriteLine(
						line_format,
						user_mapping.ToOriginalID(reader.GetUser()),
						item_mapping.ToOriginalID(reader.GetItem()),
						recommender.Predict(reader.GetUser(), reader.GetItem()).ToString(CultureInfo.InvariantCulture));
			}
		}

		/// <summary>Rate a given set of instances and write it to a file</summary>
		/// <param name="recommender">rating predictor</param>
		/// <param name="interactions">test cases</param>
		/// <param name="filename">the name of the file to write the predictions to</param>
		/// <param name="user_mapping">an <see cref="Mapping"/> object for the user IDs</param>
		/// <param name="item_mapping">an <see cref="Mapping"/> object for the item IDs</param>
		/// <param name="line_format">a format string specifying the line format; {0} is the user ID, {1} the item ID, {2} the rating</param>
		/// <param name="header">if specified, write this string to the first line</param>
		public static void WritePredictions(
			this IRecommender recommender,
			IInteractions interactions,
			string filename,
			IMapping user_mapping = null, IMapping item_mapping = null,
			string line_format = "{0}\t{1}\t{2}",
			string header = null)
		{
			using (var writer = FileSystem.CreateStreamWriter(filename))
				WritePredictions(recommender, interactions, writer, user_mapping, item_mapping, line_format);
		}
	}
}
