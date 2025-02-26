class Program
{
    static void Main()
    { 
        Console.WriteLine("Program berjalan, memantau gempa...");
		CancellationTokenSource alarmCts = new CancellationTokenSource();
        Task t2 = Task.Run(() => MonitorGempa(alarmCts.Token));
		Task t1 = Task.Run(() =>
        {
           	Thread.Sleep(5000);
            alarmCts.Cancel(); 
        });
		Task.WaitAny(t1,t2);
        Console.ReadLine(); 
    }

    static void MonitorGempa(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            Console.WriteLine("Monitoring gempa");
            Thread.Sleep(1000); 
        }
        Console.WriteLine("Gempa nih...");
    }
}
