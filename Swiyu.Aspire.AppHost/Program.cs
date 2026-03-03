using Microsoft.AspNetCore.Http;
using Projects;

const string HTTP = "http";
const string IDENTITY_PROVIDER = "identityProvider";
const string WEB_CLIENT = "webClient";
const string API_SERVICE = "apiService";

var builder = DistributedApplication.CreateBuilder(args);

// management & public endpoints
IResourceBuilder<ContainerResource>? swiyuVerifier = null;
IResourceBuilder<ContainerResource>? swiyuIssuer = null;
IResourceBuilder<ProjectResource>? swiyuMgmt = null;
IResourceBuilder<ProjectResource>?  swiyuProxy = null;
IResourceBuilder<ProjectResource>? identityProvider = null;

// E-ID database
var postGresUser = builder.AddParameter("postgresuser");
var postGresPassword = builder.AddParameter("postgrespassword", secret: true);
var postGresDbIssuer = builder.AddParameter("postgresdbissuer");

var postGresJdbcIssuer = builder.AddParameter("postgresjdbcissuer");
var postGresDbVerifier = builder.AddParameter("postgresdbverifier");
var postGresJdbcVerifier = builder.AddParameter("postgresjdbcverifier");

// Issuer
var issuerExternalUrl = builder.AddParameter("issuerexternalurl");
var issuerId = builder.AddParameter("issuerid");
var issuerDidSdJwtVerficiationMethod = builder.AddParameter("issuerdidsdjwtverificationmethod");
var issuerSdJwtKey = builder.AddParameter("issuersdjwtkey", secret: true);
var issuerOpenIdConfigFile = builder.AddParameter("issueropenidconfigfile");
var issuerMetaDataConfigFile = builder.AddParameter("issuermetadataconfigfile");
var issuerTokenTtl = builder.AddParameter("issuertokenttl");

var issuerName = builder.AddParameter("issuername");
var businessPartnerId = builder.AddParameter("businesspartnerid", secret: true);
var swiyuCustomerKey = builder.AddParameter("swiyucustomerkey", secret: true);
var swiyuCustomerSecret = builder.AddParameter("swiyucustomerSecret", secret: true);
var swiyuRefreshToken = builder.AddParameter("swiyurefreshtoken", secret: true);
var swiyuAccessToken = builder.AddParameter("swiyuaccesstoken", secret: true);

// Verifier
var verifierExternalUrl = builder.AddParameter("verifierexternalurl");
var verifierOpenIdClientMetaDataFile = builder.AddParameter("verifieropenidclientmetadatafile");
var verifierDid = builder.AddParameter("verifierdid");
var didVerifierMethod = builder.AddParameter("didverifiermethod");
var verifierName = builder.AddParameter("verifiername");
var verifierSigningKey = builder.AddParameter("verifiersigningkey", true);

var idpWellKnownEndpoint = builder.AddParameter("idpwellknownendpoint");
var idpJwksUri = builder.AddParameter("idpjwksuri");
var verifierJwtIssuer = builder.AddParameter("verifierjwtissuer");

/////////////////////////////////////////////////////////////////
// Verifier OpenID Endpoint: Must be deployed to a public URL
/////////////////////////////////////////////////////////////////
// Verifier Management Endpoint: TODO Add JWT security verifier
// Add security to management API, disabled
// https://github.com/swiyu-admin-ch/swiyu-verifier?tab=readme-ov-file#security
/////////////////////////////////////////////////////////////////
swiyuVerifier = builder.AddContainer("swiyu-verifier", "ghcr.io/swiyu-admin-ch/swiyu-verifier", "latest")
    //.WaitFor(identityProvider)
    .WithEnvironment("EXTERNAL_URL", verifierExternalUrl)
    .WithEnvironment("OPENID_CLIENT_METADATA_FILE", verifierOpenIdClientMetaDataFile)
    .WithEnvironment("VERIFIER_DID", verifierDid)
    .WithEnvironment("DID_VERIFICATION_METHOD", didVerifierMethod)
    .WithEnvironment("SIGNING_KEY", verifierSigningKey)
    .WithEnvironment("POSTGRES_USER", postGresUser)
    .WithEnvironment("POSTGRES_PASSWORD", postGresPassword)
    .WithEnvironment("POSTGRES_DB", postGresDbVerifier)
    .WithEnvironment("POSTGRES_JDBC", postGresJdbcVerifier)
    .WithEnvironment("SPRING_SECURITY_OAUTH2_RESOURCESERVER_JWT_ISSUERURI", verifierJwtIssuer)
    //.WithHttpEndpoint(port: 8084, targetPort: 8080, name: HTTP);  // local development
    .WithHttpEndpoint(port: 80, targetPort: 8080, name: HTTP); // for deployment 

/////////////////////////////////////////////////////////////////
// Issuer OpenID Endpoint: Must be deployed to a public URL
/////////////////////////////////////////////////////////////////
// Issuer Management Endpoint: TODO Add JWT security issuer
// Add security to management API, disabled
// https://github.com/swiyu-admin-ch/swiyu-issuer?tab=readme-ov-file#security
/////////////////////////////////////////////////////////////////
swiyuIssuer = builder.AddContainer("swiyu-issuer", "ghcr.io/swiyu-admin-ch/swiyu-issuer", "latest")
    .WithEnvironment("EXTERNAL_URL", issuerExternalUrl)
    .WithEnvironment("SPRING_APPLICATION_NAME", issuerName)
    .WithEnvironment("ISSUER_ID", issuerId)

    .WithEnvironment("DID_STATUS_LIST_VERIFICATION_METHOD", issuerDidSdJwtVerficiationMethod)
    .WithEnvironment("STATUS_LIST_KEY", issuerSdJwtKey)
    .WithEnvironment("SWIYU_PARTNER_ID", businessPartnerId)
    .WithEnvironment("SWIYU_STATUS_REGISTRY_CUSTOMER_KEY", swiyuCustomerKey)
    .WithEnvironment("SWIYU_STATUS_REGISTRY_CUSTOMER_SECRET", swiyuCustomerSecret)

    .WithEnvironment("SWIYU_STATUS_REGISTRY_ACCESS_TOKEN", swiyuAccessToken)
    .WithEnvironment("SWIYU_STATUS_REGISTRY_BOOTSTRAP_REFRESH_TOKEN", swiyuRefreshToken)
    .WithEnvironment("SWIYU_STATUS_REGISTRY_TOKEN_URL", "https://keymanager-prd.api.admin.ch/keycloak/realms/APIGW/protocol/openid-connect/token")

    .WithEnvironment("DID_SDJWT_VERIFICATION_METHOD", issuerDidSdJwtVerficiationMethod)
    .WithEnvironment("SDJWT_KEY", issuerSdJwtKey)
    .WithEnvironment("OPENID_CONFIG_FILE", issuerOpenIdConfigFile)
    .WithEnvironment("METADATA_CONFIG_FILE", issuerMetaDataConfigFile)
    .WithEnvironment("TOKEN_TTL", issuerTokenTtl)

    .WithEnvironment("SWIYU_STATUS_REGISTRY_API_URL", "https://status-reg-api.trust-infra.swiyu-int.admin.ch")
    .WithEnvironment("LOGGING_LEVEL_CH_ADMIN_BJ_SWIYU", "DEBUG")
    .WithEnvironment("SWIYU_STATUS_REGISTRY_AUTH_ENABLE_REFRESH_TOKEN_FLOW", "true")

    .WithEnvironment("POSTGRES_USER", postGresUser)
    .WithEnvironment("POSTGRES_PASSWORD", postGresPassword)
    .WithEnvironment("POSTGRES_DB", postGresDbIssuer)
    .WithEnvironment("POSTGRES_JDBC", postGresJdbcIssuer)
    //.WithHttpEndpoint(port: 8082, targetPort: 8080, name: HTTP); // local development
    .WithHttpEndpoint(port: 80, targetPort: 8080, name: HTTP); // for deployment

var sqlServer = builder.AddAzureSqlServer("sqlserver");
var database = sqlServer.AddDatabase("database", "IdpSwiyuPasskeysSts");

var migrationService = builder.AddProject<Idp_Swiyu_Passkeys_Sts_Domain_Migrations>("migrations")
    .WithReference(database)
    .WaitFor(sqlServer);

swiyuProxy = builder.AddProject<Projects.Swiyu_Endpoints_Proxy>("swiyu-endpoints-proxy")
    .WaitFor(swiyuIssuer)
    .WaitFor(swiyuVerifier)
    .WithEnvironment("SwiyuVerifierMgmtUrl", swiyuVerifier.GetEndpoint(HTTP))
    .WithEnvironment("SwiyuIssuerMgmtUrl", swiyuIssuer.GetEndpoint(HTTP))
    .WithExternalHttpEndpoints();

var swiyuManagementClientId = builder.AddParameter("swiyumanagementclientid");
var swiyuManagementClientSecret = builder.AddParameter("swiyumanagementclientsecret", true);
var swiyuManagementAuthority = builder.AddParameter("swiyumanagementauthority");
var swiyuManagementScope = builder.AddParameter("swiyumanagementscope");
var webClientUrl = builder.AddParameter("WebClientUrl");

identityProvider = builder.AddProject<Projects.Idp_Swiyu_Passkeys_Sts>(IDENTITY_PROVIDER)
    .WithExternalHttpEndpoints()
    .WithReference(database)
    .WaitForCompletion(migrationService)
    .WithEnvironment("SwiyuVerifierMgmtUrl", swiyuVerifier.GetEndpoint(HTTP))
    .WithEnvironment("SwiyuOid4vpUrl", verifierExternalUrl)
    .WithEnvironment("ISSUER_ID", issuerId)
    .WithEnvironment("SwiyuManagementClientId", swiyuManagementClientId)
    .WithEnvironment("SwiyuManagementClientSecret", swiyuManagementClientSecret)
    .WithEnvironment("SwiyuManagementAuthority", swiyuManagementAuthority)
    .WithEnvironment("SwiyuManagementScope", swiyuManagementScope)
    .WithEnvironment("WebClientUrl", webClientUrl)
    .WaitFor(swiyuVerifier)
    .WaitFor(swiyuProxy)
    .WithHttpHealthCheck("/health");

// OIDC web endpoints
var webOidcClientId = builder.AddParameter("WebOidcClientId");
var webOidcAuthority = builder.AddParameter("WebOidcAuthority");

var apiService = builder.AddProject<Projects.Idp_Swiyu_Passkeys_ApiService>(API_SERVICE)
    .WithReference(identityProvider)
    .WaitFor(identityProvider)
    .WithHttpHealthCheck("/health")
    .WithEnvironment("WebOidcAuthority", webOidcAuthority);

builder.AddProject<Projects.Idp_Swiyu_Passkeys_Web>(WEB_CLIENT)
    .WithExternalHttpEndpoints()
    .WithEnvironment("IdentityProviderUrl", webOidcAuthority)
    .WithEnvironment("WebOidcAuthority", webOidcAuthority)
    .WithEnvironment("WebOidcClientId", webOidcClientId)
    .WithHttpHealthCheck("/health")
    .WaitFor(identityProvider)
    .WithReference(identityProvider)
    .WithReference(apiService)
    .WaitFor(apiService);

if (builder.ExecutionContext.IsRunMode)
{
    sqlServer.RunAsContainer(container =>
    {
        container.WithDataVolume();
    });
}

swiyuMgmt = builder.AddProject<Projects.Swiyu_Aspire_Mgmt>("swiyu-mgmt")
    .WithExternalHttpEndpoints()
    .WithEnvironment("SwiyuVerifierMgmtUrl", swiyuVerifier.GetEndpoint(HTTP))
    .WithEnvironment("SwiyuIssuerMgmtUrl", swiyuIssuer.GetEndpoint(HTTP))
    .WithEnvironment("SwiyuOid4vciUrl", issuerExternalUrl)
    .WithEnvironment("SwiyuOid4vpUrl", verifierExternalUrl)
    .WithEnvironment("ISSUER_ID", issuerId)
    .WaitFor(swiyuIssuer)
    .WaitFor(swiyuVerifier)
    .WaitFor(swiyuProxy);

builder.Build().Run();

