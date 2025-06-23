using Microsoft.Extensions.Hosting;
using System.Diagnostics;

var builder = DistributedApplication.CreateBuilder(args);

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


// Issuer
var swiyuOid4vci = builder.AddContainer("swiyu-oid4vci", "ghcr.io/swiyu-admin-ch/eidch-issuer-agent-oid4vci", "latest")
    .WithEnvironment("EXTERNAL_URL", issuerExternalUrl)
    .WithEnvironment("ISSUER_ID", issuerId)
    .WithEnvironment("DID_SDJWT_VERIFICATION_METHOD", issuerDidSdJwtVerficiationMethod)
    .WithEnvironment("SDJWT_KEY", issuerSdJwtKey)
    //.WithEnvironment("OPENID_CONFIG_FILE", issuerOpenIdConfigFile)
    //.WithEnvironment("METADATA_CONFIG_FILE", issuerMetaDataConfigFile)
    //.WithEnvironment("TOKEN_TTL", issuerTokenTtl)
    .WithEnvironment("POSTGRES_USER", postGresUser)
    .WithEnvironment("POSTGRES_PASSWORD", postGresPassword)
    .WithEnvironment("POSTGRES_DB", postGresDbIssuer)
    .WithEnvironment("POSTGRES_JDBC", postGresJdbcIssuer)
    .WithHttpEndpoint(port: 80, targetPort: 8080, name: "swiyu-oid4vci-endpoint")
    .WithExternalHttpEndpoints();

// Verifier
var verifierExternalUrl = builder.AddParameter("verifierexternalurl");
var verifierOpenIdClientMetaDataFile = builder.AddParameter("verifieropenidclientmetadatafile");
var verifierDid = builder.AddParameter("verifierdid");
var didVerifierMethod = builder.AddParameter("didverifiermethod");
var verifierName = builder.AddParameter("verifiername");
var verifierSigningKey = builder.AddParameter("verifiersigningkey");

// Verifier
var swiyuOid4vp = builder.AddContainer("swiyu-oid4vp", "ghcr.io/swiyu-admin-ch/eidch-verifier-agent-oid4vp", "latest")
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
    .WithHttpEndpoint(port: 80, targetPort: 8080, name: "swiyu-oid4vp-endpoint")
    .WithExternalHttpEndpoints();

if(builder.Environment.IsDevelopment())
{
    var swiyuVerifierMgmt = builder.AddContainer("swiyu-verifier-mgmt", "ghcr.io/swiyu-admin-ch/eidch-verifier-agent-management", "latest")
        .WithEnvironment("EXTERNAL_URL", verifierExternalUrl)
        .WithEnvironment("POSTGRES_USER", postGresUser)
        .WithEnvironment("POSTGRES_PASSWORD", postGresPassword)
        .WithEnvironment("POSTGRES_DB", postGresDbVerifier)
        .WithEnvironment("POSTGRES_JDBC", postGresJdbcVerifier)
        .WithHttpEndpoint(port: 8082, targetPort: 8080, name: "swiyu-verifier-mgmt-endpoint");

    var swiyuIssuerMgmt = builder.AddContainer("swiyu-issuer-mgmt", "ghcr.io/swiyu-admin-ch/eidch-issuer-agent-management", "latest")
       .WithEnvironment("EXTERNAL_URL", issuerExternalUrl)
       .WithEnvironment("SPRING_APPLICATION_NAME", issuerName)
       .WithEnvironment("ISSUER_ID", issuerId)

       .WithEnvironment("DID_STATUS_LIST_VERIFICATION_METHOD", issuerDidSdJwtVerficiationMethod)
       .WithEnvironment("STATUS_LIST_KEY", issuerSdJwtKey)
       .WithEnvironment("SWIYU_PARTNER_ID", businessPartnerId)
       .WithEnvironment("SWIYU_STATUS_REGISTRY_CUSTOMER_KEY", swiyuCustomerKey)
       .WithEnvironment("SWIYU_STATUS_REGISTRY_CUSTOMER_SECRET", swiyuCustomerSecret)

       // TODO
       //.WithEnvironment("SWIYU_STATUS_REGISTRY_ACCESS_TOKEN", issuerId)
       //.WithEnvironment("SWIYU_STATUS_REGISTRY_BOOTSTRAP_REFRESH_TOKEN", issuerId)
       //.WithEnvironment("SWIYU_STATUS_REGISTRY_TOKEN_URL", issuerId)

       .WithEnvironment("SWIYU_STATUS_REGISTRY_API_URL", "https://status-reg-api.trust-infra.swiyu-int.admin.ch")
       .WithEnvironment("LOGGING_LEVEL_CH_ADMIN_BIT_EID", "DEBUG")
       .WithEnvironment("SWIYU_STATUS_REGISTRY_AUTH_ENABLE_REFRESH_TOKEN_FLOW", "true")

       .WithEnvironment("POSTGRES_USER", postGresUser)
       .WithEnvironment("POSTGRES_PASSWORD", postGresPassword)
       .WithEnvironment("POSTGRES_DB", postGresDbIssuer)
       .WithEnvironment("POSTGRES_JDBC", postGresJdbcIssuer)
       .WithHttpEndpoint(port: 8084, targetPort: 8080, name: "swiyu-issuer-mgmt-endpoint");
}

builder.AddProject<Projects.EmployeeOnboarding>("employeeonboarding")
    .WithExternalHttpEndpoints();

builder.Build().Run();

