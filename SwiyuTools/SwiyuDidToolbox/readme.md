
Docs: https://swiyu-admin-ch.github.io/cookbooks/onboarding-base-and-trust-registry/#create-a-did-or-create-the-did-log-you-need-to-continue
Download: https://github.com/swiyu-admin-ch/didtoolbox-java/releases

# Create Your First DID

https://swiyu-admin-ch.github.io/cookbooks/onboarding-base-and-trust-registry/#command-syntax

```
java -jar didtoolbox.jar create --identifier-registry-url $IDENTIFIER_REGISTRY_URL
```

## Example
java -jar didtoolbox.jar create --identifier-registry-url https://identifier-reg.trust-infra.swiyu-int.admin.ch/api/v1/did/cd692f1a-b322-44bb-8396-9e87cc3af692


# Upload DID log content

https://swiyu-admin-ch.github.io/cookbooks/onboarding-base-and-trust-registry/#upload-did-log