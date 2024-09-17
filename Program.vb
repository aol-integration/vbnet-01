Imports System
Imports System.Net.Http

Module Program
    Sub Main(args As String())
        Test("https://account.accurate.id/api/api-token.do")

        Console.WriteLine("Program End")
    End Sub


    Sub Test(Url As String)

        Dim ApiToken As String = "..."
        Dim SignatureSecret As String = "..."

        Dim DateTimeNow As DateTime = DateTime.Now
        Dim Timestamp As String = DateTimeNow.ToString("dd/MM/yyyy HH:mm:ss")

        Dim HashPayload As Byte() = System.Text.Encoding.UTF8.GetBytes(Timestamp)
        Dim HashKey As Byte() = System.Text.Encoding.UTF8.GetBytes(SignatureSecret)
        Dim Hmac As New System.Security.Cryptography.HMACSHA256(HashKey)
        Dim HashResult As Byte() = Hmac.ComputeHash(HashPayload)
        Hmac.Clear()

        Dim Signature As String = BitConverter.ToString(HashResult).ToLower().Replace("-", "")

        Console.WriteLine(Url)
        Console.WriteLine(Timestamp)
        Console.WriteLine(Signature)

        Using client As New HttpClient()
            Using requestMessage As New HttpRequestMessage(HttpMethod.Get, Url)

                requestMessage.Headers.Authorization = New Headers.AuthenticationHeaderValue("Bearer", ApiToken)
                requestMessage.Headers.Add("X-Api-Timestamp", Timestamp)
                requestMessage.Headers.Add("X-Api-Signature", Signature)
                requestMessage.Headers.Add("X-Language-Profile", "US")
                Using response As System.Net.Http.HttpResponseMessage = client.SendAsync(requestMessage).Result
                    Dim responseBody As String = response.Content.ReadAsStringAsync().Result
                    Console.WriteLine(responseBody)
                    Console.WriteLine("")
                End Using

            End Using
        End Using

        Return True
    End Sub

End Module
