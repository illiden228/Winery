using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ItemCellView : BaseMonobehaviour
    {
        public struct Ctx
        {
            public Sprite background;
            public string name;
        }

        [SerializeField] private Image _background;
        [SerializeField] private TMP_Text _nameLabel;
        private Ctx _ctx;

        public void Init(Ctx ctx)
        {
            _ctx = ctx;
            _background.sprite = _ctx.background;
            _nameLabel.text = _ctx.name;
        }
    }
}