using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
namespace MuGame
{
    abstract public class StateBase
    {
        abstract public void Enter();
        abstract public void Execute(float delta_time);
        abstract public void Exit();
    }
}
