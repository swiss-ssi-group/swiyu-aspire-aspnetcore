# swiyu using ASP.NET Core and Microsoft Aspire

Getting started:

https://swiyu-admin-ch.github.io/cookbooks/onboarding-base-and-trust-registry/

## Architecture overview

![Architecture](https://github.com/swiss-ssi-group/swiyu-aspire-aspnetcore/blob/main/images/overview.drawio.png)

## Getting started Aspire and swiyu

Aspire is used to develop and implement the applications. Four container applications provided by swiyu are used to implement the swiyu logic. The end user application uses the provided APIs.

### swiyu containers

- ghcr.io/swiyu-admin-ch/eidch-issuer-agent-oid4vci
- ghcr.io/swiyu-admin-ch/eidch-verifier-agent-oid4vp
- ghcr.io/swiyu-admin-ch/eidch-verifier-agent-management
- ghcr.io/swiyu-admin-ch/eidch-issuer-agent-management

### public configuration files for this demo

https://github.com/swiss-ssi-group/swiyu-config-files

## Database

The postgres database for development is deployed to Azure and is a public database with a Firewall. The local dev machines need to be enabled in the firewall. A public DB is used so that the application can be tested without the need of ngrok or other such tools.

## swiyu

Support: usertesting@swiyu.admin.ch

## Configuration data

```json
{
  //"client_id": "${VERIFIER_DID}",

  "client_id": "did:tdw:QmUUDG5WgyjmfJdbhbFT5PbVHkN4hNzHYCnH3BrYPztXoH:identifier-reg.trust-infra.swiyu-int.admin.ch:api:v1:did:ddd8c7ee-0b38-41e3-84c3-b161e82d2801",
  "client_name#en": "swiyu Swiyu.Aspire Verfifier",
  "client_name#fr": "Vérificateur de démonstration de développement",
  "client_name#de-DE": "Entwicklungs-Demo-Verifizierer",
  "client_name#de-CH": "Entwickligs-Demo-Verifizier",
  "client_name#de": "Entwicklungs-Demo-Verifizierer (Fallback DE)",
  "client_name": "Swiyu.Aspire Swiyu Demo Verifier (Base)",
  "logo_uri": "www.example.com/logo.png",
  "logo_uri#fr": "www.example.com/logo_fr.png"
}
```

More configuration:

```json
// https://swiyu-admin-ch.github.io/cookbooks/onboarding-generic-issuer/
// issuer
"Parameters:issuerexternalurl": "TODO",
"Parameters:issuerid": "TODO",
"Parameters:issuerdidsdjwtverificationmethod": "TODO",
"Parameters:issuersdjwtkey": "TODO",
"Parameters:issueropenIdconfigfile": "TODO",
"Parameters:issuermetadataconfigfile": "TODO",
"Parameters:issuertokenttl": "TODO",

"Parameters:issuername": "TODO",
"Parameters:businesspartnerid": "TODO",
"Parameters:swiyucustomerkey": "TODO",
"Parameters:swiyucustomersecret": "TODO",
"Parameters:swiyurefreshtoken": "TODO",
"Parameters:swiyuaccesstoken": "TODO",

// https://swiyu-admin-ch.github.io/cookbooks/onboarding-generic-verifier/
// verifier
"Parameters:verifiername": "TODO",
"Parameters:verifierdid": "TODO",
"Parameters:verifieropenidclientmetadatafile": "TODO",
"Parameters:verifierexternalurl": "TODO",
"Parameters:didverifiermethod": "TODO",
"Parameters:verifiersigningkey": "TODO",

// DB
"Parameters:postgresuser": "TODO",
"Parameters:postgrespassword": "TODO",
"Parameters:postgresjdbcverifier": "TODO",
"Parameters:postgresdbverifier": "TODO",
"Parameters:postgresjdbcissuer": "TODO",
"Parameters:postgresdbissuer": "TODO",
```

## Notes

For local development, use "\n" to end the lines in the PEM secrets, in production use original secret

Load the config file from some public end point

After deployment, some secrets get updated with incorrect encoding, they need to be update in the production.

Notes: you can only issuer credentials when you have valid tokens. Get from:

PostgreSQL: Update max_connections to 100

https://portal.trust-infra.swiyu-int.admin.ch/ui/organizations

Use ImageMagick, other image libs are not container friendly

https://github.com/manuelbl/QrCodeGenerator/tree/master/Demo-ImageMagick

## Links

https://swiyu-admin-ch.github.io/

https://www.eid.admin.ch/en/public-beta-e

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
