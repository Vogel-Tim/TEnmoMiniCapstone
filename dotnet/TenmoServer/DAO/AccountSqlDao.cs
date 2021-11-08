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

                Console.WriteLine(ex.Message);
            }

            return accounts;
        }

        public Account GetAccount(int userId = 0, int accountId = 0)
        {
            Account account = new Account();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    sqlConn.Open();
                    string selectStatement = "SELECT A.account_id, A.user_id, A.balance FROM accounts A " +
                                             "JOIN users U ON A.user_id = U.user_id WHERE U.user_id = @user_id OR account_id = @account_id;";
                    SqlCommand sqlCmd = new SqlCommand(selectStatement, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@user_id", userId);
                    sqlCmd.Parameters.AddWithValue("@account_id", accountId);
                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    if (reader.Read())
                    {
                        account = MapAccount(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return account;
        }

        private Account MapAccount(SqlDataReader reader)
        {
            Account account = new Account();
            account.Id = Convert.ToInt32(reader["account_id"]);
            account.UserId = Convert.ToInt32(reader["user_id"]);
            account.Balance = Convert.ToDecimal(reader["balance"]);

            return account;
        }
    }
}
