using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SwordAndFather.Models;
using Dapper;

namespace SwordAndFather.Data
{
    public class UserRepository
    {
        const string ConnectionString = "Server = localhost; Database = SwordAndFather; Trusted_Connection = True;";
        // Add user //
        public User AddUser(string username, string password)
        {

            // Setting Connection to a Variable //
            using (var db = new SqlConnection(ConnectionString))
            {
                // Query First Record or Whatever is the default which will be Null with no rows returned //
                var newUser = db.QueryFirstOrDefault<User>(@"
                    Insert into users (username, password)
                    Output inserted.*  
                    Values(username, @password)",
                   // Anoymous type is used with curly braces //
                   // Setting up parameters //
                   new { username, password });

                if (newUser != null)
                {
                    return newUser;
                }

            }


            throw new Exception("No user found");
        }

        // Delete User //
        public void DeleteUser(int userId)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                // Martin's recommendation //
                var parameter = new { Id = userId };

                var deleteQuery = "Delete From Users Where Id = @id";
                // Excecutes don't return anything //
                var rowsAffected = db.Execute(deleteQuery, parameter);
                // If you don't define a parameter it will not have a value and will give you an error //
                if (rowsAffected != 1)
                {
                    throw new Exception("Didn't do right");
                }
            }
        }

        // Update User //

        public User UpdateUser(User userToUpdate)
        {

            using (var db = new SqlConnection(ConnectionString))

            {
               var rowsAffected =  db.Execute(@"update Users
                            Set username = @username,
                                password = @password
                                Where id = @id", userToUpdate);
                if (rowsAffected == 1)
                return userToUpdate;
           }

            throw new Exception("Could not update user");
        }

        // Setting IENumerable for User to Get all users //
        // Exposes an enumerator, which supports a simple iteration over a non-generic collection. //
        // Get all users //
        public IEnumerable<User> GetAll()
        {
            // Wrapping in a using statement so we don't have to open or close connection //
            using (var db = new SqlConnection(ConnectionString))

            {
                // Returning Database Query to select Username, Password and ID from Users.. obviously //
                return db.Query<User>("select username, password, id from users");
            }

        }

    }
}

    


