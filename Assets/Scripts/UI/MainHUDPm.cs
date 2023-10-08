using Core;
using Game.Player;
using Tools;
using UniRx;
using UnityEngine;

namespace UI
{
    public class MainHUDPm : BaseDisposable
    {
        public struct Ctx
        {
            public IResourceLoader resourceLoader;
            public Canvas mainCanvas;
            public IReadOnlyProfile profile;
        }

        private readonly Ctx _ctx;
        private const string VIEW_PREFAB_NAME = "MainHUD";
        private MainHUDView _view;
        private InventoryPm _inventory;
        
        public MainHUDPm(Ctx ctx)
        {
            _ctx = ctx;
            _ctx.resourceLoader.LoadPrefab("fake", VIEW_PREFAB_NAME, OnViewLoaded);
        }

        private void OnViewLoaded(GameObject viewPrefab)
        {
            _view = GameObject.Instantiate(viewPrefab, _ctx.mainCanvas.transform).GetComponent<MainHUDView>();
            ReactiveProperty<bool> open = AddDispose(new ReactiveProperty<bool>(true));
            _view.Init(new MainHUDView.Ctx
            {
                viewDisposables = AddDispose(new CompositeDisposable()),
                inventoryButtonClick = () =>
                {
                    if(_inventory != null)
                        open.Value = true;
                    else
                        CreateInventory(open);
                }
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

        protected override void OnDispose()
        {
            _inventory.Dispose();
            base.OnDispose();
        }
    }
}