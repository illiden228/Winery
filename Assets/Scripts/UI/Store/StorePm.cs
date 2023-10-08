﻿using System;
using System.Collections.Generic;
using Core;
using Data;
using Game.Player;
using Game.Purchasing;
using Tools;
using Tools.Extensions;
using UniRx;
using UnityEngine;

namespace UI.Store
{
    public class StorePm : BaseDisposable
    {
        public struct Ctx
        {
            public IResourceLoader resourceLoader;
            public Canvas mainCanvas;
            public IReadOnlyReactiveProperty<bool> open;
            public Action onCloseClick;
            public IInventory inventory;
            public ReactiveEvent<Purchase> purchaseEvent;
        }

        private readonly Ctx _ctx;
        private const string VIEW_PREFAB_NAME = "Store";
        private StoreView _view;
        private List<ItemCellPm> _itemCells;

        public StorePm(Ctx ctx)
        {
            _ctx = ctx;
            
            _itemCells = new List<ItemCellPm>();
            _ctx.resourceLoader.LoadPrefab("fake", VIEW_PREFAB_NAME, OnViewLoaded);
        }

        private void OnViewLoaded(GameObject viewPrefab)
        {
            _view = GameObject.Instantiate(viewPrefab, _ctx.mainCanvas.transform).GetComponent<StoreView>();
            
            _view.Init(new StoreView.Ctx
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
        
        private ItemCellPm CreateCell(SeedlingData seedling)
        {
            ItemCellPm.Ctx itemCellCtx = new ItemCellPm.Ctx
            {
                resourceLoader = _ctx.resourceLoader,
                container = _view.Container,
                plantData = seedling,
                onClick = () =>
                {
                    _ctx.purchaseEvent.Notify(new Purchase
                    {
                        SeedlingData = new SeedlingData
                        {
                            Plant = seedling.Plant
                        },
                    });
                }
            };
            return new ItemCellPm(itemCellCtx);
        }
    }
}