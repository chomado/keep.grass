// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;

namespace keep.grass.Helpers
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
	{
		private static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}

		#region Setting Constants

		private const string SettingsKey = "settings_key";
		private static readonly string SettingsDefault = string.Empty;

		#endregion


		public static string GeneralSettings
		{
			get
			{
				return AppSettings.GetValueOrDefault<string>(SettingsKey, SettingsDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue<string>(SettingsKey, value);
			}
		}

		private const string LanguageKey = "Language";
		private static readonly string LanguageDefault = string.Empty;
		public static string Language
		{
			get
			{
				return AppSettings.GetValueOrDefault<string>(LanguageKey, LanguageDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue<string>(LanguageKey, value);
			}
		}

		private const string UserNameKey = "UserName";
		private static readonly string UserNameDefault = string.Empty;
		public static string UserName
		{
			get
			{
				return AppSettings.GetValueOrDefault<string>(UserNameKey, UserNameDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue<string>(UserNameKey, value);
			}
		}

		public static T Get<T>(string Key, T DefaultValue)
		{
			return AppSettings.GetValueOrDefault<T>(Key, DefaultValue);
		}
		public static void Set<T>(string Key, T NewValue)
		{
			AppSettings.AddOrUpdateValue<T>(Key, NewValue);
		}

		public static TimeSpan[] AlertTimeSpanTable = new []
		{
			TimeSpan.FromTicks(0),
			TimeSpan.FromMinutes(5),
			TimeSpan.FromMinutes(10),
			TimeSpan.FromMinutes(15),
			TimeSpan.FromMinutes(30),
			TimeSpan.FromMinutes(45),
			TimeSpan.FromHours(1),
			TimeSpan.FromHours(2),
			TimeSpan.FromHours(3),
			TimeSpan.FromHours(6),
			TimeSpan.FromHours(9),
			TimeSpan.FromHours(12),
			TimeSpan.FromHours(18),
		};
		public static string AlertTimeSpanToDisplayName(TimeSpan left)
		{
			if (TimeSpan.FromHours(1) < left)
			{
				return String.Format("{0} hours left", left.TotalHours);
			}
			else
			if (TimeSpan.FromHours(1) == left)
			{
				return "1 hour left";
			}
			else
			if (TimeSpan.FromMinutes(1) < left)
			{
				return String.Format("{0} minutes left", left.TotalMinutes);
			}
			else
			if (TimeSpan.FromMinutes(1) == left)
			{
				return "1 minute left";
			}
			else
			{
				return "Just 24 hours later";
			}
		}
		public static string AlertTimeSpanToSettingKey(TimeSpan left)
		{
			return String.Format("alert{0}{1}", left.Hours, left.Minutes);
		}
		public static bool GetAlert(TimeSpan key)
		{
			return Get(AlertTimeSpanToSettingKey(key), false);
		}
		public static void SetAlert(TimeSpan Key, bool NewValue)
		{
			Set(AlertTimeSpanToSettingKey(Key), NewValue);
		}

	}
}