
namespace ServerCommon
{
	/// <summary>
	/// 遊戲Logger
	/// </summary>
	public class GameLogger : CommonLib.Utility.Logger
	{
		/// <summary>
		/// 內部用的Log物件
		/// </summary>
		private readonly log4net.ILog m_log = null;

		public GameLogger(log4net.ILog log)
		{
			m_log = log;
		}

		public override void LogMessage(CommonLib.Utility.Logger.LogTypeEnum type, string text, params object[] args)
		{
			if (m_log == null)
			{
				return;
			}
			string message = (args.Length == 0 ? text : string.Format(text, args));
			switch (type)
			{
				case GameLogger.LogTypeEnum.Debug:
					m_log.Debug(message);
					break;
				case GameLogger.LogTypeEnum.Error:
					m_log.Error(message);
					break;
				case GameLogger.LogTypeEnum.Info:
					m_log.Info(message);
					break;
				case GameLogger.LogTypeEnum.Warn:
					m_log.Warn(message);
					break;
			}
		}
	}
}
