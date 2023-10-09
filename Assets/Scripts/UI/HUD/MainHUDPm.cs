using Core;
using Data;
using Game.Player;
using Game.Purchasing;
using Tools;
using Tools.Extensions;
using UI.Store;
using UniRx;
using UnityEngine;

namespace UI.HUD
{
    public class MainHUDPm : BaseDisposable
    {
        public struct Ctx
        {
            public IResourceLoader resourceLoader;
            public Canvas mainCanvas;
            public IReadOnlyProfile profile;
            public ReactiveEvent<Purchase> purchaseEvent;
            public ReactiveCollection<Item> stock;
        }

        private readonly Ctx _ctx;
        private const string VIEW_PREFAB_NAME = "MainHUD";
        private MainHUDView _view;
        private InventoryPm _inventory;
        private StorePm _store;
        
        public MainHUDPm(Ctx ctx)
        {
            _ctx = ctx;
            _ctx.resourceLoader.LoadPrefab("fake", VIEW_PREFAB_NAME, OnViewLoaded);
        }

        private void OnViewLoaded(GameObject viewPrefab)
        {
            _view = GameObject.Instantiate(viewPrefab, _ctx.mainCanvas.transform).GetComponent<MainHUDView>();
            ReactiveProperty<bool> inventoryOpen = AddDispose(new ReactiveProperty<bool>(true));
            ReactiveProperty<bool> storeOpen = AddDispose(new ReactiveProperty<bool>(true));
            _view.Init(new MainHUDView.Ctx
            {
                viewDisposables = AddDispose(new CompositeDisposable()),
                inventoryButtonClick = () =>
                {
                    if(_inventory != null)
                        inventoryOpen.Value = true;
                    else
                        CreateInventory(inventoryOpen);
                },
                storeButtonClick = () =>
                {
                    if(_store != null)
                        storeOpen.Value = true;
                    else
                        CreateStore(storeOpen);
                },
            });
        }

        private void CreateInventory(ReactiveProperty<bool> open)
        {
            _inventory = new InventoryPm(new InventoryPm.Ctx
            {
                mainCanvas = _ctx.mainCanvas,
                resourceLoader = _ctx.resourceLoader,
                open = open,
                onCloseClick = () => open.Value = false,
                inventory = _ctx.profile.Inventory
            });
        }
        
        private void CreateStore(ReactiveProperty<bool> open)
        {
            _store = new StorePm(new StorePm.Ctx
            {
                mainCanvas = _ctx.mainCanvas,
                resourceLoader = _ctx.resourceLoader,
                open = open,
                onCloseClick = () => open.Value = false,
                purchaseEvent = _ctx.purchaseEvent,
                stock = _ctx.stock
            });
        }

        protected override void OnDispose()
        {
            _inventory?.Dispose();
            _store?.Dispose();
            if(_view != null)
                GameObject.Destroy(_view.gameObject);
            base.OnDispose();
        }
    }
}