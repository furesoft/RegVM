namespace RefVM
{
    public class Operand
    {
        public OperandType Type { get; set; }
        public object Value { get; set; }

        public T As<T>()
        {
            return (T)Value;
        }
    }
}