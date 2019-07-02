using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary
{
    public enum AccountType
    {
        Ordinary,
        Deposit
    }

    class Bank<T> where T : Account
    {
        T[] accounts;

        public string Name { get; private set; }

        public Bank(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Метод банка для открытие счета клиенту
        /// </summary>
        /// <param name="accountType"></param>
        /// <param name="sum"></param>
        /// <param name="addSumHandler"></param>
        /// <param name="withdrawSumHandler"></param>
        /// <param name="calculationHandler"></param>
        /// <param name="closeAccountHandler"></param>
        /// <param name="openAccountHandler"></param>
        public void Open(AccountType accountType, decimal sum,
            AccountStateHandler addSumHandler, AccountStateHandler withdrawSumHandler,
            AccountStateHandler calculationHandler, AccountStateHandler closeAccountHandler,
            AccountStateHandler openAccountHandler)
        {
            T newAccount = default;

            switch (accountType)
            {
                case AccountType.Ordinary:
                    newAccount = new DemandAccount(sum, 1) as T;
                    break;
                case AccountType.Deposit:
                    newAccount = new DepositAccount(sum, 40) as T;
                    break;
            }

            if(newAccount == null)
            {
                throw new Exception("Ошибка создания счета");
            }
            if(accounts == null)
            {
                accounts = new T[] { newAccount };
            }
            else
            {
                T[] tempAccounts = new T[accounts.Length + 1];
                for (int i = 0; i < accounts.Length; i++)
                {
                    tempAccounts[i] = accounts[i];
                }
                tempAccounts[tempAccounts.Length - 1] = newAccount;
                accounts = tempAccounts;
            }

            // Установка обработчиков событий
            newAccount.Added += addSumHandler;
            newAccount.Withdrawed += withdrawSumHandler;
            newAccount.Closed += closeAccountHandler;
            newAccount.Opened += openAccountHandler;
            newAccount.Calculated += calculationHandler;
        }

        /// <summary>
        /// Поиск счета в банке
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T FindAccount(int id)
        {
            for (int i = 0; i < accounts.Length; i++)
            {
                if(accounts[i].Id == id)
                {
                    return accounts[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Перегруженная версия поиска счета
        /// </summary>
        /// <param name="id"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public T FindAccount(int id, out int index)
        {
            for (int i = 0; i < accounts.Length; i++)
            {
                if(accounts[i].Id == id)
                {
                    index = i;
                    return accounts[i];
                }
            }
            index = -1;
            return null;
        }

        /// <summary>
        /// Добавление средств на счет
        /// </summary>
        /// <param name="sum"></param>
        /// <param name="id"></param>
        public void Put(decimal sum, int id)
        {
            T account = FindAccount(id);
            if(account == null)
            {
                throw new Exception("Счет не найден");
            }
            account.Put(sum);
        }

        public void Withdraw(decimal sum, int id)
        {
            T account = FindAccount(id);
            if (account == null)
            {
                throw new Exception("Счет не найден");
            }
            account.Withdraw(sum);
        }

        public void Close(int id)
        {
            T account = FindAccount(id, out int index);
            if(account == null)
            {
                throw new Exception("Счет не найден");
            }

            account.Close();

            if(accounts.Length <= 1)
            {
                accounts = null;
            }
            else
            {
                T[] tempAccounts = new T[accounts.Length - 1];
                for (int i = 0, j = 0; i < accounts.Length; i++)
                {
                    if(i != index)
                    {
                        tempAccounts[j++] = accounts[i];
                    }
                }
                accounts = tempAccounts;
            }
        }

        public void CalculatePercentage()
        {
            if (accounts == null)
                return;
            for (int i = 0; i < accounts.Length; i++)
            {
                T account = accounts[i];
                account.IncrementDays();
                account.Calculate();
            }
        }
    }
}
