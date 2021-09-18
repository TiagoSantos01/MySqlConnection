using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

public class ConnectionController
{
    string column;
    string value;
    string table;
    string where;
    string join;
    MySqlConnection conn;
    string[] connectionString;
    string receiveEnv;
    private Env env = new Env();

    public ConnectionController()
    {
        this.column = "";
        this.value = "";
        this.table = "";
        this.where = "";
        this.join = "";

        string[] connectionString = {"server=" + env.get("server"),"user="+env.get("user"),
         "database="+env.get("database"), "port="+env.get("port"),
         "password="+ env.get("password")};

        this.receiveEnv = string.Join(";", connectionString);
        this.conn = new MySqlConnection(receiveEnv);

        try
        {
            this.conn.Open();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
            return;
        }
    }
    ~ConnectionController()
    {
        this.conn.Close();
    }

    public void Column(string column)
    {
        if (this.column.Length > 0)
            this.column += ", " + column;
        else
            this.column = column;
    }
        public void Value(string value)
    {
        if (this.value.Length > 0)
            this.value += ", " + value;
        else
            this.value = value;
    }
    public void Table(string table)
    {
        this.table = table;
    }
    public void Innerjoin(string join, string where)
    {
        this.join += "inner join " + join + " on " + where + " ";
    }
    public void Leftjoin(string join, string where)
    {
        this.join += "left join " + join + " on " + where + " ";
    }
    public void Rightjoin(string join, string where)
    {
        this.join += "right join " + join + " on " + where + " ";
    }

    public void Fulljoin(string join, string where)
    {
        this.join += "full join " + join + " on " + where + " ";
    }

    public void Where(string where)
    {
        if (this.where.Length > 0)
            this.where += " and " + where;
        else
            this.where = "where " + where;
    }

    public List<T> Select<T>()where T: new()
    {
        List<object> result = new List<object>();
        string sql = ("SELECT " + column + " FROM " + table + " " + join + " " + where);
        MySqlCommand cmd = new MySqlCommand(sql, this.conn);
        IDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            var array = new Dictionary<string, string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                array.Add(reader.GetName(i), reader.GetString(i));
            }
            result.Add(array);
        }
        reader.Close();
        this.conn.Close();
        return new Utilities().ObjectToClass<T>(result);
    }


    public object Insert()
    {
        object result=new object();

        string sql = ("INSERT INTO " + table + " (" + column + ") " +"VALUES"+ " (" + value + ")" + where);
        MySqlCommand cmd = new MySqlCommand(sql, this.conn);
        IDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var array = new Dictionary<string, string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                array.Add(reader.GetName(i), reader.GetString(i));
            }
            result=array;
        }
        reader.Close();
        this.conn.Close();
        return result;
    }
}

