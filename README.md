# Yinyang Utilities Oracle

[![Yinyang Utilities Oracle](https://img.shields.io/nuget/v/Yinyang.Utilities.Oracle.svg)](https://www.nuget.org/packages/Yinyang.Utilities.Oracle/) [![Yinyang Utilities Oracle Core](https://img.shields.io/nuget/v/Yinyang.Utilities.Oracle.Core.svg)](https://www.nuget.org/packages/Yinyang.Utilities.Oracle.Core/)

Oracle Connection Utility for C# .NET.

C#用Oracle接続ユーティリティです。

---

## Getting started

Install Yinyang Utilities Oracle nuget package.

NuGet パッケージ マネージャーからインストールしてください。

- [.Net Framework](https://www.nuget.org/packages/Yinyang.Utilities.Oracle/)

> ```powershell
> Install-Package Yinyang.Utilities.Oracle
> ```

- [.Net](https://www.nuget.org/packages/Yinyang.Utilities.Oracle.Core/)

> ```powershell
> Install-Package Yinyang.Utilities.Oracle.Core
> ```

---

## Basic Usage

```c#
// Init
using var db = new OracleDatabase(ConnectionString);

// Database Open
db.Open();

// Transaction Start
db.BeginTransaction();

// SQL
db.CommandText = "INSERT INTO test2 VALUES(:id, :value)";

// Add Parameter
db.AddParameter(":id", 1);
db.AddParameter(":value", "abcdefg");

// Execute
if (1 != db.ExecuteNonQuery())
{
    // Transaction Rollback
    db.Rollback();
    return;
}

// Command and Parameter Reset
db.Refresh();

// SQL
db.CommandText = "select * from test2 where id = :id";

// Add Parameter
db.AddParameter(":id", 1);

// Execute
var result = db.ExecuteReaderFirst<Entity>();

if (null == result)
{
    db.Rollback();
    return;
}

// Transaction Commit
db.Commit();

// Database Close
db.Close();


```

