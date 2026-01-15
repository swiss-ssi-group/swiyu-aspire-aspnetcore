# swiyu using ASP.NET Core and Microsoft Aspire

Basic setup to issue and verify swiyu credentials using the swiyu public beta, ASP.NET Core and Aspire. 

[![.NET](https://github.com/swiss-ssi-group/swiyu-aspire-aspnetcore/actions/workflows/dotnet.yml/badge.svg)](https://github.com/swiss-ssi-group/swiyu-aspire-aspnetcore/actions/workflows/dotnet.yml) [![SonarCloud](https://github.com/damienbod/EndToEndSecurity/actions/workflows/sonarbuild.yml/badge.svg)](https://github.com/swiss-ssi-group/swiyu-aspire-aspnetcore/actions/workflows/sonarbuild.yml)

[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=swiss-ssi-group_swiyu-aspire-aspnetcore&metric=bugs)](https://sonarcloud.io/summary/overall?id=swiss-ssi-group_swiyu-aspire-aspnetcore)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=swiss-ssi-group_swiyu-aspire-aspnetcore&metric=code_smells)](https://sonarcloud.io/summary/overall?id=swiss-ssi-group_swiyu-aspire-aspnetcore)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=swiss-ssi-group_swiyu-aspire-aspnetcore&metric=duplicated_lines_density)](https://sonarcloud.io/summary/overall?id=swiss-ssi-group_swiyu-aspire-aspnetcore)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=swiss-ssi-group_swiyu-aspire-aspnetcore&metric=reliability_rating)](https://sonarcloud.io/summary/overall?id=swiss-ssi-group_swiyu-aspire-aspnetcore)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=swiss-ssi-group_swiyu-aspire-aspnetcore&metric=security_rating)](https://sonarcloud.io/summary/overall?id=swiss-ssi-group_swiyu-aspire-aspnetcore)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=swiss-ssi-group_swiyu-aspire-aspnetcore&metric=sqale_index)](https://sonarcloud.io/summary/overall?id=swiss-ssi-group_swiyu-aspire-aspnetcore)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=swiss-ssi-group_swiyu-aspire-aspnetcore&metric=sqale_rating)](https://sonarcloud.io/summary/overall?id=swiss-ssi-group_swiyu-aspire-aspnetcore)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=swiss-ssi-group_swiyu-aspire-aspnetcore&metric=vulnerabilities)](https://sonarcloud.io/summary/overall?id=swiss-ssi-group_swiyu-aspire-aspnetcore)

[Demo](https://swiyu-mgmt.redpebble-62dbc6b1.switzerlandnorth.azurecontainerapps.io/)

## Blogs

- [Issue and verify credentials using the Swiss Digital identity public beta, ASP.NET Core and .NET Aspire](https://damienbod.com/2025/08/04/issuer-and-verify-credentials-using-the-swiss-digital-identity-public-beta-asp-net-core-and-net-aspire/)
- [Use swiyu, the Swiss E-ID to authenticate users with Duende and .NET Aspire](https://damienbod.com/2025/10/27/use-swiyu-the-swiss-e-id-to-authenticate-users-with-duende-and-net-aspire/)
- [Implement MFA using swiyu, the Swiss E-ID with Duende IdentityServer, ASP.NET Core Identity and .NET Aspire](https://damienbod.com/2025/11/10/implement-mfa-using-swiyu-the-swiss-e-id-with-duende-identityserver-asp-net-core-identity-and-net-aspire/)
- [Implement forgot your password using swiyu, ASP.NET Core Identity and Aspire](https://damienbod.com/2025/11/17/implement-forgot-your-password-using-swiyu-asp-net-core-identity-and-aspire/)

## Architecture overview

![Architecture](https://github.com/swiss-ssi-group/swiyu-aspire-aspnetcore/blob/main/images/overview.drawio.png)

## Getting started:

- [swiyu](https://swiyu-admin-ch.github.io/cookbooks/onboarding-base-and-trust-registry/)
- [Dev docs](DEV.md)
- [Changelog](CHANGELOG.md)

## Podman

https://podman-desktop.io/docs/troubleshooting/troubleshooting-podman

```bash
podman machine start
```

## ef DB Migrations
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Used OSS packages, containers, repositories 

- ImageMagick: https://github.com/manuelbl/QrCodeGenerator/tree/master/Demo-ImageMagick
- Microsoft Aspire: https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview
- Net.Codecrete.QrCodeGenerator: https://github.com/manuelbl/QrCodeGenerator/
- swiyu
  - ghcr.io/swiyu-admin-ch/swiyu-issuer
  - ghcr.io/swiyu-admin-ch/swiyu-verifier

## Swagger

https://swiyu-issuer.redpebble-62dbc6b1.switzerlandnorth.azurecontainerapps.io/swagger-ui/index.html
https://swiyu-verifier.redpebble-62dbc6b1.switzerlandnorth.azurecontainerapps.io/swagger-ui/index.html


## Links

https://swiyu-admin-ch.github.io/

https://www.eid.admin.ch/en/public-beta-e

https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview

https://www.npmjs.com/package/ngrok

https://swiyu-admin-ch.github.io/specifications/interoperability-profile/

https://andrewlock.net/converting-a-docker-compose-file-to-aspire/

https://swiyu-admin-ch.github.io/cookbooks/onboarding-generic-verifier/

https://github.com/orgs/swiyu-admin-ch/projects/2/views/2

## Standards

https://identity.foundation/trustdidweb/

https://openid.net/specs/openid-4-verifiable-credential-issuance-1_0.html

https://openid.net/specs/openid-4-verifiable-presentations-1_0.html

https://datatracker.ietf.org/doc/draft-ietf-oauth-selective-disclosure-jwt/

https://datatracker.ietf.org/doc/draft-ietf-oauth-sd-jwt-vc/

https://datatracker.ietf.org/doc/draft-ietf-oauth-status-list/

https://www.w3.org/TR/vc-data-model-2.0/
