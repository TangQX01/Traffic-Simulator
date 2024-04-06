using UnityEngine;

namespace SIP.Common
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T m_instance;

        public static T Instance
        {
            get { return m_instance; }
        }

        protected void Awake()
        {
            if (m_instance == null)
            {
                m_instance = (T)this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
