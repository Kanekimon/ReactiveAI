using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class StateAttribute<T>
{
    public string Key { get; set; }
    public T Value { get; set; }
    public System.Type ValueType => typeof(T);
}

