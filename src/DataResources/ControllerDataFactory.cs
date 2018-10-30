using System;
using System.Collections.Generic;
using meGaton.DataResources;

namespace meGatonDR{
	public class ControllerDataFactory{
		public GameController[] GetContollerEnable(bool mouse,bool keyborad,bool gamepad){
			var res = new List<GameController>();
			if(mouse)res.Add(GameController.Mouse);
			if(keyborad)res.Add(GameController.Keyboard);
			if(gamepad)res.Add(GameController.Xbox);
			return res.ToArray();
		}
	}
}