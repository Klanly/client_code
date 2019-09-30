
using System;
using System.Collections;
using System.Collections.Generic;
using Cross;
namespace GameFramework
{
 

	abstract public class ClientConfig : clientBase
	{
		public ClientConfig( gameMain m):base(m)
		{
		}
		private int _loadedFileCnt;
		private int _totalToLoadFileCnt;
		override public void init()
		{ 
			onInit();	
			this.confM.regFormatFunc("client", formatClientconf);			 
		}
		abstract protected void onInit();
		//{
		//	//localConf = ClientConfigManager.create() as ClientConfigManager;
		//	//localConf.init(this);
		//}
		protected void formatClientconf( Variant conf )
        {
            for (int i = 0; i < conf["conf"].Count; i++)
            {
                Variant clientconf = conf["conf"][i];
				string name = clientconf["name"]._str;
                configParser confInst = createInst( name, true ) as configParser;
                if (confInst == null)
                {
					DebugTrace.print( " configParser ["+name+"] create failed! " );
                    continue;
                }
                confInst.initSet( clientconf["file"]._str, clientconf["preload"]._bool );                
            }

            loadPreloadClientConfig(() => { });
        }
 
		private void loadPreloadClientConfig(Action onFin)
        {
            List<configParser> toLoadClientConfigVec = new List<configParser>();

            foreach (IObjectPlugin val in m_objectPlugins.Values )
            {
				configParser conf = val as configParser;
                if (conf.preload)
                {
                    toLoadClientConfigVec.Add(conf);
                }
            }

            _loadedFileCnt = 0;
            _totalToLoadFileCnt = toLoadClientConfigVec.Count;

            _loadNextPreloadClientConfig(toLoadClientConfigVec, onFin);
        }

        protected void _loadNextPreloadClientConfig(List<configParser> toLoadClientConfigVec, Action onFin)
        {
            if (toLoadClientConfigVec.Count <= 0)
            {
				DebugTrace.print( "configParser ended!" );
                onFin();				
                return;
            }
            _loadedFileCnt++;

            configParser toLoadConf = toLoadClientConfigVec[toLoadClientConfigVec.Count - 1];

			DebugTrace.print( "try configParser["+toLoadConf.controlId +"]" );

            toLoadClientConfigVec.RemoveAt(toLoadClientConfigVec.Count - 1);
            toLoadConf.loadconfig( this.confM, (configParser confbase) =>
            {
                _loadNextPreloadClientConfig(toLoadClientConfigVec, onFin);
            });
        }

		public void loadConfigs( string fileName, Action onfin )
		{
			confM.loadConfigs( fileName, onfin );
		}
		private ConfigManager confM
		{
			get{
				return CrossApp.singleton.getPlugin("conf") as ConfigManager;
			}
		}
	}
}