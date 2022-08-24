public class StateAttribute<T>
{
    public string Key { get; set; }
    public T Value { get; set; }
    public System.Type ValueType => typeof(T);
}

