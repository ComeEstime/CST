using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace CardRH
{
    public class CardView : MonoBehaviour
    {
        [Header("Card Details")] [SerializeField]
        private TextMeshProUGUI txtTitle;

        [SerializeField] private TextMeshProUGUI txtDescription;
        [SerializeField] private TextMeshProUGUI txtCost;
        [SerializeField] private TextMeshProUGUI txtCardType;
        [SerializeField] private Image imgBorder;
        [SerializeField] private Image imgArt;
        [SerializeField] private GameObject GoldenBorder;

        [SerializeField] public CardSO cardData;

        public void SetData(CardSO card)
        {
            this.cardData = card;
            UpdateCardUI();
        }

        private void Start()
        {
            //UpdateCardUI();
        }

        public void UpdateCardUI()
        {
            if (cardData == null) return;

            if (txtTitle != null) txtTitle.text = cardData.Title ?? "";
            if (txtCardType != null)
            {
                switch (cardData.Type)
                {
                    case CardType.Skill:
                        txtCardType.text = "Savoir-faire";
                        break;

                    case CardType.SoftSkill:
                        txtCardType.text = "Savoir-être";
                        break;

                    case CardType.Context:
                        txtCardType.text = "Contexte";
                        break;

                    default:
                        txtCardType.text = "None";
                        break;
                }
            }

            if (txtDescription != null) txtDescription.text = cardData.Description ?? "";
            if (txtCost != null) txtCost.text = cardData.Cost.ToString();
            if (imgBorder != null) imgBorder.sprite = cardData.Border;
            if (imgArt != null) imgArt.sprite = cardData.Art;
            if (GoldenBorder) GoldenBorder.SetActive(cardData.IsGolden);
        }

        //DEBUG PACKAGE
#if UNITY_EDITOR
        private void EditorRefreshUI()
        {
            if (cardData == null) return;

            if (txtTitle) txtTitle.text = cardData.Title ?? "";
            if (txtDescription) txtDescription.text = cardData.Description ?? "";
            if (txtCost) txtCost.text = cardData.Cost.ToString();
            if (txtCardType)
                txtCardType.text = cardData.Type switch
                {
                    CardType.Skill => "Savoir-faire",
                    CardType.SoftSkill => "Savoir-être",
                    CardType.Context => "Contexte",
                    _ => "None"
                };
            if (imgBorder) imgBorder.sprite = cardData.Border;
            if (imgArt) imgArt.sprite = cardData.Art;
        }

        private void OnValidate()
        {
            if (!Application.isPlaying
                && cardData != null
                && txtTitle && txtDescription && txtCost && txtCardType && imgBorder && imgArt)
            {
                EditorRefreshUI();
            }
        }
#endif
    }
}
