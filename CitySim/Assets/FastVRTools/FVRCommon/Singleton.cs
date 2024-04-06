namespace SIP.Common
{
    public class Singleton<T> where T : class, new()
    {
        private static T m_instance;

        public static readonly object lockObj = new object();

        public static T Instance
        {
            get
            {
                if(m_instance == null)
                {
                    lock(lockObj)
                    {
                        if (m_instance == null) m_instance = new T();
                    }
                }
                return m_instance;
            }
        }
    }
}