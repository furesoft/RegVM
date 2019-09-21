namespace Ref.Core
{
    public class Stack
    {
        public int Length { get; private set; }
        public int Position { get; private set; }

        public Stack(int capacity = 50)
        {
            _data = new object[capacity];
            Position = -1;
        }

        public void Clear()
        {
            for (int i = 0; i < Position; i++)
            {
                _data[i] = null;
                Position = -1;
            }
        }

        public object Pop()
        {
            Length--;
            return _data[Position--];
        }

        public void Push(object value)
        {
            _data[++Position] = value;
            Length++;
        }

        private object[] _data;
    }
}