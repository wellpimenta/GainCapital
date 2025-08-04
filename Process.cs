using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace TesteNubank
{

    public class Operation
    {
        public string OperationType { get; set; }
        public decimal UnitCost { get; set; }
        public int Quantity { get; set; }
    }

    public class TaxResult
    {
        public decimal Tax { get; set; }
    }

    public class PortfolioState
    {
        public decimal wAveragePrice { get; set; }
        public int TotalQuantity { get; set; }
        public decimal AccumulatedLoss { get; set; }
    }

    public class CapitalCalculator
    {
        public List<decimal> CalculateTaxes(List<Operation> operations)
        {
            var taxes = new List<decimal>();
            var state = new PortfolioState();

            foreach (var op in operations)
            {
                decimal tax = 0;

                if (op.OperationType == "buy")
                {
                    state = ProcessBuyOperation(state, op);
                }
                else if (op.OperationType == "sell")
                {
                    (tax, state) = ProcessSellOperation(state, op);
                }

                taxes.Add(tax);
            }

            return taxes;
        }

        private PortfolioState ProcessBuyOperation(PortfolioState currentState, Operation operation)
        {
            if (currentState.TotalQuantity == 0)
            {
                return new PortfolioState
                {
                    wAveragePrice = operation.UnitCost,
                    TotalQuantity = operation.Quantity,
                    AccumulatedLoss = currentState.AccumulatedLoss
                };
            }

            decimal newAverage = ((currentState.TotalQuantity * currentState.wAveragePrice) +
                                (operation.Quantity * operation.UnitCost)) /
                                (currentState.TotalQuantity + operation.Quantity);

            return new PortfolioState
            {
                wAveragePrice = Math.Round(newAverage, 2),
                TotalQuantity = currentState.TotalQuantity + operation.Quantity,
                AccumulatedLoss = currentState.AccumulatedLoss
            };
        }

        private (decimal tax, PortfolioState newState) ProcessSellOperation(PortfolioState currentState, Operation operation)
        {
            decimal totalOperationValue = operation.UnitCost * operation.Quantity;
            decimal tax = 0;

            if (totalOperationValue <= 20000)
            {
                var loss = CalculateLoss(currentState, operation);
                return (0, new PortfolioState
                {
                    wAveragePrice = currentState.wAveragePrice,
                    TotalQuantity = currentState.TotalQuantity - operation.Quantity,
                    AccumulatedLoss = currentState.AccumulatedLoss + loss
                });
            }

            decimal profit = (operation.UnitCost - currentState.wAveragePrice) * operation.Quantity;
            decimal profitAfterLossDeduction = profit - currentState.AccumulatedLoss;

            if (profitAfterLossDeduction > 0)
            {
                tax = Math.Round(profitAfterLossDeduction * 0.2m, 2);
            }

            var newAccumulatedLoss = profit < 0 ?
                currentState.AccumulatedLoss - profit :
                Math.Max(0, currentState.AccumulatedLoss - profit);

            return (tax, new PortfolioState
            {
                wAveragePrice = currentState.wAveragePrice,
                TotalQuantity = currentState.TotalQuantity - operation.Quantity,
                AccumulatedLoss = newAccumulatedLoss
            });
        }

        private decimal CalculateLoss(PortfolioState state, Operation operation)
        {
            if (operation.UnitCost < state.wAveragePrice)
                return (state.wAveragePrice - operation.UnitCost) * operation.Quantity;
            return 0;
        }
    }
}