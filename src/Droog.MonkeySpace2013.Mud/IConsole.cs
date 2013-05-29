using System;
using System.Collections.Generic;
using System.Linq;

namespace Droog.MonkeySpace2013.Mud {
    public interface IConsole {
        void Blit(IView view, IEnumerable<char[]> take);
        void Focus(View view);
        void Blur(View view);
        View Focused { get; }
    }

    class ConsoleEnvironment : IConsole {
        private readonly HashSet<IView> _views = new HashSet<IView>();

        public void AddView(IView view) {
            _views.Add(view);
            if(view.HasFocus) {
                foreach(var v in _views.Where(x => x != view)) {
                    v.Blur();
                }
            }
            InitCursor();
        }

        public void Blit(IView view, IEnumerable<char[]> buffer) {
            AssertView(view);
            var top = view.Top;
            var left = view.Left;
            foreach(var line in buffer) {
                Console.SetCursorPosition(left, top);
                Console.Write(line);
                top++;
            }
            InitCursor();
        }

        private void AssertView(IView view) {
            if(!_views.Contains(view)) {
                throw new Exception("view does not belong to the environment");
            }
        }

        public void Focus(View view) {
            Focused = view;
        }

        public void Blur(View view) {
            Focused = null;
        }

        public View Focused { get; private set; }

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