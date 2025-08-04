using System;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;

namespace TesteNubank
{
    public class CalculatorCapitalTests
    {
        private readonly CapitalCalculator _calculator = new();

        [Fact]
        public void TestCase1()
        {
            var operations = new List<Operation>
        {
            new() { OperationType = "buy", UnitCost = 10.00m, Quantity = 100 },
            new() { OperationType = "sell", UnitCost = 15.00m, Quantity = 50 },
            new() { OperationType = "sell", UnitCost = 15.00m, Quantity = 50 }
        };

            var taxes = _calculator.CalculateTaxes(operations);
            Assert.Equal(new List<decimal> { 0, 0, 0 }, taxes);
        }

        [Fact]
        public void TestCase2()
        {
            var operations = new List<Operation>
        {
            new() { OperationType = "buy", UnitCost = 10.00m, Quantity = 10000 },
            new() { OperationType = "sell", UnitCost = 20.00m, Quantity = 5000 },
            new() { OperationType = "sell", UnitCost = 5.00m, Quantity = 5000 }
        };

            var taxes = _calculator.CalculateTaxes(operations);
            Assert.Equal(new List<decimal> { 0, 10000, 0 }, taxes);
        }

        [Fact]
        public void TestCase3()
        {
            var operations = new List<Operation>
        {
            new() { OperationType = "buy", UnitCost = 10.00m, Quantity = 10000 },
            new() { OperationType = "sell", UnitCost = 5.00m, Quantity = 5000 },
            new() { OperationType = "sell", UnitCost = 20.00m, Quantity = 3000 }
        };

            var taxes = _calculator.CalculateTaxes(operations);
            Assert.Equal(new List<decimal> { 0, 0, 1000 }, taxes);
        }

        [Fact]
        public void TestCase6()
        {
            var operations = new List<Operation>
        {
            new() { OperationType = "buy", UnitCost = 10.00m, Quantity = 10000 },
            new() { OperationType = "sell", UnitCost = 2.00m, Quantity = 5000 },
            new() { OperationType = "sell", UnitCost = 20.00m, Quantity = 2000 },
            new() { OperationType = "sell", UnitCost = 20.00m, Quantity = 2000 },
            new() { OperationType = "sell", UnitCost = 25.00m, Quantity = 1000 }
        };

            var taxes = _calculator.CalculateTaxes(operations);
            Assert.Equal(new List<decimal> { 0, 0, 0, 0, 3000 }, taxes);
        }

        [Fact]
        public void TestWeightedAverageCalculation()
        {
            var operations = new List<Operation>
        {
            new() { OperationType = "buy", UnitCost = 10.00m, Quantity = 10 },
            new() { OperationType = "buy", UnitCost = 5.00m, Quantity = 5 }
        };

            var taxes = _calculator.CalculateTaxes(operations);
            var state = new PortfolioState { wAveragePrice = 8.33m, TotalQuantity = 15 };

            Assert.Equal(0, taxes.Sum());
        }
    }

}
