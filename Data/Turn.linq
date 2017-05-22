<Query Kind="Expression">
  <Connection>
    <ID>c93c173a-30d1-4506-88b2-87f3c50ccd1a</ID>
    <Persist>true</Persist>
    <Driver Assembly="IQDriver" PublicKeyToken="5b59726538a49684">IQDriver.IQDriver</Driver>
    <Provider>System.Data.SQLite</Provider>
    <CustomCxString>Data Source=C:\projects\LittleLarry\Data\LittleLarryData_3.db;FailIfMissing=True</CustomCxString>
    <AttachFileName>C:\projects\LittleLarry\Data\LittleLarryData_3.db</AttachFileName>
    <DisplayName>LittleLarry3</DisplayName>
    <DriverData>
      <StripUnderscores>false</StripUnderscores>
      <QuietenAllCaps>false</QuietenAllCaps>
    </DriverData>
  </Connection>
</Query>

from i in Data.Select(d => d.Turn).Distinct().OrderBy(d => d)
let q = (from j in Data.Select(d => d.Turn)
		 where j == i
		 select j).Count()
select new { Turn = i, Count = q, Percentage = q / (double)Data.Select(d => d.Turn).Count() }