using System;
using System.Collections.Generic;

namespace FinanceManagementSystem
{
    // a. Transaction record to represent financial data
    public record Transaction(int Id, DateTime Date, decimal Amount, string Category);

    // b. Interface ITransactionProcessor with Process method
    public interface ITransactionProcessor
    {
        void Process(Transaction transaction);
    }

    // c. Three concrete classes implementing ITransactionProcessor
    public class BankTransferProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[BANK TRANSFER] Processing ${transaction.Amount:F2} for {transaction.Category} (ID: {transaction.Id})");
            Console.WriteLine($"Bank transfer completed on {transaction.Date:yyyy-MM-dd}");
            Console.WriteLine();
        }
    }

    public class MobileMoneyProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[MOBILE MONEY] Processing ${transaction.Amount:F2} for {transaction.Category} (ID: {transaction.Id})");
            Console.WriteLine($"Mobile money transfer completed on {transaction.Date:yyyy-MM-dd}");
            Console.WriteLine();
        }
    }

    public class CryptoWalletProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[CRYPTO WALLET] Processing ${transaction.Amount:F2} for {transaction.Category} (ID: {transaction.Id})");
            Console.WriteLine($"Cryptocurrency transaction completed on {transaction.Date:yyyy-MM-dd}");
            Console.WriteLine();
        }
    }

    // d. Base class Account
    public class Account
    {
        public string AccountNumber { get; }
        public decimal Balance { get; protected set; }

        public Account(string accountNumber, decimal initialBalance)
        {
            AccountNumber = accountNumber;
            Balance = initialBalance;
        }

        public virtual void ApplyTransaction(Transaction transaction)
        {
            Balance -= transaction.Amount;
            Console.WriteLine($"Transaction applied to account {AccountNumber}. New balance: ${Balance:F2}");
        }
    }

    // e. Sealed class SavingsAccount
    public sealed class SavingsAccount : Account
    {
        public SavingsAccount(string accountNumber, decimal initialBalance) 
            : base(accountNumber, initialBalance)
        {
        }

        public override void ApplyTransaction(Transaction transaction)
        {
            if (transaction.Amount > Balance)
            {
                Console.WriteLine("Insufficient funds");
                Console.WriteLine($"Attempted to deduct ${transaction.Amount:F2}, but balance is only ${Balance:F2}");
            }
            else
            {
                Balance -= transaction.Amount;
                Console.WriteLine($"Deducted ${transaction.Amount:F2} from savings account {AccountNumber}");
                Console.WriteLine($"Updated balance: ${Balance:F2}");
            }
            Console.WriteLine();
        }
    }

    // f. FinanceApp class
    public class FinanceApp
    {
        private List<Transaction> _transactions;

        public FinanceApp()
        {
            _transactions = new List<Transaction>();
        }

        public void Run()
        {
            Console.WriteLine("=== Finance Management System ===");
            Console.WriteLine();

            // i. Instantiate SavingsAccount
            SavingsAccount account = new SavingsAccount("SAV-12345", 1000m);
            Console.WriteLine($"Created savings account {account.AccountNumber} with initial balance: ${account.Balance:F2}");
            Console.WriteLine();

            // ii. Create three Transaction records
            Transaction transaction1 = new Transaction(1, DateTime.Now.AddDays(-2), 150.50m, "Groceries");
            Transaction transaction2 = new Transaction(2, DateTime.Now.AddDays(-1), 85.25m, "Utilities");
            Transaction transaction3 = new Transaction(3, DateTime.Now, 200.00m, "Entertainment");

            // iii. Create processors
            MobileMoneyProcessor mobileProcessor = new MobileMoneyProcessor();
            BankTransferProcessor bankProcessor = new BankTransferProcessor();
            CryptoWalletProcessor cryptoProcessor = new CryptoWalletProcessor();

            // Process and apply transactions
            Console.WriteLine("--- Processing Transactions ---");

            // MobileMoneyProcessor → Transaction 1
            mobileProcessor.Process(transaction1);
            account.ApplyTransaction(transaction1);

            // BankTransferProcessor → Transaction 2
            bankProcessor.Process(transaction2);
            account.ApplyTransaction(transaction2);

            // CryptoWalletProcessor → Transaction 3
            cryptoProcessor.Process(transaction3);
            account.ApplyTransaction(transaction3);

            // v. Add all transactions to list
            _transactions.Add(transaction1);
            _transactions.Add(transaction2);
            _transactions.Add(transaction3);

            // Display summary
            Console.WriteLine("--- Transaction Summary ---");
            Console.WriteLine($"Total transactions processed: {_transactions.Count}");
            decimal totalAmount = 0;
            foreach (var transaction in _transactions)
            {
                Console.WriteLine($"ID {transaction.Id}: ${transaction.Amount:F2} ({transaction.Category}) - {transaction.Date:yyyy-MM-dd}");
                totalAmount += transaction.Amount;
            }
            Console.WriteLine($"Total amount processed: ${totalAmount:F2}");
            Console.WriteLine($"Final account balance: ${account.Balance:F2}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Create instance of FinanceApp and call Run()
            FinanceApp app = new FinanceApp();
            app.Run();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
