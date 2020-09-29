namespace RPG.Core
{
    public class SafeValue<T>
    {
        private T _value;
        private bool _initialze = false;
        private System.Func<T> onInitializationFunc;

        public SafeValue(System.Func<T> onFunc)
        {
            this.onInitializationFunc = onFunc;
        }

        public T Value
        {
            get
            {
                this.ForceInit();
                return this._value;
            }

            set
            {
                this._value = value;
            }
        }

        public void ForceInit()
        {
            if (this._initialze) return;
            this._initialze = true;

            this._value = this.onInitializationFunc();
        }
    }
}