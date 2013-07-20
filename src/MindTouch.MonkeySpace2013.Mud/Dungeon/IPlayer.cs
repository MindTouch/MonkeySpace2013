using System;
using System.Collections.Generic;

namespace MindTouch.MonkeySpace2013.Mud.Dungeon {
    public interface IPlayer: IDisposable {

        string Name { get; }

        IEnumerable<string> Listen();
        string Look();
        string Go(Direction direction);
        void Say(string said);
        string GetLocation();
    }
}