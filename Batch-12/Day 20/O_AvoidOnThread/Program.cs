class Program {
    static void Main() {
        Thread t1 = new Thread(() => { });
        t1.Priority = ThreadPriority.Highest; //Forbidden
        t1.Start();
        t1.Abort(); //Forbidden
    }
}