namespace Droog.MonkeySpace2013.Mud {
    public interface ILog {
        void Debug(string format, params object[] args);
        void Error(string format, params object[] args);
    }
}