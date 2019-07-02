using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary
{
    public abstract class Account : IAccount
    {
        /// <summary>
        /// Событие, возникающее при выводе денег
        /// </summary>
        protected internal event AccountStateHandler Withdrawed;
        /// <summary>
        /// Событие, возникающее при пополнении счета
        /// </summary>
        protected internal event AccountStateHandler Added;
        /// <summary>
        /// Событие, возникающее при открытии счета
        /// </summary>
        protected internal event AccountStateHandler Opened;
        /// <summary>
        /// Событие, возникающее при закрытие счета
        /// </summary>
        protected internal event AccountStateHandler Closed;
        /// <summary>
        /// Событие, возникающее при начислении процентов
        /// </summary>
        protected internal event AccountStateHandler Calculated;

        protected int _id;
        static int counter = 0;

        /// <summary>
        /// Переменная для хранения суммы
        /// </summary>
        protected decimal _sum;
        /// <summary>
        /// Переменная для хранения процента
        /// </summary>
        protected int _percentage;

        /// <summary>
        /// Время с момента открытия счета
        /// </summary>
        protected int _days = 0;

        public Account(decimal sum, int percentage)
        {
            _sum = sum;
            _percentage = percentage;
            _id = ++counter;
        }

        /// <summary>
        /// Текущая сумма на счету
        /// </summary>
        public decimal CurrentSum
        {
            get { return _sum; }
        }

        /// <summary>
        /// Процент счёта
        /// </summary>
        public int Percentage
        {
            get { return _percentage; }
        }

        public int Id
        {
            get { return _id; }
        }

        /// <summary>
        /// Метод вызова событий
        /// </summary>
        /// <param name="e">Передаваемые данные событием</param>
        /// <param name="handler">Обработчик</param>
        private void CallEvent(AccountEventArgs e, AccountStateHandler handler)
        {
            if(handler != null && e != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Виртуальный метод вызова события открытия счёта
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnOpened(AccountEventArgs e)
        {
            CallEvent(e, Opened);
        }

        /// <summary>
        /// Виртуальный метод вызова события закрытия счета
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnClosed(AccountEventArgs e)
        {
            CallEvent(e, Closed);
        }

        /// <summary>
        /// Виртуальный метод вызова события пополнения счёта
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnAdded(AccountEventArgs e)
        {
            CallEvent(e, Added);
        }

        /// <summary>
        /// Виртуальный метод вызова события списания со счета
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnWithdrawed(AccountEventArgs e)
        {
            CallEvent(e, Withdrawed);
        }

        /// <summary>
        /// Виртуальный метод вызова события начисления процентов
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnCalculated(AccountEventArgs e)
        {
            CallEvent(e, Calculated);
        }

        /// <summary>
        /// Метод пополнения счёта
        /// </summary>
        /// <param name="sum"></param>
        public virtual void Put(decimal sum)
        {
            _sum += sum;
            OnAdded(new AccountEventArgs($"На счет поступило {sum}", sum));
        }

        /// <summary>
        /// Метод списания со счёта
        /// </summary>
        /// <param name="sum"></param>
        /// <returns></returns>
        public virtual decimal Withdraw(decimal sum)
        {
            decimal result = 0;
            if(sum <= _sum)
            {
                _sum -= sum;
                result = sum;
                OnWithdrawed(new AccountEventArgs($"Сумма {sum} снята со счета {_id}", sum));
            }
            else
            {
                OnWithdrawed(new AccountEventArgs($"Недостаточно денег на счете {_id}", 0));
            }

            return result;
        }

        /// <summary>
        /// Метод открытия счёта
        /// </summary>
        protected internal virtual void Open()
        {
            OnOpened(new AccountEventArgs($"Открыт новый счет! Id счета: {_id}", _sum));
        }

        /// <summary>
        /// Метод закрытия счёта
        /// </summary>
        protected internal virtual void Close()
        {
            OnClosed(new AccountEventArgs($"Счет {_id} закрыт. Итоговая сумма: {CurrentSum}", CurrentSum));
        }

        /// <summary>
        /// Метод увеличения счётчика количества дней
        /// </summary>
        protected internal void IncrementDays()
        {
            _days++;
        }

        /// <summary>
        /// Метод начисления процентов
        /// </summary>
        protected internal virtual void Calculate()
        {
            decimal increment = _sum * _percentage / 100;
            _sum += increment;
            OnCalculated(new AccountEventArgs($"Начислены проценты в размере: {increment}", increment));
        }
    }
}