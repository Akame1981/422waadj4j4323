using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BANK
{
    public class AccountManager
    {
        private string filePath = "accounts.json";
        private List<Account> accounts;
        public Account CurrentAccount { get; private set; } // To store the currently logged-in account

        public AccountManager()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                accounts = JsonConvert.DeserializeObject<List<Account>>(json) ?? new List<Account>();
            }
            else
            {
                accounts = new List<Account>();
            }
        }

        public void CreateAccount(string username, string password, string email)
        {
            Account newAccount = new Account
            {
                Username = username,
                Password = password,
                Email = email,
                AccountType = "User", // Default account type
                Balance = 0.00m // Default initial balance
            };

            accounts.Add(newAccount);
            SaveAccounts();
        }

        public bool Login(string username, string password)
        {
            CurrentAccount = accounts.Find(a => a.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && a.Password == password);
            return CurrentAccount != null; // Returns true if login is successful
        }

        public decimal GetBalance()
        {
            return CurrentAccount?.Balance ?? 0; // Returns balance of the currently logged-in account
        }

        public void AddMoney(decimal amount, string username = null)
        {
            if (CurrentAccount == null || (username == null && CurrentAccount.AccountType != "Admin"))
            {
                throw new InvalidOperationException("Only admins can add money to other accounts.");
            }

            if (username != null) // Admin adding money to another user
            {
                Account targetAccount = accounts.Find(a => a.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
                if (targetAccount == null)
                {
                    throw new InvalidOperationException("Target account does not exist.");
                }
                targetAccount.Balance += amount; // Increase target account balance
            }
            else // User adding to their own account
            {
                CurrentAccount.Balance += amount; // Increase balance
            }

            SaveAccounts();
        }

        public void TransferMoney(string toUsername, decimal amount)
        {
            if (CurrentAccount == null)
            {
                throw new InvalidOperationException("You must be logged in to transfer money.");
            }

            Account recipientAccount = accounts.Find(a => a.Username.Equals(toUsername, StringComparison.OrdinalIgnoreCase));
            if (recipientAccount == null)
            {
                throw new InvalidOperationException("Recipient account does not exist.");
            }

            if (CurrentAccount.Balance < amount)
            {
                throw new InvalidOperationException("Insufficient balance.");
            }

            CurrentAccount.Balance -= amount; // Deduct from current account
            recipientAccount.Balance += amount; // Add to recipient account
            SaveAccounts();
        }

        private void SaveAccounts()
        {
            // Use Newtonsoft.Json.Formatting for clarity
            string json = JsonConvert.SerializeObject(accounts, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}