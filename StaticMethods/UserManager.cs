using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MblexApp.Services;
using MblexApp.Models;

namespace MblexApp
{
    public static class UserManager
    {
        private static string connectionString = "Server=tcp:jtappserver.database.windows.net,1433;Initial Catalog=MblexDB;Persist Security Info=False;User ID=jthuko;Password=Jnzusyo77!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public static bool LoginUser(string usernameOrEmail, string password)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                bool loginSuccess = AttemptLogin(connection, usernameOrEmail, password);
                return loginSuccess;
            }
        }

        private static bool AttemptLogin(SqlConnection connection, string usernameOrEmail, string password)
        {
            using (var command = new SqlCommand("sp_GetPasswordHash", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UsernameOrEmail", usernameOrEmail);
                command.Parameters.Add(new SqlParameter("@PasswordHash", SqlDbType.NVarChar, 128) { Direction = ParameterDirection.Output });

                command.ExecuteNonQuery();

                // Retrieve the password hash from the output parameter
                string storedPasswordHash = command.Parameters["@PasswordHash"].Value as string;

                if (!string.IsNullOrEmpty(storedPasswordHash) && VerifyPassword(password, storedPasswordHash))
                {
                    // Check if user settings already exist
                    var existingUserSettings = AuthenticationService.GetUserSettings();
                    var userDetails = GetUserDetails(usernameOrEmail);
                    if (existingUserSettings == null)
                    {
                        // User settings do not exist, save them
                        var userSettings = new UserSettings
                        {
                            Username = usernameOrEmail,
                            Password = storedPasswordHash,
                            LastLoginTime = DateTime.Now,
                            IsPremium = userDetails.IsPremium,
                            IsLoggedIn = true
                            
                        };

                        AuthenticationService.SaveUserSettings(userSettings);
                    }
                    else
                    {
                        // User settings already exist, update them
                        existingUserSettings.LastLoginTime = DateTime.Now;
                        existingUserSettings.IsLoggedIn = true;
                        existingUserSettings.IsPremium = userDetails.IsPremium;
                        AuthenticationService.SaveUserSettings(existingUserSettings);
                    }

                    return true;
                }
            }

            return false; // Login failed
        }

        // Rest of the code remains the same
        private static bool VerifyPassword(string password, string storedPasswordHash)
        {
            using (var sha256 = new SHA256Managed())
            {
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                string hashedPassword = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                return storedPasswordHash == hashedPassword;
            }
        }

        public static bool CreateUser(string username, string email, string password)
        {
            // Hash the user's password
            string passwordHash = HashPassword(password);

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Check if the username or email is already in use (optional)
                if (IsUsernameOrEmailInUse(connection, username, email))
                {
                    return false; // Username or email is already in use
                }

                // Call the stored procedure to insert the user into the database
                using (var command = new SqlCommand("sp_InsertUser", connection))
                {
                    var isPremium = 0;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    command.Parameters.AddWithValue("@IsPremium", isPremium);

                    command.ExecuteNonQuery();
                }

                return true; // User was successfully created
            }
        }

        public static bool UpdatePremiumForUser(string username, bool isPremium)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Check if the username exists (using IsUsernameOrEmailInUse method)
                if (!IsUsernameOrEmailInUse(connection, username, ""))
                {
                    return false; // Username does not exist
                }

                // Update IsPremium using parameterized SQL UPDATE statement
                string updateQuery = "UPDATE Users SET IsPremium = @IsPremium WHERE Username = @Username";

                using (var command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@IsPremium", isPremium ? 1 : 0);

                    command.ExecuteNonQuery();
                }

                return true; // IsPremium was successfully updated
            }
        }

        public static bool UpdateUsernameOrEmail(string currentUsername, string newUsername, string newEmail)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Check if the new username or email is already in use
                if (IsUsernameOrEmailInUse(connection, newUsername, newEmail))
                {
                    return false; // New username or email is already in use
                }

                // Update username and/or email using parameterized SQL UPDATE statement
                string updateQuery = "UPDATE Users SET ";

                if (!string.IsNullOrEmpty(newUsername))
                {
                    updateQuery += "Username = @NewUsername";
                }

                if (!string.IsNullOrEmpty(newEmail))
                {
                    updateQuery += string.IsNullOrEmpty(newUsername) ? "" : ", ";
                    updateQuery += "Email = @NewEmail";
                }

                updateQuery += " WHERE Username = @CurrentUsername";

                using (var command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@CurrentUsername", currentUsername);

                    if (!string.IsNullOrEmpty(newUsername))
                    {
                        command.Parameters.AddWithValue("@NewUsername", newUsername);
                    }

                    if (!string.IsNullOrEmpty(newEmail))
                    {
                        command.Parameters.AddWithValue("@NewEmail", newEmail);
                    }

                    command.ExecuteNonQuery();
                }

                return true; // Username and/or email were successfully updated
            }
        }

        public static bool UpdateUserPassword(string username, string newPassword)
        {
            // Hash the new password
            string newPasswordHash = HashPassword(newPassword);

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Check if the username exists (using IsUsernameOrEmailInUse method)
                if (!IsUsernameOrEmailInUse(connection, username, ""))
                {
                    return false; // Username does not exist
                }

                // Update user password using parameterized SQL UPDATE statement
                string updateQuery = "UPDATE Users SET PasswordHash = @NewPasswordHash WHERE Username = @Username";

                using (var command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@NewPasswordHash", newPasswordHash);

                    command.ExecuteNonQuery();
                }

                return true; // User password was successfully updated
            }
        }

        private static bool IsUsernameOrEmailInUse(SqlConnection connection, string username, string email)
        {
            // Define a SELECT query to check if the username or email exists in the Users table.
            string selectQuery = "SELECT 1 FROM Users WHERE Username = @Username OR Email = @Email";

            using (var command = new SqlCommand(selectQuery, connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Email", email);

                using (var reader = command.ExecuteReader())
                {
                    // If the reader has any rows, that means the username or email is already in use.
                    return reader.HasRows;
                }
            }
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = new SHA256Managed())
            {
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
        public static UserDetails GetUserDetails(string usernameOrEmail)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Define a SELECT query to get user details from the Users table
                string selectQuery = "SELECT Username, Email, IsPremium FROM Users WHERE Username = @UsernameOrEmail OR Email = @UsernameOrEmail";

                using (var command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@UsernameOrEmail", usernameOrEmail);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Create a UserDetails object and populate it with data from the database
                            var userDetails = new UserDetails
                            {                               
                                Username = reader["Username"].ToString(),
                                Email = reader["Email"].ToString(),
                                IsPremium = Convert.ToBoolean(reader["IsPremium"]),
                                // Add other properties as needed
                            };

                            return userDetails;
                        }
                    }
                }
            }

            return null; // User not found
        }

    }

}
