using System;

public class ValueChangedEventArgs<T> : EventArgs
{
    public T PreviosValue { get; private set; }
    public T Value {  get; private set; }

    public ValueChangedEventArgs(T previosValue, T value)
    {
        PreviosValue = previosValue;
        Value = value;
    }
}
