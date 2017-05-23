using System;
using System.Collections.Concurrent;
using System.Threading;

namespace FixedThreadPool
{
    public class FixedThreadPool : IThreadPool
    {
        private int ThreadCount { get; set; }
        private bool IsStoping { get; set; }
        private readonly object _stopLock = new object();
        private ConcurrentQueue<Tuple<Task, Priority>> _waitingTasks = new ConcurrentQueue<Tuple<Task, Priority>>() ;

        /// <summary>
        /// —оздает пул потоков с количеством потоков равным количеству €дер процессора.
        /// </summary>
        public FixedThreadPool() : this(Environment.ProcessorCount) { }

        /// <summary>
        /// —оздает пул потоков с количеством потоков равным количеству €дер процессора.
        /// </summary>
        public FixedThreadPool(int threadCount)
        {
            if (threadCount < 1)
                throw new ArgumentOutOfRangeException(nameof(threadCount), " оличество потоков должно быть больше нул€.");

            ThreadCount = threadCount;
            for (var i = 0; i < ThreadCount; i++)
            {
                var threadName = string.Format($"Task thread #{i}");
                var taskThread = new Thread(DoWork) { Name = threadName };
                taskThread.Start();
            }
        }
        public bool Execute(Task task) => Execute(task, Priority.Normal);
        public bool Execute(Task task, Priority priority)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task), "The Task can't be null.");

            if (IsStoping) return false;

            AddTask(new Tuple<Task,Priority>(task, priority));
            return true;
        }
        private void DoWork()
        {
            while (true)
            {
                var currentTask = SelectTask();
                currentTask?.Execute();
            }
        }

        private Task SelectTask()
        {
            throw new NotImplementedException();
        }

        private void AddTask(Tuple<Task, Priority> tuple)
        {
            _waitingTasks.Enqueue(tuple);
        }

        public void Stop()
        {
            lock (_stopLock)
            {
                IsStoping = true;
            }
        }
    }
}