using System.Text.RegularExpressions;

namespace Mcg.Webservice.Cncf.Api
{
	/// <summary>
	/// Various type extensions and helpers for strings.
	/// </summary>
	public static class TypeExtensions
	{
		private static readonly Regex InvalidCharsRegex = new Regex(@"(\W\d\W|\W|\d)", RegexOptions.Compiled);
		private static readonly Regex WhitespaceRegex = new Regex(@"_{2,}", RegexOptions.Compiled);

		/// <summary>
		/// Converts a string into an 32bit integer.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static int ToInt(this string value)
		{
			return int.Parse(value);
		}

		/// <summary>
		/// Converts a string into a boolean value.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool ToBool(this string value)
		{
			var result = bool.Parse(value);
			return result;
		}

		/// <summary>
		/// removes all special and unprintable chars from a string, replacing them with an '_' (underscore).
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		internal static string SafeString(this string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return string.Empty;
			}

			var sanitized = InvalidCharsRegex.Replace(value, "_");
			return WhitespaceRegex.Replace(sanitized, "_").ToLower();
		}

		/// <summary>
		/// Converts the string into the specified enumeration type.
		/// </summary>
		/// <typeparam name="TEnum">The type of enumeration expected to be returned.</typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		internal static (bool success, TEnum newValue) ToEnum<TEnum>(this string value) where TEnum : struct
		{
			var isOk = System.Enum.TryParse<TEnum>(value, true, out TEnum enumValue);
			return (success: isOk, newValue: enumValue);
		}

		internal static byte[] ToBytes(this string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return null;
			}

			return System.Text.Encoding.UTF8.GetBytes(value);
		}
	}
}
