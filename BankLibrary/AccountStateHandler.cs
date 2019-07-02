using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary
{
    public delegate void AccountStateHandler(object sender, AccountEventArgs e);

    public class AccountEventArgs
    {
        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; private set; }
        /// <summary>
        /// Сумма, на которую имзенился счет
        /// </summary>
        public decimal Sum { get; private set; }

        public AccountEventArgs(string msg, decimal sum)
        {
            Message = msg;
            Sum = sum;
        }
    }
}
