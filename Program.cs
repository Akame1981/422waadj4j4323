using BANK;
using System;

class Program
{
    static void Main(string[] args)
    {
        AccountManager accountManager = new AccountManager();

        while (true)
        {
            Console.WriteLine("1. Create Account");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Exit");
            Console.Write("Select an option: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    CreateAccount(accountManager);
                    break;

                case "2":
                    Login(accountManager);
                    break;

                case "3":
                    return;

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    private static void CreateAccount(AccountManager accountManager)
    {
        Console.WriteLine("Enter username:");
        string username = Console.ReadLine();

        Console.WriteLine("Enter password:");
        string password = Console.ReadLine();

        Console.WriteLine("Enter email:");
        string email = Console.ReadLine();

        accountManager.CreateAccount(username, password, email);
        Console.WriteLine("Account created with default balance: 0.00");
    }

    private static void Login(AccountManager accountManager)
    {
        Console.WriteLine("Enter username:");
        string username = Console.ReadLine();

        Console.WriteLine("Enter password:");
        string password = Console.ReadLine();

        if (accountManager.Login(username, password))
        {
            Console.WriteLine($"Login successful! Welcome, {accountManager.CurrentAccount.Username}.");
            Console.WriteLine($"Your balance is: {accountManager.GetBalance()}");

            // Provide options based on account type
            if (accountManager.CurrentAccount.AccountType == "Admin")
            {
                AdminMenu(accountManager);
            }
            else
            {
                UserMenu(accountManager);
            }
        }
        else
        {
            Console.WriteLine("Login failed. Please check your username and password.");
        }
    }
    private static void AdminMenu(AccountManager accountManager)
    {
        while (true)
        {
            Console.WriteLine("\nAdmin Menu:");
            Console.WriteLine("1. Add Money to Your Account");
            Console.WriteLine("2. Add Money to Another User");
            Console.WriteLine("3. Logout");
            Console.Write("Select an option: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    AddMoney(accountManager);
                    break;

                case "2":
                    AddMoneyToUser(accountManager);
                    break;

                case "3":
                    return; // Logout

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    private static void AddMoneyToUser(AccountManager accountManager)
    {
        Console.WriteLine("Enter target username:");
        string targetUsername = Console.ReadLine();

        Console.WriteLine("Enter amount to add:");
        decimal amount;

        while (!decimal.TryParse(Console.ReadLine(), out amount) || amount <= 0)
        {
            Console.WriteLine("Please enter a valid positive amount:");
        }

        try
        {
            accountManager.AddMoney(amount, targetUsername);
            Console.WriteLine($"Successfully added {amount} to {targetUsername}'s balance.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    private static void UserMenu(AccountManager accountManager)
    {
        while (true)
        {
            Console.WriteLine("\nUser Menu:");
            Console.WriteLine("1. Transfer Money");
            Console.WriteLine("2. Check Balance");
            Console.WriteLine("3. Logout");
            Console.Write("Select an option: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    TransferMoney(accountManager);
                    break;

                case "2":
                    Console.WriteLine($"Your balance is: {accountManager.GetBalance()}");
                    break;

                case "3":
                    return; // Logout

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    private static void AddMoney(AccountManager accountManager)
    {
        Console.WriteLine("Enter amount to add:");
        decimal amount;

        while (!decimal.TryParse(Console.ReadLine(), out amount) || amount <= 0)
        {
            Console.WriteLine("Please enter a valid positive amount:");
        }

        try
        {
            accountManager.AddMoney(amount);
            Console.WriteLine($"Successfully added {amount} to your balance.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static void TransferMoney(AccountManager accountManager)
    {
        Console.WriteLine("Enter recipient username:");
        string recipientUsername = Console.ReadLine();

        Console.WriteLine("Enter amount to transfer:");
        decimal amount;

        while (!decimal.TryParse(Console.ReadLine(), out amount) || amount <= 0)
        {
            Console.WriteLine("Please enter a valid positive amount:");
        }

        try
        {
            accountManager.TransferMoney(recipientUsername, amount);
            Console.WriteLine($"Successfully transferred {amount} to {recipientUsername}.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
