<Query Kind="Expression">
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

from i in Data.Select(d => d.Turn).Distinct().OrderBy(d => d)
let q = (from j in Data.Select(d => d.Turn)
		 where j == i
		 select j).Count()
select new { Turn = i, Count = q, Percentage = q / (double)Data.Select(d => d.Turn).Count() }