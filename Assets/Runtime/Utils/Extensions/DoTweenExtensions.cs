using DG.Tweening;

namespace Runtime.Utils.Extensions
{
    public static class DoTweenExtensions
    {
        public static void KillActive(this Tween tween)
        {
            if (tween != null)
            {
                if (tween.IsActive())
                {
                    tween.Kill();
                }
            }
        }
    }
}