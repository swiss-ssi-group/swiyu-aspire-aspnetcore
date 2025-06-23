# swiyu-employee

Getting started:

https://swiyu-admin-ch.github.io/cookbooks/onboarding-base-and-trust-registry/

Support: usertesting@swiyu.admin.ch

```json
{
  //"client_id": "${VERIFIER_DID}",

  "client_id": "did:tdw:QmUUDG5WgyjmfJdbhbFT5PbVHkN4hNzHYCnH3BrYPztXoH:identifier-reg.trust-infra.swiyu-int.admin.ch:api:v1:did:ddd8c7ee-0b38-41e3-84c3-b161e82d2801",
  "client_name#en": "swiyu Employee Verfifier",
  "client_name#fr": "Vérificateur de démonstration de développement",
  "client_name#de-DE": "Entwicklungs-Demo-Verifizierer",
  "client_name#de-CH": "Entwickligs-Demo-Verifizier",
  "client_name#de": "Entwicklungs-Demo-Verifizierer (Fallback DE)",
  "client_name": "Employee Swiyu Demo Verifier (Base)",
  "logo_uri": "www.example.com/logo.png",
  "logo_uri#fr": "www.example.com/logo_fr.png"
}
```

### Configuration data

```json
// https://swiyu-admin-ch.github.io/cookbooks/onboarding-generic-issuer/
// issuer
"Parameters:issuerexternalurl": "TODO",
"Parameters:issuerid": "TODO",
"Parameters:issuerdidsdjwtverificationmethod": "TODO",
"Parameters:issuersdjwtkey": "TODO",
"Parameters:issueropenIdconfigfile": "TODO",
"Parameters:issuermetadataconfigfile": "TODO",
"Parameters:issuertokenttl": "TODOs",

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

### Notes

For local development, use "\n" to end the lines in the PEM secrets, in production use original secret

Load the config file from some public end point

After deployment, some secrets get updated with incorrect encoding, they need to be update in the production.

## Links

https://swiyu-admin-ch.github.io/

https://www.eid.admin.ch/en/public-beta-e

https://www.npmjs.com/package/ngrok

https://swiyu-admin-ch.github.io/specifications/interoperability-profile/

https://andrewlock.net/converting-a-docker-compose-file-to-aspire/

https://swiyu-admin-ch.github.io/cookbooks/onboarding-generic-verifier/

## Standards

https://identity.foundation/trustdidweb/

https://openid.net/specs/openid-4-verifiable-credential-issuance-1_0.html

https://openid.net/specs/openid-4-verifiable-presentations-1_0.html

https://datatracker.ietf.org/doc/draft-ietf-oauth-selective-disclosure-jwt/

https://datatracker.ietf.org/doc/draft-ietf-oauth-sd-jwt-vc/

https://datatracker.ietf.org/doc/draft-ietf-oauth-status-list/

https://www.w3.org/TR/vc-data-model-2.0/
