using Connectors.Interfaces;
using DataLayer.Interfaces;
using DataLayer.Models.Instruments;
using DataLayer.Models.Strategies.Base;
using System;
using System.Collections.Generic;

namespace DataLayer.Models.Strategies;

public class OptionStrategy : BaseStrategy, IOrderHolder
{
    #region Props

    #region PublicProps
    #region DB
    #region Instrument
    public int? OptionId { get; set; }
    public DbOption Option { get; set; }
    #endregion
    #region Straddle
    public int? LongStraddleId { get; set; }

    public LongStraddle LongStraddle { get; set; }
    #endregion
    #region Orders
    public List<DbOrder> StrategyOrders { get; set; } = new();
    #endregion
    #endregion
    #endregion

    #region _privateProps
    private DbOrder? _openOrder;
    private IRepository<OptionStrategy>? _repository;
    private IRepository<DbOrder>? _orderRepository;

    #endregion

    #endregion

    #region Methods

    #region PublicMethods

    public void Start(IConnector connector, IRepository<OptionStrategy> repository, IRepository<DbOrder> orderRepository)
    {
        _repository = repository;
        _orderRepository = orderRepository;

        if (Option != null)
            connector.CacheOption(Option);
    }

    public void Work(string account)
    {
        switch (StrategyLogic)
        {
            case Enums.StrategyLogic.OpenPoition when _openOrder == null:
                openPositionLogic(account);
                break;
            case Enums.StrategyLogic.ClosePostion:
                break;
            case Enums.StrategyLogic.Done:
                break;
        }
    }

    public void OnOrderFilled(int orderId)
    {
        if (_openOrder == null) return;
        if (_openOrder.OrderId != orderId) return;

        Position = _openOrder.FilledQuantity;

        if (_repository != null)
        {
            _repository.UpdateAsync(this);
        }

        if (_orderRepository != null && _openOrder != null)
        {
            _orderRepository.CreateAsync(_openOrder);
        }
        _openOrder = null;
        /*
         * необходимо сохранять изменения стратегии и 
         * ордера.
         */
    }

    public void OnCanceled(int orderId)
    {
        if (_openOrder == null) return;
        if (_openOrder.OrderId != orderId) return;
        _openOrder = null;
    }

    public void onFilledQunatityChanged(int orderId)
    {
        //throw new NotImplementedException();
    }

    #endregion

    #region privateMethods
    private void openPositionLogic(string account)
    {
        if (Volume > Math.Abs(Position))
        {
            if (Option != null)
            {
                _openOrder = new DbOrder
                {
                    Direction = Direction,
                    Account = account,
                    TotalQuantity = Volume - Math.Abs(Position),
                    OptionStrategyId = Id,
                };
                if (!Option.SendOrder(_openOrder, this))
                {
                    _openOrder = null;
                }
            }
        }
    }
    #endregion

    #endregion
}