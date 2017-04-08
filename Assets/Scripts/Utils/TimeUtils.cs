////////////////////////////////////////////////////
//// File Name :        TimeUtils.cs
//// Tables :              nothing
//// Autor :               rebolomo
//// Create Date :      2015.8.24
//// Content :           
////////////////////////////////////////////////////
namespace UnityMVC.Utils
{
	using UnityEngine;
	using System.Collections;
	using System;

	public class TimeUtils
	{
		public static long BinaryStamp ()
		{
			return DateTime.UtcNow.ToBinary ();
		}

		/// <summary>
		/// Times from unix timestamp.
		/// </summary>
		/// <returns>The from unix timestamp.</returns>
		/// <param name="unixTimestamp">Unix timestamp.</param>
		/// <param name="kind">Kind.</param>
		public static DateTime ConvertToDate (long unixTimeStamp)
		{
			DateTime unixYear0 = new DateTime (1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
			long unixTimeStampInTicks = unixTimeStamp * TimeSpan.TicksPerSecond;
			DateTime now = new DateTime (unixYear0.Ticks + unixTimeStampInTicks);
			TimeZone localZone = TimeZone.CurrentTimeZone;
			TimeSpan currentOffset = localZone.GetUtcOffset (now);
			DateTime targetTime = now.Add (currentOffset);
			return targetTime;
        }
        
        /// <summary>
		/// Times from unix timestamp in ticks.
		/// </summary>
		/// <returns>The from unix timestamp in ticks.</returns>
		/// <param name="unixTimeStampInTicks">Unix time stamp in ticks.</param>
		/// <param name="kind">Kind.</param>
		public static DateTime ConvertToDataInTicks (long unixTimeStampInTicks)
		{
			DateTime unixYear0 = new DateTime (1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
			DateTime now = new DateTime (unixYear0.Ticks + unixTimeStampInTicks);
			TimeZone localZone = TimeZone.CurrentTimeZone;
			TimeSpan currentOffset = localZone.GetUtcOffset (now);
			DateTime targetTime = now.Add (currentOffset);
			return targetTime;
		}
		
        /// <summary>
		/// Unixs the timestamp from date time.
		/// </summary>
		/// <returns>The timestamp from date time.</returns>
		/// <param name="date">Date.</param>
		public static long ConvertToTime (DateTime date)
		{
			long unixTimestamp = date.Ticks - new DateTime (1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).Ticks;
			unixTimestamp /= TimeSpan.TicksPerSecond;
			return unixTimestamp;
		}

	}
}