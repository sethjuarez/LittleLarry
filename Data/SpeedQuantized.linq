<Query Kind="Statements">
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

var final = from i in Data.Select(d => (int)(d.Speed * 100)).Distinct().OrderBy(d => d)
			let q = (from j in Data.Select(d => (int)(d.Speed * 100))
					 where j == i
					 select j).Count()
			select new { Speed = i, Total = q };
			
var agg = from i in final.ToList()
		  group i by i.Speed into g
		  select new { Speed = g.Key, Count = g.Sum(d => d.Total) };
			
			
agg.Dump("Final");