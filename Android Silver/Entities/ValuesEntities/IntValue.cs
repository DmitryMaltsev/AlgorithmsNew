namespace Android_Silver.Entities.ValuesEntities
{
    public class IntValue : ValueEntity
    {
        private int _value;

        public int Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        public IntValue(int min, int max)
        {
            Min = min; Max = max;
        }

        //public static implicit operator IntValue(int v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
