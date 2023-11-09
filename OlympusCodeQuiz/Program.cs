namespace OlympusCodeQuiz
{
    internal class Program
    {
        const string FILE_NAME = "out.txt";
        const string DIRECTORY = "log";
        const int THREAD_COUNT = 10;
        const int LINES_PER_THREAD = 10;


        static void Main(string[] args)
        {
            FileLineWriter fileLineWriter = new FileLineWriter(DIRECTORY, FILE_NAME);
            fileLineWriter.InitFile();

            var writer = new FileLockLineWriter(THREAD_COUNT, LINES_PER_THREAD, fileLineWriter);
            writer.WriteLines();

            Console.WriteLine("done");
            Console.Read();
        }

        
    }
}