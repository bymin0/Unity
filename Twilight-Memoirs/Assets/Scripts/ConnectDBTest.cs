using System;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class ConnectDBTest : MonoBehaviour
{
    private string dbPath;
    void Start()
    {
        dbPath = Application.streamingAssetsPath + "/test.db";
        Debug.Log($"Database Path : {dbPath}");

        try
        {
            using (var connection = new SqliteConnection($"URI=file:{dbPath}"))
            {
                connection.Open();
                Debug.Log("Database connected successfully!");

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "Select * FROM userTest";
                    Debug.Log($"Executing SQL: {command.CommandText}");
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Debug.Log($"ID: {reader["ID"]}, Name: {reader["name"]}, Level: {reader["Level"]}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Database connection failed: {ex.Message}");
        }
    }
}