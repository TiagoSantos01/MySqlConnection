using System;
using System.IO;

public class Env
{  
    public void Load()
    {
        if (!File.Exists("config.env"))
            return;

        foreach (var line in File.ReadAllLines("config.env"))
        {
            var parts = line.Split('=', (char)StringSplitOptions.RemoveEmptyEntries);
             
            if (parts.Length == 2)
                Environment.SetEnvironmentVariable(parts[0], parts[1]);

        }
    }
    public string get(string env) {
        return Environment.GetEnvironmentVariable(env);
    }
}
