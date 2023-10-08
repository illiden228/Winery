using System;
using System.Collections.Generic;
using Core;
using Data;
using Game.Player;
using Tools;
using UniRx;
using UnityEngine;

namespace UI
{
    public class InventoryPm : BaseDisposable
    {
        public struct Ctx
        {
            public IResourceLoader resourceLoader;
            public Canvas mainCanvas;
            public IReadOnlyReactiveProperty<bool> open;
            public Action onCloseClick;
            public IInventory inventory;
        }

        private readonly Ctx _ctx;
        private const string VIEW_PREFAB_NAME = "Inventory";
        private InventoryView _view;
        private List<ItemCellPm> _itemCells;

        public InventoryPm(Ctx ctx)
        {
            _ctx = ctx;
            _itemCells = new List<ItemCellPm>();
            _ctx.resourceLoader.LoadPrefab("fake", VIEW_PREFAB_NAME, OnViewLoaded);
        }

        private void OnViewLoaded(GameObject viewPrefab)
        {
            _view = GameObject.Instantiate(viewPrefab, _ctx.mainCanvas.transform).GetComponent<InventoryView>();
            
            _view.Init(new InventoryView.Ctx
            {
                viewDisposables = AddDispose(new CompositeDisposable()),
                open = _ctx.open,
                onCloseClick = _ctx.onCloseClick
            });
            
            foreach (var seedling in _ctx.inventory.Seedlings)
            {
                _itemCells.Add(CreateCell(seedling));
            }
        }
        
        private ItemCellPm CreateCell(Item item)
        {
            ItemCellPm.Ctx itemCellCtx = new ItemCellPm.Ctx
            {
                resourceLoader = _ctx.resourceLoader,
                container = _view.Container,
                item = item,
            };
            return new ItemCellPm(itemCellCtx);
        }

        protected override void OnDispose()
        {
            GameObject.Destroy(_view.gameObject);
            base.OnDispose();
        }
    }
}