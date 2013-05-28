using System;
using System.Linq;

namespace Droog.MonkeySpace2013.Mud {
    public class FramedView : View {
        public FramedView(int left, int top, int width, int height, ILog logger = null)
            : base(left + 1, top + 1, width - 2, height - 2, logger) {
            Console.SetCursorPosition(left, top);
            Console.Write("+" + Enumerable.Repeat("-", width - 2).Aggregate((x, y) => x + y) + "+");
            top++;
            Console.SetCursorPosition(left, top);
            for(var i = 0; i < height - 2; i++) {
                Console.Write("|" + Enumerable.Repeat(" ", width - 2).Aggregate((x, y) => x + y) + "|");
                top++;
                Console.SetCursorPosition(left, top);
            }

            // Hack for writing the last character on the last line without getting the console to scroll
            Console.Write("+");
            Console.MoveBufferArea(0, Console.CursorTop, 1, 1, Console.WindowWidth - 1, Console.CursorTop);
            Console.SetCursorPosition(0,Console.CursorTop);
            Console.Write("+" + Enumerable.Repeat("-", width - 2).Aggregate((x, y) => x + y));
        }
    }
}