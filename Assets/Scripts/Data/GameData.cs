using Data;
using Mono.Data.SqliteClient;
using SimpleJSON;
using System.Data;
using UnityEngine;

public class GameData : Singleton<GameData>
{

    public readonly string TABLE_ROLES = "roles";
    public readonly string TABLE_SKILLS = "skills";
    public readonly string TABLE_BUFFS = "buffs";
    public readonly string TABLE_EVENTS = "events";
    public readonly string TABLE_ITEMS = "items";
    public readonly string TABLE_EVENTOPTIONS = "eventopions";
    public readonly string TABLE_ROLEGROUP = "rolegroup";
    public readonly string TABAL_AREA = "area";

    readonly string _sqlDBLocation = "URI=file:coretbf.db";

    /// <summary>
    /// DB objects
    /// </summary>
    private IDbConnection _connection = null;
    private IDbCommand _command = null;
    private IDataReader _reader = null;

    public void Init()
    {
        _connection = new SqliteConnection(_sqlDBLocation);
        _command = _connection.CreateCommand();
    }

    public void Release()
    {
        if (_reader != null && !_reader.IsClosed)
            _reader.Close();
        _reader = null;

        if (_command != null)
            _command.Dispose();
        _command = null;

        if (_connection != null && _connection.State != ConnectionState.Closed)
            _connection.Close();
        _connection = null;

        EventDataer.Inst.Release();
    }

    public IDataReader ExecuteQuery(string cmd)
    {
        Debug.Log("DB>>ExecuteQuery:" + cmd);//#########

        _connection.Open();
        _command.CommandText = cmd;
        _reader = _command.ExecuteReader();
        return _reader;
    }

   public int Execute(string cmd)
    {
        Debug.Log("DB>>Execute:" + cmd);//#########

        _connection.Open();
        _command.CommandText = cmd;
        int r = _command.ExecuteNonQuery();

        Debug.Log("DB>>Result:" + r);
        
        return r;
    }

    public void SetConnect(bool connect)
    {
        if (connect && _connection.State == ConnectionState.Closed)
        {
            _connection.Open();
        }

        if (!connect && _connection.State == ConnectionState.Open)
        {
            _connection.Close();
        }
    }

    public IDataReader ExecuteQueryWithID(string table, int id)
    {
        return ExecuteQuery($"select * from '{table}' where id = {id}");
    }

    public IDataReader ExecuteQueryWithID(string table, string id)
    {
        return ExecuteQuery($"select * from '{table}' where id = {id}");
    }

    public void EndQuery()
    {
        _reader.Close();
        _connection.Close();
    }

    public JSONNode GetSimpleData(string table, int id) 
    {
        IDataReader dataReader = ExecuteQuery($"select data from '{table}' where id = {id}");
        if (dataReader.Read())
        {
            return JSONNode.Parse(dataReader.GetString(0));
        }
        else
        {
            return null;
        }
    }
}