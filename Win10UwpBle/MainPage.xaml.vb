' The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409
Imports Windows.Storage
Imports Windows.Storage.Streams
Imports Win10UwpBle
Imports Windows.Devices.Bluetooth.Advertisement
''' <summary>
''' An empty page that can be used on its own or navigated to within a Frame.
''' </summary>
Public NotInheritable Class MainPage
    Inherits Page

    Dim HEX_ARRAY As Array = "0123456789ABCDEF".ToCharArray()

    ' manufacturer
    Dim ID_MANUFACTURER As String = "FFEE"
    Dim ID_FF As String = "FF"

    ' sensor identification
    Dim ID_TEMPERATURE As String = "01"
    Dim ID_HUMIDITY As String = "04"
    Dim ID_PRESSURE As String = "05"
    Dim ID_ORIENTATION As String = "06"
    Dim ID_PIR As String = "07"
    Dim ID_MOTION As String = "08"
    Dim ID_MECH_SHOCK As String = "09"
    Dim ID_BATTERY As String = "0A"

    ' alarm mode
    Dim ID_AL_TEMPERATURE As String = "81"
    Dim ID_AL_HUMIDITY As String = "84"
    Dim ID_AL_PRESSURE As String = "85"
    Dim ID_AL_ORIENTATION As String = "86"
    Dim ID_AL_PIR As String = "87"
    Dim ID_AL_MOTION As String = "88"
    Dim ID_AL_MECH_SHOCK As String = "89"
    Dim ID_AL_BATTERY As String = "8A"

    'data block length (byte)
    Dim LENGTH_ID As Integer = 2
    Dim LENGTH_TEMPERATURE As Integer = 4
    Dim LENGTH_HUMIDITY As Integer = 4
    Dim LENGTH_PRESSURE As Integer = 6
    Dim LENGTH_ORIENTATION As Integer = 12
    Dim LENGTH_PIR As Integer = 2
    Dim LENGTH_MOTION As Integer = 2
    Dim LENGTH_MECH_SHOCK As Integer = 6
    Dim LENGTH_BATTERY As Integer = 2


































    Dim paska As New BluetoothLEAdvertisementWatcher
    Dim advertisementFilter As BluetoothLEAdvertisementFilter


    Public Event BLE(w As BluetoothLEAdvertisementWatcher, e As BluetoothLEAdvertisementReceivedEventArgs)

    Private Sub MainPage_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded


        'Dim manufacturer = advertisementFilter.Advertisement.ManufacturerData

        Dim scan As BluetoothLEScanningMode
        scan = BluetoothLEScanningMode.Active


        AddHandler paska.Received, AddressOf OnAdvertisementReceived

        paska.Start()





    End Sub

    Private Sub OnAdvertisementReceived(sender As BluetoothLEAdvertisementWatcher, e As BluetoothLEAdvertisementReceivedEventArgs)

        Dim advertisement As BluetoothLEAdvertisement = e.Advertisement
        Debug.WriteLine(advertisement.ManufacturerData.ToString)
        Debug.WriteLine(advertisement.LocalName.ToString)
        Debug.WriteLine(e.BluetoothAddress.ToString("x"))

        Dim datalista As IList(Of BluetoothLEAdvertisementDataSection)

        Dim dd = New BluetoothLEAdvertisementDataSection

        Dim arrays As Byte()
        datalista = advertisement.DataSections

        For Each h In datalista
            Dim bb As IBuffer
            bb = h.Data

            arrays = GetStreamAsByteArray(bb)



            Dim rawdata = New Byte(h.Data.Length - 1) {}
            Using reader = DataReader.FromBuffer(h.Data)
                reader.ReadBytes(rawdata)
            End Using

            Debug.WriteLine("RawData " & BitConverter.ToString(rawdata))

        Next


        Dim manufacturerSections = e.Advertisement.ManufacturerData
        If manufacturerSections.Count > 0 Then
            Dim manufacturerdata = manufacturerSections(0)
            Dim data = New Byte(manufacturerdata.Data.Length - 1) {}

            Using reader = DataReader.FromBuffer(manufacturerdata.Data)
                reader.ReadBytes(data)
            End Using

            Debug.WriteLine("Manufacturerdata" & String.Format("0x{0}: {1}", manufacturerdata.CompanyId.ToString("X"), BitConverter.ToString(data)))


        End If


        Debug.WriteLine(String.Format(vbLf & "[{0}] [{1}]: Rssi={2}dBm, localName={3}", e.Timestamp.ToString("hh\:mm\:ss\.fff"), e.AdvertisementType.ToString(), e.RawSignalStrengthInDBm.ToString(), e.Advertisement.LocalName))






    End Sub
    Private Function GetStreamAsByteArray(ByVal stream As IBuffer) As Byte()

        Return stream.ToArray()

    End Function



End Class

'Public NotInheritable Class BluetoothLEAdvertisementWatcher
'    'Implements BluetoothLEAdvertisementWatcher




'End Class


