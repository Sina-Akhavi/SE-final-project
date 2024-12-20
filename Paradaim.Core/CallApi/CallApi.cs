using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;

/// <summary>
/// Summary description for CallApi
/// </summary>
public static class CallApi<TResponse, TRequest>
{

    public static TResponse? Get(string url, TRequest data)
    {
        try
        {
            WebClient cli = new WebClient();
            cli.Headers[HttpRequestHeader.ContentType] = "application/json";
            cli.Encoding = Encoding.UTF8;
            string response = cli.UploadString(new Uri(url), "GET",
                Newtonsoft.Json.JsonConvert.SerializeObject(data));
            if (typeof(TResponse).IsPrimitive || typeof(TResponse) == typeof(string))
                return (TResponse)Convert.ChangeType(response, typeof(TResponse));

            return Newtonsoft.Json.JsonConvert.DeserializeObject<TResponse>(response);
        }
        catch (WebException ex)
        {
            if (ex.Response != null)
            {
                string response = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                throw new Exception(response);
            }
            throw;
        }
    }

    public static TResponse? Post(string url, string data)
    {
        try
        {          
            var strData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            WebClient cli = new WebClient();
            cli.Headers[HttpRequestHeader.ContentType] = "application/json";           
            cli.Encoding = Encoding.UTF8;
            var response = cli.UploadString(new Uri(url), "POST", data);
            if (typeof(TResponse).IsPrimitive || typeof(TResponse) == typeof(string))
                return (TResponse)Convert.ChangeType(response, typeof(TResponse));
            return Newtonsoft.Json.JsonConvert.DeserializeObject<TResponse>(response);
        }
        catch (WebException ex)
        {
            if (ex.Response != null)
            {
                string response = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                throw new Exception(response);
            }
            throw;
        }
    }
    public static TResponse? Post(string url, TRequest data, List<KeyValuePair<string, string>> headers)
    {
        try
        {
            var strData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            WebClient cli = new WebClient();
            cli.Headers[HttpRequestHeader.ContentType] = "application/json";

            // Add headers to the WebClient
            foreach (var header in headers)
            {
                cli.Headers.Add(header.Key, header.Value);
            }

            cli.Encoding = Encoding.UTF8;

            // Send the request
            var response = cli.UploadString(new Uri(url), "POST", strData);

            // Convert the response to the expected type
            if (typeof(TResponse).IsPrimitive || typeof(TResponse) == typeof(string))
                return (TResponse)Convert.ChangeType(response, typeof(TResponse));

            return Newtonsoft.Json.JsonConvert.DeserializeObject<TResponse>(response);
        }
        catch (WebException ex)
        {
            var lineNumber = GetLineNumber(ex);
            LogErrorToDatabase(ex.Message, ex.StackTrace ?? string.Empty, lineNumber);

            if (ex.Response != null)
            {
                string response = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                throw new Exception(response);
            }
            throw;
        }
    }
    private static int? GetLineNumber(Exception ex)
    {
        if (ex.StackTrace == null) return null;

        // Attempt to find the line number from the stack trace
        var stackLines = ex.StackTrace.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        foreach (var line in stackLines)
        {
            // Look for "line {number}" in the stack trace
            var match = System.Text.RegularExpressions.Regex.Match(line, @"\bline\s(\d+)\b");
            if (match.Success && int.TryParse(match.Groups[1].Value, out var lineNumber))
            {
                return lineNumber;
            }
        }
        return null; // If no line number is found
    }
    private static void LogErrorToDatabase(string message, string stackTrace, int? lineNumber)
    {
        try
        {
            using (var connection = new SqlConnection("Server=137.117.169.145;user=Deviceflo;password=iotdev@123;database=DevicefloDev;"))
            {
                connection.Open();

                string query = @"
                INSERT INTO errorLogs (StepNumber, ControllerName, ActionName, TImex, Exception)
                VALUES (@StepNumber, @ControllerName, @ActionName, @TImex, @Exception)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StepNumber", lineNumber);
                    command.Parameters.AddWithValue("@ControllerName", "Sensors");
                    command.Parameters.AddWithValue("@ActionName", "GetSensorByName");
                    command.Parameters.AddWithValue("@TImex", DateTime.Now);
                    command.Parameters.AddWithValue("@Exception", message);

                    command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception dbEx)
        {
            // Log to a fallback mechanism (e.g., file, event viewer) if database logging fails
            File.AppendAllText("FallbackLog.txt", $"{DateTime.Now}: {dbEx.Message}\n{dbEx.StackTrace}\n");
        }
    }
    public static TResponse? Post(string url, TRequest data)
    {
        try
        {
            var strData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            WebClient cli = new WebClient();
            cli.Headers[HttpRequestHeader.ContentType] = "application/json";
            cli.Encoding = Encoding.UTF8;
            var response = cli.UploadString(new Uri(url), "POST", strData);
            if (typeof(TResponse).IsPrimitive || typeof(TResponse) == typeof(string))
                return (TResponse)Convert.ChangeType(response, typeof(TResponse));

            return Newtonsoft.Json.JsonConvert.DeserializeObject<TResponse>(response);
        }
        catch (WebException ex)
        {
            if (ex.Response != null)
            {
                string response = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<TResponse>(response);
            }
            throw;
        }
    }
    public static TResponse? Post(string url, TRequest data,string token)
    {
        try
        {
            var strData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            WebClient cli = new WebClient();
            cli.Headers.Add("x-api-key", token);
            cli.Headers[HttpRequestHeader.ContentType] = "application/json";
            cli.Encoding = Encoding.UTF8;
            var response = cli.UploadString(new Uri(url), "POST", strData);
            if (typeof(TResponse).IsPrimitive || typeof(TResponse) == typeof(string))
                return (TResponse)Convert.ChangeType(response, typeof(TResponse));

            return Newtonsoft.Json.JsonConvert.DeserializeObject<TResponse>(response);
        }
        catch (WebException ex)
        {
            if (ex.Response != null)
            {
                string response = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<TResponse>(response);
            }
            throw;
        }
    }

    
    public static TResponse? Put(string url, TRequest data,string token)
    {
        try
        {
            WebClient cli = new WebClient();
             cli.Headers.Add("x-api-key", token);
            cli.Headers[HttpRequestHeader.ContentType] = "application/json";
            cli.Encoding = Encoding.UTF8;
            string response = cli.UploadString(new Uri(url), "PUT",
                Newtonsoft.Json.JsonConvert.SerializeObject(data));
            if (typeof(TResponse).IsPrimitive || typeof(TResponse) == typeof(string))
                return (TResponse)Convert.ChangeType(response, typeof(TResponse));

            return Newtonsoft.Json.JsonConvert.DeserializeObject<TResponse>(response);
        }
        catch (WebException ex)
        {
            if (ex.Response != null)
            {
                string response = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                throw new Exception(response);
            }
            throw;
        }
    }
    public static TResponse? Put(string url, TRequest data)
    {
        try
        {
            WebClient cli = new WebClient();
            cli.Headers[HttpRequestHeader.ContentType] = "application/json";
            cli.Encoding = Encoding.UTF8;
            string response = cli.UploadString(new Uri(url), "PUT",
                Newtonsoft.Json.JsonConvert.SerializeObject(data));
            if (typeof(TResponse).IsPrimitive || typeof(TResponse) == typeof(string))
                return (TResponse)Convert.ChangeType(response, typeof(TResponse));

            return Newtonsoft.Json.JsonConvert.DeserializeObject<TResponse>(response);
        }
        catch (WebException ex)
        {
            if (ex.Response != null)
            {
                string response = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                throw new Exception(response);
            }
            throw;
        }
    }
     
public static TResponse? Delete(string url,TRequest data,string token)
    {
        try
        {
            WebClient cli = new WebClient();
             cli.Headers.Add("x-api-key", token);
            cli.Headers[HttpRequestHeader.ContentType] = "application/json";
            cli.Encoding = Encoding.UTF8;
            string response = cli.UploadString(new Uri(url), "DELETE",
                Newtonsoft.Json.JsonConvert.SerializeObject(data));
            if (typeof(TResponse).IsPrimitive || typeof(TResponse) == typeof(string))
                return (TResponse)Convert.ChangeType(response, typeof(TResponse));

            return Newtonsoft.Json.JsonConvert.DeserializeObject<TResponse>(response);
        }
        catch (WebException ex)
        {
            if (ex.Response != null)
            {
                string response = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                throw new Exception(response);
            }
            throw;
        }
    }
    
}


public static class CallApi<TResponse> where TResponse : class
{
    public static TResponse? Get(string url)
    {
        WebClient client = new WebClient();
        var result = client.DownloadString(new Uri(url));
        return Newtonsoft.Json.JsonConvert.DeserializeObject<TResponse>(result);
    }
    public static TResponse? Get(string url, string token)
    {
        WebClient client = new WebClient();
        client.Headers.Add("x-api-key", token);
        var result = client.DownloadString(new Uri(url));
        return Newtonsoft.Json.JsonConvert.DeserializeObject<TResponse>(result);
    }
    public static TResponse? Get(string url, List<KeyValuePair<string, string>> headers)
    {
        WebClient client = new WebClient();
        // Add headers to the WebClient
        foreach (var header in headers)
        {
            client.Headers.Add(header.Key, header.Value);
        }
        var result = client.DownloadString(new Uri(url));
        return Newtonsoft.Json.JsonConvert.DeserializeObject<TResponse>(result);
    }

}
