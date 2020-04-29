/*-------------------------------------------------------------------------------------

© Copyright 2018 Johnson Controls, Inc.
Use or Copying of all or any part of this program, except as
permitted by License Agreement, is prohibited.

-------------------------------------------------------------------------------------*/


namespace Apollo.ERService.SpecflowTests.TestSettings
{
    public interface ITestConfiguration
    {
        string ImsEndpoint { get; }
        string ImsClientId { get; }
        string ImsClientSecret { get; }
        string PrimaryUserName { get; }
        string PrimaryUserPassword { get; }
        string SecondaryUsername { get; }
        string SecondaryPassword { get; }
        string InvalidImsEndpoint { get; }
        string InvalidImsClientSecret { get; }
        string ImsScopes { get; }
        string ACLEndpoint { get; }
        string ERApiEndpoint { get; }
        string GraphApiEndpoint { get; }
        string FilesApiEndpoint { get; }
    }
}