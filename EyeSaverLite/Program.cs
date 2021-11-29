using System;
using System.Runtime.InteropServices;

/*
#define SW_HIDE             0
#define SW_SHOWNORMAL       1
#define SW_NORMAL           1
#define SW_SHOWMINIMIZED    2
#define SW_SHOWMAXIMIZED    3
#define SW_MAXIMIZE         3
#define SW_SHOWNOACTIVATE   4
#define SW_SHOW             5
#define SW_MINIMIZE         6
#define SW_SHOWMINNOACTIVE  7
#define SW_SHOWNA           8
#define SW_RESTORE          9
#define SW_SHOWDEFAULT      10
#define SW_FORCEMINIMIZE    11
#define SW_MAX              11
*/

namespace EyeSaverLite {
	class Program {
		[DllImport("kernel32.dll")]
		static extern IntPtr GetConsoleWindow();

		[DllImport("user32.dll")]
		static extern bool ShowWindow( IntPtr hWnd, int nCmdShow );

		static readonly int SW_HIDE = 0;
		static readonly int SW_SHOW = 5;
		static readonly int SW_MINIMIZE = 6;
		static IntPtr handle = GetConsoleWindow();
		static readonly TimeSpan zeroSpan = new TimeSpan(0,0,0);
		static readonly TimeSpan secondSpan = new TimeSpan(0,0,1);

		static bool SpanNotNull(TimeSpan span) {
			return span != zeroSpan;
		}
		static void RestTime(TimeSpan span) {
			Random random = new Random(DateTime.Now.Second);
			Console.Clear();
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Chill out time =)");

			ShowWindow(handle, SW_SHOW);

			while( SpanNotNull(span = span - secondSpan) ) {
				ShowWindow(handle, 3);
				Console.SetCursorPosition(0, 1);
				System.Threading.Thread.Sleep(1000);
				Console.Write(span);
				ShowWindow(handle, SW_MINIMIZE);
			}
			Console.ResetColor();
			ShowWindow(handle, SW_HIDE);
		}
		static void Timer( TimeSpan rt, TimeSpan wt) {
			TimeSpan restTime = rt;
			TimeSpan workTime = wt;

			DateTime nextRest = DateTime.Now + workTime;
			ShowWindow(handle, SW_HIDE);
			Console.Clear();
			Console.WriteLine("Rest end.");
			Console.WriteLine($"Next rest:\t{nextRest}");
			while( true ) {
				if( DateTime.Now >= nextRest ) {
					RestTime(restTime);
					nextRest = DateTime.Now + workTime;
					Console.Clear();
					Console.WriteLine("Rest end.");
					Console.WriteLine($"Next rest:\t{nextRest}");
				} else {
					System.Threading.Thread.Sleep(1000);
				}
			}
		}
		static void Main( string[] args ) {
			TimeSpan rt = new TimeSpan();
			TimeSpan wt = new TimeSpan();

			if( args.Length != 2) {
				Exit("Wrong argument recieved. Exit.", -1);
			}
			if( !TimeSpan.TryParse(args[0], out rt) || !TimeSpan.TryParse(args[1], out wt) ) {
				Exit("Wrong argument recieved. Exit.", -1);
			}
				

			Timer(rt, wt);
		}
		static void Exit( string message, int returnCode ) {
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(message);
			Console.ResetColor();
			Environment.Exit(returnCode);
		}
	}
}
