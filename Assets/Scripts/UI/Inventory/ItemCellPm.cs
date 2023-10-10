﻿using System;
using Core;
using Data;
using Tools;
using UniRx;
using UnityEngine;

namespace UI
{
    public class ItemCellPm : BaseDisposable
    {
        public struct Ctx
        {
            public IResourceLoader resourceLoader;
            public Transform container;
            public Item item;
            public Action onClick;
            public ReactiveProperty<int> cost;
            public bool forStore;
        }

        private readonly Ctx _ctx;
        private ItemCellView _view;
        private const string VIEW_PREFAB_NAME = "ItemCellView";

        public Item Item => _ctx.item;

        public ItemCellPm(Ctx ctx)
        {
            _ctx = ctx;
            _ctx.resourceLoader.LoadPrefab("fake", VIEW_PREFAB_NAME, OnViewLoaded);
        }

        private void OnViewLoaded(GameObject viewPrefab)
        {
            _view = GameObject.Instantiate(viewPrefab, _ctx.container).GetComponent<ItemCellView>();

            ReactiveProperty<int> count = new ReactiveProperty<int>();
            ReactiveProperty<int> cost = new ReactiveProperty<int>();
            AddDispose(_ctx.item.ObserveEveryValueChanged(x => x.Count).Subscribe(_ =>
            {
                count.Value = _ctx.item.Count;
            }));
            
            AddDispose(_ctx.item.ObserveEveryValueChanged(x => x.Cost).Subscribe(_ =>
            {
                cost.Value = _ctx.item.Cost;
            }));
            
            _view.Init(new ItemCellView.Ctx
            {
                viewDisposables = AddDispose(new CompositeDisposable()),
                background = _ctx.item.Icon,
                name = _ctx.item.Name,
                count = count,
                onItemClick = _ctx.onClick,
                forStore = _ctx.forStore,
                cost = cost
            });
        }

        protected override void OnDispose()
        {
            if(_view != null)
                GameObject.Destroy(_view.gameObject);
            base.OnDispose();
        }
    }
}