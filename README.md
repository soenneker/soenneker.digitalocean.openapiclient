[![](https://img.shields.io/nuget/v/soenneker.digitalocean.openapiclient.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.digitalocean.openapiclient/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.digitalocean.openapiclient/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.digitalocean.openapiclient/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.digitalocean.openapiclient.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.digitalocean.openapiclient/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.digitalocean.openapiclient/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.digitalocean.openapiclient/actions/workflows/codeql.yml)

# Soenneker.DigitalOcean.OpenApiClient

A Kiota-generated .NET client for DigitalOcean’s REST API.

## Installation

```bash
dotnet add package Soenneker.DigitalOcean.OpenApiClient
```

## Create a client

```csharp
using System.Net.Http.Headers;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Soenneker.DigitalOcean.OpenApiClient;

var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", accessToken);

var adapter = new HttpClientRequestAdapter(
    new AnonymousAuthenticationProvider(),
    httpClient: httpClient)
{
    BaseUrl = "https://api.digitalocean.com"
};

var client = new DigitalOceanOpenApiClient(adapter);
```

Keep the token outside source control and reuse the `HttpClient`, adapter, and generated client instead of constructing them for every request. The companion `Soenneker.DigitalOcean.OpenApiClientUtil` package provides dependency-injection registration and cached construction when preferred.

## List droplets

```csharp
using Soenneker.DigitalOcean.OpenApiClient.Models;

AllDropletsResponse? response = await client.V2.Droplets.GetAsync(
    request =>
    {
        request.QueryParameters.Page = 1;
        request.QueryParameters.PerPage = 50;
    },
    cancellationToken);

IEnumerable<Droplet> droplets = response?.Droplets ?? [];
```

Pagination is explicit. Read the response metadata/links and request subsequent pages as needed; the client does not automatically enumerate every page.

## Errors and generated API shape

DigitalOcean error responses are mapped to `Soenneker.DigitalOcean.OpenApiClient.Models.Error`, which derives from Kiota’s `ApiException`. Inspect its `Id`, `Message`, and `RequestId` where available, and handle rate limiting (`429`) according to your application’s retry policy. Cancellation tokens are passed through to the request adapter.

Request builders follow the URL hierarchy, for example `client.V2.Droplets`, `client.V2.Domains`, and `client.V2.Kubernetes`. Collection indexers address item routes.

The client and model files are generated. Avoid editing them directly because regeneration replaces those changes. Endpoint names, nullability, and model shapes can change when DigitalOcean’s specification changes; review package updates before upgrading production consumers.
