using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using TeasPrep;
using TeasPrep.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

public class AppService
 { 
    private readonly string connectionString = "Server=tcp:jtappserver.database.windows.net,1433;Initial Catalog=MblexDB;Persist Security Info=False;User ID=jthuko;Password=Jnzusyo77!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

    public AppService(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public async Task<ObservableCollection<PublicQuestion>> GetPublicQuestionsAsync(int subjectID)
    {
        List<PublicQuestion> publicQuestions = new List<PublicQuestion>();
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string questionQuery = "SELECT * FROM PublicQuestions Where SubjectID = @SubjectID";

                using (SqlCommand questionCommand = new SqlCommand(questionQuery, connection))
                {
                    questionCommand.Parameters.AddWithValue("@SubjectID", subjectID);
                    using (SqlDataReader questionReader = await questionCommand.ExecuteReaderAsync())
                    {

                        while (await questionReader.ReadAsync())
                        {
                            PublicQuestion question = new PublicQuestion
                            {
                                QuestionID = questionReader.GetInt32(questionReader.GetOrdinal("QuestionID")),
                                Text = questionReader.GetString(questionReader.GetOrdinal("Text")),
                                IsPublic = questionReader.GetBoolean(questionReader.GetOrdinal("IsPublic")),
                                UserID = questionReader.IsDBNull(questionReader.GetOrdinal("UserID")) ? (int?)null : questionReader.GetInt32(questionReader.GetOrdinal("UserID")),
                                SubjectID = questionReader.GetInt32(questionReader.GetOrdinal("SubjectID")),
                                Choices = new List<Choice>()
                            };

                            publicQuestions.Add(question);
                        }
                    }
                }


                // Retrieve and associate choices for each question
                foreach (PublicQuestion question in publicQuestions)
                {
                    string choicesQuery = "SELECT * FROM Choices WHERE QuestionID = @QuestionID";
                    using (SqlCommand choicesCommand = new SqlCommand(choicesQuery, connection))
                    {
                        choicesCommand.Parameters.AddWithValue("@QuestionID", question.QuestionID);

                        using (SqlDataReader choicesReader = await choicesCommand.ExecuteReaderAsync())
                        {
                            while (await choicesReader.ReadAsync())
                            {
                                Choice choice = new Choice
                                {
                                    ChoiceID = choicesReader.GetInt32(choicesReader.GetOrdinal("ChoiceID")),
                                    QuestionID = choicesReader.GetInt32(choicesReader.GetOrdinal("QuestionID")),
                                    ChoiceText = choicesReader.GetString(choicesReader.GetOrdinal("ChoiceText")),
                                    IsCorrect = choicesReader.GetBoolean(choicesReader.GetOrdinal("IsCorrect"))
                                };

                                question.Choices.Add(choice);
                            }
                        }
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            // Handle the SQL exception
            HandleDatabaseConnectionError(ex);
        }
        catch (Exception ex)
        {
            HandleDatabaseConnectionError(ex);
        }

        return new ObservableCollection<PublicQuestion>(publicQuestions);
    }

    public PublicQuestion GetPublicQuestion(int questionID)
    {
        PublicQuestion question = null;

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM PublicQuestions WHERE QuestionID = @QuestionID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@QuestionID", questionID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            question = new PublicQuestion
                            {
                                QuestionID = reader.GetInt32(reader.GetOrdinal("QuestionID")),
                                Text = reader.GetString(reader.GetOrdinal("Text")),
                                IsPublic = reader.GetBoolean(reader.GetOrdinal("IsPublic")),
                                UserID = reader.IsDBNull(reader.GetOrdinal("UserID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("UserID")),
                                SubjectID = reader.GetInt32(reader.GetOrdinal("SubjectID")),
                                Choices = new List<Choice>()
                            };
                        }
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            // Handle the SQL exception
            HandleDatabaseConnectionError(ex);
        }
        catch (Exception ex)
        {
            // Handle other exceptions
            HandleDatabaseConnectionError(ex);
        }

        if (question != null)
        {
            // Retrieve and associate choices for the question
            question.Choices = GetChoicesForQuestion(questionID);
        }

        return question;
    }

    public List<Choice> GetChoicesForQuestion(int questionID)
    {
        List<Choice> choices = new List<Choice>();

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Choices WHERE QuestionID = @QuestionID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@QuestionID", questionID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Choice choice = new Choice
                            {
                                ChoiceID = reader.GetInt32(reader.GetOrdinal("ChoiceID")),
                                QuestionID = reader.GetInt32(reader.GetOrdinal("QuestionID")),
                                ChoiceText = reader.GetString(reader.GetOrdinal("ChoiceText")),
                                IsCorrect = reader.GetBoolean(reader.GetOrdinal("IsCorrect"))
                            };

                            choices.Add(choice);
                        }
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            // Handle the SQL exception
            HandleDatabaseConnectionError(ex);
        }
        catch (Exception ex)
        {
            // Handle other exceptions
            HandleDatabaseConnectionError(ex);
        }
        return choices;
    }

    public void AddPublicQuestion(PublicQuestion question)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Step 1: Insert the question into the PublicQuestions table
                string insertQuestionQuery = "INSERT INTO PublicQuestions (Text, IsPublic, UserID, SubjectID) " +
                    "VALUES (@Text, @IsPublic, @UserID, @SubjectID); SELECT SCOPE_IDENTITY();";

                using (SqlCommand insertQuestionCommand = new SqlCommand(insertQuestionQuery, connection))
                {
                    insertQuestionCommand.Parameters.AddWithValue("@Text", question.Text);
                    insertQuestionCommand.Parameters.AddWithValue("@IsPublic", question.IsPublic);
                    insertQuestionCommand.Parameters.AddWithValue("@UserID", question.UserID ?? (object)DBNull.Value);
                    insertQuestionCommand.Parameters.AddWithValue("@SubjectID", question.SubjectID);

                    // Get the new question's ID
                    int newQuestionID = Convert.ToInt32(insertQuestionCommand.ExecuteScalar());

                    // Step 2: Insert choices into the Choices table
                    string insertChoicesQuery = "INSERT INTO Choices (QuestionID, ChoiceText, IsCorrect) VALUES (@QuestionID, @ChoiceText, @IsCorrect)";

                    foreach (Choice choice in question.Choices)
                    {
                        using (SqlCommand insertChoicesCommand = new SqlCommand(insertChoicesQuery, connection))
                        {
                            insertChoicesCommand.Parameters.AddWithValue("@QuestionID", newQuestionID);
                            insertChoicesCommand.Parameters.AddWithValue("@ChoiceText", choice.ChoiceText);
                            insertChoicesCommand.Parameters.AddWithValue("@IsCorrect", choice.IsCorrect);

                            insertChoicesCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            // Handle the SQL exception
            HandleDatabaseConnectionError(ex);
        }
        catch (Exception ex)
        {
            // Handle other exceptions
            HandleDatabaseConnectionError(ex);
        }
    }



    public void UpdatePublicQuestion(PublicQuestion question)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE PublicQuestions SET Text = @Text, IsPublic = @IsPublic, UserID = @UserID, SubjectID = @SubjectID WHERE QuestionID = @QuestionID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@QuestionID", question.QuestionID);
                    command.Parameters.AddWithValue("@Text", question.Text);
                    command.Parameters.AddWithValue("@IsPublic", question.IsPublic);
                    command.Parameters.AddWithValue("@UserID", question.UserID ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@SubjectID", question.SubjectID);

                    command.ExecuteNonQuery();
                }
            }
        }
        catch (SqlException ex)
        {
            // Handle the SQL exception
            HandleDatabaseConnectionError(ex);
        }
        catch (Exception ex)
        {
            // Handle other exceptions
            HandleDatabaseConnectionError(ex);
        }
    }



    public void PatchQuestion(int questionId, Dictionary<string, object> changes)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string updateQuery = "UPDATE Questions SET ";

                foreach (var change in changes)
                {
                    if (change.Key.Equals("Text", StringComparison.OrdinalIgnoreCase))
                    {
                        updateQuery += "Text = @Text, ";
                    }
                    else if (change.Key.Equals("IsPublic", StringComparison.OrdinalIgnoreCase))
                    {
                        updateQuery += "IsPublic = @IsPublic, ";
                    }
                    // Add more properties to update as needed
                }

                // Remove the trailing comma and space
                updateQuery = updateQuery.Substring(0, updateQuery.Length - 2);

                updateQuery += " WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    foreach (var change in changes)
                    {
                        command.Parameters.AddWithValue("@" + change.Key, change.Value);
                    }
                    command.Parameters.AddWithValue("@Id", questionId);

                    command.ExecuteNonQuery();
                }
            }
        }
        catch (SqlException ex)
        {
            // Handle the SQL exception
            HandleDatabaseConnectionError(ex);
        }
        catch (Exception ex)
        {
            // Handle other exceptions
            HandleDatabaseConnectionError(ex);
        }
    }


    public void DeleteQuestion(int questionId)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "DELETE FROM Questions WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", questionId);

                    command.ExecuteNonQuery();
                }
            }
        }
        catch (SqlException ex)
        {
            // Handle the SQL exception
            HandleDatabaseConnectionError(ex);
        }
        catch (Exception ex)
        {
            // Handle other exceptions
            HandleDatabaseConnectionError(ex);
        }
    }

    private void HandleDatabaseConnectionError(Exception ex)
    {
        // You can log the exception for debugging purposes
        Console.WriteLine($"Database connection error: {ex.Message}");

        // Show an alert to the user
        // Note: You need to inject the necessary services to show an alert in your specific environment
        // The following code is a generic example and might not work directly in your project
        Application.Current.MainPage.DisplayAlert("Error", "Unable to connect to the database.", "OK");
    }

}