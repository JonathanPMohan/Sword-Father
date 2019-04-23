using System.Collections.Generic;
using System.Data.SqlClient;
using SwordAndFather.Models;

namespace SwordAndFather.Data
{
    public class UserRepository
    {
        static List<User> _users = new List<User>();

        public User AddUser(string username, string password)
        {
            var newUser = new User(username, password);

            newUser.Id = _users.Count + 1;

            _users.Add(newUser);

            return newUser;
        }

        // How to create a SQL connection //
        public List<User> GetAll()
        {
            // Setting users to a new list //
            var users = new List<User>();
            // Setting the conenction to a variable //
            var connection = new SqlConnection("Server = localhost; Database = SwordAndFather; Trusted_Connection = True;");
            // Open Connection //
            connection.Open();
            // creating the get all users command //
            var getAllUsersCommand = connection.CreateCommand();
            // Writing the Command Select All Users //
            getAllUsersCommand.CommandText = @"select username, password, id 
                                                from users";
            // Sending the Command to the Data from the API cloud by setting it up as a variable --

            var reader = getAllUsersCommand.ExecuteReader();
            // While loop Method returns a true or false //
            // Add constructor to return users //
            while (reader.Read())
            {
                // This returns the id as an Int //
                var id = (int)reader["Id"];
                var username = reader["username"].ToString();
                var password = reader["password"].ToString();
                var user = new User(username, password) { Id = id };

                // Adding Users to Users //
                users.Add(user);
            }
            
            // Closing the COnnection //
            connection.Close();

            // Returning all users //
            return users;
        }

    }
}

