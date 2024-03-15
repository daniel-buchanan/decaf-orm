namespace decaf
{
	public enum LogLevel
	{
		/// <summary>
        /// In debug mode, all log messages will be output.
        /// </summary>
		Debug = 0,

		/// <summary>
        /// In information mode, all log messages with a status of information
        /// or higher will be output.
        /// </summary>
		Information = 1,

        /// <summary>
        /// In warning mode, all log messages with status of warning or higher
        /// will be output.
        /// </summary>
        Warning = 2,

        /// <summary>
        /// In error mode, only messages which pertain to errors will be output.
        /// </summary>
        Error = 3
	}
}

