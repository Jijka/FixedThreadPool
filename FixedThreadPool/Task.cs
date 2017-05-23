using System;

namespace FixedThreadPool
{
    public class Task : ITask
    {
        private Action Func { get; }
        private bool IsRunned { get; set; }

        /// <summary>
        /// Создает задачу с указанным приоритетом. 
        /// </summary>
        /// <param name="func">Делегат содержащий метода для задачи.</param>
        public Task(Action func)
        {
            Func = func;
        }

        public void Execute()
        {
            lock (this)
            {
                IsRunned = true;
            }
            Func();
        }
    }
}