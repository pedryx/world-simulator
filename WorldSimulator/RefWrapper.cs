namespace WorldSimulator;
internal class RefWrapper<T>
    where T : struct
{
    public T Value;

    public static implicit operator T(RefWrapper<T> wrapper) => wrapper.Value;
}
