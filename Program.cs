using System;
using System.Collections.Generic;
using System.IO; 
using Npgsql;

class Program
{
    
    private static string connectionString = "Host=localhost;Port=5432;Database=db1;Username=postgres;Password=a";
    private static string logFilePath = "log.txt"; 

    static void Main(string[] args)
    {
        ClearLog();
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    Log("Начало транзакции.");

                    
                    var savepointName = "my_savepoint";
                    var savepointCommand = new NpgsqlCommand($"SAVEPOINT {savepointName}", connection);
                    LogQuery(savepointCommand);
                    savepointCommand.Transaction = transaction;
                    savepointCommand.ExecuteNonQuery();
                    Log("Успешно");

                    
                    ReadData(connection);

                   
                    InsertData(connection);

                    
                    UpdateData(connection);

                 
                    DeleteData(connection);

                 
                    transaction.Rollback();
                    Log("Транзакция успешно завершена и откачена.");
                }
                catch (Exception ex)
                {
                    Log($"Ошибка: {ex.Message}");

                
                    var rollbackCommand = new NpgsqlCommand($"ROLLBACK TO SAVEPOINT my_savepoint", connection);
                    LogQuery(rollbackCommand);
                    rollbackCommand.Transaction = transaction;
                    rollbackCommand.ExecuteNonQuery();
                    Log("Успешно");

                    Log("Откат к точке сохранения выполнен.");

                
                    
                }
            }

        
            ReadLog();



        }
    }

    private static void Log(string message)
    {
        using (StreamWriter writer = new StreamWriter(logFilePath, true))
        {
            writer.WriteLine($"{DateTime.Now}: {message}");
        }
    }

    private static void LogQuery(NpgsqlCommand command)
    {

        Log($"SQL Запрос: {command.CommandText}");
        foreach (NpgsqlParameter param in command.Parameters)
        {
            Log($"Параметр: {param.ParameterName} = {param.Value}");
        }
    }

    private static void ReadLog()
    {
        Console.WriteLine("\nСодержимое лога:\n");
        if (File.Exists(logFilePath))
        {
            using (StreamReader reader = new StreamReader(logFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }
        else
        {
            Console.WriteLine("Файл лога не найден.");
        }
    }

    private static void ClearLog()
    {
        if (File.Exists(logFilePath))
        {
            File.WriteAllText(logFilePath, string.Empty); 
            Log("Лог очищен.");
        }
        else
        {
            Console.WriteLine("Файл лога не найден для очистки.");
        }
    }

    private static void ReadData(NpgsqlConnection connection)
    {
        List<string> tables = new List<string>() { "Students", "Courses", "Enrollments" };
        foreach (string table in tables)
        {
            Console.WriteLine($"\ntable: {table}\n");
            var command = new NpgsqlCommand($"SELECT * FROM {table}", connection);
            LogQuery(command);
            Log("Успешно");
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.WriteLine($"{reader.GetName(i)}: {reader.GetValue(i)};");
                    }
                    Console.WriteLine();
                }
            }
        }
    }

    private static void InsertData(NpgsqlConnection connection)
    {
        var insertStudentCommand = new NpgsqlCommand("INSERT INTO Students (StudentID, FirstName, LastName, Email, DateOfBirth, EnrollmentDate, PhoneNumber, IsActive, GPA, GrantSum) VALUES (6, 'Анна', 'Смирнова', 'anna.smirnova@example.com', '2001-02-15', '2023-09-01', '1234567899', TRUE, 3.85, 5500.00)", connection);
        LogQuery(insertStudentCommand);
        insertStudentCommand.ExecuteNonQuery();
        Log("Успешно");

        var insertCourseCommand = new NpgsqlCommand("INSERT INTO Courses (CourseID, CourseName, Credits, StartDate, EndDate, Description) VALUES (6, 'Искусственный интеллект', 5, '2025-02-01', '2025-06-30', 'Основы искусственного интеллекта.')", connection);
        LogQuery(insertCourseCommand);
        insertCourseCommand.ExecuteNonQuery();
        Log("Успешно");

        var insertEnrollmentCommand = new NpgsqlCommand("INSERT INTO Enrollments (EnrollmentID, StudentID, CourseID, Grade, EnrollmentDateTime) VALUES (10, 6, 6, NULL, CURRENT_TIMESTAMP)", connection);
        LogQuery(insertEnrollmentCommand);
        insertEnrollmentCommand.ExecuteNonQuery();
        Log("Успешно");
    }

    private static void UpdateData(NpgsqlConnection connection)
    {
      
        var updateStudentsCommand = new NpgsqlCommand(
            "UPDATE Students SET " +
            "StudentId = 10, " +
            "FirstName = 'Обновлено', " +
            "LastName = 'Обновлено', " +
            "Email = 'updated@example.com', " +
            "DateOfBirth = '2000-01-01', " +
            "EnrollmentDate = '2025-01-01', " +
            "PhoneNumber = '0000000000', " +
            "IsActive = FALSE, " +
            "GPA = 0.0, " +
            "GrantSum = 0.0" +
            "WHERE StudentId = 1",
            connection);
        LogQuery(updateStudentsCommand);
        updateStudentsCommand.ExecuteNonQuery();
        Log("Успешно");

     
        var updateCoursesCommand = new NpgsqlCommand(
            "UPDATE Courses SET " +
            "CourseId = 100, " +
            "CourseName = 'Обновленный курс', " +
            "Credits = 0, " +
            "StartDate = '2025-01-01', " +
            "EndDate = '2025-12-31', " +
            "Description = 'Описание обновлено'" +
            "WHERE CourseId = 1",
            connection);
        LogQuery(updateCoursesCommand);
        updateCoursesCommand.ExecuteNonQuery();
        Log("Успешно");

 
        var updateEnrollmentsCommand = new NpgsqlCommand(
            "UPDATE Enrollments SET " +
            "EnrollmentId = 101, " +
            "Grade = 0.0, " +
            "EnrollmentDateTime = CURRENT_TIMESTAMP " +
            "WHERE StudentId = 10 AND CourseId = 100",
            connection);
        LogQuery(updateEnrollmentsCommand);
        updateEnrollmentsCommand.ExecuteNonQuery();
        Log("Успешно");
    }

    private static void DeleteData(NpgsqlConnection connection)
    {
        var deleteStudentCommand = new NpgsqlCommand("DELETE FROM Students WHERE StudentID = 6", connection);
        LogQuery(deleteStudentCommand);
        deleteStudentCommand.ExecuteNonQuery();
        Log("Успешно");

        var deleteCourseCommand = new NpgsqlCommand("DELETE FROM Courses WHERE CourseID = 6", connection);
        LogQuery(deleteCourseCommand);
        deleteCourseCommand.ExecuteNonQuery();
        Log("Успешно");

        var deleteEnrollmentCommand = new NpgsqlCommand("DELETE FROM Enrollments WHERE EnrollmentID = 10", connection);
        LogQuery(deleteEnrollmentCommand);
        deleteEnrollmentCommand.ExecuteNonQuery();
        Log("Успешно");
    }
}
