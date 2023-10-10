using System;
using Core;
using Data;
using Factories;
using Game.Character;
using Game.Player;
using Tools.Extensions;
using UnityEngine;

namespace Game.Selectables
{
    public class BarrelPm : ProdutionGenerator
    {
        public struct Ctx
        {
            public ItemDataFactory itemDataFactory;
            public Inventory inventory;
        }

        private readonly Ctx _ctx;
        private BarrelView _view;
        private ProductionState _currentState;
        private IDisposable _barrleProductionCallDisposable;
        private JuiceData _juiceData;

        public BarrelPm(Ctx ctx)
        {
            _ctx = ctx;
        }

        public override void StartGeneration(Item to, Item from = null)
        {
            if (_view == null)
            {
                Debug.LogError("View is missing");
                return;
            }
        }

        public void InitView(BarrelView view)
        {
            _view = view;
            _view.Init(new BarrelView.Ctx
            {
                onSelect = OnSelect,
                getStatus = OnGetSelectStatus
            });
        }
        
        private SelectableStatus OnGetSelectStatus()
        {
            switch (_currentState)
            {
                case ProductionState.Empty:
                    {
                        return new SelectableStatus
                        {
                            NeedSelector = true,
                            RequiredTypeForSelector = typeof(JuiceData),
                            AnimationTriggerName = CharacterAnimation.Triggers.Take
                        };
                    }
                case ProductionState.InProcess:
                    {

                        break;
                    }
                case ProductionState.Ready:
                    {
                        return new SelectableStatus
                        {
                            NeedSelector = false,
                            AnimationTriggerName = CharacterAnimation.Triggers.Collect
                        };
                    }
            }

            return null;
        }

        private void OnSelect(Item item)
        {
            switch (_currentState)
            {
                case ProductionState.Empty:
                    {
                        if (_barrleProductionCallDisposable !=null)
                            _barrleProductionCallDisposable.Dispose();
                        
                        _ctx.inventory.RemoveFromInventory(item, 1);

                        _currentState = ProductionState.InProcess;

                        _juiceData = (item as JuiceData);

                        Debug.Log($"Бочка начала производство вина: {item.Name}");

                        _barrleProductionCallDisposable = ReactiveExtensions.DelayedCall(_juiceData.ProductionTime, () =>
                        {
                            Debug.Log($"Бочка произвела: {item.Name}");

                            if (_barrleProductionCallDisposable != null)
                                _barrleProductionCallDisposable.Dispose();

                            _currentState = ProductionState.Ready;
                        });

                        SoundManager.Instance.PlayBarrel();

                        break;
                    }
                case ProductionState.InProcess:
                    {

                        break;
                    }
                case ProductionState.Ready:
                    {
                        SoundManager.Instance.PlayBarrel();

                        TransferWineToInventory();
                        break;
                    }
            }
        }

        private void TransferWineToInventory()
        {
            Debug.Log($"В инвентарь добавлено: {_juiceData.Production.Name} 1 шт.");
            _ctx.inventory.AddItemToInventory(_ctx.itemDataFactory.CreateObject(_juiceData.Production), _juiceData.ProductionCount);
            _currentState = ProductionState.Empty;
        }
    }
}