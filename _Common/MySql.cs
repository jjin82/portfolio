using System;
using System.Text;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

public enum DB_KIND
{
    GF_ACCOUNT,
    GF_COMMON,
    
    GF_GAME01,

    GF_RANKING = 100,
}

namespace DB
{
    public partial class MySql : IDisposable
    {
        public MySql(int DBKind)
        {
            if (false == s_connectionStrings.TryGetValue((DB_KIND)DBKind, out string? connectionString))
            {
                Logger.CRITICAL($"Not exist DB.. kind( {DBKind} )");
                _connection = null;
                return;
            }

            Open(connectionString);
        }

        public MySql(DB_KIND kind)
        {
            if (false == s_connectionStrings.TryGetValue(kind, out string? connectionString))
            {
                Logger.CRITICAL($"Not exist DB.. kind( {kind} )");
                _connection = null;
                return;
            }

            Open(connectionString);
        }

        public MySql(string connectionString)
        {
            Open(connectionString);
        }

        private void Open(string connectionString)
        {
            try
            {
                _connection = new MySqlConnection(connectionString);
                _connection.Open();
            }
            catch (Exception e)
            {
                _connection = null;
                Logger.EXCEPTION(e, connectionString);
            }
        }

        public void Dispose()   
        {
            _reader?.Close();
            _connection?.Dispose();
        }

        public bool IsValid()
        {
            return (null != _connection);
        }

        public void PREPARE(string query)
        {
            if (null == _connection)
            {
                Logger.CRITICAL($"is null MySqlConnection.... query( {query} )");
                return;
            }

            try
            {
                _command = new MySqlCommand(query, _connection);
            }
            catch (Exception e)
            {
                Logger.EXCEPTION(e, query);
            }
        }

        public bool EXECUTE()
        {
            if (null == _connection) 
                return false; 

            try
            {
                _reader = _command?.ExecuteReader();
                if(null == _reader)
                {
                    Logger.CRITICAL($"failed to EXECUTE().... {_command?.CommandText ?? ""}");
                    return false;
                }
            }
            catch (Exception e)
            {
                Logger.EXCEPTION(e, _command?.CommandText ?? "");
                return false;
            }

            return true;
        }

        public int EXECUTE_UPDATE()
        {
            if (null == _connection) return 0;

            try
            {
                return _command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Logger.EXCEPTION(e, _command?.CommandText);
            }

            return 0;
        }

        public bool FETCH()
        {
            if (null == _reader)  return false;
            if (_reader.IsClosed) return false;

            try
            {
                return _reader.Read();
            }
            catch (Exception e)
            {
                Logger.EXCEPTION(e, _command?.CommandText ?? "");
            }

            return false;
        }

        public bool NEXT_FETCH()
        {
            if (null == _reader) return false;

            try
            {
                return _reader.NextResult();
            }
            catch (Exception e)
            {
                Logger.EXCEPTION(e, _command?.CommandText ?? "");
            }

            return false;
        }

        public void SET_PARAM(string field, object param)
        {
            if (null == _command) return;

            try
            {
                _command.Parameters.Add(new MySqlParameter(field, param));
            }
            catch (Exception e)
            {
                Logger.EXCEPTION(e, _command?.CommandText ?? "");
            }
        }

        public void SET_PARAM(string field, string s)
        {
            if (null == _command) return;

            try
            {
                _command.Parameters.Add(new MySqlParameter(field, s));
            }
            catch (Exception e)
            {
                Logger.EXCEPTION(e, _command?.CommandText ?? "");
            }
        }

        public void GET_DATA<T>(string filed, out T o)
        {
            o = default;

            try
            {
                if (null == _reader)
                    return;

                if (false == (_reader[filed] is T))
                {
                    if (_reader[filed].Equals(DBNull.Value))
                    {
                        switch (typeof(T))
                        {
                            case Type t when t == typeof(string):
                                o = (T)(object)"";                          
                                break;
                            case Type t when t == typeof(DateTime):
                                o = (T)(object)DateTime.FromFileTime(0);
                                break;
                            default:
                                Logger.WARNING_PRINT($"filed is null= {filed}, {_reader[filed].GetType()} to {typeof(T)}  ==> {_command?.CommandText ?? ""}");
                                break;
                        }
                        
                        return;
                    }

                    Logger.CRITICAL($"different filed types= {filed}, {_reader[filed].GetType()} to {typeof(T)}  ==> {_command?.CommandText ?? ""}");
                    return;
                }

                o = (T)_reader[filed];
            }
            catch (Exception e)
            {
                Logger.EXCEPTION(e, _command?.CommandText ?? "");
            }
        }

        public void GET_DATA(string filed, out string s)
        {
            s = "";

            try
            {
                if (null == _reader)
                    return;

                if (false == (_reader[filed] is string))
                {
                    if (_reader[filed].Equals(DBNull.Value))
                        return;

                    Logger.CRITICAL($"different filed types= {filed}, {_reader[filed].GetType()} to string  ==> {_command?.CommandText ?? ""}");
                    return;
                }

                s = _reader[filed] as string;
            }
            catch (Exception e)
            {
                Logger.EXCEPTION(e, _command?.CommandText ?? "");
            }
        }

        private MySqlConnection? _connection = null;
        private MySqlCommand?    _command    = null;
        private MySqlDataReader? _reader     = null;
    }

    // static.
    public partial class MySql : IDisposable
    {
        public static bool LoadConfigDB(string iniFileName)
        {
            string[] sections = Config.GetSectionNames(iniFileName);
            if (0 == sections.Length)
            {
                Logger.CRITICAL($"failed to open file( {iniFileName}");
                return false;
            }

            foreach (var section in sections)
            {
                try
                {
                    DB_KIND kind    = (DB_KIND)Enum.Parse(typeof(DB_KIND), section);
                    string host     = Config.ToString(iniFileName, kind.ToString(), "host");
                    int    port     = Config.ToInt(iniFileName,    kind.ToString(), "port");
                    string user     = Config.ToString(iniFileName, kind.ToString(), "user");
                    string password = Config.ToString(iniFileName, kind.ToString(), "password");
                    string database = Config.ToString(iniFileName, kind.ToString(), "database");

                    // DB 접속 정보 등록.
                    bool result = MySql.RegConnection(kind, host, port, database, user, password);
                    if (false == result)
                    {
                        Logger.CRITICAL($"[ Database ] - Schemas( {kind} ), host( {host} ), port( {port} ), database( {database} ), user( {user} )");
                        return false;
                    }
                    else
                    {
                        Logger.INFO_PRINT($"[ Database ] - Schemas( {kind} ), host( {host} ), port( {port} ), database( {database} ), user( {user} )");
                    }

                    // 데이터 베이스 등록.
                    s_dataBases.Add(kind, database);
                }
                catch (Exception e)
                {
                    Logger.EXCEPTION(e);
                    return false;
                }
            }

            return true;
        }

        public static bool RegConnection(DB_KIND kind, string server, int port, string database, string user, string pw)
        {
            StringBuilder connectionString = new StringBuilder();
            connectionString.Append($"Server={server};");
            connectionString.Append($"Port={port};");
            connectionString.Append($"Database={database};");
            connectionString.Append($"Uid={user};");
            connectionString.Append($"Pwd={pw};");
            connectionString.Append($"CharSet=utf8mb4");
            

            return RegConnection(kind, connectionString.ToString());
        }

        public static bool RegConnection(DB_KIND kind, string connectionString)
        {
            using (var con = new MySql(connectionString))
            {
                if (false == con.IsValid())
                    return false;
            }

            s_connectionStrings.Add(kind, connectionString);

            return true;
        }

        public static void Truncate()
        {
            // 라이브에서만 동작 안함
            if (ServerConfig.IsLive())
                return;

            foreach (var e in  s_dataBases)
            {
                using (var con = new MySql(e.Key))
                {
                    con.PREPARE($"SELECT Concat('TRUNCATE TABLE ',table_schema,'.',TABLE_NAME, ';') as truncateQuery FROM INFORMATION_SCHEMA.TABLES where table_schema = '{e.Value}'");
                    con.EXECUTE();

                    while (con.FETCH())
                    {
                        string truncateQuery = "";
                        con.GET_DATA("truncateQuery", out truncateQuery);

                        using (var con2 = new MySql(e.Key))
                        {
                            con2.PREPARE(truncateQuery);
                            var result = con2.EXECUTE();
                         
                            Logger.INFO_PRINT($"◈ ◈ ◈   {truncateQuery},  =>  RESULT( {result} )   ◈ ◈ ◈");
                        }
                    }
                }
            }
        }

        public static bool IsValid(DB_KIND kind)
        {
            return s_connectionStrings.ContainsKey(kind);
        }

        private static Dictionary<DB_KIND, string> s_connectionStrings  = new Dictionary<DB_KIND, string>();
        private static Dictionary<DB_KIND, string> s_dataBases          = new Dictionary<DB_KIND, string>();
    }
}
