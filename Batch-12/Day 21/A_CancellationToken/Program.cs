class Program
{
    static void Main()
    {
        Console.WriteLine("Download starting...");
        StartDownload();
        Console.WriteLine("Download Finished");
        Console.ReadLine();
    }

    private static void StartDownload()
    {
        CancellationTokenSource alarmCts = new CancellationTokenSource();
        Task t2 = Task.Run(() => DownloadFile(alarmCts.Token));
        Task t1 = Task.Run(() => ReadUserInput(alarmCts));
        Task.WaitAny(t1, t2);
    }

    private static void ReadUserInput(CancellationTokenSource alarmCts)
    {
        Console.ReadLine();
        alarmCts.Cancel();
    }

    static void DownloadFile(CancellationToken ct)
    {
        for(int i=0;i < 100;i++) {
			Console.WriteLine($"Downloading : {i} %");
			Thread.Sleep(100);
			if(ct.IsCancellationRequested) {
				return;
			}
		}
    }
}
