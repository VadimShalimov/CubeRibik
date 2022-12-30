using System.Threading;

namespace Runtime.Utils.Extensions
{
    public static class CtsExtensions
    {
        public static void CancelToken(ref CancellationTokenSource tokenSource)
        {
            if (tokenSource != null)
            {
                tokenSource.Cancel();
                tokenSource.Dispose();

                tokenSource = null;
            }
        }
    }
}