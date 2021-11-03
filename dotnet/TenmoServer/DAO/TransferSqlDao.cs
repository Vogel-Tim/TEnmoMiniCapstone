﻿using System;
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

        public List<Transfer> List(int id)
        {
            List<Transfer> transferList = new List<Transfer>();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    sqlConn.Open();

                    string selectStatement = "SELECT * FROM transfers WHERE account_from = @id OR account_to = @id;";
                    SqlCommand sqlCmd = new SqlCommand(selectStatement, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@id", id);
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

                throw new NotImplementedException();
            }
            return transferList;
        }

        public Transfer GetTransfer()
        {
            throw new NotImplementedException();
        }

        //public Account UpdateAccountBalance(int id)
        //{
        //    try
        //    {
        //        using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
        //        {

        //            sqlConn.Open();
        //            string updateStatement = "UPDATE accounts SET balance = "
        //        }
        //    }
        //    catch (SqlException)
        //    {

        //        throw new NotImplementedException();
        //    }
        //}

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
            catch (Exception)
            {
                throw;
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
            catch (SqlException)
            {

                throw;
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
            catch (SqlException)
            {

                throw;
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
