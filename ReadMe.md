# .NET-Web-Authentication-Cookies-Extensions

Additions and extensions for .NET web-authentication-cookies (ASP.NET Core).

[![NuGet](https://img.shields.io/nuget/v/RegionOrebroLan.Web.Authentication.Cookies.svg?label=NuGet)](https://www.nuget.org/packages/RegionOrebroLan.Web.Authentication.Cookies)

## 1 DistributedCacheTicketStore

The DistributedCacheTicketStore uses DataProtection. In a load-balanced environment the DataProtection also have to be configured/setup to support load-balancing.

- [Configurable data-protection](https://github.com/RegionOrebroLan/.NET-DataProtection-Extensions)

## 2 Notes

I have not succeeded using the same database for both the cache and the data-protection when using Sqlite. So in the sample I use two databases, one for the cache and another for the data-protection.

It is possible to use one database for both cache and data-protection when using SqlServer.

## 3 Notes

- [MemoryCacheTicketStore (dotnet/aspnetcore)](https://github.com/dotnet/aspnetcore/blob/main/src/Security/Authentication/Cookies/samples/CookieSessionSample/MemoryCacheTicketStore.cs)
- [MemoryCacheTicketStore (aspnet/Security)](https://github.com/aspnet/Security/blob/master/samples/CookieSessionSample/MemoryCacheTicketStore.cs)
- [MemoryCacheTicketStore (VahidN/DNTIdentity)](https://github.com/VahidN/DNTIdentity/blob/master/src/ASPNETCoreIdentitySample.Services/Identity/MemoryCacheTicketStore.cs)
- [Storing ASP.NET core identity authorization tickets in Redis](https://mikerussellnz.github.io/.NET-Core-Auth-Ticket-Redis/)