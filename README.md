# SoapSimulator
ASP.Core Server for Simulating SOAP Protocol. Only useful as a testing environment. 
This sample uses opensource package <a target='_blank' href='https://github.com/DigDes/SoapCore'>SoapCore</a>
## General Usage
The current configuartion ignores any request validation this is mainly for cases where you just want to test various responses from actual soap services.
To change this behaviour set <code>RequestValidation</code> property inside <code>appsettings.json</code> file to true.
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source = sysConfig.db"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  },
  "AllowedHosts": "*",
  "urls": "http://::5003",
  "RequestValidation": true
}
```
## Sample Response from the server for the soap request <code>http://locahost:5003/soap/GetTransactions</code>
```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:soapcore="http://soapsimulator/SoapSimulator.Core" xmlns:core="http://schemas.datacontract.org/2004/07/SoapSimulator.Core" xmlns:array="http://schemas.microsoft.com/2003/10/Serialization/Arrays">
    <s:Body>
        <soapcore:ExecuteActionResponse>
            <soapcore:ExecuteActionResult xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
                <soapcore:Data i:type="core:DynamicXmlObject">
                    <Transaction>
                        <Transaction>
                            <Date>2022-02-16T17:30:45</Date>
                            <CreditValueDate>2022-02-16T17:30:45</CreditValueDate>
                            <DebitValueDate>2022-02-16T17:30:45</DebitValueDate>
                            <CreditAmount>250.00</CreditAmount>
                            <DebitAccount>10010002</DebitAccount>
                            <CreditAccount>10010003</CreditAccount>
                            <Narration>Transfer to Savings</Narration>
                            <DestinationBankSortCode>BR003</DestinationBankSortCode>
                        </Transaction>
                        <Transaction>
                            <Date>2022-02-17T14:20:10</Date>
                            <CreditValueDate>2022-02-17T14:20:10</CreditValueDate>
                            <DebitValueDate>2022-02-17T14:20:10</DebitValueDate>
                            <CreditAmount>120.00</CreditAmount>
                            <DebitAccount>10010002</DebitAccount>
                            <CreditAccount>10010001</CreditAccount>
                            <Narration>ATM Withdrawal</Narration>
                            <DestinationBankSortCode>BR001</DestinationBankSortCode>
                        </Transaction>
                        <Transaction>
                            <Date>2022-02-18T09:12:34</Date>
                            <CreditValueDate>2022-02-18T09:12:34</CreditValueDate>
                            <DebitValueDate>2022-02-18T09:12:34</DebitValueDate>
                            <CreditAmount>2500.00</CreditAmount>
                            <DebitAccount>10010002</DebitAccount>
                            <CreditAccount>10010001</CreditAccount>
                            <Narration>Salary Payment</Narration>
                            <DestinationBankSortCode>BR001</DestinationBankSortCode>
                        </Transaction>
                    </Transaction>
                </soapcore:Data>
                <soapcore:Message></soapcore:Message>
                <soapcore:Method>GetTransactions</soapcore:Method>
                <soapcore:Status>Success</soapcore:Status>
            </soapcore:ExecuteActionResult>
        </soapcore:ExecuteActionResponse>
    </s:Body>
</s:Envelope>
```