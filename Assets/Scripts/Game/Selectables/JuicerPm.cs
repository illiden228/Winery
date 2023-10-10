using Core;
using Data;
using Factories;
using Game.Character;
using Game.Player;
using System;
using UnityEngine;
using Tools.Extensions;

namespace Game.Selectables
{
    public class JuicerPm : ProdutionGenerator
    {
        public struct Ctx
        {
            public ItemDataFactory itemDataFactory;
            public Inventory inventory;
        }

        public enum JuicerState
        {
            Empty,
            InProcess,
            Ready
        }

        private readonly Ctx _ctx;
        private JuicerState _currentJuicerState;
        private IDisposable _juicerProductionCallDisposable;
        private GrapeData _grapeData;
        private JuicerView _view;

        public JuicerPm(Ctx ctx)
        {
            _ctx = ctx;
        }

        public void InitView(JuicerView view)
        {
            _view = view;
            _view.Init(new JuicerView.Ctx
            {
                onSelect = OnSelect,
                getStatus = OnGetSelectStatus
            });
        }
        
        public override void StartGeneration(Item to, Item from = null)
        {
            if (_view == null)
            {
                Debug.LogError("View is missing");
                return;
            }
        }

        private SelectableStatus OnGetSelectStatus()
        {
            switch (_currentJuicerState)
            {
                case JuicerState.Empty:
                    {
                        return new SelectableStatus
                        {
                            NeedSelector = true,
                            RequiredTypeForSelector = typeof(GrapeData),
                            AnimationTriggerName = CharacterAnimation.Triggers.Take
                        };
                    }
                case JuicerState.InProcess:
                    {

                        break;
                    }
                case JuicerState.Ready:
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
            switch (_currentJuicerState)
            {
                case JuicerState.Empty:
                    {
                        if (_juicerProductionCallDisposable !=null)
                            _juicerProductionCallDisposable.Dispose();
                        
                        _ctx.inventory.RemoveFromInventory(item, 1);

                        _currentJuicerState = JuicerState.InProcess;

                        _grapeData = (item as GrapeData);

                        Debug.Log($"Соковыжималка начала производство сока: {item.Name}");

                        _juicerProductionCallDisposable = ReactiveExtensions.DelayedCall(_grapeData.ProductionTime, () =>
                        {
                            Debug.Log($"Соковыжималка произвела: {item.Name}");

                            if (_juicerProductionCallDisposable != null)
                                _juicerProductionCallDisposable.Dispose();

                            _currentJuicerState = JuicerState.Ready;
                        });
                        SoundManager.Instance.PlaySimpleButton();
                        break;
                    }
                case JuicerState.InProcess:
                    {

                        break;
                    }
                case JuicerState.Ready:
                    {
                        SoundManager.Instance.PlayJuice();
                        TransferJuiceToInventory();
                        break;
                    }
            }
        }

        private void TransferJuiceToInventory()
        {
            Debug.Log($"В инвентарь добавлено: {_grapeData.Production.Name} 1 шт.");
            _ctx.inventory.AddItemToInventory(_ctx.itemDataFactory.CreateObject(_grapeData.Production), _grapeData.ProductionCount);
            _currentJuicerState = JuicerState.Empty;
        }
    }
}