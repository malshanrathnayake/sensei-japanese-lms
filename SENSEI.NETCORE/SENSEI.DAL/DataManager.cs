using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace devspark_core_data_access_layer
{
    public class DataManager<TEntity> where TEntity : class
    {
        private string _connectionString;

        public DataManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool InsertData(string procedureName, string jsonString)
        {
            bool status = false;

            try
            {

                using (var sqlConnection = new SqlConnection(_connectionString))
                {
                    using (var sqlCommand = new SqlCommand(procedureName, sqlConnection))
                    {

                        try
                        {

                            sqlCommand.CommandType = CommandType.StoredProcedure;
                            sqlCommand.Parameters.AddWithValue("@jsonString", jsonString);

                            var executionStatusParam = new SqlParameter
                            {
                                ParameterName = "@executionStatus",
                                SqlDbType = SqlDbType.Bit,
                                Direction = ParameterDirection.Output,
                            };

                            sqlCommand.Parameters.Add(executionStatusParam);

                            sqlConnection.Open();
                            sqlCommand.ExecuteNonQuery();

                            status = (bool)sqlCommand.Parameters["@executionStatus"].Value;

                            return status;
                        }
                        catch (Exception ex)
                        {
                            return status;
                        }
                        finally
                        {
                            sqlConnection.Close();
                        }

                    }
                }

            }catch (Exception ex)
            {
                return status;
            }
        }

        public bool UpdateData(string procedureName, string jsonString)
        {
            bool status = false;

            try
            {

                using (var sqlConnection = new SqlConnection(_connectionString))
                {
                    using (var sqlCommand = new SqlCommand(procedureName, sqlConnection))
                    {

                        try
                        {

                            sqlCommand.CommandType = CommandType.StoredProcedure;
                            sqlCommand.Parameters.AddWithValue("@jsonString", jsonString);

                            var executionStatusParam = new SqlParameter
                            {
                                ParameterName = "@executionStatus",
                                SqlDbType = SqlDbType.Bit,
                                Direction = ParameterDirection.Output,
                            };

                            sqlCommand.Parameters.Add(executionStatusParam);

                            sqlConnection.Open();
                            sqlCommand.ExecuteNonQuery();

                            status = (bool)sqlCommand.Parameters["@executionStatus"].Value;

                            return status;
                        }
                        catch (Exception ex)
                        {
                            return status;
                        }
                        finally
                        {
                            sqlConnection.Close();
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                return status;
            }

        }

        public (bool, long) UpdateDataReturnPrimaryKey(string procedureName, string jsonString)
        {
            bool status = false;
            long primarykey = 0;

            try
            {

                using (var sqlConnection = new SqlConnection(_connectionString))
                {
                    using (var sqlCommand = new SqlCommand(procedureName, sqlConnection))
                    {

                        try
                        {

                            sqlCommand.CommandType = CommandType.StoredProcedure;
                            sqlCommand.Parameters.AddWithValue("@jsonString", jsonString);

                            var executionStatusParam = new SqlParameter
                            {
                                ParameterName = "@executionStatus",
                                SqlDbType = SqlDbType.Bit,
                                Direction = ParameterDirection.Output,
                            };

                            var primarykeyParam = new SqlParameter
                            {
                                ParameterName = "@primaryKey",
                                SqlDbType = SqlDbType.BigInt,
                                Direction = ParameterDirection.Output,
                            };

                            sqlCommand.Parameters.Add(executionStatusParam);
                            sqlCommand.Parameters.Add(primarykeyParam);

                            sqlConnection.Open();
                            sqlCommand.ExecuteNonQuery();

                            status = (bool)sqlCommand.Parameters["@executionStatus"].Value;
                            primarykey = (long)sqlCommand.Parameters["@primaryKey"].Value;

                            return (status, primarykey);
                        }
                        catch (Exception ex)
                        {
                            return (status, primarykey);
                        }
                        finally
                        {
                            sqlConnection.Close();
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                return (status, primarykey);
            }

        }

        public ICollection<TEntity> RetrieveData(string procedureName, SqlParameter[] parameters = null)
        {
            ICollection<TEntity> data = new List<TEntity>();

            using (var sqlConnection = new SqlConnection(_connectionString))
            using (var sqlCommand = new SqlCommand(procedureName, sqlConnection))
            {
                try
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandTimeout = 1800;

                    if (parameters != null)
                        sqlCommand.Parameters.AddRange(parameters);

                    sqlConnection.Open();

                    var jsonResult = new StringBuilder();

                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                jsonResult.Append(reader.GetValue(0)?.ToString());
                            }
                        }
                        else
                        {
                            jsonResult.Append("[]");
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(jsonResult.ToString()))
                    {
                        data = JsonConvert.DeserializeObject<ICollection<TEntity>>(jsonResult.ToString()) ?? new List<TEntity>();
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    sqlConnection.Close();
                }

                return data;
            }
        }


        public (ICollection<TEntity>, long) RetrieveDataWithCount(string procedureName, SqlParameter[] parameters = null)
        {
            ICollection<TEntity> data = new List<TEntity>();
            long count = 0;

            using (var sqlConnection = new SqlConnection(_connectionString))
            using (var sqlCommand = new SqlCommand(procedureName, sqlConnection))
            {
                try
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandTimeout = 1800;

                    if (parameters != null)
                        sqlCommand.Parameters.AddRange(parameters);

                    var outputParameterCount = new SqlParameter("@count", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    sqlCommand.Parameters.Add(outputParameterCount);

                    sqlConnection.Open();

                    var jsonResult = new StringBuilder();

                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                jsonResult.Append(reader.GetValue(0)?.ToString());
                            }
                        }
                        else
                        {
                            jsonResult.Append("[]");
                        }
                    }

                    if (sqlCommand.Parameters["@count"].Value != DBNull.Value)
                    {
                        count = Convert.ToInt64(sqlCommand.Parameters["@count"].Value);
                    }

                    if (!string.IsNullOrWhiteSpace(jsonResult.ToString()))
                    {
                        data = JsonConvert.DeserializeObject<ICollection<TEntity>>(jsonResult.ToString()) ?? new List<TEntity>();
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    sqlConnection.Close();
                }
            }

            return (data, count);
        }


        public bool DeleteData(string procedureName, SqlParameter[] parameters = null)
        {
            bool status = false;

            try
            {
                using (var sqlConnection = new SqlConnection(_connectionString))
                {
                    using (var sqlCommand = new SqlCommand(procedureName, sqlConnection))
                    {
                        try
                        {
                            sqlCommand.CommandType = CommandType.StoredProcedure;

                            if (parameters != null)
                            {
                                sqlCommand.Parameters.AddRange(parameters);
                            }

                            var executionStatusParam = new SqlParameter
                            {
                                ParameterName = "@executionStatus",
                                SqlDbType = SqlDbType.Bit,
                                Direction = ParameterDirection.Output
                            };

                            sqlCommand.Parameters.Add(executionStatusParam);

                            sqlConnection.Open();
                            sqlCommand.ExecuteNonQuery();

                            status = (bool)sqlCommand.Parameters["@executionStatus"].Value;

                            return status;

                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                        finally
                        {

                            sqlConnection.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return status;
            }



        }
        
    }
}
