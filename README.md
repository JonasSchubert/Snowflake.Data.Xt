# Snowflake.Data.Xt ❄️

Nuget package for fluent snowflake access.

## Usage

```bash
dotnet add package Snowflake.Data.Xt
```

### Example

This package provides multiple attributes and extension methods to try to make it easier querying data from snowflake.\
This is an example for the table `EXAMPLE_COST` joined with `COST_CENTER`.\
It uses a left join. Check the [attribute implementation](./src/Attributes/SnowflakeJoinAttribute.cs) for all join options.

#### `Example.cs`

```c#
using System;
using Snowflake.Data.Xt;

namespace SnowflakeApplication;

// Define the main table to read from. Attribute is only allowed once.
[SnowflakeTable(
  name: "EXAMPLE_COST",     // Name is optional. If not provided, it would be parsed from the class name.
  alias: "cost")]           // Alias has to be set for joins, otherwise is optional.
// Define a table to join from.
[SnowflakeJoin(
  table: "COST_CENTER",              // The table to join from.
  alias: "costCenter",               // The alias to use.
  type: SnowflakeJoinAttribute.Left, // The join type (Cross, FullOuter, Inner (default), Left, LeftOuter, Natural, Right or RightOuter).
  condition: "cost.COST_CENTER_ID = costCenter.ID")] // The join condition, aliases have to be used.
public class Example
{
  [SnowflakeColumn(name: "VALUE")] // Defines the column to map from. Uses the default table if not provided otherwise.
  public decimal Value { get; set; }

  [SnowflakeColumn(name: "ADDRESS", table: "COST_CENTER")] // Uses the joined table.
  public string Address { get; set; }

  [SnowflakeColumn] // If no name is provided, it will be parsed from the property name. Here: IS_READY
  public bool IsReady { get; set; }
}
```

#### `ExamplesRepository.cs`

```c#
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Snowflake.Data.Xt;

namespace SnowflakeApplication;

public class ExamplesRepository : IExamplesRepository
{
  public async Task<IList<Example>> GetByAddressAsync(string address, CancellationToken cancellationToken = default)
  {
    var snowflakeCommand = new SnowflakeDbCommand<Example>() // Creates a default select statement
      .Select(item => item.Value)               // Only select the value
      .Where(item => item.Address == address)   // Filter for the address
      .OrderByAsc(item => item.Value);          // Sort by value ascending

    var list = await snowflakeCommand
      .ToListAsync(                             // Returns a list
        new List<(string, DbType, object)> { }, // Parameter list if required, here empty
        cancellationToken);

    return list;
  }
}
```

#### Modifier

You can add multiple modifiers to you command:

| Name | Parameters | Description |
| --- | --- | --- |
| [`GroupBy`](./src/Command/Modifier/SnowflakeCommand.GroupBy.cs) | `string text` or a predicate | Add a group by clause. e.g. `GROUP BY PLANR` |
| [`Having`](./src/Command/Modifier/SnowflakeCommand.Having.cs) | `string text` | Add a having predicate. e.g. `HAVING count(*) > 10` |
| [`IsDistinct`](./src/Command/Modifier/SnowflakeCommand.IsDistinct.cs) | `-` | The select statement will be distinct. |
| [`OrderBy`](./src/Command/Modifier/SnowflakeCommand.OrderBy.cs) | `string text` or a predicate | Add an order by text. e.g. `PLANR ASC` |
| [`Select`](./src/Command/Modifier/SnowflakeCommand.Select.cs) |  a predicate | Add an order by text. e.g. `PLANR ASC` |
| [`Top`](./src/Command/Modifier/SnowflakeCommand.Top.cs) | `int amount` | The select statement will return the `TOP AMOUNT` found entries. |
| [`Where`](./src/Command/Modifier/SnowflakeCommand.Where.cs) | `string text` or a predicate | Add a where clause text. e.g. `PLANR IS NOT NULL AND PLANR LIKE "%test"` |

#### Methods

You can query for a single entry or a list - sync or async:

| Name | Parameters | Description |
| --- | --- | --- |
| [`FirstOrDefault`](./src/Command/Methods/SnowflakeCommand.FirstOrDefault.cs) | `IList<(string, DbType, object)>? parameterList = default` | Query synchron for one entry. |
| [`FirstOrDefaultAsync`](./src/Command/Methods/SnowflakeCommand.FirstOrDefaultAsync.cs) | `IList<(string, DbType, object)>? parameterList = default, CancellationToken cancellationToken = default` | Query asynchron for one entry. |
| [`ToList`](./src/Command/Methods/SnowflakeCommand.ToList.cs) | `IList<(string, DbType, object)>? parameterList = default` | Query synchron for a list. |
| [`ToListAsync`](./src/Command/Methods/SnowflakeCommand.ToListAsync.cs) | `IList<(string, DbType, object)>? parameterList = default, CancellationToken cancellationToken = default` | Query asynchron for a list. |

### Environment Variables

| Name | Required | Default value | Description |
| --- | --- | --- | --- |
| `SNOWFLAKE_ACCOUNT` | `true` | `null` | The snowflake account. Also check [official docs](https://github.com/snowflakedb/snowflake-connector-net#usage) for .NET connector. |
| `SNOWFLAKE_AUTHENTICATOR` | `true` | `null` | The method of authentication in snowflake. Also check [official docs](https://github.com/snowflakedb/snowflake-connector-net#usage) for .NET connector. Should be `snowflake_jwt`. |
| `SNOWFLAKE_DATABASE` | `false` | `null` | The database to connect to. |
| `SNOWFLAKE_LOG_ENABLED` | `false` | `true` | Whether to log information using serilog or not. |
| `SNOWFLAKE_PRIVATE_KEY_PASSWORD` | `true` | `null` | The password for the private key. Also check [official docs](https://github.com/snowflakedb/snowflake-connector-net#usage) for .NET connector. |
| `SNOWFLAKE_PRIVATE_KEY_FILE` | `true` | `null` | The path to the private key file. Should be the absolute path to the file. Also check [official docs](https://github.com/snowflakedb/snowflake-connector-net#usage) for .NET connector. |
| `SNOWFLAKE_SCHEMA` | `false` | `null` | The schema to use. |
| `SNOWFLAKE_USER` | `true` | `null` | The user for the snowflake login. Also check [official docs](https://github.com/snowflakedb/snowflake-connector-net#usage) for .NET connector. |
| `SNOWFLAKE_WAREHOUSE` | `false` | `null` | The warehouse to use. |

## License

`Snowflake.Data.Xt` is distributed under the MIT license. [See LICENSE](LICENSE) for details.

```
MIT License

Copyright (c) 2023 Jonas Schubert

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```
