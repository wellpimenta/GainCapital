# Solution for Capital Gains

This solution implements a command-line program in C# to calculate taxes on stock buy and sell operations, following the specified rules.

## Technical Decisions

1. **Code Structure**:
   - Clear separation between models, business logic, and interface
   - Use of simple classes to represent operations and portfolio state

2. **Libraries**:
   - `System.Text.Json` for JSON serialization/deserialization (.NET standard)
   - `xunit` for unit testing

3. **Rounding**:
   - All values are rounded to 2 decimal places as specified

4. **State Handling**:
   - Each input line is processed independently
   - The portfolio state (weighted average, quantity, accumulated losses) is maintained during the processing of a line

## How to Run

1. **Prerequisites**:
   - .NET SDK 6.0 or higher

2. **Build and Run**:
   ```bash
   dotnet build
   dotnet run