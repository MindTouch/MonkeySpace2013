using System;
using System.Collections.Generic;

namespace MindTouch.MonkeySpace2013.Mud {
    public interface IPlayer: IDisposable {
        IEnumerable<string> Listen();
        string Look();
        string Go(Direction direction);
        void Say(string said);
        string GetLocation();
    }
}