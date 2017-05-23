namespace FixedThreadPool
{
    public interface IThreadPool
    {
        bool Execute(Task task, Priority priority);
        void Stop();
    }
}