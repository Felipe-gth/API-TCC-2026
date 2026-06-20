namespace Properties;

using MySql.Data.MySqlClient;

public class DBConnection
{   
    public static MySqlConnection Connection()
    {
        try
        {
            var strConnection = "server=localhost;uid=root;password=14570;database=clinica";
            var connection = new MySqlConnection(strConnection);
            connection.Open();

            return connection;
        }
        catch (Exception  ex)
        {
            throw new Exception($"Error connecting to database: {ex.Message}");
        } 
    }
}

