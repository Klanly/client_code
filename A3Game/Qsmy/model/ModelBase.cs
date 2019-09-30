
using GameFramework;
namespace MuGame
{
  public  class ModelBase<T> : GameEventDispatcher where T : class,new() 
    {
        private static T _instance;

        public static T getInstance()
        {
            if (_instance == null)
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
            }
            return _instance;
        }
    }
}
