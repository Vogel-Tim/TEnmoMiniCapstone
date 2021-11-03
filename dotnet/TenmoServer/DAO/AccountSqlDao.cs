using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;
using System.Data.SqlClient;

namespace TenmoServer.DAO
{
    public class AccountSqlDao : IAccountSqlDao
    {

        private readonly string ConnectionString;

        public AccountSqlDao(string connString)
        {
            ConnectionString = connString;
        }

        public List<Account> List()
        {
            List<Account> accounts = new List<Account>();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    sqlConn.Open();
                    string selectStatement = "SELECT * FROM accounts;";
                    SqlCommand sqlCmd = new SqlCommand(selectStatement, sqlConn);
                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Account account = MapAccount(reader);
                        accounts.Add(account);
                    }
                }
            }
            catch (SqlException ex)
            {

                throw new NotImplementedException();
            }

            return accounts;
        }

        public Account GetAccount(int id)
        {
            Account account = new Account();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    sqlConn.Open();
                    string selectStatement = "SELECT * FROM accounts WHERE account_id = @account_id;";
                    SqlCommand sqlCmd = new SqlCommand(selectStatement, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@account_id", id);
                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    if (reader.Read())
                    {
                        account = MapAccount(reader);
                    }
                }
            }
            catch (SqlException)
            {
                throw new NotImplementedException();
            }
            return account;
        }

        //public bool Delete(int id)
        //{
        //    throw new NotImplementedException();
        //}

        private Account MapAccount(SqlDataReader reader)
        {
            Account account = new Account();
            account.Id = Convert.ToInt32(reader["account_id"]);
            account.UserId = Convert.ToInt32(reader["user_id"]);
            account.Balance = Convert.ToDecimal(reader["balance"]);

            return account;
        }
        
        //private decimal BalanceUpdate(decimal amount, int idTo, int idFrom)
        //{
        //    List<decimal> balances = new List<decimal>();
        //    try
        //    {
        //        using (SqlConnection SqlConn = new SqlConnection(ConnectionString))
        //        {
        //            SqlConn.Open();
        //            string selectStatement = "SELECT balance FROM accounts WHERE account_id IN(@idTo, @idFrom);";
        //            SqlCommand sqlCmd = new SqlCommand(selectStatement, SqlConn);
        //            sqlCmd.Parameters.AddWithValue("@idTo", idTo);
        //            sqlCmd.Parameters.AddWithValue("@idFrom", idFrom);
        //            SqlDataReader reader = sqlCmd.ExecuteReader();

        //            while (reader.Read())
        //            {
        //                Account account = MapAccount(reader);
        //                balances.Add(account.Balance);
        //            }
        //        }
        //    }
        //    catch (SqlException)
        //    {

        //        throw new NotImplementedException();
        //    }
        //}
    }
}
