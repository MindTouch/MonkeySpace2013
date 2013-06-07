using System;
using System.Collections.Generic;

namespace MindTouch.TextUI {
    public abstract class Host : IHost {

        protected readonly object _syncroot = new object();
        protected readonly List<IPanel> _panels = new List<IPanel>();
        private bool invalidating = false;

        public int Width { get { return GetWidth(); } }
        public int Height { get { return GetHeight(); } }

        protected abstract int GetWidth();
        protected abstract int GetHeight();

        public IEnumerable<IPanel> Children { get { return _panels; } }

        public void AddChild(IPanel panel) {
            lock(_syncroot) {
                panel.Blur();
                panel.Hide();
                panel.Parent = this as IPanelHost;
                panel.Environment = this as IEnvironment;
                panel.PanelChanged += OnPanelChanged;
                _panels.Add(panel);
                panel.Invalidate();
            }
        }

        private void OnPanelChanged(object sender, EventArgs args) {
            var panel = sender as IPanel;
            if(panel == null) {
                return;
            }
            AssertPanel(panel);
            lock(_syncroot) {
                var invalidator = false;
                try {
                    if(!invalidating) {
                        invalidating = true;
                        invalidator = true;
                    }
                    if(panel.HasFocus) {
                        _panels.Remove(panel);
                        foreach(var p in _panels) {
                            p.Blur();
                        }
                        _panels.Add(panel);
                    }
                    if(invalidator) {
                        Update();
                    }
                } finally {
                    if(invalidator) {
                        invalidating = false;
                    }
                }
            }
        }

        protected void AssertPanel(IPanel panel) {
            if(!_panels.Contains(panel)) {
                throw new InvalidOperationException("Panel does belong to this host");
            }
        }

        protected abstract void Update();

        protected char[][] BuildCanvas() {
            var buffer = new char[Height][];
            for(var i = 0; i < buffer.Length; i++) {
                buffer[i] = new char[Width];
            }
            foreach(var panel in _panels) {
                var panelCanvas = panel.GetCanvas();
                if(panel.Left > Width || panel.Left + panel.Width < 0 || panel.Top > Height || panel.Top + panel.Height < 0) {

                    // panel is outside the host's bounds
                    continue;
                }
                for(var row = panel.Top; row < Height; row++) {
                    var rowSourceStart = Math.Max(0, -panel.Left);
                    var rowDestinationStart = Math.Max(0, panel.Left);
                    var rowWidth = Math.Min(panel.Width, panel.Width - rowSourceStart - Math.Max(0, rowDestinationStart + panel.Width - rowSourceStart - Width));
                    var panelRow = row - panel.Top;
                    Array.Copy(panelCanvas[panelRow], rowDestinationStart, buffer[row], rowDestinationStart, rowWidth);
                }
            }
            return buffer;
        }

        public void RemoveChild(IPanel panel) {
            lock(_syncroot) {
                panel.PanelChanged -= OnPanelChanged;
                if(!_panels.Remove(panel)) {
                    return;
                }
                panel.Parent = null;
                panel.Environment = null;
                Update();
            }
        }

        public abstract event EventHandler DimensionsChanged;
    }
}