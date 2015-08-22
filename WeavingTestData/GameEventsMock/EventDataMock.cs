namespace WeavingTestData.GameEventsMock
{
    public class EventDataMock<T>
    {
        private readonly string _eventName;

        public delegate void OnEvent(T arg);

        public EventDataMock(string eventName)
        {
            _eventName = eventName;
        }


        public void Add(OnEvent param)
        {
            
        }

        public void Remove(OnEvent param)
        {
            
        }
    }
}
