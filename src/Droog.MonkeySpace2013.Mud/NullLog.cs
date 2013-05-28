namespace Droog.MonkeySpace2013.Mud {
    public class NullLog : ILog {
        public static readonly ILog Instance = new NullLog();
        public void Debug(string format, params object[] args) {
        }

        public void Error(string format, params object[] args) {
        }
    }
}