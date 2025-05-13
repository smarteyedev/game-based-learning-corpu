using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Smarteye.AnimationHelper
{
    public static class AnimationHelper
    {
        // Menyimpan tween aktif untuk tiap CanvasGroup
        private static Dictionary<CanvasGroup, Tween> activeTweens = new Dictionary<CanvasGroup, Tween>();

        public static void FadeInCanvasGroup(CanvasGroup _cg, float _duration, Action _onComplete = null)
        {
            // Jika sudah ada tween aktif untuk CanvasGroup ini, hentikan dan hapus dari dictionary
            if (activeTweens.TryGetValue(_cg, out Tween existingTween))
            {
                if (existingTween.IsActive() && existingTween.IsPlaying())
                {
                    existingTween.Kill(); // Hentikan animasi sebelumnya
                }
                activeTweens.Remove(_cg);
            }

            // Buat tween baru
            Tween newTween = _cg.DOFade(1f, _duration)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    _onComplete?.Invoke();
                    activeTweens.Remove(_cg); // Hapus dari dictionary setelah selesai
                });

            // Simpan tween baru ke dictionary
            activeTweens[_cg] = newTween;
        }
    }
}