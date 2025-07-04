using Aspire.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

const string HTTP = "http";

// public
IResourceBuilder<ContainerResource>? swiyuOid4vci = null;
IResourceBuilder<ContainerResource>? swiyuOid4vp = null;

// management
IResourceBuilder<ContainerResource>? swiyuVerifierMgmt = null;
IResourceBuilder<ContainerResource>? swiyuIssuerMgmt = null;
IResourceBuilder<ProjectResource>? swiyuAspireMgmt = null;

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

if (builder.Environment.IsDevelopment())
{
    // Issuer
    swiyuOid4vci = builder.AddContainer("swiyu-oid4vci", "ghcr.io/swiyu-admin-ch/eidch-issuer-agent-oid4vci", "latest")
        .WithEnvironment("EXTERNAL_URL", issuerExternalUrl)
        .WithEnvironment("ISSUER_ID", issuerId)
        .WithEnvironment("DID_SDJWT_VERIFICATION_METHOD", issuerDidSdJwtVerficiationMethod)
        .WithEnvironment("SDJWT_KEY", issuerSdJwtKey)
        .WithEnvironment("OPENID_CONFIG_FILE", issuerOpenIdConfigFile)
        .WithEnvironment("METADATA_CONFIG_FILE", issuerMetaDataConfigFile)
        .WithEnvironment("TOKEN_TTL", issuerTokenTtl)
        .WithEnvironment("POSTGRES_USER", postGresUser)
        .WithEnvironment("POSTGRES_PASSWORD", postGresPassword)
        .WithEnvironment("POSTGRES_DB", postGresDbIssuer)
        .WithEnvironment("POSTGRES_JDBC", postGresJdbcIssuer)
        .WithExternalHttpEndpoints();

    swiyuOid4vci.WithHttpEndpoint(port: 80, targetPort: 8080, name: HTTP);
}
else
{
    // Issuer
    swiyuOid4vci = builder.AddContainer("swiyu-oid4vci", "ghcr.io/swiyu-admin-ch/eidch-issuer-agent-oid4vci", "latest")
        .WithEnvironment("EXTERNAL_URL", issuerExternalUrl)
        .WithEnvironment("ISSUER_ID", issuerId)
        .WithEnvironment("DID_SDJWT_VERIFICATION_METHOD", issuerDidSdJwtVerficiationMethod)
        .WithEnvironment("SDJWT_KEY", issuerSdJwtKey)
        .WithEnvironment("OPENID_CONFIG_FILE", issuerOpenIdConfigFile)
        .WithEnvironment("METADATA_CONFIG_FILE", issuerMetaDataConfigFile)
        .WithEnvironment("TOKEN_TTL", issuerTokenTtl)
        .WithEnvironment("POSTGRES_USER", postGresUser)
        .WithEnvironment("POSTGRES_PASSWORD", postGresPassword)
        .WithEnvironment("POSTGRES_DB", postGresDbIssuer)
        .WithEnvironment("POSTGRES_JDBC", postGresJdbcIssuer)
        .WithExternalHttpEndpoints();

    swiyuOid4vci.WithHttpEndpoint(port: 80, targetPort: 8080, name: HTTP);
}

// Verifier
var verifierExternalUrl = builder.AddParameter("verifierexternalurl");
var verifierOpenIdClientMetaDataFile = builder.AddParameter("verifieropenidclientmetadatafile");
var verifierDid = builder.AddParameter("verifierdid");
var didVerifierMethod = builder.AddParameter("didverifiermethod");
var verifierName = builder.AddParameter("verifiername");
var verifierSigningKey = builder.AddParameter("verifiersigningkey");

if (builder.Environment.IsDevelopment())
{
    // Verifier
    swiyuOid4vp = builder.AddContainer("swiyu-oid4vp", "ghcr.io/swiyu-admin-ch/eidch-verifier-agent-oid4vp", "latest")
        .WithEnvironment("EXTERNAL_URL", verifierExternalUrl)
        .WithEnvironment("OPENID_CLIENT_METADATA_FILE", verifierOpenIdClientMetaDataFile)
        .WithEnvironment("VERIFIER_DID", verifierDid)
        .WithEnvironment("DID_VERIFICATION_METHOD", didVerifierMethod)
        .WithEnvironment("VERIFIER_NAME", verifierName)
        .WithEnvironment("SIGNING_KEY", verifierSigningKey)
        .WithEnvironment("POSTGRES_USER", postGresUser)
        .WithEnvironment("POSTGRES_PASSWORD", postGresPassword)
        .WithEnvironment("POSTGRES_DB", postGresDbVerifier)
        .WithEnvironment("POSTGRES_JDBC", postGresJdbcVerifier)
        .WithExternalHttpEndpoints();

    swiyuOid4vp.WithHttpEndpoint(port: 80, targetPort: 8080, name: HTTP);
}
else
{
    // Verifier
    swiyuOid4vp = builder.AddContainer("swiyu-oid4vp", "ghcr.io/swiyu-admin-ch/eidch-verifier-agent-oid4vp", "latest")
        .WithEnvironment("EXTERNAL_URL", verifierExternalUrl)
        .WithEnvironment("OPENID_CLIENT_METADATA_FILE", verifierOpenIdClientMetaDataFile)
        .WithEnvironment("VERIFIER_DID", verifierDid)
        .WithEnvironment("DID_VERIFICATION_METHOD", didVerifierMethod)
        .WithEnvironment("VERIFIER_NAME", verifierName)
        .WithEnvironment("SIGNING_KEY", verifierSigningKey)
        .WithEnvironment("POSTGRES_USER", postGresUser)
        .WithEnvironment("POSTGRES_PASSWORD", postGresPassword)
        .WithEnvironment("POSTGRES_DB", postGresDbVerifier)
        .WithEnvironment("POSTGRES_JDBC", postGresJdbcVerifier)
        .WithExternalHttpEndpoints();

    swiyuOid4vp.WithHttpEndpoint(port: 80, targetPort: 8080, name: HTTP);
}

swiyuVerifierMgmt = builder.AddContainer("swiyu-verifier-mgmt", "ghcr.io/swiyu-admin-ch/eidch-verifier-agent-management", "latest")
    .WithEnvironment("OID4VP_URL", verifierExternalUrl)
    .WithEnvironment("POSTGRES_USER", postGresUser)
    .WithEnvironment("POSTGRES_PASSWORD", postGresPassword)
    .WithEnvironment("POSTGRES_DB", postGresDbVerifier)
    .WithEnvironment("POSTGRES_JDBC", postGresJdbcVerifier)
    .WithHttpEndpoint(port: 80, targetPort: 8080, name: HTTP); // for deployment
    //.WithHttpEndpoint(port: 8084, targetPort: 8080, name: HTTP);  // local development

swiyuIssuerMgmt = builder.AddContainer("swiyu-issuer-mgmt", "ghcr.io/swiyu-admin-ch/eidch-issuer-agent-management", "latest")
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

    .WithEnvironment("SWIYU_STATUS_REGISTRY_API_URL", "https://status-reg-api.trust-infra.swiyu-int.admin.ch")
    .WithEnvironment("LOGGING_LEVEL_CH_ADMIN_BIT_EID", "DEBUG")
    .WithEnvironment("SWIYU_STATUS_REGISTRY_AUTH_ENABLE_REFRESH_TOKEN_FLOW", "true")

    .WithEnvironment("POSTGRES_USER", postGresUser)
    .WithEnvironment("POSTGRES_PASSWORD", postGresPassword)
    .WithEnvironment("POSTGRES_DB", postGresDbIssuer)
    .WithEnvironment("POSTGRES_JDBC", postGresJdbcIssuer)
    .WithHttpEndpoint(port: 80, targetPort: 8080, name: HTTP); // for deployment
    //.WithHttpEndpoint(port: 8082, targetPort: 8080, name: HTTP); // local development

swiyuAspireMgmt = builder.AddProject<Projects.Swiyu_Aspire_Mgmt>("swiyuaspiremgmt")
    .WithExternalHttpEndpoints()
    .WithEnvironment("SwiyuVerifierMgmtUrl", swiyuVerifierMgmt.GetEndpoint(HTTP))
    .WithEnvironment("SwiyuIssuerMgmtUrl", swiyuIssuerMgmt.GetEndpoint(HTTP))
    .WithEnvironment("SwiyuOid4vciUrl", issuerExternalUrl)
    .WithEnvironment("SwiyuOid4vpUrl", verifierExternalUrl)
    .WithEnvironment("ISSUER_ID", issuerId)
    .WaitFor(swiyuIssuerMgmt)
    .WaitFor(swiyuVerifierMgmt);


builder.Build().Run();

