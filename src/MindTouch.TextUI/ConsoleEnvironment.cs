using System;
using System.Linq;

namespace MindTouch.TextUI {
    public class ConsoleEnvironment : Host, IEnvironment {

        private int _cursorLeft;
        private int _cursorTop;
        private IPanel _cursorPanel;

        protected override void Update() {
            lock(_syncroot) {
                Console.CursorVisible = false;
                var buffer = BuildCanvas();
                for(var row = 0; row < buffer.Length - 1; row++) {
                    Console.SetCursorPosition(0, row);
                    Console.Write(buffer[row]);
                }
                // this is the last console line and it goes all the way to the left edge. Time for
                // some hackery to avoid the cursor pushing the buffer up by a line
                var last = Height - 1;
                var line = buffer[last];
                Console.SetCursorPosition(0, last);
                Console.Write(line.Last());
                Console.MoveBufferArea(0, 0, 1, 1, Console.WindowWidth - 1, last);
                Console.SetCursorPosition(0, last);
                Console.Write(line.Take(line.Length - 1).ToArray());
                ShowCursor();
            }
        }

        public override event EventHandler DimensionsChanged;

        protected override int GetWidth() {
            return Console.WindowWidth;
        }

        protected override int GetHeight() {
            return Console.WindowHeight;
        }

        public void ShowCursor(IPanel panel, int left, int top) {
            if(panel.HasFocus) {
                _cursorLeft = left;
                _cursorTop = top;
                _cursorPanel = panel;
            }
            ShowCursor();
        }

        public void HideCursor(IPanel panel) {
            if(_cursorPanel == panel) {
                _cursorPanel = null;
            }
            Console.CursorVisible = false;
        }

        private void ShowCursor() {
            if(!CursorPanelHasFocus(_cursorPanel)) {
                Console.CursorVisible = false;
                return;
            }
            Console.CursorLeft = _cursorLeft;
            Console.CursorTop = _cursorTop;
            Console.CursorVisible = true;

        }

        private bool CursorPanelHasFocus(IPanel panel) {
            if(!panel.HasFocus) {
                return false;
            }
            if(panel.Parent != null) {
                return CursorPanelHasFocus(panel.Parent);
            }
            return panel.Environment == this;
        }
    }
}