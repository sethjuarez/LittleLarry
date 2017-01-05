<Query Kind="Statements">
  <Connection>
    <ID>242b8e44-ca14-48fb-8248-86c172146be9</ID>
    <Persist>true</Persist>
    <Driver Assembly="IQDriver" PublicKeyToken="5b59726538a49684">IQDriver.IQDriver</Driver>
    <Provider>System.Data.SQLite</Provider>
    <CustomCxString>Data Source=C:\Users\SETH-LAPTOP\Desktop\LittleLarryData.db;FailIfMissing=True</CustomCxString>
    <AttachFileName>C:\Users\SETH-LAPTOP\Desktop\LittleLarryData.db</AttachFileName>
    <DisplayName>larry</DisplayName>
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