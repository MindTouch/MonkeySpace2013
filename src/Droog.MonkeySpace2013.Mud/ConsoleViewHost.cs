using System;
using System.Collections.Generic;
using System.Linq;

namespace Droog.MonkeySpace2013.Mud {
    class ConsoleViewHost : IViewHost {
        private readonly List<IPane> _panes = new List<IPane>();

        public void AddView(IPane view) {
            _panes.Add(view);
            if(view.HasFocus) {
                foreach(var v in _panes.Where(x => x != view)) {
                    v.Blur();
                }
            }
            InitCursor();
        }

        public void Invalidate() {
            foreach(var pane in _panes.Where(x => x.IsVisible)) {
                pane.Invalidate();
            }
        }

        public void Draw(IPane pane) {
            AssertPane(pane);

            // TODO : deal with views that extend outside the visible console
            var top = pane.Top;
            var left = pane.Left;
            foreach(var line in pane.VisibleBuffer) {
                Console.SetCursorPosition(left, top);
                if(top == Console.WindowHeight - 1 && left + line.Length == Console.WindowWidth) {

                    // this is the last console line and it goes all the way to the left edge. Time for
                    // some hackery to avoid the cursor pushing the buffer up by a line
                    Console.SetCursorPosition(left, top);
                    Console.Write(line.Last());
                    Console.MoveBufferArea(left, top, 1, 1, Console.WindowWidth - 1, top);
                    Console.SetCursorPosition(left, top);
                    Console.Write(line.Take(line.Length - 1).ToArray());
                } else {
                    Console.Write(line);
                }
                top++;
            }
            InitCursor();
        }

        private void AssertPane(IPane view) {
            if(!_panes.Contains(view)) {
                throw new Exception("pane does not belong to the environment");
            }
        }

        public void Focus(IPane view) {
            Focused = view;
        }

        public void Blur(IPane view) {
            Focused = null;
        }

        public IPane Focused { get; private set; }
        public ConsoleKeyInfo ReadKey() {
            return Console.ReadKey(true);
        }

        public event ConsoleCancelEventHandler CancelKeyPress {
            add { Console.CancelKeyPress += value; }
            remove { Console.CancelKeyPress -= value; }
        }

        private void InitCursor() {
            if(Focused == null) {
                Console.CursorVisible = false;
            } else {
                Console.SetCursorPosition(Focused.Left + Focused.CursorLeft, Focused.Top + Focused.CursorTop);
                Console.CursorVisible = true;
            }
        }
    }
}