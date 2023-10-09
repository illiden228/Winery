using System;
using System.Collections.Generic;
using Core;
using Data;
using Game.Player;
using Tools;
using Tools.Extensions;
using UniRx;
using UnityEngine;

namespace UI.Select
{
    public class SelectorPm : BaseDisposable
    {
        public struct Ctx
        {
            public IResourceLoader resourceLoader;
            public Canvas mainCanvas;
            public IInventory inventory;
            public ReactiveEvent<SelectorInfo> selectorEvent;
        }

        private readonly Ctx _ctx;
        private const string VIEW_PREFAB_NAME = "Selector";
        private SelectorView _view;
        private List<ItemCellPm> _itemCells;
        private ReactiveProperty<bool> _open;

        public SelectorPm(Ctx ctx)
        {
            _ctx = ctx;
            _itemCells = new List<ItemCellPm>();
            _open = AddDispose(new ReactiveProperty<bool>(false));
            AddDispose(_ctx.selectorEvent.SubscribeWithSkip(OnSelectorEvent));
        }

        private void OnSelectorEvent(SelectorInfo info)
        {
            _open.Value = info.Open;
            if(!info.Open)
                return;
            ClearCells();
            if (_view == null)
            {
                _ctx.resourceLoader.LoadPrefab("fake", VIEW_PREFAB_NAME, prefab => OnViewLoaded(prefab, info));
                return;
            }

            InitView(info);
        }

        private void OnViewLoaded(GameObject viewPrefab, SelectorInfo info)
        {
            _view = GameObject.Instantiate(viewPrefab, _ctx.mainCanvas.transform).GetComponent<SelectorView>();
            InitView(info);
        }

        private void InitView(SelectorInfo info)
        {
            _view.Init(new SelectorView.Ctx
            {
                viewDisposables = AddDispose(new CompositeDisposable()),
                open = _open
            });

            var list = _ctx.inventory.GetItemsWithType(info.Type);
            foreach (var item in list)
            {
                _itemCells.Add(CreateCell(item, () =>
                {
                    info.Item.Value = item;
                    _open.Value = false;
                }));
            }
        }
        
        private ItemCellPm CreateCell(Item item, Action onSelect)
        {
            ItemCellPm.Ctx itemCellCtx = new ItemCellPm.Ctx
            {
                resourceLoader = _ctx.resourceLoader,
                container = _view.Container,
                item = item,
                onClick = onSelect
            };
            return new ItemCellPm(itemCellCtx);
        }

        private void ClearCells()
        {
            for (int i = 0; i < _itemCells.Count; i++)
            {
                _itemCells[0].Dispose();
            }
            
            _itemCells.Clear();
        }
        
        protected override void OnDispose()
        {
            ClearCells();
            if(_view != null)
                GameObject.Destroy(_view.gameObject);
            base.OnDispose();
        }
    }
}