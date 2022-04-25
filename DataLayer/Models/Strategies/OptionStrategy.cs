using Connectors.Enums;
using Connectors.Interfaces;
using DataLayer.Enums;
using DataLayer.Interfaces;
using DataLayer.Models.Instruments;
using DataLayer.Models.Strategies.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DataLayer.Models.Strategies;

public class OptionStrategy : BaseStrategy, IOrderHolder
{
    #region Props

    #region PublicProps

    #region DB

    #region Instrument

    public int OptionId { get; set; }
    public DbOption Option { get; set; }

    #endregion

    #region Straddle

    public int LongStraddleId { get; set; }
    public LongStraddle LongStraddle { get; set; }

    #endregion

    #region Orders

    public List<DbOrder> StrategyOrders { get; set; } = new();

    #region StrategyLogic

    private StrategyLogic _strategyLogic;
    public override StrategyLogic StrategyLogic
    {
        get => _strategyLogic;
        set
        {
            _strategyLogic = value;
            if (_repository != null)
            {
                _repository.UpdateAsync(this);
            }
            if (_openOrder != null && Option != null)
            {
                Option.CancelOrder(_openOrder.OrderId);
            }
        }
    }

    #endregion

    [NotMapped]
    public Direction CloseDirection => Direction == Direction.Buy ? Direction.Sell : Direction.Buy;

    #endregion

    #endregion

    [NotMapped]
    public decimal PnL => StrategyOrders
        .Sum(o => o.Direction == Direction.Buy ?
            -o.FilledQuantity * o.AvgFilledPrice :
            o.FilledQuantity * o.AvgFilledPrice);

    [NotMapped]
    public decimal Commission => StrategyOrders.Sum(o => o.Commission);
    [NotMapped]
    public decimal PnlInCurrency => Option == null ? 0m : PnL * Option.Multiplier;
    [NotMapped]
    public decimal UnrealizedPnlInCurrency
    {
        get
        {
            if (Option == null) return 0m;
            if (Option.TheorPrice == 0) return 0m;
            if (Position > 0) return (PnL + Option.TheorPrice) * Option.Multiplier;
            if (Position < 0) return (PnL - Option.TheorPrice) * Option.Multiplier;
            return 0m;
        }
    }
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
            case StrategyLogic.OpenPoition when _openOrder == null:
                openPositionLogic(account);
                break;
            case StrategyLogic.ClosePostion when _openOrder == null:
                closePositionLogic(account);
                break;
            case StrategyLogic.Done:
                break;
        }
    }
    public void OnOrderFilled(int orderId)
    {
        if (_openOrder == null) return;
        if (_openOrder.OrderId != orderId) return;

        _openOrder.ExecuteTime = DateTime.Now;

        if (_openOrder.Direction == Direction)
            Position = _openOrder.FilledQuantity;
        else if (_openOrder.Direction == CloseDirection)
            Position -= _openOrder.FilledQuantity;

        if (_repository != null)
        {
            _repository.UpdateAsync(this);
        }

        if (_orderRepository != null && _openOrder != null)
        {
            if (StrategyOrders.Contains(_openOrder))
            {
                _orderRepository.UpdateAsync(_openOrder).Wait();
            }
            else
            {
                _orderRepository.CreateAsync(_openOrder).Wait();
                StrategyOrders.Add(_openOrder);
            }
        }

        _openOrder = null;
    }
    public void Stop()
    {
        if (Option == null) return;
        if (_openOrder == null) return;

        Option.CancelOrder(_openOrder.Id);
    }
    public void OnCanceled(int orderId)
    {
        if (_openOrder == null) return;
        if (_openOrder.OrderId != orderId) return;
        if (_orderRepository != null)
        {
            if (StrategyOrders.Contains(_openOrder))
                _orderRepository.UpdateAsync(_openOrder).Wait();
            else
            {
                _orderRepository.CreateAsync(_openOrder).Wait();
                StrategyOrders.Add(_openOrder);
            }
        }
        _openOrder = null;
    }
    public void OnSubmit(int orderId)
    {
        if (_openOrder == null) return;
        if (_openOrder.OrderId != orderId) return;
        if (_orderRepository != null)
        {
            _orderRepository.CreateAsync(_openOrder).Wait();
            if (!StrategyOrders.Contains(_openOrder))
                StrategyOrders.Add(_openOrder);
        }
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
                    GeneratedTime = DateTime.Now
                };
                if (!Option.SendOrder(_openOrder, this))
                {
                    _openOrder = null;
                }
            }
        }
    }
    private void closePositionLogic(string account)
	{
        if (Position != 0)
		{
            if (Option != null)
			{
                _openOrder = new DbOrder
                {
                    Direction = CloseDirection,
                    Account = account,
                    TotalQuantity = Math.Abs(Position),
                    OptionStrategyId = Id,
                    GeneratedTime = DateTime.Now
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