using System;
using GameFramework;
using Cross;

using System.Collections.Generic;

namespace MuGame
{
    public class svrPaycardConfInfo : configParser
    {
        protected Variant _oneDayData;
		protected Variant _oneChargeData;
        public svrPaycardConfInfo(ClientConfig m) : base(m)
		{
		}
        public static svrPaycardConfInfo create(ClientConfig m)
        {
            return new svrPaycardConfInfo(m);
        }
        public bool on_paycard_oneday_data(ByteArray data)
		{
            //try
            //{
            //    _oneDayData =JSON.parse(data.toString());// Package.inst.unserialize_obj(data);
            //}
            //catch(par:*)
            //{
            //    DebugTrace.add(DebugTrace.DTT_ERR, "SvrPaycardConfig on_paycard_oneday_data err:" + par);			
            //}
			
            return true;
		}
		
		public bool on_paycard_onecharge_data(ByteArray data)
		{
            //try
            //{
            //    _oneChargeData =JSON.parse(data.toString());// Package.inst.unserialize_obj(data);
            //}
            //catch(par:*)
            //{
            //    DebugTrace.add(DebugTrace.DTT_ERR, "SvrPaycardConfig on_paycard_onecharge_data err:" + par);			
            //}
			
			return true;
		}

        //public function get oneDayData():Object
        //{
        //    return _oneDayData;
        //}
		
        //public function get oneChargeData():Object
        //{
        //    return _oneChargeData;
        //}
    }
}
