using System;
using Core;
using Data;
using Game.Character;
using Game.Player;
using Game.Purchasing;
using Tools.Extensions;
using UnityEngine;

namespace Game.Selectables
{
    public class CarPm : ProdutionGenerator
    {
        public struct Ctx
        {
            public Inventory inventory;
            public ReactiveEvent<Purchase> purchaseEvent;
        }

        private readonly Ctx _ctx;
        private CarView _view;
        private ProductionState _currentState;
        private IDisposable _barrleProductionCallDisposable;
        private WineData _wineData;

        public CarPm(Ctx ctx)
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

        public void InitView(CarView view)
        {
            _view = view;
            _view.Init(new CarView.Ctx
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
                        RequiredTypeForSelector = typeof(WineData),
                        AnimationTriggerName = CharacterAnimation.Triggers.Take
                    };
                }
                case ProductionState.InProcess:
                {
                    break;
                }
                case ProductionState.Ready:
                {
                    break;
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
                    if (_barrleProductionCallDisposable != null)
                        _barrleProductionCallDisposable.Dispose();

                    _ctx.inventory.RemoveFromInventory(item);

                    _currentState = ProductionState.InProcess;

                    _wineData = (item as WineData);

                    Debug.Log($"Бочка начала производство вина: {item.Name}");

                    _barrleProductionCallDisposable = ReactiveExtensions.DelayedCall(_wineData.ProductionTime, () =>
                    {
                        Debug.Log($"Бочка произвела: {item.Name}");

                        if (_barrleProductionCallDisposable != null)
                            _barrleProductionCallDisposable.Dispose();

                        _currentState = ProductionState.Ready;
                        
                        
                        _ctx.purchaseEvent.Notify(new Purchase
                        {
                            Item = item,
                            PurchaseType = PurchaseType.Sell,
                            Callback = () =>
                            {
                                Debug.Log($"Денюжки получены в размере {item.Cost * item.Count}");
                                _currentState = ProductionState.Empty;
                            }
                        });
                    });
                    break;
                }
                case ProductionState.InProcess:
                {
                    break;
                }
                case ProductionState.Ready:
                {
                    
                    break;
                }
            }
        }
    }
}