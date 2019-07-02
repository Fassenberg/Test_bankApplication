using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary
{
    class DemandAccount : Account
    {
        public DemandAccount(decimal sum, int percentage) : base(sum, percentage)
        {

        }

        protected internal override void Open()
        {
            base.OnOpened(new AccountEventArgs($"Открыт новый счёт до востребования! Id счёта: {_id}", _sum));
        }
    }
}
