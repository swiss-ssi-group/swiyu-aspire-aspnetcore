## Create Swiyu DID space

See: https://swiyu-admin-ch.github.io/cookbooks/onboarding-base-and-trust-registry/#create-did-space

$SWIYU_IDENTIFIER_REGISTRY_ACCESS_TOKEN: Select from correct application
$SWIYU_PARTNER_ID: https://portal.trust-infra.swiyu-int.admin.ch/ui/organizations
$SWIYU_IDENTIFIER_REGISTRY_URL: https://swiyu-admin-ch.github.io/cookbooks/onboarding-base-and-trust-registry/#base-urls

> Note: Use a valid access token.

```
curl \
  -H "Authorization: Bearer $SWIYU_IDENTIFIER_REGISTRY_ACCESS_TOKEN" \
  -X POST "$SWIYU_IDENTIFIER_REGISTRY_URL/api/v1/identifier/business-entities/$SWIYU_PARTNER_ID/identifier-entries"
```
