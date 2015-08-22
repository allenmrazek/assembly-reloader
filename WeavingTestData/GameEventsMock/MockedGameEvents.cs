namespace WeavingTestData.GameEventsMock
{
    public static class MockedGameEvents
    {
        public static EventDataMock<EventReport> TestEvent = new EventDataMock<EventReport>("TestEvent");

        public static EventDataMock<bool> AnotherEvent = new EventDataMock<bool>(";skdfjds");
        public static EventDataMock<string> TestEventasd = new EventDataMock<string>("jjjjjkkk");
        public static EventDataMock<float> TestEventff = new EventDataMock<float>("lk;j;lkjasdf");
        public static EventDataMock<EventReport> TestEventasdfsdf = new EventDataMock<EventReport>("lliwerwer");
        public static EventDataMock<EventReport> TestEventffs = new EventDataMock<EventReport>("blah");
    }
}
