namespace Foro_C
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = StartUp.InicializarApp(args);

            app.Run();
        }
    }
}