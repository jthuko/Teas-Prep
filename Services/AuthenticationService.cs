using MblexApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MblexApp.Services
{
    public static class AuthenticationService
    {
        private const string SettingsKey = "UserSettings";

        public static UserSettings GetUserSettings()
        {
            try
            {
                var settingsJson = SecureStorage.GetAsync(SettingsKey).Result;

                if (!string.IsNullOrEmpty(settingsJson))
                {
                    return JsonSerializer.Deserialize<UserSettings>(settingsJson);
                }
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., key not found)
                Console.WriteLine($"Error retrieving user settings: {ex.Message}");
            }

            return null;
        }

        public static void SaveUserSettings(UserSettings settings)
        {
            try
            {
                var settingsJson = JsonSerializer.Serialize(settings);
                SecureStorage.SetAsync(SettingsKey, settingsJson);
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine($"Error saving user settings: {ex.Message}");
            }
        }
    }
}
