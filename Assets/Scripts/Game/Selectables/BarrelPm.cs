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
        
        public enum BarrelState
        {
            Empty,
            InProcess,
            Ready
        }

        private readonly Ctx _ctx;
        private BarrelView _view;
        private BarrelState _currentBarrelState;
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
            switch (_currentBarrelState)
            {
                case BarrelState.Empty:
                    {
                        return new SelectableStatus
                        {
                            NeedSelector = true,
                            RequiredTypeForSelector = typeof(JuiceData),
                            AnimationTriggerName = CharacterAnimation.Triggers.Take
                        };
                    }
                case BarrelState.InProcess:
                    {

                        break;
                    }
                case BarrelState.Ready:
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
            switch (_currentBarrelState)
            {
                case BarrelState.Empty:
                    {
                        if (_barrleProductionCallDisposable !=null)
                            _barrleProductionCallDisposable.Dispose();
                        
                        _ctx.inventory.RemoveFromInventory(item);

                        _currentBarrelState = BarrelState.InProcess;

                        _juiceData = (item as JuiceData);

                        Debug.Log($"Бочка начала производство вина: {item.Name}");

                        _barrleProductionCallDisposable = ReactiveExtensions.DelayedCall(_juiceData.ProductionTime, () =>
                        {
                            Debug.Log($"Бочка произвела: {item.Name}");

                            if (_barrleProductionCallDisposable != null)
                                _barrleProductionCallDisposable.Dispose();

                            _currentBarrelState = BarrelState.Ready;
                        });
                        break;
                    }
                case BarrelState.InProcess:
                    {

                        break;
                    }
                case BarrelState.Ready:
                    {
                        TransferJuiceToInventory();
                        break;
                    }
            }
        }

        private void TransferJuiceToInventory()
        {
            Debug.Log($"В инвентарь добавлено: {_juiceData.Production.Name} 1 шт.");
            _ctx.inventory.AddItemToInventory(_ctx.itemDataFactory.CreateObject(_juiceData.Production));
            _currentBarrelState = BarrelState.Empty;
        }
    }
}