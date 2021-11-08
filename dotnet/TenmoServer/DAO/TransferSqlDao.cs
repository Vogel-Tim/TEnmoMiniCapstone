using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;
using System.Data.SqlClient;

namespace TenmoServer.DAO
{
    public class TransferSqlDao : ITransferSqlDao
    {
        private readonly string ConnectionString;

        public TransferSqlDao(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public List<Transfer> List(int userid)
        {
            List<Transfer> transferList = new List<Transfer>();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    string selectStatement = "SELECT T.transfer_id, T.account_from, T.account_to, T.amount, T.transfer_type_id, T.transfer_status_id FROM transfers T " +
                                             "JOIN accounts A ON T.account_from = A.account_id " +
                                             "JOIN users U ON A.user_id = U.user_id " +
                                             "WHERE account_to = (SELECT account_id FROM accounts WHERE user_id = @user_id) OR U.user_id = @user_id;";
                    SqlCommand sqlCmd = new SqlCommand(selectStatement, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@user_id", userid);
                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Transfer transfer = MapTransfer(reader);
                        transferList.Add(transfer);
                    } 
                }
            }
            catch (SqlException ex)
            {

                Console.WriteLine(ex.Message);
            }
            return transferList;
        }

        public Transfer GetTransfer(int id)
        {
            Transfer transfer = new Transfer();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    sqlConn.Open();
                    string selectStatement = "SELECT * FROM transfers WHERE transfer_id = @transfer_id;";
                    SqlCommand sqlCmd = new SqlCommand(selectStatement, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@transfer_id", id);
                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    if (reader.Read())
                    {
                        transfer = MapTransfer(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return transfer;
        }

        public bool Update(Transfer transfer)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {

                    sqlConn.Open();
                    string updateStatement = "UPDATE transfers SET transfer_status_id = @transfer_status_id WHERE transfer_id = @transfer_id;";
                    SqlCommand sqlCmd = new SqlCommand(updateStatement, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@transfer_status_id", transfer.StatusId);
                    sqlCmd.Parameters.AddWithValue("@transfer_id", transfer.Id);

                    int rowsAffected = sqlCmd.ExecuteNonQuery();
                    return rowsAffected == 1;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public Transfer Create(Transfer transfer)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    string insertStatement = "INSERT INTO transfers(transfer_type_id, transfer_status_id, account_from, account_to, amount) " +
                                             "VALUES(@transfer_type_id, @transfer_status_id, @account_from, @account_to, @amount);";
                    SqlCommand sqlCmd = new SqlCommand(insertStatement, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@transfer_type_id", transfer.TypeId);
                    sqlCmd.Parameters.AddWithValue("@transfer_status_id", transfer.StatusId);
                    sqlCmd.Parameters.AddWithValue("@account_from", transfer.AccountFrom);
                    sqlCmd.Parameters.AddWithValue("@account_to", transfer.AccountTo);
                    sqlCmd.Parameters.AddWithValue("@amount", transfer.Amount);

                    sqlCmd.ExecuteNonQuery();
                    sqlCmd = new SqlCommand("SELECT @@IDENTITY", sqlConn);
                    int newId = Convert.ToInt32(sqlCmd.ExecuteScalar());
                    transfer.Id = newId;

                    return transfer;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Transfer();
            }           
        }

        public bool Send(Transfer transfer)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    sqlConn.Open();
                    string updateStatement = "UPDATE accounts SET balance = balance - @amount WHERE account_id = @account_id;";
                    SqlCommand sqlCmd = new SqlCommand(updateStatement, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@amount", transfer.Amount);
                    sqlCmd.Parameters.AddWithValue("@account_id", transfer.AccountFrom);
                    int rowsAffected = sqlCmd.ExecuteNonQuery();
                    return rowsAffected == 1;
                }
            }
            catch (SqlException ex)
            {

                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool Receive(Transfer transfer)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    sqlConn.Open();
                    string updateStatement = "UPDATE accounts SET balance = balance + @amount WHERE account_id = @account_id;";
                    SqlCommand sqlCmd = new SqlCommand(updateStatement, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@amount", transfer.Amount);
                    sqlCmd.Parameters.AddWithValue("@account_id", transfer.AccountTo);
                    int rowsAffected = sqlCmd.ExecuteNonQuery();
                    return rowsAffected == 1;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private Transfer MapTransfer(SqlDataReader reader)
        {
            Transfer transfer = new Transfer();

            transfer.Id = Convert.ToInt32(reader["transfer_id"]);
            transfer.TypeId = Convert.ToInt32(reader["transfer_type_id"]);
            transfer.StatusId = Convert.ToInt32(reader["transfer_status_id"]);
            transfer.AccountFrom = Convert.ToInt32(reader["account_from"]);
            transfer.AccountTo = Convert.ToInt32(reader["account_to"]);
            transfer.Amount = Convert.ToDecimal(reader["amount"]);
            
            return transfer;
        }
    }
}
