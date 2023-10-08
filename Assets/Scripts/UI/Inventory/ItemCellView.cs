using System;
using Core;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ItemCellView : BaseMonobehaviour
    {
        public struct Ctx
        {
            public CompositeDisposable viewDisposables;
            public Action onItemClick;
            public Sprite background;
            public string name;
            public IReadOnlyReactiveProperty<int> count;
        }

        [SerializeField] private Button _itemButton;
        [SerializeField] private Image _background;
        [SerializeField] private TMP_Text _nameLabel;
        [SerializeField] private TMP_Text _countLabel;
        private Ctx _ctx;

        public void Init(Ctx ctx)
        {
            _ctx = ctx;
            _background.sprite = _ctx.background;
            _nameLabel.text = _ctx.name;

            _ctx.count.Subscribe(count =>
            {
                _countLabel.text = count.ToString();
            }).AddTo(_ctx.viewDisposables);

            if(_itemButton != null)
                _itemButton.OnClickAsObservable()
                    .Subscribe(_ => _ctx.onItemClick?.Invoke())
                    .AddTo(_ctx.viewDisposables);
        }
    }
}