using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Godot;

namespace GodotExtensions;

public static class PrintGlobals {
	public static void Print(
		object? what,
		[CallerMemberName] string caller = "",
		[CallerLineNumber] int line = 0,
		[CallerFilePath] string file = "") {
		string ticks = LogConfig.GetTicksString(Time.GetTicksMsec());

		string objectString = LogConfig.CreateObjectToString(what);
		string printMessage = FormatPrintMessage(objectString, caller, line, file, ticks);

		GD.Print(printMessage);
	}

	public static void PrintWarning(
		object? what,
		[CallerMemberName] string caller = "",
		[CallerLineNumber] int line = 0,
		[CallerFilePath] string file = "") {
		string ticks = LogConfig.GetTicksString(Time.GetTicksMsec());

		string objectString = LogConfig.CreateObjectToString(what);
		string printMessage = FormatPrintMessage(objectString, caller, line, file, ticks);
		string pushMessage = FormatPushMessage(objectString, ticks);

		GD.PrintRich("[color=yellow]" + printMessage + "[/color]");
		GD.PushWarning(pushMessage);
	}

	public static void PrintError(
		object? what,
		[CallerMemberName] string caller = "",
		[CallerLineNumber] int line = 0,
		[CallerFilePath] string file = ""
	) {
		string ticks = LogConfig.GetTicksString(Time.GetTicksMsec());

		string objectString = LogConfig.CreateObjectToString(what);
		string printMessage = FormatPrintMessage(objectString, caller, line, file, ticks);
		string pushMessage = FormatPushMessage(objectString, ticks);

		GD.PrintErr(printMessage);
		GD.PushError(pushMessage);
	}


	/// <summary>
	///     Only runs in debug mode.
	/// </summary>
	internal static void DebugAssert(bool condition, string message, [CallerMemberName] string caller = "", [CallerLineNumber] int line = 0, [CallerFilePath] string file = "") {
		// only run in debug mode
		if (OS.IsDebugBuild() == false)
			return;

		if (condition)
			return;

		string ticks = LogConfig.GetTicksString(Time.GetTicksMsec());
		string printMessage = FormatPrintMessage(message, caller, line, file, ticks);
		string pushMessage = FormatPushMessage(message, ticks);

		GD.PrintErr($"Assert failed: {printMessage}");
		GD.PushError($"Assert failed: {pushMessage}");
		Debugger.Break();
		Debug.Assert(false, message);
	}

	static string FormatPrintMessage(string what, string caller, int line, string filePath, string ticksFormatted) {
		string fileName = filePath.GetFile().GetBaseName();
		string clientOrServer = LogConfig.runContextPrefix;
		string lineStuff = LogConfig.SHOW_LINE_NUMBERS ? $"{line}:" : "";
		var prefix = $"{lineStuff}{fileName}.{caller}()"; //+ $" \t| {what}";
		string padding = LogConfig.CreatePadding(LogConfig.TAB_PADDING_CHARS, prefix.Length) + " | ";
		return prefix + padding + what;
	}

	static string FormatPushMessage(string what, string ticksFormatted) => $"{LogConfig.runContextPrefix}| {ticksFormatted} \t| {what}";
}

public static class LogConfig {
	public const bool SHOW_LINE_NUMBERS = false;
	public const int TAB_PADDING_CHARS = 0;
	public static string runContextPrefix = "";

	public static string CreateObjectToString(object? what) {
		var objectString = "<null object>";
		if (what is null)
			return objectString;

		var toString = what.ToString();
		objectString = toString ?? $"<object of type {what.GetType().Name} with null ToString() method>";
		return objectString;
	}

	public static string GetTicksString(ulong ticks) {
		string ticksPadded = ticks.ToString().PadLeft(10, '-');
		return ticksPadded;
	}

	// Makes all the log messages line up nicely
	public static string CreatePadding(int maxPaddingLength, int messageSize) {
		int paddingLength = maxPaddingLength - messageSize;
		StringBuilder paddingBuilder = new();
		for (var i = 0; i < paddingLength; i++)
			paddingBuilder = paddingBuilder.Append(' ');

		return paddingBuilder.ToString();
	}
}