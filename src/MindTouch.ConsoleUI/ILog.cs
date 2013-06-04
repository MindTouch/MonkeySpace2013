namespace MindTouch.ConsoleUI {
    public interface ILog {
        void Debug(string format, params object[] args);
        void Error(string format, params object[] args);
    }
}