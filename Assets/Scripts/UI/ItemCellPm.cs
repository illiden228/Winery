using Core;
using Data;
using Tools;
using UnityEngine;

namespace UI
{
    public class ItemCellPm : BaseDisposable
    {
        public struct Ctx
        {
            public IResourceLoader resourceLoader;
            public Transform container;
            public SeedlingData plantData;
        }

        private readonly Ctx _ctx;
        private ItemCellView _view;
        private const string VIEW_PREFAB_NAME = "ItemCellView";

        public ItemCellPm(Ctx ctx)
        {
            _ctx = ctx;
            _ctx.resourceLoader.LoadPrefab("fake", VIEW_PREFAB_NAME, OnViewLoaded);
        }

        private void OnViewLoaded(GameObject viewPrefab)
        {
            _view = GameObject.Instantiate(viewPrefab, _ctx.container).GetComponent<ItemCellView>();
            _view.Init(new ItemCellView.Ctx
            {
                background = _ctx.plantData.Plant.Icon,
                name = _ctx.plantData.Plant.Name
            });
        }
    }
}