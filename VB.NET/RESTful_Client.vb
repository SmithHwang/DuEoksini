Imports System
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.IO
Imports System.Threading
Imports Newtonsoft.Json 'json.Net 패키지 인스톨 후 선언 
Imports Newtonsoft.Json.Linq

Public Module Smiple_JSON_Test
	Dim data_raw As String '전역 변수 선언 
	Public Sub main() '메인 선언
		Dim response As System.Net.HttpWebResponse = Nothing
		System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 '연결을 위한 객체 생성
		' JSON 실제 전송 부분 ' 기본 양식 : Dim data = Encoding.UTF8.GetBytes("{""ids"": [8]}")
		data_raw = "{""GruPreScript"": {""PatientInfo"": { ""PatientNo"": ""000000""}}}"
		Dim data = Encoding.UTF8.GetBytes(data_raw) 'UTF-8 형식으로 데이터 전송
		Dim result_post = SendRequest(data, "application/json", "POST") ' Send data 양식 JSON 
	End Sub

	Public Function SendRequest(jsonDataBytes As Byte(), contentType As String, method As String) As String
		'이하 변수 선언  dim(변수명) AS (변수의 데이터 형식) 
		Dim request As HttpWebRequest
		Dim response As HttpWebResponse '= Nothing
		Dim state_code As Int32
		Dim Response_Stream As Stream
		Dim Response_reader As StreamReader
		Dim text As String

		request = WebRequest.Create("http://192.0.0.1:10000/prescripterr") '로컬 서버 
		request.Method = method
		Console.WriteLine("코드 시작")
		Using requestStream = request.GetRequestStream()
			requestStream.Write(jsonDataBytes, 0, jsonDataBytes.Length) 'JSON 데이터 전송
			requestStream.Close() 'JSON 연결 종료
		End Using

		Try
			Console.WriteLine("첫 시도")
			response = DirectCast(request.GetResponse(), HttpWebResponse)
			state_code = response.StatusCode() '상태 코드 선언
			Response_Stream = response.GetResponseStream() '응답 스트림
			Response_reader = New StreamReader(Response_Stream) '응답 전문 선언
			text = Response_reader.ReadToEnd

			Console.Write("status code" + " : ")
			Console.WriteLine(state_code) ' 상태 코드 출력
			Console.Write("response Post" + " : ")
			Console.WriteLine(text) '메시지 출력
			'Console.Write("Send Data" + " : ") 'Console.WriteLine(data_raw) '전송한 JSON 출력

		Catch ex As WebException '에러처리
			Console.WriteLine("예외 처리")
			response = ex.Response '예외처리된 response 수신
			Console.WriteLine(response)
			Response_reader = New StreamReader(response.GetResponseStream()) '응답 전문 선언
			text = Response_reader.ReadToEnd

			Console.Write("status code" + " : ")
			Console.WriteLine(response.StatusCode) '상태 코드 출력
			Console.Write("response Post" + " : ")
			Console.WriteLine(text) '메시지 출력
		Catch '기타 예외처리
			Console.WriteLine("Other Exception")
		Finally '종결문
			Console.WriteLine("End")
		End Try

	End Function
End Module

'참조http://vezi95.blogspot.com/2014/12/vbnet-json-newtonsoftjson.html  JSON 양식
'참조http://www.csharpstudy.com/web/article/16-HttpWebRequest-%ED%99%9C%EC%9A%A9  POST 예제
