namespace Runtime.Utils
{
    public struct ReplaceableValue<T>
    {
        public readonly T OldValue;
        
        public readonly T NewValue;

        public ReplaceableValue(T oldValue, T newValue)
        {
            OldValue = oldValue;

            NewValue = newValue;
        }
    }
}