# .NET-Web-Authentication-Cookies-Extensions

Additions and extensions for .NET web-authentication-cookies (ASP.NET Core).

[![NuGet](https://img.shields.io/nuget/v/RegionOrebroLan.Web.Authentication.Cookies.svg?label=NuGet)](https://www.nuget.org/packages/RegionOrebroLan.Web.Authentication.Cookies)

## 1 Features

### 1.1 Configurable ITicketStore

**Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions.SessionStore** is of type **Microsoft.AspNetCore.Authentication.Cookies.ITicketStore**. You can implement your own.

This library contains 2:
- [DistributedCacheTicketStore](/Source/Project/DistributedCacheTicketStore.cs)
- [MemoryCacheTicketStore](/Source/Project/MemoryCacheTicketStore.cs)

The ITicketStore is configurable in appsettings.json.

#### 1.1.1 DistributedCacheTicketStore

The DistributedCacheTicketStore uses DataProtection. In a load-balanced environment the DataProtection also have to be configured/setup to support load-balancing.

- [Configurable data-protection](https://github.com/RegionOrebroLan/.NET-DataProtection-Extensions)

## 2 Examples

- [MemoryCacheTicketStore](/Source/Sample/Application/appsettings.MemoryCache-TicketStore.json)
- [Redis-DistributedCacheTicketStore](/Source/Sample/Application/appsettings.Redis-DistributedCache-TicketStore.json) - you need to setup Redis, see below
- [Sqlite-DistributedCacheTicketStore](/Source/Sample/Application/appsettings.Sqlite-DistributedCache-TicketStore.json)
- [SqlServer-DistributedCacheTicketStore](/Source/Sample/Application/appsettings.SqlServer-DistributedCache-TicketStore.json)

### 2.1 Redis

Setup Redis locally with Docker:

	docker run --rm -it -p 6379:6379 redis

## 3 Notes

I have not succeeded using the same database for both the cache and the data-protection when using Sqlite. So in the sample I use two databases, one for the cache and another for the data-protection.

It is possible to use one database for both cache and data-protection when using SqlServer.

- [MemoryCacheTicketStore (dotnet/aspnetcore)](https://github.com/dotnet/aspnetcore/blob/main/src/Security/Authentication/Cookies/samples/CookieSessionSample/MemoryCacheTicketStore.cs)
- [MemoryCacheTicketStore (aspnet/Security)](https://github.com/aspnet/Security/blob/master/samples/CookieSessionSample/MemoryCacheTicketStore.cs)
- [MemoryCacheTicketStore (VahidN/DNTIdentity)](https://github.com/VahidN/DNTIdentity/blob/master/src/ASPNETCoreIdentitySample.Services/Identity/MemoryCacheTicketStore.cs)
- [Storing ASP.NET core identity authorization tickets in Redis](https://mikerussellnz.github.io/.NET-Core-Auth-Ticket-Redis/)