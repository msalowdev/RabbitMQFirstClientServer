param([String]$RabbitDllPath = "not specified")

$RabbitDllPath = Resolve-Path $RabbitDllPath 
Write-Host "Rabbit DLL Path: " 
Write-Host $RabbitDllPath -foregroundcolor green

set-ExecutionPolicy Unrestricted

$absoluteRabbitDllPath = Resolve-Path $RabbitDllPath

Write-Host "Absolute Rabbit DLL Path: " 
Write-Host $absoluteRabbitDllPath -foregroundcolor green

[Reflection.Assembly]::LoadFile($absoluteRabbitDllPath)


Write-Host "Setting up RabbitMQ Connection Factory"
$factory = new-object RabbitMQ.Client.ConnectionFactory
Write-Host "Factory found"
$hostNameProp = [RabbitMQ.Client.ConnectionFactory].GetField(“HostName”)
Write-Host "Hostname property found"
$hostNameProp.SetValue($factory, “192.168.1.17”)

$usernameProp = [RabbitMQ.Client.ConnectionFactory].GetField(“UserName”)
$usernameProp.SetValue($factory, “demouser”)

$passwordProp = [RabbitMQ.Client.ConnectionFactory].GetField(“Password”)
$passwordProp.SetValue($factory, “test123”)

$createConnectionMethod = [RabbitMQ.Client.ConnectionFactory].GetMethod(“CreateConnection”, [Type]::EmptyTypes)
$connection = $createConnectionMethod.Invoke($factory, “instance,public”, $null, $null, $null)

Write-Host "Setting up RabbitMQ Model"
$model = $connection.CreateModel()

Write-Host "Creating Queue"
$model.QueueDeclare(“Module2.Sample1”, $true, $false, $false, $null)

Write-Host "Setup complete"