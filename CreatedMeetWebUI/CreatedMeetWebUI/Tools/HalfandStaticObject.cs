using CreatedMeetWebUI.Models;

namespace CreatedMeetWebUI.Tools
{
    public sealed class HalfandStaticObject
    {
        public MeetingViewModel SelectedRegistry;
        private static readonly object _lock = new object();
        private static HalfandStaticObject _instance;

        private HalfandStaticObject()
        {
            SelectedRegistry = new MeetingViewModel();
        }

        public static HalfandStaticObject GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new HalfandStaticObject();
                    }
                }
            }
            return _instance;
        }
    }
}
