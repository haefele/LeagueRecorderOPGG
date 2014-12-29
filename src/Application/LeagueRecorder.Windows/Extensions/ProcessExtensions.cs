using System.Diagnostics;
using System.Threading.Tasks;

namespace LeagueRecorder.Windows.Extensions
{
    public static class ProcessExtensions
    {
        public static Task WaitForExitAsync(this Process process)
        {
            var completionSource = new TaskCompletionSource<object>();

            process.EnableRaisingEvents = true;
            process.Exited += (s, e) => completionSource.TrySetResult(null);

            return completionSource.Task;
        }
    }
}