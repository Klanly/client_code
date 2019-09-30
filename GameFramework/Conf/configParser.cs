using System;
using System.Collections.Generic; 
 
using Cross;
namespace GameFramework
{
    public class configParser : GameEventDispatcher , IObjectPlugin
    {
        protected Variant m_conf;
        protected string m_file;
		protected bool m_preload;
		protected bool m_isloaded;
		protected IClientBase m_mgr;
        public configParser( ClientConfig m )
        { 
            m_mgr = m as IClientBase;
        }

		virtual public void init() 
		{
          
        }

        public void initSet(string url,bool preload) {
            m_file = url;
            m_preload = preload;
        }

		private string _controlId;
		public string controlId
		{
			get {
				return _controlId;
			}
			set{
				_controlId = value;
			}
		}
        public Variant conf {
            get { return m_conf; }
        }
        public string file
        {
            get { return m_file; }
        }
        public bool preload
        {
            get { return m_preload; }
        }
        public bool isloaded
        {
            get { return m_isloaded; }
        }
        public void loadconfig( ConfigManager confmgr, Action<configParser> onfin ) {
            configParser thisptr = this;
            confmgr.loadExtendConfig(m_file, (Variant conf) =>
            {
                m_conf = _formatConfig(conf);
                m_isloaded = true;
                onData();
                onfin(thisptr);
            });
        }
        virtual protected Variant _formatConfig(Variant conf)
		{
			return conf;
		}
        virtual protected void onData() { 
        
        }
    }
}
