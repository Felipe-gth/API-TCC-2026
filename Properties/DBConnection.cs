namespace Properties;

using MySql.Data.MySqlClient;

public class DBConnection
{   
    public static MySqlConnection Connection()
    {
        try
        {
            var strConnection = "server=localhost;uid=root;password=;database=clinica";
            var connection = new MySqlConnection(strConnection);
            connection.Open();

            return connection;
        }
        catch (Exception  ex)
        {
            throw new Exception($"Erro ao conectar no banco: {ex.Message}");
        } 
    }
}

