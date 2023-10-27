namespace H00N.Network
{
    public interface IFlushable<T>
    {
        public void Push(T item);
        public void Flush();
    }
}
