using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Data.SQLite;
using Newtonsoft.Json.Linq;
using RestSharp;

string formatLogString(string logevent)
{
    return "[" + DateTime.Now + "] " + logevent;
}

System.Console.WriteLine("API Server version 0.0.5");
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();

var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("database_configuration.json").Build();

string connectionString = "Server=" + config.GetValue<string>("dbhost") + ";" +
                            "Database=" + config.GetValue<string>("dbname") + ";" +
                            "Trusted_Connection=False;" +
                            "User Id=" + config.GetValue<string>("dbuser") + ";" +
                            "Password=" + config.GetValue<string>("dbpass") + ";";

app.Logger.LogDebug(formatLogString(connectionString));

var connection = new SqlConnection(connectionString);

connection.Open();

var configSQLite3 = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("database_sqlite3_configuration.json").Build();

string connectionStringSQLite3 = "Data Source=" +
            configSQLite3.GetValue<string>("dbname") + ";Version=3;New=True;Compress=True;";

app.Logger.LogDebug(formatLogString(connectionStringSQLite3));

SQLiteConnection sqlite3Connection;
sqlite3Connection = new SQLiteConnection(connectionStringSQLite3);
sqlite3Connection.Open();

var configExternalServices = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("external_services.json").Build();

app.MapGet("/register", (HttpRequest request, HttpResponse response) =>
{

    app.Logger.LogInformation(formatLogString("GET /register " + "from " + request.Host));
    string sql = @"SELECT id, firstname, lastname, claim, phoneNumber, 
                    email, claim_details, age, priceusd, course, priceuah FROM [dbo].[items]";

    using var cmd = new SqlCommand(sql, connection);
    using SqlDataReader rdr = cmd.ExecuteReader();
    List<ListDictionary> answer = new List<ListDictionary>();

    while (rdr.Read())
    {
        ListDictionary list = new ListDictionary();
        list.Add("id", rdr.GetInt32(0).ToString());
        list.Add("firstName", rdr.GetString(1));
        list.Add("lastName", rdr.GetString(2));

        if (!rdr.IsDBNull(3))
        {
            list.Add("claim", rdr.GetString(3));
        }
        else
        {
            list.Add("claim", "");
        }

        list.Add("phoneNumber", rdr.GetString(4));
        list.Add("email", rdr.GetString(5));

        if (!rdr.IsDBNull(6))
        {
            list.Add("claim_details", rdr.GetString(6));
        }
        else
        {
            list.Add("claim_details", "");
        }

        list.Add("age", rdr.GetInt32(7).ToString());
        list.Add("priceusd", rdr.GetDecimal(8).ToString());
        list.Add("course", rdr.GetDecimal(9).ToString());
        list.Add("priceuah", rdr.GetDecimal(10).ToString());
        answer.Add(list);
    }

    response.Headers.AccessControlAllowOrigin = "*";
    response.ContentType = "application/json";
    response.StatusCode = 200;

    return answer;
});

app.MapPost("/register", async (HttpRequest request, HttpResponse response) =>
{
    app.Logger.LogInformation(formatLogString("POST /register " + "from " + request.Host));
    string body = "";
    using (var reader = new StreamReader(request.Body, null, true, 1024, true))
    {
        body = await reader.ReadToEndAsync();
    }

    app.Logger.LogDebug(formatLogString(body));
    JObject bodyJson = JObject.Parse(body);
    string sql = String.Format(@"INSERT INTO [dbo].[items] 
                                (firstname, 
                                lastname, 
                                claim, 
                                phoneNumber, 
                                email,
                                claim_details, 
                                age, 
                                priceusd, 
                                course, 
                                priceuah) 
                            VALUES (
                        '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', {6}, {7}, {8}, {9})",
                        bodyJson["firstName"], 
                        bodyJson["lastName"],
                        bodyJson["claim"], 
                        bodyJson["phoneNumber"],
                        bodyJson["email"], 
                        bodyJson["claim_details"],
                        bodyJson["age"], 
                        bodyJson["priceusd"], 
                        bodyJson["course"], 
                        bodyJson["priceuah"]);

    app.Logger.LogDebug(formatLogString(sql));
    using var cmd = new SqlCommand(sql, connection);
    cmd.ExecuteNonQuery();

    response.Headers.AccessControlAllowOrigin = "*";
    response.StatusCode = 201;
    return "Created";
});

app.MapGet("/getcourse", async (HttpRequest request, HttpResponse response) =>
{
    app.Logger.LogInformation(formatLogString("GET /getcourse " + "from " + request.Host));
    
    var client = new RestClient(configExternalServices["coursesServiceURL"]);
    var courseRequest = new RestRequest(configExternalServices["path"], Method.Get);
    var queryResult = (await client.ExecuteAsync(courseRequest)).Content;

    response.Headers.AccessControlAllowOrigin = "*";
    response.ContentType = "application/json";
    response.StatusCode = 200;
    return queryResult;
});

app.MapPost("/feedback", async (HttpRequest request, HttpResponse response) =>
{
    app.Logger.LogInformation(formatLogString("POST /feedback " + "from " + request.Host));

    string body = "";

    using (var reader = new StreamReader(request.Body, null, true, 1024, true))
    {
        body = await reader.ReadToEndAsync();
    }

    app.Logger.LogDebug(formatLogString(body));

    JObject bodyJson = JObject.Parse(body);
    var email = bodyJson["email"];
    var msgtext = bodyJson["msgText"];

    var sqlite3Cmd = sqlite3Connection.CreateCommand();

    string sqlite3InsertQuery = @"INSERT INTO feedback (email, msgtext) VALUES ('" + email +
                                "', '" + msgtext + "')";

    app.Logger.LogDebug(formatLogString(sqlite3InsertQuery));

    sqlite3Cmd.CommandText = sqlite3InsertQuery;
    sqlite3Cmd.ExecuteNonQuery();

    response.Headers.AccessControlAllowOrigin = "*";
    response.StatusCode = 201;

    return "OK";
});


app.MapMethods("/feedback", new[] { "OPTIONS" }, (HttpRequest request, HttpResponse response) => {

    app.Logger.LogDebug(formatLogString("OPTIONS /feedback " + "from " + request.Host));

    response.Headers.AccessControlAllowOrigin = "*";
    response.Headers.AccessControlAllowMethods = "*";
    response.Headers.AccessControlAllowHeaders = "*";
    response.StatusCode = 200;

    return "OK";
});

app.MapMethods("/register", new[] { "OPTIONS" }, (HttpRequest request, HttpResponse response) => {

    app.Logger.LogDebug(formatLogString("OPTIONS /register " + "from " + request.Host));

    response.Headers.AccessControlAllowOrigin = "*";
    response.Headers.AccessControlAllowMethods = "*";
    response.Headers.AccessControlAllowHeaders = "*";
    response.StatusCode = 200;

    return "OK";
});

app.Run();