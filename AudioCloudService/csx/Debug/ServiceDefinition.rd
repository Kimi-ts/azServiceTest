<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="AudioCloudService" generation="1" functional="0" release="0" Id="96369863-3e13-4e19-adba-363c52710537" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="AudioCloudServiceGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="AudioWeb:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/AudioCloudService/AudioCloudServiceGroup/LB:AudioWeb:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="AudioWeb:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/AudioCloudService/AudioCloudServiceGroup/MapAudioWeb:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="AudioWebInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/AudioCloudService/AudioCloudServiceGroup/MapAudioWebInstances" />
          </maps>
        </aCS>
        <aCS name="AudioWorker:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/AudioCloudService/AudioCloudServiceGroup/MapAudioWorker:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="AudioWorkerInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/AudioCloudService/AudioCloudServiceGroup/MapAudioWorkerInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:AudioWeb:Endpoint1">
          <toPorts>
            <inPortMoniker name="/AudioCloudService/AudioCloudServiceGroup/AudioWeb/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapAudioWeb:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/AudioCloudService/AudioCloudServiceGroup/AudioWeb/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapAudioWebInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/AudioCloudService/AudioCloudServiceGroup/AudioWebInstances" />
          </setting>
        </map>
        <map name="MapAudioWorker:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/AudioCloudService/AudioCloudServiceGroup/AudioWorker/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapAudioWorkerInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/AudioCloudService/AudioCloudServiceGroup/AudioWorkerInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="AudioWeb" generation="1" functional="0" release="0" software="C:\Users\tatyana.shvets\documents\visual studio 2015\Projects\AudioCloudService\AudioCloudService\csx\Debug\roles\AudioWeb" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;AudioWeb&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;AudioWeb&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;AudioWorker&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/AudioCloudService/AudioCloudServiceGroup/AudioWebInstances" />
            <sCSPolicyUpdateDomainMoniker name="/AudioCloudService/AudioCloudServiceGroup/AudioWebUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/AudioCloudService/AudioCloudServiceGroup/AudioWebFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="AudioWorker" generation="1" functional="0" release="0" software="C:\Users\tatyana.shvets\documents\visual studio 2015\Projects\AudioCloudService\AudioCloudService\csx\Debug\roles\AudioWorker" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="-1" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;AudioWorker&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;AudioWeb&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;AudioWorker&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/AudioCloudService/AudioCloudServiceGroup/AudioWorkerInstances" />
            <sCSPolicyUpdateDomainMoniker name="/AudioCloudService/AudioCloudServiceGroup/AudioWorkerUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/AudioCloudService/AudioCloudServiceGroup/AudioWorkerFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="AudioWebUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="AudioWorkerUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="AudioWebFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="AudioWorkerFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="AudioWebInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="AudioWorkerInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="fa1a8e26-adc6-4601-a514-a0c39e6169d0" ref="Microsoft.RedDog.Contract\ServiceContract\AudioCloudServiceContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="40758c66-be68-4054-a969-fc16b0824cbf" ref="Microsoft.RedDog.Contract\Interface\AudioWeb:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/AudioCloudService/AudioCloudServiceGroup/AudioWeb:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>