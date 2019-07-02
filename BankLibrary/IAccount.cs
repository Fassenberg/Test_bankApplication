using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary
{
    interface IAccount
    {
        /// <summary>
        /// Положить деньги на счёт
        /// </summary>
        /// <param name="sum"></param>
        void Put(decimal sum);
        /// <summary>
        /// Списать деньги со счета
        /// </summary>
        /// <param name="sum"></param>
        /// <returns></returns>
        decimal Withdraw(decimal sum);
    }
}
