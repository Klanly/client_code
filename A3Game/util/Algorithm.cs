using System;
using System.Collections.Generic;
using Cross;
using GameFramework;

namespace MuGame
{
    public class Algorithm
	{

        public delegate double AlgorithmTypeFun(Variant att, double tm);
		//---------------------------------------------- tween --------------------------------------------------------	
		// att ={ begin=, change=, cnt= } ); cnt 抖动次
		static public double TweenShake( Variant att,  double tm )
		{
			double tmp = tm/att["att"]["duration"]._double;
			double losetimes = tmp*2;
			if( tmp > 1 ) tmp -= (int)tmp;
			
			double rtmp = (att["cnt"]._double*2 - losetimes)/(att["cnt"]._double*2);
			double newr = att["change"]._double * rtmp * rtmp;
			return att["begin"]._double + newr * Math.Sin(2*Math.PI*tmp);
		}
		

		static public double TweenLine( Variant att,  double tm )
		{
			return att["begin"]._double + tm / att["duration"]._double * att["change"]._double;
		}
		static public double TweenCubicEaseIn( Variant att,  double tm )
		{
			double tmp = tm/att["duration"]._double;
			return att["begin"]._double + tmp * tmp * tmp * att["change"]._double;
		}	
		static public double TweenCubicEaseOut( Variant att,  double tm )
		{
			double tmp = tm/att["duration"]._double - 1;			
			return att["begin"]._double +  (tmp * tmp * tmp + 1) * att["change"]._double;
		}		
		static public double TweenCubicEaseInOut(  Variant att,  double tm )
		{
			double tmp = tm/att["duration"]._double*2;		
			if( tmp < 1 )
			{
				return att["begin"]._double + tmp * tmp * tmp * att["change"]._double / 2;
			}
			else
			{
				tmp -= 2;
				return att["begin"]._double + (tmp * tmp * tmp + 2) * att["change"]._double / 2;
			}
		}
		static public double TweenBounceEaseOut(  Variant att,  double tm  )
		{
			double t=tm;
			double d = att["duration"]._double;
			double b = att["begin"]._double;
			double c = att["change"]._double;
			if (t==0) 
			{
				return b; 
			}
			if ((t/=d)==1)
			{
				return b+c;  
			}
			double p=d*0.3f;
			double a=c; 
			double s=p/4; 
			return (a*Math.Pow(2,-10*t) * Math.Sin( (t*d-s)*(2*Math.PI)/p ) + c + b);
		}
		static public double TweenQuadEaseIn(  Variant att,  double tm )
		{
			double tmp = tm/att["duration"]._double;
			return att["begin"]._double + tmp * tmp * att["change"]._double;
		}	
		static public double TweenQuadEaseOut( Variant att,  double tm )
		{
			double tmp = tm/att["duration"]._double;			
			return att["begin"]._double +  tmp * (2-tmp) * att["change"]._double;
		}		
		static public double TweenQuadEaseInOut( Variant att,  double tm )
		{
			double tmp = tm/att["duration"]._double*2;		
			if( tmp < 1 )
			{
				return att["begin"]._double + tmp * tmp * att["change"]._double / 2;
			}
			else
			{
				tmp -= 1;
				return att["begin"]._double + tmp * (2-tmp) * att["change"]._double / 2 + att["change"]._double / 2;
			}
		}
		
		static public double TweenQuadCircular( Variant att,  double tm )
		{
			double tmp = tm/att["duration"]._double;
			if( tmp > 1 ) tmp -= (int)tmp;
			
			return att["begin"]._double + tmp * (5*tmp-2)/3 * att["change"]._double;
		}
		// to and from 往返
		static public double TweenQuadEaseInTF(Variant att,  double tm)
		{
			double tmp = tm/att["duration"]._double;
			if( tmp > 1 ) tmp -= (int)tm;
			
			tmp *= 2;
			if( tmp > 1 ) tmp = 2-tmp;
			
			return att["begin"]._double + tmp * tmp *att["change"]._double;
		}
		// to and from 往返
		static public double TweenQuadEaseOutTF(Variant att,  double tm )
		{
			double tmp = tm/att["duration"]._double;
			if( tmp > 1 ) tmp -= (int)tmp;
			
			tmp *= 2;
			if( tmp > 1 ) tmp = 2-tmp;
			
			return att["begin"]._double +  tmp * (2-tmp) * att["change"]._double;
		}
		// to and from 往返
		static public double TweenQuadEaseInOutTF(Variant att,  double tm )
		{
			double tmp = tm/att["duration"]._double;
			if( tmp > 1 ) tmp -= (int)tmp;
			
			tmp *= 2;
			if( tmp > 1 ) tmp = 2-tmp;
			
			tmp *= 2;
			if( tmp < 1 )
			{
				return att["begin"]._double + tmp * tmp *att["change"]._double / 2;
			}
			else
			{
				tmp -= 1;
				return att["begin"]._double + tmp * (2-tmp) * att["change"]._double / 2 + att["change"]._double / 2;
			}
		}	
		static public double TweenExpoEaseOut(Variant att,  double tm)
		{
			double t = tm;
			if(tm == att["duration"]._double)
			{
				return  att["begin"]._double + att["change"]._double;
			}
			t = tm/ att["duration"]._double;
			return att["change"]._double * (-Math.Pow(2, -10 * t) + 1) + att["begin"]._double;
		}
        static public AlgorithmTypeFun GetTwennFun(string type = "Line")
		{
			if("Line"==type)
			{
				return TweenLine;
			}
			else if("QuadraticIn"==type)
			{
				return TweenQuadEaseIn;
			}
			else if("QuadraticOut"==type)
			{
				return TweenQuadEaseOut;
			}
			else if("CubicIn"==type)
			{
				return TweenCubicEaseIn;
			}
			else if("CubicOut"==type)
			{
				return TweenCubicEaseOut;
			}
			return TweenLine;
		}
	}
}
