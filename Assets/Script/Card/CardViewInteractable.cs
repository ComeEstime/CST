using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CardRH
{
    /// <summary>
    /// Ajoute sur le GameObject de ta carte (ou un parent).
    /// Gère le hover (scale) et expose un UnityEvent OnClick.
    /// </summary>
    public class CardViewInteractable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Header("Cible (RectTransform)")]
        [SerializeField] private RectTransform target; // si vide, prendra ce transform

        [Header("Effet de survol")]
        [SerializeField] private float hoverScale = 1.07f;
        [SerializeField] private float duration = 0.12f;
        [SerializeField] private AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
        [Header("Click")]
        public UnityEvent OnClick;

        private Vector3 baseScale;
        private Coroutine anim;

        private void Awake()
        {
            if (target == null) target = transform as RectTransform;
        }

        private void Start()
        {
            baseScale = target.localScale;
        }

        public void OnPointerEnter(PointerEventData eventData) => StartAnim(baseScale * hoverScale);
        public void OnPointerExit(PointerEventData eventData)  => StartAnim(baseScale);
        public void OnPointerClick(PointerEventData eventData) => OnClick?.Invoke();

        private void StartAnim(Vector3 to)
        {
            if (anim != null) StopCoroutine(anim);
            anim = StartCoroutine(AnimateScale(to));
        }

        private IEnumerator AnimateScale(Vector3 to)
        {
            Vector3 from = target.localScale;
            float t = 0f;
            while (t < 1f)
            {
                t += Time.unscaledDeltaTime / Mathf.Max(0.0001f, duration);
                target.localScale = Vector3.LerpUnclamped(from, to, curve.Evaluate(Mathf.Clamp01(t)));
                yield return null;
            }
            target.localScale = to;
            anim = null;
        }

        public void Click()
        {
            GameManager gm = GameManager.Instance;
            if (GameManager.Instance.CurrentPhase == GamePhase.DeckBuild & TryGetComponent<CardView>(out CardView c))
            {
                if(c.cardData.InDeck) gm.CardViewDeck.RemoveCard(c);
                else gm.CardViewDeck.AddCard(c);
            }
            else if (gm.CurrentPhase == GamePhase.FindCandidat &
                     TryGetComponent<CardView>(out CardView v) & !v.cardData.InDeck)
            {
                v.cardData.InDeck = true;
                v.UpdateCardUI();
                gm.RemoveTime(1);
            }
        }
    }
}
