using Microsoft.Data.Sqlite;
using Service.Core.Abstractions.Logging;
using Service.Core.DefinedTypes;
using System.Text.Json;
using System.Text.Json.Serialization;
using static Service.Core.Abstractions.Logging.IAllowLogging;

namespace Service.Core.Logging
{
    /// <summary>
    /// A logger class which logs data into a sqlite database.
    /// </summary>
    public class SqliteLogger : ILogger
    {
        /// <summary>
        /// A helper object to insert data into sql lite database
        /// </summary>
        private class LogInsertObject : IDisposable
        {
            private SqliteCommand insertCommand;

            private SqliteConnection connection;

            private SqliteParameter logLevelParameter;
            private SqliteParameter sectionParamter;
            private SqliteParameter messageParameter;
            private SqliteParameter dataParameter;
            private SqliteParameter timeStampParameter;

            public LogInsertObject(SqliteConnection connection)
            {
                this.connection = connection;
            }

            public void Insert(LogSection section, params LogData[] logsData)
            {
                try
                {
                    using var transaction = connection.BeginTransaction();

                    InitCommand(transaction);
                    foreach (var data in logsData)
                    {
                        logLevelParameter.Value = data.LogLevel.ToString();
                        sectionParamter.Value = section.ToString();
                        messageParameter.Value = data.Message;
                        dataParameter.Value =  data.Data is null ? DBNull.Value : JsonSerializer.Serialize(data.Data);
                        timeStampParameter.Value = DateTime.Now;
                        
                        insertCommand.Parameters.AddRange(new[] { logLevelParameter, sectionParamter, messageParameter, dataParameter, timeStampParameter });

                        insertCommand.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch { }
            }

            public void Dispose()
            {
            }

            private void InitCommand(SqliteTransaction transaction)
            {
                string commandText = $"insert into {LOGS_TABLE} values ($logLevel, $section, $message, $data, $timeStamp)";

                insertCommand = new SqliteCommand(commandText, connection, transaction);

                logLevelParameter = insertCommand.CreateParameter();    
                sectionParamter = insertCommand.CreateParameter();
                messageParameter = insertCommand.CreateParameter();
                dataParameter = insertCommand.CreateParameter();
                timeStampParameter = insertCommand.CreateParameter();

                logLevelParameter.ParameterName = "$logLevel";
                sectionParamter.ParameterName = "$section";
                messageParameter.ParameterName = "$message";
                dataParameter.ParameterName = "$data";
                timeStampParameter.ParameterName = "$timeStamp";

            }

        }

        internal const string DATABASE_PATH = "logs.sqlite";

        internal const string LOGS_TABLE = "Logs";

        internal const string CONNECTION_STRING = $"Data Source={DATABASE_PATH}";

        public SqliteLogger()
        { 
            CheckTable();
        }

        public void Create(LogSection section, IEnumerable<LogData> logData) => Insert(section, logData.ToArray());        

        public void Create(LogSection section, LogData logData) =>  Insert(section, logData );

        public void Dispose()
        { 
        }

        public IEnumerable<ILogMessage> GetMessages(int count)
        {
            var result = new List<LogMessage>();
            
            using var connection = new SqliteConnection(CONNECTION_STRING);

            connection.Open();

            // fetch logs from database limited by the amount of logs given as parameter.
            using var readCommand = new SqliteCommand($"select logLevel, section, message, data, timeStamp from {LOGS_TABLE} order by timeStamp desc limit $count", connection);
            var tableNameParameter = readCommand.CreateParameter();
            var countParameter = readCommand.CreateParameter();

            countParameter.Value = count;
            countParameter.ParameterName = "$count";

            readCommand.Parameters.Add(countParameter);

            using var reader = readCommand.ExecuteReader();

            // convert database entries into in memory objects
            while (reader.Read())
            {
                result.Add(new LogMessage(reader));
            }

            // return dataset
            return result;
        }

        #region Private

        private void Insert(LogSection section, params LogData[] logData)
        {
            using var connection = new SqliteConnection(CONNECTION_STRING);

            connection.Open();

            using var logInsertObject = new LogInsertObject(connection);

            logInsertObject.Insert(section, logData);
        }

        private void CheckTable()
        {
            using var connection = new SqliteConnection(CONNECTION_STRING);

            connection.Open();

            using SqliteCommand command = new SqliteCommand($"create table if not exists {LOGS_TABLE}(logLevel varchar(20), section varchar(30), message varchar(512), data nvarchar(2048) null, timeStamp datetime)", connection);

            command.ExecuteNonQuery();
        }

        #endregion
    }
}